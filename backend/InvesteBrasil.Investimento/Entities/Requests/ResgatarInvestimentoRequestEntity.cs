namespace InvesteBrasil.Investimento.Entities.Requests
{
    public class ResgatarInvestimentoRequestEntity
    {
        public long Id { get; set; }

        public DateOnly DataResgate { get; set; }
    }
}
