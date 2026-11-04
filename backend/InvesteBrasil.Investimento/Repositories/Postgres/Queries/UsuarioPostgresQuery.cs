namespace InvesteBrasil.Investimento.Repositories.Postgres.Queries
{
    internal sealed class UsuarioPostgresQuery
    {
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
