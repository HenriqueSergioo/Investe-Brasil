using InvesteBrasil.Infra.Services.Abstracts;
using InvesteBrasil.Infra.Shared;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Enums;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Services.Interfaces;
using InvesteBrasil.Investimento.Services.Validators;

namespace InvesteBrasil.Investimento.Services
{
    public class InvestimentoService : ServiceBase<InvestimentoValidador>, IInvestimentoService
    {
        private readonly IInvestimentoRepository investimentoRepository;

        public InvestimentoService(IInvestimentoRepository investimentoRepository)
        {
            this.investimentoRepository = investimentoRepository;
        }

        public async Task<IResultadoValidacao<InvestimentoEntityResponse>> IncluirInvestimentoAsync(long codigoUsuario, IncluirInvestimentoRequestEntity request)
        {
            try
            {
                var hoje = Hoje();
                if (request is null)
                    return new ResultadoValidacao<InvestimentoEntityResponse>(Validator.IncluirInvestimento(null!, hoje));

                if (request.DataCriacao == default)
                    request.DataCriacao = hoje;

                var validacao = Validator.IncluirInvestimento(request, hoje);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<InvestimentoEntityResponse>(validacao);

                var entity = new InvestimentoEntity
                {
                    CodigoUsuario = codigoUsuario,
                    NomeProprietario = request.NomeProprietario.Trim(),
                    DataCriacao = request.DataCriacao,
                    ValorInicial = decimal.Round(request.ValorInicial, 2, MidpointRounding.AwayFromZero),
                    NumeroCpf = SomenteNumeros(request.NumeroCpf),
                    DescricaoTipoInvestimento = request.DescricaoTipoInvestimento.Trim(),
                    NomeInstituicao = request.NomeInstituicao.Trim(),
                    DescricaoLiquidez = request.DescricaoLiquidez.Trim(),
                    DataVencimento = request.DataVencimento,
                    DescricaoObjetivo = request.DescricaoObjetivo.Trim(),
                    DescricaoObservacao = request.DescricaoObservacao?.Trim() ?? string.Empty,
                    SituacaoInvestimento = SituacaoInvestimento.Investido
                };

                var incluido = await investimentoRepository.IncluirAsync(entity);

                return new ResultadoValidacao<InvestimentoEntityResponse>(new InvestimentoEntityResponse { CodigoInvestimento = incluido.CodigoInvestimento });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<InvestimentoEntityResponse>(ex, "[InvestimentoService] - Erro ao tentar incluir o investimento.");
            }
        }

        public async Task<IResultadoValidacao<PaginacaoEntity<InvestimentoListagemEntity>>> ConsultarInvestimentosAsync(
            long codigoUsuario,
            string? nomeProprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina)
        {
            try
            {
                if (pagina <= 0)
                    pagina = 1;

                if (tamanhoPagina <= 0 || tamanhoPagina > 100)
                    tamanhoPagina = 10;

                var (itens, total) = await investimentoRepository.ConsultarAsync(
                    codigoUsuario,
                    nomeProprietario,
                    dataInicial,
                    dataFinal,
                    pagina,
                    tamanhoPagina);

                var hoje = Hoje();
                var listagem = itens.Select(item => MapearListagem(item, hoje)).ToList();

                return new ResultadoValidacao<PaginacaoEntity<InvestimentoListagemEntity>>(new PaginacaoEntity<InvestimentoListagemEntity>
                {
                    Itens = listagem,
                    Pagina = pagina,
                    TamanhoPagina = tamanhoPagina,
                    TotalItens = total,
                    TotalPaginas = (int)Math.Ceiling(total / (double)tamanhoPagina)
                });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<PaginacaoEntity<InvestimentoListagemEntity>>(ex, "[InvestimentoService] - Erro ao tentar consultar os investimentos.");
            }
        }

        public async Task<IResultadoValidacao<InvestimentoDetalheEntity>> ConsultarInvestimentoAsync(long codigoUsuario, long codigoInvestimento, DateOnly? dataReferencia = null)
        {
            try
            {
                var validacao = Validator.ConsultarInvestimento(codigoInvestimento);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<InvestimentoDetalheEntity>(validacao);

                var investimento = await investimentoRepository.ConsultarPorIdAsync(codigoInvestimento, codigoUsuario);
                if (investimento == null)
                    return new ResultadoValidacao<InvestimentoDetalheEntity>("Investimento não encontrado.");

                return new ResultadoValidacao<InvestimentoDetalheEntity>(MapearDetalhe(investimento, dataReferencia ?? Hoje()));
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<InvestimentoDetalheEntity>(ex, "[InvestimentoService] - Erro ao tentar consultar o investimento.");
            }
        }

