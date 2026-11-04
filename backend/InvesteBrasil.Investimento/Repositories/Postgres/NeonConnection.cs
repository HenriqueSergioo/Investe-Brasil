using Microsoft.Extensions.Configuration;

namespace InvesteBrasil.Investimento.Repositories.Postgres
{
    public static class NeonConnection
    {
        public static string? Obter(IConfiguration configuration)
        {
            var valor = configuration.GetConnectionString("Neon");
            if (string.IsNullOrWhiteSpace(valor))
                valor = configuration["DATABASE_URL"] ?? Environment.GetEnvironmentVariable("DATABASE_URL");

            if (string.IsNullOrWhiteSpace(valor))
                return null;

            valor = valor.Trim().Trim('"').Trim('\'');
            if (valor.StartsWith("postgres://", StringComparison.OrdinalIgnoreCase)
                || valor.StartsWith("postgresql://", StringComparison.OrdinalIgnoreCase))
                return ConverterUri(valor);

            return valor;
        }

        private static string ConverterUri(string uri)
        {
            var parsed = new Uri(uri);
            var partes = parsed.UserInfo.Split(':', 2);
            var usuario = Uri.UnescapeDataString(partes[0]);
            var senha = partes.Length > 1 ? Uri.UnescapeDataString(partes[1]) : string.Empty;
            var banco = parsed.AbsolutePath.Trim('/');
            var porta = parsed.IsDefaultPort ? 5432 : parsed.Port;

            return $"Host={parsed.Host};Port={porta};Database={banco};Username={usuario};Password={senha};SSL Mode=Require;Trust Server Certificate=true;Pooling=true;Minimum Pool Size=0;Maximum Pool Size=10;Timeout=15;Command Timeout=30;No Reset On Close=true";
        }
    }
}
