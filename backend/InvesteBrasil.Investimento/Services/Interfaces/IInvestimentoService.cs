using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;

namespace InvesteBrasil.Investimento.Services.Interfaces
{
    public interface IInvestimentoService
    {
        Task<IResultadoValidacao<InvestimentoEntityResponse>> IncluirInvestimentoAsync(long codigoUsuario, IncluirInvestimentoRequestEntity request);

        Task<IResultadoValidacao<PaginacaoEntity<InvestimentoListagemEntity>>> ConsultarInvestimentosAsync(
            long codigoUsuario,
            string? nomeProprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina);

        Task<IResultadoValidacao<InvestimentoDetalheEntity>> ConsultarInvestimentoAsync(long codigoUsuario, long codigoInvestimento, DateOnly? dataReferencia = null);

        Task<IResultadoValidacao<InvestimentoDetalheEntity>> ResgatarInvestimentoAsync(long codigoUsuario, ResgatarInvestimentoRequestEntity request);

        Task<IResultadoValidacao<SaldoGeralEntity>> ConsultarSaldoGeralAsync(long codigoUsuario);
    }

    public class InvestimentoEntityResponse
    {
        public long CodigoInvestimento { get; set; }
    }
}
