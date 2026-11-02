namespace InvesteBrasil.Investimento.Entities
{
    public class InvestimentoEntity
    {
        public long Id { get; set; }

        public string Proprietario { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public decimal ValorInicial { get; set; }

        public string Cpf { get; set; } = string.Empty;

        public string TipoInvestimento { get; set; } = string.Empty;

        public string Instituicao { get; set; } = string.Empty;

        public string Liquidez { get; set; } = string.Empty;

        public DateOnly? DataVencimento { get; set; }

        public string Objetivo { get; set; } = string.Empty;

        public string Observacao { get; set; } = string.Empty;

        public DateOnly? DataResgate { get; set; }

        public string Situacao { get; set; } = Enums.SituacaoInvestimento.Investido;
    }
}
