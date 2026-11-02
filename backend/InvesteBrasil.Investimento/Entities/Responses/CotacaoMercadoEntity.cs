namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class CotacaoMercadoEntity
    {
        public string Codigo { get; set; } = string.Empty;

        public string Nome { get; set; } = string.Empty;

        public decimal Valor { get; set; }

        public decimal? VariacaoPercentual { get; set; }

        public string Unidade { get; set; } = "moeda";

        public string Fonte { get; set; } = string.Empty;

        public string AtualizadoEm { get; set; } = string.Empty;
    }
}
