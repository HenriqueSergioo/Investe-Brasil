using InvesteBrasil.Infra.Services.Validators.Abstracts;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities.Requests;

namespace InvesteBrasil.Investimento.Services.Validators
{
    public class AutenticacaoValidador : Validator
    {
        public IResultadoValidacao Entrar(EntrarRequestEntity request)
        {
            if (request == null)
                return Erro("Os dados de acesso são obrigatórios.");

            if (string.IsNullOrWhiteSpace(request.Usuario))
                return Erro("Informe o e-mail.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                return Erro("Informe a senha.");

            return Sucesso();
        }

        public IResultadoValidacao CadastrarUsuario(CadastrarUsuarioRequestEntity request)
        {
            if (request == null)
                return Erro("Os dados de cadastro são obrigatórios.");

            if (string.IsNullOrWhiteSpace(request.Nome))
                return Erro("Informe o nome.");

            if (request.Nome.Trim().Length > 200)
                return Erro("O nome deve ter no máximo 200 caracteres.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                return Erro("Informe a senha.");

            if (request.Senha.Length < 6)
                return Erro("A senha deve ter pelo menos 6 caracteres.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return Erro("Informe o e-mail.");

            if (!EmailValido(request.Email))
                return Erro("Informe um e-mail válido.");

            return Sucesso();
        }

        private static bool EmailValido(string email)
        {
            try
            {
                var endereco = new System.Net.Mail.MailAddress(email.Trim());
                return endereco.Address.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
