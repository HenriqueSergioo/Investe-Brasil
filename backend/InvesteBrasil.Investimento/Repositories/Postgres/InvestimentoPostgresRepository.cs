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

        public async Task GarantirEsquemaAsync()
        {
            await ExecuteNonQueryAsync(InvestimentoPostgresQuery.CriarTabela);
            await ExecuteNonQueryAsync(InvestimentoPostgresQuery.MigrarColunas);
            await ExecuteNonQueryAsync(InvestimentoPostgresQuery.CriarColunas);
            await ExecuteNonQueryAsync(InvestimentoPostgresQuery.RemoverCompartilhados);
            await ExecuteNonQueryAsync(InvestimentoPostgresQuery.CriarIndices);
        }

        public async Task<InvestimentoEntity> IncluirAsync(InvestimentoEntity entity)
        {
            var codigoInvestimento = await ExecuteScalarAsync(
                InvestimentoPostgresQuery.Incluir,
                Parametro("codigoUsuario", entity.CodigoUsuario),
                Parametro("nomeProprietario", entity.NomeProprietario.Trim()),
                Parametro("dataCriacao", NpgsqlDbType.Date, entity.DataCriacao),
                Parametro("valorInicial", entity.ValorInicial),
                Parametro("numeroCpf", string.IsNullOrWhiteSpace(entity.NumeroCpf) ? null : entity.NumeroCpf),
                Parametro("descricaoTipoInvestimento", entity.DescricaoTipoInvestimento),
                Parametro("nomeInstituicao", entity.NomeInstituicao),
                Parametro("descricaoLiquidez", entity.DescricaoLiquidez),
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
            string? nomeProprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina)
        {
            var total = Convert.ToInt32(await ExecuteScalarAsync(
                InvestimentoPostgresQuery.Contar,
                Filtros(codigoUsuario, nomeProprietario, dataInicial, dataFinal)) ?? 0);

            var parametrosConsulta = Filtros(codigoUsuario, nomeProprietario, dataInicial, dataFinal)
                .Concat(
                [
                    Parametro("tamanhoPagina", tamanhoPagina),
                    Parametro("offset", (pagina - 1) * tamanhoPagina)
                ])
                .ToArray();

            var itens = new List<InvestimentoEntity>();
            await using var dataReader = await ExecuteReaderAsync(InvestimentoPostgresQuery.Consultar, parametrosConsulta);
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

        private Npgsql.NpgsqlParameter[] Filtros(long codigoUsuario, string? nomeProprietario, DateOnly? dataInicial, DateOnly? dataFinal)
        {
            return
            [
                Parametro("codigoUsuario", codigoUsuario),
                Parametro("nomeProprietario", NpgsqlDbType.Varchar, string.IsNullOrWhiteSpace(nomeProprietario) ? null : nomeProprietario.Trim()),
                Parametro("dataInicial", NpgsqlDbType.Date, dataInicial.HasValue ? dataInicial.Value : null),
                Parametro("dataFinal", NpgsqlDbType.Date, dataFinal.HasValue ? dataFinal.Value : null)
            ];
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
                NumeroCpf = LerTexto(reader, "NUM_CPF"),
                DescricaoTipoInvestimento = LerTexto(reader, "DES_TIPO_INVESTIMENTO"),
                NomeInstituicao = LerTexto(reader, "NOM_INSTITUICAO"),
                DescricaoLiquidez = LerTexto(reader, "DES_LIQUIDEZ"),
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
