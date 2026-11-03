using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Responses;
using Microsoft.Extensions.Logging;

namespace InvesteBrasil.Investimento.Services.Notificacao
{
    public class NotificacaoLogService : INotificacaoService
    {
        private readonly ILogger<NotificacaoLogService> logger;

        public NotificacaoLogService(ILogger<NotificacaoLogService> logger)
        {
            this.logger = logger;
        }

        public Task NotificarCriacaoAsync(InvestimentoEntity investimento)
        {
            logger.LogInformation(
                "Notificação de criação: investimento {CodigoInvestimento} do proprietário {NomeProprietario} no valor {ValorInicial}.",
                investimento.CodigoInvestimento,
                investimento.NomeProprietario,
                investimento.ValorInicial);

            return Task.CompletedTask;
        }

        public Task NotificarResgateAsync(InvestimentoEntity investimento, InvestimentoDetalheEntity detalhe)
        {
            logger.LogInformation(
                "Notificação de resgate: investimento {CodigoInvestimento} resgatado em {Data}. Valor líquido {ValorLiquido} após imposto de {Imposto}.",
                investimento.CodigoInvestimento,
                investimento.DataResgate,
                detalhe.ValorLiquido,
                detalhe.ValorImposto);

            return Task.CompletedTask;
        }
    }
}
