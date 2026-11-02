using Microsoft.Extensions.Configuration;
using Npgsql;

namespace InvesteBrasil.Investimento.Repositories.Postgres
{
    public class PostgresDbContext : IDisposable
    {
        public NpgsqlConnection Connection { get; }

        public PostgresDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Neon")
                ?? throw new InvalidOperationException("Connection string 'Neon' não configurada.");

            Connection = new NpgsqlConnection(connectionString);
        }

        public async Task AbrirAsync()
        {
            if (Connection.State != System.Data.ConnectionState.Open)
                await Connection.OpenAsync();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}
