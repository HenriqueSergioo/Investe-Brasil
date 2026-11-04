using InvesteBrasil.Investimento.Security;

namespace InvesteBrasil.Investimento.Tests
{
    public class HashSenhaTests
    {
        [Fact]
        public void Gerar_MesmaSenha_DeveGerarOMesmoHash()
        {
            var primeiro = HashSenha.Gerar("123456");
            var segundo = HashSenha.Gerar("123456");
            Assert.Equal(primeiro, segundo);
        }

        [Fact]
        public void Gerar_SenhasDiferentes_DevemGerarHashesDiferentes()
        {
            var senha = HashSenha.Gerar("123456");
            var outra = HashSenha.Gerar("admin123");
            Assert.NotEqual(senha, outra);
        }

        [Fact]
        public void Gerar_NaoDeveDevolverASenhaEmTexto()
        {
            var hash = HashSenha.Gerar("123456");
            Assert.NotEqual("123456", hash);
            Assert.Equal(64, hash.Length);
        }
    }
}
