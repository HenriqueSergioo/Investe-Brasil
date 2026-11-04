namespace InvesteBrasil.Investimento.Repositories.Postgres.Queries
{
    internal sealed class InvestimentoPostgresQuery
    {
        internal static string Colunas =>
            @"""COD_INVESTIMENTO"", ""COD_USUARIO"", ""NOM_PROPRIETARIO"", ""DAT_CRIACAO"", ""VLR_INICIAL"", ""DAT_VENCIMENTO"", ""DES_OBJETIVO"", ""DES_OBSERVACAO"", ""DAT_RESGATE"", ""STA_INVESTIMENTO""";

        internal static string Incluir => @"
            INSERT INTO investimento (""COD_USUARIO"", ""NOM_PROPRIETARIO"", ""DAT_CRIACAO"", ""VLR_INICIAL"", ""DAT_VENCIMENTO"", ""DES_OBJETIVO"", ""DES_OBSERVACAO"", ""STA_INVESTIMENTO"")
            VALUES (@codigoUsuario, @nomeProprietario, @dataCriacao, @valorInicial, @dataVencimento, @descricaoObjetivo, @descricaoObservacao, @situacaoInvestimento)
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
             ORDER BY ""DAT_CRIACAO"" DESC, ""COD_INVESTIMENTO"" DESC
             LIMIT @tamanhoPagina OFFSET @offset;
        ";

        internal static string Contar => @"
            SELECT COUNT(1)
              FROM investimento
             WHERE ""COD_USUARIO"" = @codigoUsuario;
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
