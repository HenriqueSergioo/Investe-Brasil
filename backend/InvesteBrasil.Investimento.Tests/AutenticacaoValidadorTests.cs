using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Services.Validators;

namespace InvesteBrasil.Investimento.Tests
{
    public class AutenticacaoValidadorTests
    {
        private readonly AutenticacaoValidador validador = new();

        [Fact]
        public void Entrar_SemUsuario_DeveFalhar()
        {
            var request = new EntrarRequestEntity { Usuario = "", Senha = "123456" };
            Assert.True(validador.Entrar(request).OcorreuErro);
        }

        [Fact]
        public void Entrar_SemSenha_DeveFalhar()
        {
            var request = new EntrarRequestEntity { Usuario = "henrique", Senha = "" };
            Assert.True(validador.Entrar(request).OcorreuErro);
        }

        [Fact]
        public void Entrar_DadosPreenchidos_DevePassar()
        {
            var request = new EntrarRequestEntity { Usuario = "henrique", Senha = "123456" };
            Assert.False(validador.Entrar(request).OcorreuErro);
        }

        [Fact]
        public void CadastrarUsuario_SenhaCurta_DeveFalhar()
        {
            var request = new CadastrarUsuarioRequestEntity
            {
                Nome = "Ana",
                Usuario = "ana",
                Email = "ana@email.com",
                Senha = "123"
            };

            Assert.True(validador.CadastrarUsuario(request).OcorreuErro);
        }

        [Fact]
        public void CadastrarUsuario_DadosValidos_DevePassar()
        {
            var request = new CadastrarUsuarioRequestEntity
            {
                Nome = "Ana Souza",
                Usuario = "ana",
                Email = "ana@email.com",
                Senha = "senha12"
            };

            Assert.False(validador.CadastrarUsuario(request).OcorreuErro);
        }

        [Fact]
        public void CadastrarUsuario_EmailInvalido_DeveFalhar()
        {
            var request = new CadastrarUsuarioRequestEntity
            {
                Nome = "Ana Souza",
                Usuario = "ana",
                Email = "ana-email",
                Senha = "senha12"
            };

            Assert.True(validador.CadastrarUsuario(request).OcorreuErro);
        }
    }
}
