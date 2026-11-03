namespace InvesteBrasil.Investimento.Entities
{
    public class InvestimentoEntity
    {
        public long CodigoInvestimento { get; set; }

        public long CodigoUsuario { get; set; }

        public string NomeProprietario { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public decimal ValorInicial { get; set; }

        public string NumeroCpf { get; set; } = string.Empty;

        public string DescricaoTipoInvestimento { get; set; } = string.Empty;

        public string NomeInstituicao { get; set; } = string.Empty;

        public string DescricaoLiquidez { get; set; } = string.Empty;

        public DateOnly? DataVencimento { get; set; }

        public string DescricaoObjetivo { get; set; } = string.Empty;

        public string DescricaoObservacao { get; set; } = string.Empty;

        public DateOnly? DataResgate { get; set; }

        public string SituacaoInvestimento { get; set; } = Enums.SituacaoInvestimento.Investido;
    }
}
