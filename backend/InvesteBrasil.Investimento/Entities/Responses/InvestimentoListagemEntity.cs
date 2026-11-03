namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class InvestimentoListagemEntity
    {
        public long CodigoInvestimento { get; set; }

        public string NomeProprietario { get; set; } = string.Empty;

        public string DescricaoTipoInvestimento { get; set; } = string.Empty;

        public string NomeInstituicao { get; set; } = string.Empty;

        public DateOnly DataCriacao { get; set; }

        public decimal ValorInicial { get; set; }

        public decimal ValorSaldoAtual { get; set; }

        public string SituacaoInvestimento { get; set; } = string.Empty;
    }
}
