namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class InvestimentoListagemEntity
    {
        public long Id { get; set; }

        public string Proprietario { get; set; } = string.Empty;

        public string TipoInvestimento { get; set; } = string.Empty;

        public string Instituicao { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public decimal ValorInicial { get; set; }

        public decimal SaldoAtual { get; set; }

        public string Situacao { get; set; } = string.Empty;
    }
}
