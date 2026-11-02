namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class InvestimentoDetalheEntity
    {
        public long Id { get; set; }

        public string Proprietario { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public DateOnly? DataResgate { get; set; }

        public decimal ValorInicial { get; set; }

        public string Cpf { get; set; } = string.Empty;

        public string TipoInvestimento { get; set; } = string.Empty;

        public string Instituicao { get; set; } = string.Empty;

        public string Liquidez { get; set; } = string.Empty;

        public DateOnly? DataVencimento { get; set; }

        public string Objetivo { get; set; } = string.Empty;

        public string Observacao { get; set; } = string.Empty;

        public decimal Ganhos { get; set; }

        public decimal SaldoEsperado { get; set; }

        public decimal AliquotaImposto { get; set; }

        public decimal ValorImposto { get; set; }

        public decimal ValorLiquido { get; set; }

        public string Situacao { get; set; } = string.Empty;
    }
}
