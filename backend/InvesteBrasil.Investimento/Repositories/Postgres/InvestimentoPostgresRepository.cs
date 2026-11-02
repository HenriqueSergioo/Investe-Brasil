using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Enums;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Repositories.Postgres.Queries;
using Npgsql;

namespace InvesteBrasil.Investimento.Repositories.Postgres
{
    public class InvestimentoPostgresRepository : IInvestimentoRepository
    {
        private readonly PostgresDbContext dbContext;

        public InvestimentoPostgresRepository(PostgresDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task GarantirEsquemaAsync()
        {
            await dbContext.AbrirAsync();
            await using var command = new NpgsqlCommand(InvestimentoPostgresQuery.GarantirEsquema, dbContext.Connection);
            await command.ExecuteNonQueryAsync();
        }

        public async Task<InvestimentoEntity> IncluirAsync(InvestimentoEntity entity)
        {
            await dbContext.AbrirAsync();
            await using var command = new NpgsqlCommand(InvestimentoPostgresQuery.Incluir, dbContext.Connection);
            command.Parameters.AddWithValue("proprietario", entity.Proprietario.Trim());
            command.Parameters.AddWithValue("dataCriacao", entity.DataCriacao);
            command.Parameters.AddWithValue("valorInicial", entity.ValorInicial);
            command.Parameters.AddWithValue("cpf", string.IsNullOrWhiteSpace(entity.Cpf) ? DBNull.Value : entity.Cpf);
            command.Parameters.AddWithValue("tipoInvestimento", (object?)entity.TipoInvestimento ?? DBNull.Value);
            command.Parameters.AddWithValue("instituicao", (object?)entity.Instituicao ?? DBNull.Value);
            command.Parameters.AddWithValue("liquidez", (object?)entity.Liquidez ?? DBNull.Value);
            command.Parameters.AddWithValue("dataVencimento", entity.DataVencimento.HasValue ? entity.DataVencimento.Value : DBNull.Value);
            command.Parameters.AddWithValue("objetivo", (object?)entity.Objetivo ?? DBNull.Value);
            command.Parameters.AddWithValue("observacao", (object?)entity.Observacao ?? DBNull.Value);
            command.Parameters.AddWithValue("situacao", entity.Situacao);
            entity.Id = (long)(await command.ExecuteScalarAsync() ?? 0);
            return entity;
        }

        public async Task<InvestimentoEntity?> ConsultarPorIdAsync(long id)
        {
            await dbContext.AbrirAsync();
            await using var command = new NpgsqlCommand(InvestimentoPostgresQuery.ConsultarPorId, dbContext.Connection);
            command.Parameters.AddWithValue("id", id);
            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return Mapear(reader);
        }

        public async Task<(IEnumerable<InvestimentoEntity> Itens, int Total)> ConsultarAsync(
            string? proprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina)
        {
            await dbContext.AbrirAsync();

            await using var countCommand = new NpgsqlCommand(InvestimentoPostgresQuery.Contar, dbContext.Connection);
            AdicionarFiltros(countCommand, proprietario, dataInicial, dataFinal);
            var total = Convert.ToInt32(await countCommand.ExecuteScalarAsync());

            await using var command = new NpgsqlCommand(InvestimentoPostgresQuery.Consultar, dbContext.Connection);
            AdicionarFiltros(command, proprietario, dataInicial, dataFinal);
            command.Parameters.AddWithValue("tamanhoPagina", tamanhoPagina);
            command.Parameters.AddWithValue("offset", (pagina - 1) * tamanhoPagina);

            var itens = new List<InvestimentoEntity>();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                itens.Add(Mapear(reader));

            return (itens, total);
        }

        public async Task<IEnumerable<InvestimentoEntity>> ConsultarAtivosAsync()
        {
            await dbContext.AbrirAsync();
            await using var command = new NpgsqlCommand(InvestimentoPostgresQuery.ConsultarAtivos, dbContext.Connection);
            command.Parameters.AddWithValue("situacao", SituacaoInvestimento.Investido);

            var itens = new List<InvestimentoEntity>();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                itens.Add(Mapear(reader));

            return itens;
        }

        public async Task ResgatarAsync(long id, DateOnly dataResgate)
        {
            await dbContext.AbrirAsync();
            await using var command = new NpgsqlCommand(InvestimentoPostgresQuery.Resgatar, dbContext.Connection);
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("dataResgate", dataResgate);
            command.Parameters.AddWithValue("situacao", SituacaoInvestimento.Resgatado);
            await command.ExecuteNonQueryAsync();
        }

        private static void AdicionarFiltros(NpgsqlCommand command, string? proprietario, DateOnly? dataInicial, DateOnly? dataFinal)
        {
            command.Parameters.AddWithValue("proprietario", string.IsNullOrWhiteSpace(proprietario) ? DBNull.Value : proprietario.Trim());
            command.Parameters.AddWithValue("dataInicial", dataInicial.HasValue ? dataInicial.Value : DBNull.Value);
            command.Parameters.AddWithValue("dataFinal", dataFinal.HasValue ? dataFinal.Value : DBNull.Value);
        }

        private static string LerTexto(NpgsqlDataReader reader, string campo)
        {
            var ordinal = reader.GetOrdinal(campo);
            return reader.IsDBNull(ordinal) ? string.Empty : reader.GetString(ordinal);
        }

        private static InvestimentoEntity Mapear(NpgsqlDataReader reader)
        {
            return new InvestimentoEntity
            {
                Id = reader.GetInt64(reader.GetOrdinal("id")),
                Proprietario = reader.GetString(reader.GetOrdinal("proprietario")),
                DataCriacao = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_criacao"))),
                ValorInicial = reader.GetDecimal(reader.GetOrdinal("valor_inicial")),
                Cpf = LerTexto(reader, "cpf"),
                TipoInvestimento = LerTexto(reader, "tipo_investimento"),
                Instituicao = LerTexto(reader, "instituicao"),
                Liquidez = LerTexto(reader, "liquidez"),
                DataVencimento = reader.IsDBNull(reader.GetOrdinal("data_vencimento"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_vencimento"))),
                Objetivo = LerTexto(reader, "objetivo"),
                Observacao = LerTexto(reader, "observacao"),
                DataResgate = reader.IsDBNull(reader.GetOrdinal("data_resgate"))
                    ? null
                    : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("data_resgate"))),
                Situacao = reader.GetString(reader.GetOrdinal("situacao"))
            };
        }
    }
}
