using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Responses;

namespace InvesteBrasil.Investimento.Services.Notificacao
{
    public interface INotificacaoService
    {
        Task NotificarCriacaoAsync(InvestimentoEntity investimento);

        Task NotificarResgateAsync(InvestimentoEntity investimento, InvestimentoDetalheEntity detalhe);
    }
}
