namespace InvesteBrasil.Investimento.Repositories.Postgres.Queries
{
    internal sealed class UsuarioPostgresQuery
    {
        internal static string CriarTabela => @"
            CREATE TABLE IF NOT EXISTS usuario (
                ""COD_USUARIO"" BIGSERIAL PRIMARY KEY,
                ""NOM_USUARIO"" VARCHAR(200) NOT NULL,
                ""DES_USUARIO"" VARCHAR(200) NOT NULL UNIQUE,
                ""DES_SENHA"" VARCHAR(128) NOT NULL,
                ""DES_EMAIL"" VARCHAR(200) NULL
            );
        ";

        internal static string MigrarColunas => @"
            DO $$
            BEGIN
                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'id')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'COD_USUARIO') THEN
                    ALTER TABLE usuario RENAME COLUMN id TO ""COD_USUARIO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'nome')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'NOM_USUARIO') THEN
                    ALTER TABLE usuario RENAME COLUMN nome TO ""NOM_USUARIO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'usuario')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'DES_USUARIO') THEN
                    ALTER TABLE usuario RENAME COLUMN usuario TO ""DES_USUARIO"";
                END IF;

                IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'senha')
                   AND NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_schema = 'public' AND table_name = 'usuario' AND column_name = 'DES_SENHA') THEN
                    ALTER TABLE usuario RENAME COLUMN senha TO ""DES_SENHA"";
                END IF;
            END $$;

            ALTER TABLE usuario ADD COLUMN IF NOT EXISTS ""DES_EMAIL"" VARCHAR(200);
            ALTER TABLE usuario ALTER COLUMN ""DES_USUARIO"" TYPE VARCHAR(200);
        ";

        internal static string Colunas =>
            @"""COD_USUARIO"", ""NOM_USUARIO"", ""DES_USUARIO"", ""DES_SENHA"", ""DES_EMAIL""";

        internal static string Incluir => @"
            INSERT INTO usuario (""NOM_USUARIO"", ""DES_USUARIO"", ""DES_SENHA"", ""DES_EMAIL"")
            VALUES (@nomeUsuario, @descricaoUsuario, @descricaoSenha, @descricaoEmail)
            RETURNING ""COD_USUARIO"";
        ";

        internal static string ConsultarPorUsuario => $@"
            SELECT {Colunas}
              FROM usuario
             WHERE LOWER(""DES_USUARIO"") = LOWER(@descricaoUsuario);
        ";

        internal static string ConsultarPorCodigo => $@"
            SELECT {Colunas}
              FROM usuario
             WHERE ""COD_USUARIO"" = @codigoUsuario;
        ";

        internal static string ConsultarPorEmail => $@"
            SELECT {Colunas}
              FROM usuario
             WHERE LOWER(""DES_EMAIL"") = LOWER(@descricaoEmail);
        ";
    }
}
