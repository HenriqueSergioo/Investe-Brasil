namespace InvesteBrasil.Investimento.Services
{
    public static class CalculadoraRendimento
    {
        public const decimal TaxaMensal = 0.0052m;

        public static int ContarMesesCompletos(DateOnly dataCriacao, DateOnly dataReferencia)
        {
            if (dataReferencia < dataCriacao)
                return 0;

            var meses = 0;
            while (dataCriacao.AddMonths(meses + 1) <= dataReferencia)
                meses++;

            return meses;
        }

        public static int ContarAnosCompletos(DateOnly dataCriacao, DateOnly dataReferencia)
        {
            if (dataReferencia < dataCriacao)
                return 0;

            var anos = 0;
            while (dataCriacao.AddYears(anos + 1) <= dataReferencia)
                anos++;

            return anos;
        }

        public static decimal CalcularSaldoEsperado(decimal valorInicial, DateOnly dataCriacao, DateOnly dataReferencia)
        {
            var meses = ContarMesesCompletos(dataCriacao, dataReferencia);
            var saldo = valorInicial;

            for (var i = 0; i < meses; i++)
                saldo = Math.Round(saldo * (1 + TaxaMensal), 2, MidpointRounding.AwayFromZero);

            return saldo;
        }

        public static decimal CalcularGanhos(decimal valorInicial, DateOnly dataCriacao, DateOnly dataReferencia)
        {
            return CalcularSaldoEsperado(valorInicial, dataCriacao, dataReferencia) - valorInicial;
        }

        public static decimal ObterAliquota(DateOnly dataCriacao, DateOnly dataReferencia)
        {
            var anos = ContarAnosCompletos(dataCriacao, dataReferencia);

            if (anos < 1)
                return 0.225m;

            if (anos < 2)
                return 0.185m;

            return 0.15m;
        }

        public static decimal CalcularImposto(decimal ganhos, decimal aliquota)
        {
            if (ganhos <= 0)
                return 0;

            return Math.Round(ganhos * aliquota, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalcularValorLiquido(decimal saldoEsperado, decimal valorImposto)
        {
            return Math.Round(saldoEsperado - valorImposto, 2, MidpointRounding.AwayFromZero);
        }
    }
}
