namespace InvesteBrasil.Investimento.Repositories.Postgres.Queries
{
    internal sealed class InvestimentoPostgresQuery
    {
        internal static string CriarTabela => @"
            CREATE TABLE IF NOT EXISTS investimento (
                ""COD_INVESTIMENTO"" BIGSERIAL PRIMARY KEY,
                ""COD_USUARIO"" BIGINT NOT NULL,
                ""NOM_PROPRIETARIO"" VARCHAR(200) NOT NULL,
                ""DAT_CRIACAO"" DATE NOT NULL,
                ""VLR_INICIAL"" NUMERIC(18,2) NOT NULL CHECK (""VLR_INICIAL"" > 0),
                ""NUM_CPF"" VARCHAR(11) NULL,
                ""DES_TIPO_INVESTIMENTO"" VARCHAR(80) NULL,
                ""NOM_INSTITUICAO"" VARCHAR(120) NULL,
                ""DES_LIQUIDEZ"" VARCHAR(40) NULL,
                ""DAT_VENCIMENTO"" DATE NULL,
                ""DES_OBJETIVO"" VARCHAR(80) NULL,
                ""DES_OBSERVACAO"" VARCHAR(500) NULL,
                ""DAT_RESGATE"" DATE NULL,
                ""STA_INVESTIMENTO"" VARCHAR(20) NOT NULL
            );
        ";

