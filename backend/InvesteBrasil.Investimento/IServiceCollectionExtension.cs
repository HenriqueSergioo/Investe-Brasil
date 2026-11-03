using InvesteBrasil.Investimento.Repositories.Postgres;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Services;
using InvesteBrasil.Investimento.Services.Interfaces;
using InvesteBrasil.Investimento.Services.Notificacao;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InvesteBrasil.Investimento
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureServicesInvestimento(this IServiceCollection services, IConfiguration configuration)
        {
            if (string.IsNullOrWhiteSpace(NeonConnection.Obter(configuration)))
                throw new InvalidOperationException("Configure DATABASE_URL no arquivo .env para conectar ao Neon.");

            services.Configure<SmtpOptions>(options =>
            {
                options.Host = configuration["SMTP_HOST"] ?? string.Empty;
                options.Port = int.TryParse(configuration["SMTP_PORT"], out var porta) ? porta : 587;
                options.User = configuration["SMTP_USER"] ?? string.Empty;
                options.Password = configuration["SMTP_PASSWORD"] ?? string.Empty;
                options.From = configuration["SMTP_FROM"] ?? string.Empty;
                options.EnableSsl = !string.Equals(configuration["SMTP_ENABLE_SSL"], "false", StringComparison.OrdinalIgnoreCase);
            });
            services.AddTransient<IInvestimentoService, InvestimentoService>();
            services.AddTransient<IAutenticacaoService, AutenticacaoService>();
            services.AddTransient<INotificacaoService, NotificacaoEmailService>();
            services.AddScoped<PostgresDbContext>();
            services.AddScoped<IInvestimentoRepository, InvestimentoPostgresRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioPostgresRepository>();

            return services;
        }
    }
}
