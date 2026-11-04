namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class InvestimentoDetalheEntity
    {
        public long CodigoInvestimento { get; set; }

        public string NomeProprietario { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public DateOnly? DataResgate { get; set; }

        public decimal ValorInicial { get; set; }

        public DateOnly? DataVencimento { get; set; }

        public string DescricaoObjetivo { get; set; } = string.Empty;

        public string DescricaoObservacao { get; set; } = string.Empty;

        public decimal ValorGanhos { get; set; }

        public decimal ValorSaldoEsperado { get; set; }

        public decimal ValorAliquotaImposto { get; set; }

        public decimal ValorImposto { get; set; }

        public decimal ValorLiquido { get; set; }

        public string SituacaoInvestimento { get; set; } = string.Empty;
    }
}
