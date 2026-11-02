namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class PainelMercadoEntity
    {
        public decimal SaldoCarteira { get; set; }

        public decimal TaxaMensalCarteira { get; set; }

        public decimal TaxaAnualCarteira { get; set; }

        public decimal? SelicAnual { get; set; }

        public decimal? DiferencaPontos { get; set; }

        public string ComparativoSelic { get; set; } = string.Empty;

        public decimal? SaldoEmDolar { get; set; }

        public decimal? SaldoEmEuro { get; set; }

        public decimal? SaldoEmBitcoin { get; set; }

        public decimal? CotacaoDolar { get; set; }

        public decimal? VariacaoDolar { get; set; }

        public decimal? CotacaoEuro { get; set; }

        public decimal? VariacaoEuro { get; set; }

        public decimal? CotacaoBitcoin { get; set; }

        public decimal? VariacaoBitcoin { get; set; }

        public string LeituraCambio { get; set; } = string.Empty;

        public IList<CotacaoMercadoEntity> Cotacoes { get; set; } = new List<CotacaoMercadoEntity>();
    }
}
