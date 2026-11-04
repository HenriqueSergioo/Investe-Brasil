using InvesteBrasil.Investimento.Entities;

namespace InvesteBrasil.Investimento.Repositories.Postgres.Interfaces
{
    public interface IUsuarioRepository
    {
        Task GarantirEsquemaAsync();

        Task<UsuarioEntity?> ConsultarPorUsuarioAsync(string usuario);

        Task<UsuarioEntity?> ConsultarPorCodigoAsync(long codigoUsuario);

        Task<UsuarioEntity?> ConsultarPorEmailAsync(string email);

        Task<UsuarioEntity> IncluirAsync(UsuarioEntity entity);
    }
}
