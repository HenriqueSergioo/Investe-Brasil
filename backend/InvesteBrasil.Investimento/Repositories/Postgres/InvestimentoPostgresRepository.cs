using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Enums;
using InvesteBrasil.Investimento.Repositories.Postgres.Abstracts;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Repositories.Postgres.Queries;
using NpgsqlTypes;

namespace InvesteBrasil.Investimento.Repositories.Postgres
{
    public class InvestimentoPostgresRepository : PostgresRepository, IInvestimentoRepository
    {
        public InvestimentoPostgresRepository(PostgresDbContext dbContext)
            : base(dbContext)
        {
        }

        public async Task<InvestimentoEntity> IncluirAsync(InvestimentoEntity entity)
        {
            var codigoInvestimento = await ExecuteScalarAsync(
                InvestimentoPostgresQuery.Incluir,
                Parametro("codigoUsuario", entity.CodigoUsuario),
                Parametro("nomeProprietario", entity.NomeProprietario.Trim()),
                Parametro("dataCriacao", NpgsqlDbType.Date, entity.DataCriacao),
                Parametro("valorInicial", entity.ValorInicial),
                Parametro("dataVencimento", NpgsqlDbType.Date, entity.DataVencimento.HasValue ? entity.DataVencimento.Value : null),
                Parametro("descricaoObjetivo", entity.DescricaoObjetivo),
                Parametro("descricaoObservacao", entity.DescricaoObservacao),
                Parametro("situacaoInvestimento", entity.SituacaoInvestimento));

            entity.CodigoInvestimento = Convert.ToInt64(codigoInvestimento ?? 0);
            return entity;
        }

        public async Task<InvestimentoEntity?> ConsultarPorIdAsync(long codigoInvestimento, long codigoUsuario)
        {
            await using var dataReader = await ExecuteReaderAsync(
                InvestimentoPostgresQuery.ConsultarPorId,
                Parametro("codigoInvestimento", codigoInvestimento),
                Parametro("codigoUsuario", codigoUsuario));

            if (!await dataReader.ReadAsync())
                return null;

            return Mapear(dataReader);
        }

        public async Task<(IEnumerable<InvestimentoEntity> Itens, int Total)> ConsultarAsync(
            long codigoUsuario,
            int pagina,
            int tamanhoPagina)
        {
            var total = Convert.ToInt32(await ExecuteScalarAsync(
                InvestimentoPostgresQuery.Contar,
                Parametro("codigoUsuario", codigoUsuario)) ?? 0);

            var itens = new List<InvestimentoEntity>();
            await using var dataReader = await ExecuteReaderAsync(
                InvestimentoPostgresQuery.Consultar,
                Parametro("codigoUsuario", codigoUsuario),
                Parametro("tamanhoPagina", tamanhoPagina),
                Parametro("offset", (pagina - 1) * tamanhoPagina));

            while (await dataReader.ReadAsync())
                itens.Add(Mapear(dataReader));

            return (itens, total);
        }

        public async Task<IEnumerable<InvestimentoEntity>> ConsultarAtivosAsync(long codigoUsuario)
        {
            var itens = new List<InvestimentoEntity>();
            await using var dataReader = await ExecuteReaderAsync(
                InvestimentoPostgresQuery.ConsultarAtivos,
                Parametro("codigoUsuario", codigoUsuario),
                Parametro("situacaoInvestimento", SituacaoInvestimento.Investido));

            while (await dataReader.ReadAsync())
                itens.Add(Mapear(dataReader));

            return itens;
        }

        public async Task ResgatarAsync(long codigoInvestimento, long codigoUsuario, DateOnly dataResgate)
        {
            await ExecuteNonQueryAsync(
                InvestimentoPostgresQuery.Resgatar,
                Parametro("codigoInvestimento", codigoInvestimento),
                Parametro("codigoUsuario", codigoUsuario),
                Parametro("dataResgate", NpgsqlDbType.Date, dataResgate),
                Parametro("situacaoInvestimento", SituacaoInvestimento.Resgatado));
        }

        private static string LerTexto(Npgsql.NpgsqlDataReader reader, string campo)
        {
            var ordinal = reader.GetOrdinal(campo);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static InvestimentoEntity Mapear(Npgsql.NpgsqlDataReader reader)
        {
            return new InvestimentoEntity
            {
                CodigoInvestimento = reader.GetInt64(reader.GetOrdinal("COD_INVESTIMENTO")),
                CodigoUsuario = reader.GetInt64(reader.GetOrdinal("COD_USUARIO")),
                NomeProprietario = reader.GetString(reader.GetOrdinal("NOM_PROPRIETARIO")),
                DataCriacao = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DAT_CRIACAO"))),
                ValorInicial = reader.GetDecimal(reader.GetOrdinal("VLR_INICIAL")),
                DataVencimento = reader.IsDBNull(reader.GetOrdinal("DAT_VENCIMENTO"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DAT_VENCIMENTO"))),
                DescricaoObjetivo = LerTexto(reader, "DES_OBJETIVO"),
                DescricaoObservacao = LerTexto(reader, "DES_OBSERVACAO"),
                DataResgate = reader.IsDBNull(reader.GetOrdinal("DAT_RESGATE"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DAT_RESGATE"))),
                SituacaoInvestimento = reader.GetString(reader.GetOrdinal("STA_INVESTIMENTO"))
            };
        }
    }
}
