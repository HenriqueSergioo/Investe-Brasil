namespace InvesteBrasil.Investimento.Entities.Requests
{
    public class IncluirInvestimentoRequestEntity
    {
        public string NomeProprietario { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public decimal ValorInicial { get; set; }

        public DateOnly? DataVencimento { get; set; }

        public string DescricaoObjetivo { get; set; } = string.Empty;

        public string DescricaoObservacao { get; set; } = string.Empty;
    }
}
