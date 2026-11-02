namespace InvesteBrasil.Investimento.Repositories.Postgres.Queries
{
    internal abstract class InvestimentoPostgresQuery
    {
        internal static string GarantirEsquema => @"
            CREATE TABLE IF NOT EXISTS investimento (
                id BIGSERIAL PRIMARY KEY,
                proprietario VARCHAR(200) NOT NULL,
                data_criacao DATE NOT NULL,
                valor_inicial NUMERIC(18,2) NOT NULL CHECK (valor_inicial > 0),
                cpf VARCHAR(11) NULL,
                tipo_investimento VARCHAR(80) NULL,
                instituicao VARCHAR(120) NULL,
                liquidez VARCHAR(40) NULL,
                data_vencimento DATE NULL,
                objetivo VARCHAR(80) NULL,
                observacao VARCHAR(500) NULL,
                data_resgate DATE NULL,
                situacao VARCHAR(20) NOT NULL
            );

            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS cpf VARCHAR(11);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS tipo_investimento VARCHAR(80);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS instituicao VARCHAR(120);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS liquidez VARCHAR(40);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS data_vencimento DATE;
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS objetivo VARCHAR(80);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS observacao VARCHAR(500);

            CREATE INDEX IF NOT EXISTS ix_investimento_proprietario ON investimento (proprietario);
            CREATE INDEX IF NOT EXISTS ix_investimento_data_criacao ON investimento (data_criacao);
        ";

        internal static string Colunas =>
            "id, proprietario, data_criacao, valor_inicial, cpf, tipo_investimento, instituicao, liquidez, data_vencimento, objetivo, observacao, data_resgate, situacao";

        internal static string Incluir => @"
            INSERT INTO investimento (proprietario, data_criacao, valor_inicial, cpf, tipo_investimento, instituicao, liquidez, data_vencimento, objetivo, observacao, situacao)
            VALUES (@proprietario, @dataCriacao, @valorInicial, @cpf, @tipoInvestimento, @instituicao, @liquidez, @dataVencimento, @objetivo, @observacao, @situacao)
            RETURNING id;
        ";

        internal static string ConsultarPorId => $@"
            SELECT {Colunas}
              FROM investimento
             WHERE id = @id;
        ";

        internal static string Consultar => $@"
            SELECT {Colunas}
              FROM investimento
             WHERE (@proprietario IS NULL OR proprietario ILIKE '%' || @proprietario || '%')
               AND (@dataInicial IS NULL OR data_criacao >= @dataInicial)
               AND (@dataFinal IS NULL OR data_criacao <= @dataFinal)
             ORDER BY data_criacao DESC, id DESC
             LIMIT @tamanhoPagina OFFSET @offset;
        ";

        internal static string Contar => @"
            SELECT COUNT(1)
              FROM investimento
             WHERE (@proprietario IS NULL OR proprietario ILIKE '%' || @proprietario || '%')
               AND (@dataInicial IS NULL OR data_criacao >= @dataInicial)
               AND (@dataFinal IS NULL OR data_criacao <= @dataFinal);
        ";

        internal static string ConsultarAtivos => $@"
            SELECT {Colunas}
              FROM investimento
             WHERE situacao = @situacao;
        ";

        internal static string Resgatar => @"
            UPDATE investimento
               SET data_resgate = @dataResgate,
                   situacao = @situacao
             WHERE id = @id;
        ";
    }
}
