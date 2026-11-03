using Npgsql;
using NpgsqlTypes;

namespace InvesteBrasil.Investimento.Repositories.Postgres.Abstracts
{
    public abstract class PostgresRepository
    {
        protected PostgresDbContext DbContext { get; }

        protected PostgresRepository(PostgresDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected static NpgsqlParameter Parametro(string nome, object? valor)
        {
            return new NpgsqlParameter(nome, valor ?? DBNull.Value);
        }

        protected static NpgsqlParameter Parametro(string nome, NpgsqlDbType tipo, object? valor)
        {
            return new NpgsqlParameter(nome, tipo) { Value = valor ?? DBNull.Value };
        }

        protected async Task<NpgsqlDataReader> ExecuteReaderAsync(string commandText, params NpgsqlParameter[] parameters)
        {
            var comando = await CriarComandoAsync(commandText, parameters);
            return await comando.ExecuteReaderAsync();
        }

        protected async Task<object?> ExecuteScalarAsync(string commandText, params NpgsqlParameter[] parameters)
        {
            await using var comando = await CriarComandoAsync(commandText, parameters);
            return await comando.ExecuteScalarAsync();
        }

        protected async Task<int> ExecuteNonQueryAsync(string commandText, params NpgsqlParameter[] parameters)
        {
            await using var comando = await CriarComandoAsync(commandText, parameters);
            return await comando.ExecuteNonQueryAsync();
        }

        private async Task<NpgsqlCommand> CriarComandoAsync(string commandText, NpgsqlParameter[] parameters)
        {
            await DbContext.AbrirAsync();
            var comando = new NpgsqlCommand(commandText, DbContext.Connection);
            if (parameters is { Length: > 0 })
            {
                foreach (var parametro in parameters)
                {
                    if (parametro.Value == null)
                        parametro.Value = DBNull.Value;

                    comando.Parameters.Add(parametro);
                }
            }

            return comando;
        }
    }
}
