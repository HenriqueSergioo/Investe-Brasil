using System.Globalization;
using System.Text.Json;
using InvesteBrasil.Infra.Shared;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Services.Interfaces;

namespace InvesteBrasil.Investimento.Services
{
    public class MercadoService : IMercadoService
    {
        private const string ClienteHttp = "Mercado";
        private static readonly TimeSpan DuracaoCache = TimeSpan.FromMinutes(5);
        private static readonly JsonSerializerOptions Json = new() { PropertyNameCaseInsensitive = true };
        private static readonly CultureInfo Cultura = CultureInfo.InvariantCulture;
        private static readonly object locker = new();
        private static IList<CotacaoMercadoEntity>? cotacoesCache;
        private static DateTimeOffset cacheExpira;

        private readonly IHttpClientFactory httpClientFactory;
        private readonly IInvestimentoRepository investimentoRepository;

        public MercadoService(IHttpClientFactory httpClientFactory, IInvestimentoRepository investimentoRepository)
        {
            this.httpClientFactory = httpClientFactory;
            this.investimentoRepository = investimentoRepository;
        }

        public async Task<IResultadoValidacao<PainelMercadoEntity>> ConsultarMercadoAsync()
        {
            try
            {
                var cotacoes = await ObterCotacoesAsync();
                var hoje = DateOnly.FromDateTime(DateTime.Today);
                var ativos = await investimentoRepository.ConsultarAtivosAsync();
                var saldo = ativos.Sum(item => CalculadoraRendimento.CalcularSaldoEsperado(item.ValorInicial, item.DataCriacao, hoje));
                saldo = Math.Round(saldo, 2, MidpointRounding.AwayFromZero);

                return new ResultadoValidacao<PainelMercadoEntity>(MontarPainel(saldo, cotacoes));
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<PainelMercadoEntity>(ex, "[MercadoService] - Erro ao consultar o mercado.");
            }
        }

        private async Task<IList<CotacaoMercadoEntity>> ObterCotacoesAsync()
        {
            lock (locker)
            {
                if (cotacoesCache != null && cacheExpira > DateTimeOffset.UtcNow)
                    return cotacoesCache;
            }

            var client = httpClientFactory.CreateClient(ClienteHttp);
            var cotacoes = new List<CotacaoMercadoEntity>();
            cotacoes.AddRange(await ConsultarAwesomeApiAsync(client));
            cotacoes.AddRange(await ConsultarBancoCentralAsync(client));

            lock (locker)
            {
                cotacoesCache = cotacoes;
                cacheExpira = DateTimeOffset.UtcNow.Add(DuracaoCache);
            }

            return cotacoes;
        }

        private static PainelMercadoEntity MontarPainel(decimal saldo, IList<CotacaoMercadoEntity> cotacoes)
        {
            var taxaAnual = decimal.Round(
                ((decimal)Math.Pow((double)(1 + CalculadoraRendimento.TaxaMensal), 12) - 1) * 100,
                2,
                MidpointRounding.AwayFromZero);

            var selic = Buscar(cotacoes, "SELIC");
            var dolar = Buscar(cotacoes, "USD");
            var euro = Buscar(cotacoes, "EUR");
            var bitcoin = Buscar(cotacoes, "BTC");
            var diferenca = selic == null
                ? (decimal?)null
                : decimal.Round(taxaAnual - selic.Valor, 2, MidpointRounding.AwayFromZero);

            return new PainelMercadoEntity
            {
                SaldoCarteira = saldo,
                TaxaMensalCarteira = CalculadoraRendimento.TaxaMensal,
                TaxaAnualCarteira = taxaAnual,
                SelicAnual = selic?.Valor,
                DiferencaPontos = diferenca,
                ComparativoSelic = MontarComparativoSelic(taxaAnual, selic?.Valor, diferenca),
                SaldoEmDolar = Converter(saldo, dolar?.Valor),
                SaldoEmEuro = Converter(saldo, euro?.Valor),
                SaldoEmBitcoin = Converter(saldo, bitcoin?.Valor, 8),
                CotacaoDolar = dolar?.Valor,
                VariacaoDolar = dolar?.VariacaoPercentual,
                CotacaoEuro = euro?.Valor,
                VariacaoEuro = euro?.VariacaoPercentual,
                CotacaoBitcoin = bitcoin?.Valor,
                VariacaoBitcoin = bitcoin?.VariacaoPercentual,
                LeituraCambio = MontarLeituraCambio(saldo, dolar, euro, bitcoin),
                Cotacoes = cotacoes
            };
        }

        private static string MontarComparativoSelic(decimal taxaAnual, decimal? selic, decimal? diferenca)
        {
            var anualTexto = taxaAnual.ToString("N2", new CultureInfo("pt-BR"));
            if (!selic.HasValue || !diferenca.HasValue)
                return $"A carteira rende 0,52% ao mês, cerca de {anualTexto}% ao ano. A Selic não veio agora, então o comparativo fica pendente.";

            var selicTexto = selic.Value.ToString("N2", new CultureInfo("pt-BR"));
            var pontos = Math.Abs(diferenca.Value).ToString("N2", new CultureInfo("pt-BR"));

            if (diferenca.Value < 0)
                return $"A taxa fixa da carteira é 0,52% ao mês ({anualTexto}% ao ano). A Selic está em {selicTexto}% ao ano, {pontos} pontos acima. Serve para decidir se vale manter, resgatar ou buscar um CDB mais próximo do juro básico.";

            if (diferenca.Value > 0)
                return $"A taxa da carteira equivale a {anualTexto}% ao ano e está {pontos} pontos acima da Selic de {selicTexto}%. Neste recorte, o rendimento contratado ganha do juro básico.";

            return $"A taxa da carteira, {anualTexto}% ao ano, está alinhada com a Selic de {selicTexto}%.";
        }

