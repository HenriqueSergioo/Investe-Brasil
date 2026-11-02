namespace InvesteBrasil.Infra.Shared.Interfaces
{
    public interface IResultadoValidacao
    {
        bool OcorreuErro { get; }
        string MensagemErro { get; }
    }

    public interface IResultadoValidacao<T> : IResultadoValidacao
    {
        T? Retorno { get; }
    }
}
