using InvesteBrasil.Infra.Services.Validators.Interfaces;
using InvesteBrasil.Infra.Shared;
using InvesteBrasil.Infra.Shared.Interfaces;

namespace InvesteBrasil.Infra.Services.Validators.Abstracts
{
    public abstract class Validator : IValidator
    {
        protected IResultadoValidacao Sucesso() => new ResultadoValidacao();

        protected IResultadoValidacao Erro(string mensagem) => new ResultadoValidacao(mensagem);
    }
}
