using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;

namespace InvesteBrasil.Investimento.Services.Interfaces
{
    public interface IAutenticacaoService
    {
        IResultadoValidacao<UsuarioAutenticadoEntity> Entrar(EntrarRequestEntity request);
    }
}
