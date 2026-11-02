using InvesteBrasil.Investimento.Entities;

namespace InvesteBrasil.Investimento.Repositories.Postgres.Interfaces
{
    public interface IInvestimentoRepository
    {
        Task<InvestimentoEntity> IncluirAsync(InvestimentoEntity entity);

        Task<InvestimentoEntity?> ConsultarPorIdAsync(long id);

        Task<(IEnumerable<InvestimentoEntity> Itens, int Total)> ConsultarAsync(
            string? proprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina);

        Task<IEnumerable<InvestimentoEntity>> ConsultarAtivosAsync();

        Task ResgatarAsync(long id, DateOnly dataResgate);

        Task GarantirEsquemaAsync();
    }
}
