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
                return Erro("Informe o usuário.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                return Erro("Informe a senha.");

            return Sucesso();
        }
    }
}
