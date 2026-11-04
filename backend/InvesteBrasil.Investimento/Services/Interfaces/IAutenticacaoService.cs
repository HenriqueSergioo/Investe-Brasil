using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;

namespace InvesteBrasil.Investimento.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<IResultadoValidacao<UsuarioAutenticadoEntity>> EntrarAsync(EntrarRequestEntity request);

        Task<IResultadoValidacao<UsuarioAutenticadoEntity>> CadastrarUsuarioAsync(CadastrarUsuarioRequestEntity request);
    }
}
