using InvesteBrasil.Infra.Services.Validators.Interfaces;

namespace InvesteBrasil.Infra.Services.Interfaces
{
    public interface IService<TValidator> where TValidator : IValidator
    {
        TValidator Validator { get; }
    }
}
