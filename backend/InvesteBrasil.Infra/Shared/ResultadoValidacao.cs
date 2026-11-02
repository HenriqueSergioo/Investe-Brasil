using InvesteBrasil.Infra.Shared.Interfaces;

namespace InvesteBrasil.Infra.Shared
{
    public class ResultadoValidacao : IResultadoValidacao
    {
        public bool OcorreuErro { get; set; }

        public string MensagemErro { get; set; } = string.Empty;

        public Exception? Excecao { get; set; }

        public ResultadoValidacao()
        {
        }

        public ResultadoValidacao(string mensagemErro)
        {
            OcorreuErro = true;
            MensagemErro = mensagemErro;
        }

        public ResultadoValidacao(Exception excecao, string mensagemErro)
        {
            OcorreuErro = true;
            Excecao = excecao;
            MensagemErro = mensagemErro;
        }
    }

    public class ResultadoValidacao<T> : IResultadoValidacao<T>
    {
        public IList<IResultadoValidacao> Validacoes { get; set; } = new List<IResultadoValidacao>();

        public T? Retorno { get; set; }

        public ResultadoValidacao(IResultadoValidacao validacao)
        {
            Validacoes.Add(validacao);
        }

        public ResultadoValidacao(string mensagemErro)
            : this(new ResultadoValidacao(mensagemErro))
        {
        }

        public ResultadoValidacao(Exception ex, string mensagemErro)
            : this(new ResultadoValidacao(ex, mensagemErro))
        {
        }

        public ResultadoValidacao(T retorno)
            : this(new ResultadoValidacao { OcorreuErro = false })
        {
            Retorno = retorno;
        }

        public bool OcorreuErro => Validacoes.Any(x => x.OcorreuErro);

        public string MensagemErro =>
            Validacoes.FirstOrDefault(x => x.OcorreuErro)?.MensagemErro ?? string.Empty;
    }
}