        internal static string MigrarColunas => @"
            DO $$
            BEGIN
                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'id')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'COD_INVESTIMENTO') THEN
                    ALTER TABLE investimento RENAME COLUMN id TO ""COD_INVESTIMENTO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'usuario_id')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'COD_USUARIO') THEN
                    ALTER TABLE investimento RENAME COLUMN usuario_id TO ""COD_USUARIO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'proprietario')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'NOM_PROPRIETARIO') THEN
                    ALTER TABLE investimento RENAME COLUMN proprietario TO ""NOM_PROPRIETARIO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'data_criacao')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DAT_CRIACAO') THEN
                    ALTER TABLE investimento RENAME COLUMN data_criacao TO ""DAT_CRIACAO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'valor_inicial')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'VLR_INICIAL') THEN
                    ALTER TABLE investimento RENAME COLUMN valor_inicial TO ""VLR_INICIAL"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'cpf')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'NUM_CPF') THEN
                    ALTER TABLE investimento RENAME COLUMN cpf TO ""NUM_CPF"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'tipo_investimento')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DES_TIPO_INVESTIMENTO') THEN
                    ALTER TABLE investimento RENAME COLUMN tipo_investimento TO ""DES_TIPO_INVESTIMENTO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'instituicao')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'NOM_INSTITUICAO') THEN
                    ALTER TABLE investimento RENAME COLUMN instituicao TO ""NOM_INSTITUICAO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'liquidez')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DES_LIQUIDEZ') THEN
                    ALTER TABLE investimento RENAME COLUMN liquidez TO ""DES_LIQUIDEZ"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'data_vencimento')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DAT_VENCIMENTO') THEN
                    ALTER TABLE investimento RENAME COLUMN data_vencimento TO ""DAT_VENCIMENTO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'objetivo')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DES_OBJETIVO') THEN
                    ALTER TABLE investimento RENAME COLUMN objetivo TO ""DES_OBJETIVO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'observacao')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DES_OBSERVACAO') THEN
                    ALTER TABLE investimento RENAME COLUMN observacao TO ""DES_OBSERVACAO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'data_resgate')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'DAT_RESGATE') THEN
                    ALTER TABLE investimento RENAME COLUMN data_resgate TO ""DAT_RESGATE"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'situacao')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'investimento' AND column_name = 'STA_INVESTIMENTO') THEN
                    ALTER TABLE investimento RENAME COLUMN situacao TO ""STA_INVESTIMENTO"";
                END IF;
            END $$;
        ";

        internal static string CriarColunas => @"
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""COD_USUARIO"" BIGINT;
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""NOM_PROPRIETARIO"" VARCHAR(200);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DAT_CRIACAO"" DATE;
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""VLR_INICIAL"" NUMERIC(18,2);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""NUM_CPF"" VARCHAR(11);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DES_TIPO_INVESTIMENTO"" VARCHAR(80);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""NOM_INSTITUICAO"" VARCHAR(120);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DES_LIQUIDEZ"" VARCHAR(40);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DAT_VENCIMENTO"" DATE;
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DES_OBJETIVO"" VARCHAR(80);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DES_OBSERVACAO"" VARCHAR(500);
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""DAT_RESGATE"" DATE;
            ALTER TABLE investimento ADD COLUMN IF NOT EXISTS ""STA_INVESTIMENTO"" VARCHAR(20);
        ";

        internal static string RemoverCompartilhados =>
            @"DELETE FROM investimento WHERE ""COD_USUARIO"" IS NULL;";

        internal static string CriarIndices => @"
            DROP INDEX IF EXISTS ix_investimento_usuario_id;
            DROP INDEX IF EXISTS ix_investimento_proprietario;
            DROP INDEX IF EXISTS ix_investimento_data_criacao;
            CREATE INDEX IF NOT EXISTS ix_investimento_cod_usuario ON investimento (""COD_USUARIO"");
            CREATE INDEX IF NOT EXISTS ix_investimento_nom_proprietario ON investimento (""NOM_PROPRIETARIO"");
            CREATE INDEX IF NOT EXISTS ix_investimento_dat_criacao ON investimento (""DAT_CRIACAO"");
        ";

        internal static string Colunas =>
            @"""COD_INVESTIMENTO"", ""COD_USUARIO"", ""NOM_PROPRIETARIO"", ""DAT_CRIACAO"", ""VLR_INICIAL"", ""NUM_CPF"", ""DES_TIPO_INVESTIMENTO"", ""NOM_INSTITUICAO"", ""DES_LIQUIDEZ"", ""DAT_VENCIMENTO"", ""DES_OBJETIVO"", ""DES_OBSERVACAO"", ""DAT_RESGATE"", ""STA_INVESTIMENTO""";

        internal static string Incluir => @"
            INSERT INTO investimento (""COD_USUARIO"", ""NOM_PROPRIETARIO"", ""DAT_CRIACAO"", ""VLR_INICIAL"", ""NUM_CPF"", ""DES_TIPO_INVESTIMENTO"", ""NOM_INSTITUICAO"", ""DES_LIQUIDEZ"", ""DAT_VENCIMENTO"", ""DES_OBJETIVO"", ""DES_OBSERVACAO"", ""STA_INVESTIMENTO"")
            VALUES (@codigoUsuario, @nomeProprietario, @dataCriacao, @valorInicial, @numeroCpf, @descricaoTipoInvestimento, @nomeInstituicao, @descricaoLiquidez, @dataVencimento, @descricaoObjetivo, @descricaoObservacao, @situacaoInvestimento)
            RETURNING ""COD_INVESTIMENTO"";
        ";

        internal static string ConsultarPorId => $@"
            SELECT {Colunas}
              FROM investimento
             WHERE ""COD_INVESTIMENTO"" = @codigoInvestimento
               AND ""COD_USUARIO"" = @codigoUsuario;
        ";

        internal static string Consultar => $@"
            SELECT {Colunas}
              FROM investimento
             WHERE ""COD_USUARIO"" = @codigoUsuario
               AND (@nomeProprietario IS NULL OR ""NOM_PROPRIETARIO"" ILIKE '%' || @nomeProprietario || '%')
               AND (@dataInicial IS NULL OR ""DAT_CRIACAO"" >= @dataInicial)
               AND (@dataFinal IS NULL OR ""DAT_CRIACAO"" <= @dataFinal)
             ORDER BY ""DAT_CRIACAO"" DESC, ""COD_INVESTIMENTO"" DESC
             LIMIT @tamanhoPagina OFFSET @offset;
        ";

        internal static string Contar => @"
            SELECT COUNT(1)
              FROM investimento
             WHERE ""COD_USUARIO"" = @codigoUsuario
               AND (@nomeProprietario IS NULL OR ""NOM_PROPRIETARIO"" ILIKE '%' || @nomeProprietario || '%')
               AND (@dataInicial IS NULL OR ""DAT_CRIACAO"" >= @dataInicial)
               AND (@dataFinal IS NULL OR ""DAT_CRIACAO"" <= @dataFinal);
        ";

        internal static string ConsultarAtivos => $@"
            SELECT {Colunas}
              FROM investimento
             WHERE ""COD_USUARIO"" = @codigoUsuario
               AND ""STA_INVESTIMENTO"" = @situacaoInvestimento;
        ";

        internal static string Resgatar => @"
            UPDATE investimento
               SET ""DAT_RESGATE"" = @dataResgate,
                   ""STA_INVESTIMENTO"" = @situacaoInvestimento
             WHERE ""COD_INVESTIMENTO"" = @codigoInvestimento
               AND ""COD_USUARIO"" = @codigoUsuario;
        ";
    }
}
