using System.Globalization;
using System.Net;
using System.Net.Mail;
using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace InvesteBrasil.Investimento.Services.Notificacao
{
    public class NotificacaoEmailService : INotificacaoService
    {
        private static readonly CultureInfo Cultura = new("pt-BR");
        private readonly IUsuarioRepository usuarioRepository;
        private readonly IOptions<SmtpOptions> smtpOptions;
        private readonly ILogger<NotificacaoEmailService> logger;

        public NotificacaoEmailService(
            IUsuarioRepository usuarioRepository,
            IOptions<SmtpOptions> smtpOptions,
            ILogger<NotificacaoEmailService> logger)
        {
            this.usuarioRepository = usuarioRepository;
            this.smtpOptions = smtpOptions;
            this.logger = logger;
        }

        public async Task NotificarCriacaoAsync(InvestimentoEntity investimento)
        {
            var assunto = "InvesteBrasil: investimento criado";
            var corpo =
                $"Olá.{Environment.NewLine}{Environment.NewLine}" +
                $"Um investimento foi criado em sua conta.{Environment.NewLine}" +
                $"Proprietário: {investimento.NomeProprietario}{Environment.NewLine}" +
                $"Valor inicial: {investimento.ValorInicial.ToString("C", Cultura)}{Environment.NewLine}" +
                $"Data da aplicação: {investimento.DataCriacao.ToString("dd/MM/yyyy", Cultura)}{Environment.NewLine}" +
                $"Código: {investimento.CodigoInvestimento}";

            await EnviarAsync(investimento.CodigoUsuario, assunto, corpo);
        }

        public async Task NotificarResgateAsync(InvestimentoEntity investimento, InvestimentoDetalheEntity detalhe)
        {
            var assunto = "InvesteBrasil: investimento resgatado";
            var corpo =
                $"Olá.{Environment.NewLine}{Environment.NewLine}" +
                $"O resgate do investimento {investimento.CodigoInvestimento} foi concluído.{Environment.NewLine}" +
                $"Data do resgate: {investimento.DataResgate?.ToString("dd/MM/yyyy", Cultura)}{Environment.NewLine}" +
                $"Ganhos: {detalhe.ValorGanhos.ToString("C", Cultura)}{Environment.NewLine}" +
                $"Imposto: {detalhe.ValorImposto.ToString("C", Cultura)}{Environment.NewLine}" +
                $"Valor líquido: {detalhe.ValorLiquido.ToString("C", Cultura)}";

            await EnviarAsync(investimento.CodigoUsuario, assunto, corpo);
        }

        private async Task EnviarAsync(long codigoUsuario, string assunto, string corpo)
        {
            var smtp = smtpOptions.Value;
            var usuario = await usuarioRepository.ConsultarPorCodigoAsync(codigoUsuario);
            var destino = usuario?.DescricaoEmail?.Trim() ?? string.Empty;

            if (!smtp.Configurado)
            {
                logger.LogInformation("SMTP não configurado. Notificação '{Assunto}' para {Destino}: {Corpo}", assunto, destino, corpo);
                return;
            }

            if (string.IsNullOrWhiteSpace(destino))
            {
                logger.LogWarning("Usuário {CodigoUsuario} sem e-mail. Notificação '{Assunto}' não foi enviada.", codigoUsuario, assunto);
                return;
            }

            try
            {
                using var mensagem = new MailMessage(smtp.From, destino, assunto, corpo);
                using var client = new SmtpClient(smtp.Host, smtp.Port)
                {
                    EnableSsl = smtp.EnableSsl,
                    Credentials = new NetworkCredential(smtp.User, smtp.Password)
                };

                await client.SendMailAsync(mensagem);
                logger.LogInformation("E-mail '{Assunto}' enviado para {Destino}.", assunto, destino);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Falha ao enviar e-mail '{Assunto}' para {Destino}.", assunto, destino);
            }
        }
    }
}
