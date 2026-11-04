namespace InvesteBrasil.Investimento.Entities.Requests
{
    public class ResgatarInvestimentoRequestEntity
    {
        public long CodigoInvestimento { get; set; }

        public DateOnly DataResgate { get; set; }
    }
}