        public async Task<IResultadoValidacao<InvestimentoDetalheEntity>> ResgatarInvestimentoAsync(long codigoUsuario, ResgatarInvestimentoRequestEntity request)
        {
            try
            {
                var investimento = request == null ? null : await investimentoRepository.ConsultarPorIdAsync(request.CodigoInvestimento, codigoUsuario);
                var validacao = Validator.ResgatarInvestimento(request!, investimento, Hoje());
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<InvestimentoDetalheEntity>(validacao);

                await investimentoRepository.ResgatarAsync(request!.CodigoInvestimento, codigoUsuario, request.DataResgate);

                investimento!.DataResgate = request.DataResgate;
                investimento.SituacaoInvestimento = SituacaoInvestimento.Resgatado;

                var detalhe = MapearDetalhe(investimento, request.DataResgate);

                return new ResultadoValidacao<InvestimentoDetalheEntity>(detalhe);
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<InvestimentoDetalheEntity>(ex, "[InvestimentoService] - Erro ao tentar resgatar o investimento.");
            }
        }

        public async Task<IResultadoValidacao<SaldoGeralEntity>> ConsultarSaldoGeralAsync(long codigoUsuario)
        {
            try
            {
                var hoje = Hoje();
                var ativos = await investimentoRepository.ConsultarAtivosAsync(codigoUsuario);
                var saldo = ativos.Sum(item => CalculadoraRendimento.CalcularSaldoEsperado(item.ValorInicial, item.DataCriacao, hoje));

                return new ResultadoValidacao<SaldoGeralEntity>(new SaldoGeralEntity
                {
                    Saldo = Math.Round(saldo, 2, MidpointRounding.AwayFromZero)
                });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<SaldoGeralEntity>(ex, "[InvestimentoService] - Erro ao tentar consultar o saldo geral.");
            }
        }

        private static InvestimentoListagemEntity MapearListagem(InvestimentoEntity item, DateOnly hoje)
        {
            var resgatado = item.SituacaoInvestimento == SituacaoInvestimento.Resgatado;
            var dataReferencia = resgatado && item.DataResgate.HasValue ? item.DataResgate.Value : hoje;

            return new InvestimentoListagemEntity
            {
                CodigoInvestimento = item.CodigoInvestimento,
                NomeProprietario = item.NomeProprietario,
                DescricaoTipoInvestimento = item.DescricaoTipoInvestimento,
                DescricaoObjetivo = item.DescricaoObjetivo,
                NomeInstituicao = item.NomeInstituicao,
                DataCriacao = item.DataCriacao,
                ValorInicial = item.ValorInicial,
                ValorSaldoAtual = resgatado
                    ? 0
                    : CalculadoraRendimento.CalcularSaldoEsperado(item.ValorInicial, item.DataCriacao, dataReferencia),
                SituacaoInvestimento = item.SituacaoInvestimento
            };
        }

        private static InvestimentoDetalheEntity MapearDetalhe(InvestimentoEntity item, DateOnly dataReferenciaPadrao)
        {
            var dataReferencia = item.DataResgate ?? dataReferenciaPadrao;
            if (dataReferencia < item.DataCriacao)
                dataReferencia = item.DataCriacao;

            var saldoEsperado = CalculadoraRendimento.CalcularSaldoEsperado(item.ValorInicial, item.DataCriacao, dataReferencia);
            var ganhos = CalculadoraRendimento.CalcularGanhos(item.ValorInicial, item.DataCriacao, dataReferencia);
            var aliquota = CalculadoraRendimento.ObterAliquota(item.DataCriacao, dataReferencia);
            var imposto = CalculadoraRendimento.CalcularImposto(ganhos, aliquota);

            return new InvestimentoDetalheEntity
            {
                CodigoInvestimento = item.CodigoInvestimento,
                NomeProprietario = item.NomeProprietario,
                DataCriacao = item.DataCriacao,
                DataResgate = item.DataResgate,
                ValorInicial = item.ValorInicial,
                NumeroCpf = item.NumeroCpf,
                DescricaoTipoInvestimento = item.DescricaoTipoInvestimento,
                NomeInstituicao = item.NomeInstituicao,
                DescricaoLiquidez = item.DescricaoLiquidez,
                DataVencimento = item.DataVencimento,
                DescricaoObjetivo = item.DescricaoObjetivo,
                DescricaoObservacao = item.DescricaoObservacao,
                ValorGanhos = ganhos,
                ValorSaldoEsperado = saldoEsperado,
                ValorAliquotaImposto = aliquota,
                ValorImposto = imposto,
                ValorLiquido = CalculadoraRendimento.CalcularValorLiquido(saldoEsperado, imposto),
                SituacaoInvestimento = item.SituacaoInvestimento
            };
        }

        private static DateOnly Hoje() => DateOnly.FromDateTime(DateTime.Today);

        private static string SomenteNumeros(string valor) =>
            new(valor.Where(char.IsDigit).ToArray());
    }
}
