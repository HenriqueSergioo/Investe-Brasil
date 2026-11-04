using InvesteBrasil.Investimento.Repositories.Postgres;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Services;
using InvesteBrasil.Investimento.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace InvesteBrasil.Investimento
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection ConfigureServicesInvestimento(this IServiceCollection services, IConfiguration configuration)
        {
            var conexao = NeonConnection.Obter(configuration);
            if (string.IsNullOrWhiteSpace(conexao))
                throw new InvalidOperationException("Configure DATABASE_URL no arquivo .env para conectar ao Neon.");

            services.AddSingleton(_ => new NpgsqlDataSourceBuilder(conexao).Build());
            services.AddTransient<IInvestimentoService, InvestimentoService>();
            services.AddTransient<IAutenticacaoService, AutenticacaoService>();
            services.AddScoped<PostgresDbContext>();
            services.AddScoped<IInvestimentoRepository, InvestimentoPostgresRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioPostgresRepository>();

            return services;
        }
    }
}
