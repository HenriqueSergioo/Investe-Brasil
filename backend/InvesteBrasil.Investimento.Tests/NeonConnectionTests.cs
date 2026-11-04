using InvesteBrasil.Investimento.Repositories.Postgres;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace InvesteBrasil.Investimento.Tests
{
    public class NeonConnectionTests
    {
        [Fact]
        public void Obter_SemConfiguracao_DeveRetornarNulo()
        {
            Assert.Null(NeonConnection.Obter(new ConfiguracaoTeste()));
        }

        [Fact]
        public void Obter_UriPostgres_DeveConverterParaNpgsql()
        {
            var configuracao = new ConfiguracaoTeste(new Dictionary<string, string?>
            {
                ["DATABASE_URL"] = "postgresql://neondb_owner:segredo@ep-teste.c-15.us-east-1.aws.neon.tech/neondb?sslmode=require"
            });

            var conexao = NeonConnection.Obter(configuracao);

            Assert.NotNull(conexao);
            Assert.Contains("Host=ep-teste.c-15.us-east-1.aws.neon.tech", conexao);
            Assert.Contains("Database=neondb", conexao);
            Assert.Contains("Username=neondb_owner", conexao);
            Assert.Contains("Password=segredo", conexao);
            Assert.Contains("SSL Mode=Require", conexao);
            Assert.Contains("Pooling=true", conexao);
            Assert.DoesNotContain("postgresql://", conexao);
        }

        [Fact]
        public void Obter_ConnectionStringNeon_DeveTerPrioridade()
        {
            var configuracao = new ConfiguracaoTeste(new Dictionary<string, string?>
            {
                ["ConnectionStrings:Neon"] = "Host=localhost;Database=investe;",
                ["DATABASE_URL"] = "postgresql://outro:x@host/db"
            });

            Assert.Equal("Host=localhost;Database=investe;", NeonConnection.Obter(configuracao));
        }

        private sealed class ConfiguracaoTeste : IConfiguration
        {
            private readonly Dictionary<string, string?> valores;

            public ConfiguracaoTeste(Dictionary<string, string?>? valores = null)
            {
                this.valores = valores ?? new Dictionary<string, string?>();
            }

            public string? this[string key]
            {
                get => valores.GetValueOrDefault(key);
                set => valores[key] = value;
            }

            public IEnumerable<IConfigurationSection> GetChildren() => [];

            public IChangeToken GetReloadToken() => new CancellationChangeToken(CancellationToken.None);

            public IConfigurationSection GetSection(string key) => new SecaoTeste(key, this);
        }

        private sealed class SecaoTeste : IConfigurationSection
        {
            private readonly ConfiguracaoTeste raiz;

            public SecaoTeste(string key, ConfiguracaoTeste raiz)
            {
                this.raiz = raiz;
                Key = key;
                Path = key;
            }

            public string Key { get; }

            public string Path { get; }

            public string? Value
            {
                get => raiz[Path];
                set => raiz[Path] = value;
            }

            public string? this[string key]
            {
                get => raiz[$"{Path}:{key}"];
                set => raiz[$"{Path}:{key}"] = value;
            }

            public IEnumerable<IConfigurationSection> GetChildren() => [];

            public IChangeToken GetReloadToken() => new CancellationChangeToken(CancellationToken.None);

            public IConfigurationSection GetSection(string key) => new SecaoTeste($"{Path}:{key}", raiz);
        }
    }
}
