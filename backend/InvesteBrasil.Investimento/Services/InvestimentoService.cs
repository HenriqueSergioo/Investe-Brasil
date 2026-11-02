using InvesteBrasil.Infra.Services.Abstracts;
using InvesteBrasil.Infra.Shared;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Enums;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Services.Interfaces;
using InvesteBrasil.Investimento.Services.Notificacao;
using InvesteBrasil.Investimento.Services.Validators;

namespace InvesteBrasil.Investimento.Services
{
    public class InvestimentoService : ServiceBase<InvestimentoValidador>, IInvestimentoService
    {
        private readonly IInvestimentoRepository investimentoRepository;
        private readonly INotificacaoService notificacaoService;

        public InvestimentoService(
            IInvestimentoRepository investimentoRepository,
            INotificacaoService notificacaoService)
        {
            this.investimentoRepository = investimentoRepository;
            this.notificacaoService = notificacaoService;
        }

        public async Task<IResultadoValidacao<InvestimentoEntityResponse>> IncluirInvestimentoAsync(IncluirInvestimentoRequestEntity request)
        {
            try
            {
                var hoje = Hoje();
                var validacao = Validator.IncluirInvestimento(request, hoje);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<InvestimentoEntityResponse>(validacao);

                var entity = new InvestimentoEntity
                {
                    Proprietario = request.Proprietario.Trim(),
                    DataCriacao = request.DataCriacao,
                    ValorInicial = decimal.Round(request.ValorInicial, 2, MidpointRounding.AwayFromZero),
                    Cpf = SomenteNumeros(request.Cpf),
                    TipoInvestimento = request.TipoInvestimento.Trim(),
                    Instituicao = request.Instituicao.Trim(),
                    Liquidez = request.Liquidez.Trim(),
                    DataVencimento = request.DataVencimento,
                    Objetivo = request.Objetivo.Trim(),
                    Observacao = request.Observacao?.Trim() ?? string.Empty,
                    Situacao = SituacaoInvestimento.Investido
                };

                var incluido = await investimentoRepository.IncluirAsync(entity);
                await notificacaoService.NotificarCriacaoAsync(incluido);

                return new ResultadoValidacao<InvestimentoEntityResponse>(new InvestimentoEntityResponse { Id = incluido.Id });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<InvestimentoEntityResponse>(ex, "[InvestimentoService] - Erro ao tentar incluir o investimento.");
            }
        }

        public async Task<IResultadoValidacao<PaginacaoEntity<InvestimentoListagemEntity>>> ConsultarInvestimentosAsync(
            string? proprietario,
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
                    proprietario,
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

        public async Task<IResultadoValidacao<InvestimentoDetalheEntity>> ConsultarInvestimentoAsync(long id, DateOnly? dataReferencia = null)
        {
            try
            {
                var validacao = Validator.ConsultarInvestimento(id);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<InvestimentoDetalheEntity>(validacao);

                var investimento = await investimentoRepository.ConsultarPorIdAsync(id);
                if (investimento == null)
                    return new ResultadoValidacao<InvestimentoDetalheEntity>("Investimento não encontrado.");

                return new ResultadoValidacao<InvestimentoDetalheEntity>(MapearDetalhe(investimento, dataReferencia ?? Hoje()));
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<InvestimentoDetalheEntity>(ex, "[InvestimentoService] - Erro ao tentar consultar o investimento.");
            }
        }

        public async Task<IResultadoValidacao<InvestimentoDetalheEntity>> ResgatarInvestimentoAsync(ResgatarInvestimentoRequestEntity request)
        {
            try
            {
                var investimento = request == null ? null : await investimentoRepository.ConsultarPorIdAsync(request.Id);
                var validacao = Validator.ResgatarInvestimento(request!, investimento, Hoje());
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<InvestimentoDetalheEntity>(validacao);

                await investimentoRepository.ResgatarAsync(request!.Id, request.DataResgate);

                investimento!.DataResgate = request.DataResgate;
                investimento.Situacao = SituacaoInvestimento.Resgatado;

                var detalhe = MapearDetalhe(investimento, request.DataResgate);
                await notificacaoService.NotificarResgateAsync(investimento, detalhe);

                return new ResultadoValidacao<InvestimentoDetalheEntity>(detalhe);
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<InvestimentoDetalheEntity>(ex, "[InvestimentoService] - Erro ao tentar resgatar o investimento.");
            }
        }

        public async Task<IResultadoValidacao<SaldoGeralEntity>> ConsultarSaldoGeralAsync()
        {
            try
            {
                var hoje = Hoje();
                var ativos = await investimentoRepository.ConsultarAtivosAsync();
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
            var resgatado = item.Situacao == SituacaoInvestimento.Resgatado;
            var dataReferencia = resgatado && item.DataResgate.HasValue ? item.DataResgate.Value : hoje;

            return new InvestimentoListagemEntity
            {
                Id = item.Id,
                Proprietario = item.Proprietario,
                TipoInvestimento = item.TipoInvestimento,
                Instituicao = item.Instituicao,
                DataCriacao = item.DataCriacao,
                ValorInicial = item.ValorInicial,
                SaldoAtual = resgatado
                    ? 0
                    : CalculadoraRendimento.CalcularSaldoEsperado(item.ValorInicial, item.DataCriacao, dataReferencia),
                Situacao = item.Situacao
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
                Id = item.Id,
                Proprietario = item.Proprietario,
                DataCriacao = item.DataCriacao,
                DataResgate = item.DataResgate,
                ValorInicial = item.ValorInicial,
                Cpf = item.Cpf,
                TipoInvestimento = item.TipoInvestimento,
                Instituicao = item.Instituicao,
                Liquidez = item.Liquidez,
                DataVencimento = item.DataVencimento,
                Objetivo = item.Objetivo,
                Observacao = item.Observacao,
                Ganhos = ganhos,
                SaldoEsperado = saldoEsperado,
                AliquotaImposto = aliquota,
                ValorImposto = imposto,
                ValorLiquido = CalculadoraRendimento.CalcularValorLiquido(saldoEsperado, imposto),
                Situacao = item.Situacao
            };
        }

        private static DateOnly Hoje() => DateOnly.FromDateTime(DateTime.Today);

        private static string SomenteNumeros(string valor) =>
            new(valor.Where(char.IsDigit).ToArray());
    }
}
