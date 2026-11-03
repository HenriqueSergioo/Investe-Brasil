using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Repositories.Postgres.Abstracts;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Repositories.Postgres.Queries;

namespace InvesteBrasil.Investimento.Repositories.Postgres
{
    public class UsuarioPostgresRepository : PostgresRepository, IUsuarioRepository
    {
        public UsuarioPostgresRepository(PostgresDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task GarantirEsquemaAsync()
        {
            await ExecuteNonQueryAsync(UsuarioPostgresQuery.CriarTabela);
            await ExecuteNonQueryAsync(UsuarioPostgresQuery.MigrarColunas);
        }

        public async Task<UsuarioEntity?> ConsultarPorUsuarioAsync(string usuario)
        {
            await using var dataReader = await ExecuteReaderAsync(
                UsuarioPostgresQuery.ConsultarPorUsuario,
                Parametro("descricaoUsuario", usuario.Trim()));

            if (!await dataReader.ReadAsync())
                return null;

            return Mapear(dataReader);
        }

        public async Task<UsuarioEntity?> ConsultarPorCodigoAsync(long codigoUsuario)
        {
            await using var dataReader = await ExecuteReaderAsync(
                UsuarioPostgresQuery.ConsultarPorCodigo,
                Parametro("codigoUsuario", codigoUsuario));

            if (!await dataReader.ReadAsync())
                return null;

            return Mapear(dataReader);
        }

        public async Task<UsuarioEntity> IncluirAsync(UsuarioEntity entity)
        {
            var codigoUsuario = await ExecuteScalarAsync(
                UsuarioPostgresQuery.Incluir,
                Parametro("nomeUsuario", entity.NomeUsuario.Trim()),
                Parametro("descricaoUsuario", entity.DescricaoUsuario.Trim()),
                Parametro("descricaoSenha", entity.DescricaoSenha),
                Parametro("descricaoEmail", entity.DescricaoEmail.Trim()));

            entity.CodigoUsuario = Convert.ToInt64(codigoUsuario ?? 0);
            return entity;
        }

        private static UsuarioEntity Mapear(Npgsql.NpgsqlDataReader reader)
        {
            var ordinalEmail = reader.GetOrdinal("DES_EMAIL");
            return new UsuarioEntity
            {
                CodigoUsuario = reader.GetInt64(reader.GetOrdinal("COD_USUARIO")),
                NomeUsuario = reader.GetString(reader.GetOrdinal("NOM_USUARIO")),
                DescricaoUsuario = reader.GetString(reader.GetOrdinal("DES_USUARIO")),
                DescricaoSenha = reader.GetString(reader.GetOrdinal("DES_SENHA")),
                DescricaoEmail = reader.IsDBNull(ordinalEmail) ? string.Empty : reader.GetString(ordinalEmail)
            };
        }
    }
}
