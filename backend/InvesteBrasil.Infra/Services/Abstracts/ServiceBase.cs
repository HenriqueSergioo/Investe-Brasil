using InvesteBrasil.Infra.Services.Interfaces;
using InvesteBrasil.Infra.Services.Validators.Interfaces;

namespace InvesteBrasil.Infra.Services.Abstracts
{
    public abstract class ServiceBase<TValidator> : IService<TValidator>
        where TValidator : IValidator, new()
    {
        public virtual TValidator Validator => new TValidator();
    }
}
