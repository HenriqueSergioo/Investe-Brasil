using InvesteBrasil.Investimento.Entities;

namespace InvesteBrasil.Investimento.Repositories.Postgres.Interfaces
{
    public interface IInvestimentoRepository
    {
        Task<InvestimentoEntity> IncluirAsync(InvestimentoEntity entity);

        Task<InvestimentoEntity?> ConsultarPorIdAsync(long codigoInvestimento, long codigoUsuario);

        Task<(IEnumerable<InvestimentoEntity> Itens, int Total)> ConsultarAsync(
            long codigoUsuario,
            int pagina,
            int tamanhoPagina);

        Task<IEnumerable<InvestimentoEntity>> ConsultarAtivosAsync(long codigoUsuario);

        Task ResgatarAsync(long codigoInvestimento, long codigoUsuario, DateOnly dataResgate);
    }
}