        private static string MontarLeituraCambio(decimal saldo, CotacaoMercadoEntity? dolar, CotacaoMercadoEntity? euro, CotacaoMercadoEntity? bitcoin)
        {
            if (dolar == null && euro == null && bitcoin == null)
                return "As cotações de câmbio não vieram agora. Quando voltarem, o saldo da carteira aparece em dólar, euro e bitcoin.";

            if (saldo <= 0)
                return "Dólar, euro e bitcoin existem para converter o saldo da carteira. Com investimentos ativos, o patrimônio aparece em moeda forte e em bitcoin.";

            var cultura = new CultureInfo("pt-BR");
            var partes = new List<string>();

            if (dolar != null)
            {
                var valor = Converter(saldo, dolar.Valor);
                var movimento = dolar.VariacaoPercentual.HasValue && dolar.VariacaoPercentual.Value > 0
                    ? "subiu"
                    : dolar.VariacaoPercentual.HasValue && dolar.VariacaoPercentual.Value < 0
                        ? "caiu"
                        : "ficou estável";
                partes.Add($"o saldo vale US$ {valor?.ToString("N2", cultura)} e o dólar {movimento} no dia");
            }

            if (euro != null)
            {
                var valor = Converter(saldo, euro.Valor);
                partes.Add($"em euro fica € {valor?.ToString("N2", cultura)}");
            }

            if (bitcoin != null)
            {
                var valor = Converter(saldo, bitcoin.Valor, 8);
                partes.Add($"em bitcoin equivaleria a {valor?.ToString("N8", cultura)} BTC, só para medir o patrimônio numa alternativa de risco");
            }

            return "Conversão do saldo atual nas cotações de hoje: " + string.Join("; ", partes) + ".";
        }

        private static decimal? Converter(decimal saldo, decimal? cotacao, int casas = 2)
        {
            if (!cotacao.HasValue || cotacao.Value <= 0)
                return null;

            return decimal.Round(saldo / cotacao.Value, casas, MidpointRounding.AwayFromZero);
        }

        private static CotacaoMercadoEntity? Buscar(IEnumerable<CotacaoMercadoEntity> cotacoes, string codigo) =>
            cotacoes.FirstOrDefault(item => item.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));

        private static async Task<IEnumerable<CotacaoMercadoEntity>> ConsultarAwesomeApiAsync(HttpClient client)
        {
            try
            {
                using var resposta = await client.GetAsync("https://economia.awesomeapi.com.br/json/last/USD-BRL,EUR-BRL,BTC-BRL");
                if (!resposta.IsSuccessStatusCode)
                    return Array.Empty<CotacaoMercadoEntity>();

                var json = await resposta.Content.ReadAsStringAsync();
                var mapa = JsonSerializer.Deserialize<Dictionary<string, AwesomeCotacaoDto>>(json, Json);
                if (mapa == null)
                    return Array.Empty<CotacaoMercadoEntity>();

                var nomes = new Dictionary<string, string>
                {
                    ["USDBRL"] = "Dólar",
                    ["EURBRL"] = "Euro",
                    ["BTCBRL"] = "Bitcoin"
                };

                return mapa
                    .Where(item => nomes.ContainsKey(item.Key) && decimal.TryParse(item.Value.Bid, NumberStyles.Any, Cultura, out _))
                    .Select(item => new CotacaoMercadoEntity
                    {
                        Codigo = item.Value.Code,
                        Nome = nomes[item.Key],
                        Valor = decimal.Parse(item.Value.Bid, NumberStyles.Any, Cultura),
                        VariacaoPercentual = decimal.TryParse(item.Value.PctChange, NumberStyles.Any, Cultura, out var variacao)
                            ? decimal.Round(variacao, 2, MidpointRounding.AwayFromZero)
                            : null,
                        Unidade = "moeda",
                        Fonte = "AwesomeAPI",
                        AtualizadoEm = item.Value.Create_date
                    });
            }
            catch
            {
                return Array.Empty<CotacaoMercadoEntity>();
            }
        }

        private static async Task<IEnumerable<CotacaoMercadoEntity>> ConsultarBancoCentralAsync(HttpClient client)
        {
            try
            {
                using var resposta = await client.GetAsync("https://api.bcb.gov.br/dados/serie/bcdata.sgs.432/dados/ultimos/1?formato=json");
                if (!resposta.IsSuccessStatusCode)
                    return Array.Empty<CotacaoMercadoEntity>();

                var json = await resposta.Content.ReadAsStringAsync();
                var serie = JsonSerializer.Deserialize<List<BancoCentralDto>>(json, Json);
                var item = serie?.FirstOrDefault();
                if (item == null || !decimal.TryParse(item.Valor, NumberStyles.Any, Cultura, out var valor))
                    return Array.Empty<CotacaoMercadoEntity>();

                return new[]
                {
                    new CotacaoMercadoEntity
                    {
                        Codigo = "SELIC",
                        Nome = "Selic",
                        Valor = valor,
                        Unidade = "percentual",
                        Fonte = "Banco Central",
                        AtualizadoEm = item.Data
                    }
                };
            }
            catch
            {
                return Array.Empty<CotacaoMercadoEntity>();
            }
        }

        private sealed class AwesomeCotacaoDto
        {
            public string Code { get; set; } = string.Empty;
            public string Bid { get; set; } = string.Empty;
            public string PctChange { get; set; } = string.Empty;
            public string Create_date { get; set; } = string.Empty;
        }

        private sealed class BancoCentralDto
        {
            public string Data { get; set; } = string.Empty;
            public string Valor { get; set; } = string.Empty;
        }
    }
}
