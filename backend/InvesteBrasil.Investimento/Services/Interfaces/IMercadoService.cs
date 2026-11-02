using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Responses;

namespace InvesteBrasil.Investimento.Services.Interfaces
{
    public interface IMercadoService
    {
        Task<IResultadoValidacao<PainelMercadoEntity>> ConsultarMercadoAsync();
    }
}
