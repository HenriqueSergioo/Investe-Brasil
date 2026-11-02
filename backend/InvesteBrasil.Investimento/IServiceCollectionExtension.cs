using InvesteBrasil.Investimento.Repositories.Memoria;
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
            services.AddTransient<IInvestimentoService, InvestimentoService>();
            services.AddTransient<IAutenticacaoService, AutenticacaoService>();
            services.AddTransient<INotificacaoService, NotificacaoLogService>();
            services.AddHttpClient("Mercado", client =>
            {
                client.Timeout = TimeSpan.FromSeconds(8);
                client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "InvesteBrasil/1.0");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            });
            services.AddScoped<IMercadoService, MercadoService>();

            var connectionString = configuration.GetConnectionString("Neon");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                services.AddSingleton<IInvestimentoRepository, InvestimentoMemoriaRepository>();
            }
            else
            {
                services.AddScoped<PostgresDbContext>();
                services.AddScoped<IInvestimentoRepository, InvestimentoPostgresRepository>();
            }

            return services;
        }
    }
}
