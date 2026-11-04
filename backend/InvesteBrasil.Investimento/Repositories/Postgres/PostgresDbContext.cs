using Npgsql;

namespace InvesteBrasil.Investimento.Repositories.Postgres
{
    public class PostgresDbContext : IDisposable
    {
        public NpgsqlConnection Connection { get; }

        public PostgresDbContext(NpgsqlDataSource dataSource)
        {
            Connection = dataSource.CreateConnection();
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
