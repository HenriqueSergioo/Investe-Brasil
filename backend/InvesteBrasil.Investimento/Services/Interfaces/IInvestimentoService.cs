using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;

namespace InvesteBrasil.Investimento.Services.Interfaces
{
    public interface IInvestimentoService
    {
        Task<IResultadoValidacao<InvestimentoEntityResponse>> IncluirInvestimentoAsync(IncluirInvestimentoRequestEntity request);

        Task<IResultadoValidacao<PaginacaoEntity<InvestimentoListagemEntity>>> ConsultarInvestimentosAsync(
            string? proprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina);

        Task<IResultadoValidacao<InvestimentoDetalheEntity>> ConsultarInvestimentoAsync(long id, DateOnly? dataReferencia = null);

        Task<IResultadoValidacao<InvestimentoDetalheEntity>> ResgatarInvestimentoAsync(ResgatarInvestimentoRequestEntity request);

        Task<IResultadoValidacao<SaldoGeralEntity>> ConsultarSaldoGeralAsync();
    }

    public class InvestimentoEntityResponse
    {
        public long Id { get; set; }
    }
}
