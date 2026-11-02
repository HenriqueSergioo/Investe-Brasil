using InvesteBrasil.Investimento.Services;

namespace InvesteBrasil.Investimento.Tests
{
    public class CalculadoraRendimentoTests
    {
        [Fact]
        public void ContarMesesCompletos_MesmoDia_DeveRetornarZero()
        {
            var criacao = new DateOnly(2024, 1, 15);
            var meses = CalculadoraRendimento.ContarMesesCompletos(criacao, criacao);
            Assert.Equal(0, meses);
        }

        [Fact]
        public void ContarMesesCompletos_UmDiaAntesDoAniversario_DeveRetornarZero()
        {
            var criacao = new DateOnly(2024, 1, 15);
            var referencia = new DateOnly(2024, 2, 14);
            Assert.Equal(0, CalculadoraRendimento.ContarMesesCompletos(criacao, referencia));
        }

        [Fact]
        public void ContarMesesCompletos_NoAniversarioMensal_DeveRetornarUm()
        {
            var criacao = new DateOnly(2024, 1, 15);
            var referencia = new DateOnly(2024, 2, 15);
            Assert.Equal(1, CalculadoraRendimento.ContarMesesCompletos(criacao, referencia));
        }

        [Fact]
        public void CalcularSaldoEsperado_UmMes_DeveAplicarJurosCompostos()
        {
            var saldo = CalculadoraRendimento.CalcularSaldoEsperado(1000m, new DateOnly(2024, 1, 15), new DateOnly(2024, 2, 15));
            Assert.Equal(1005.20m, saldo);
        }

        [Fact]
        public void CalcularSaldoEsperado_DoisMeses_DeveCapitalizarOGanho()
        {
            var saldo = CalculadoraRendimento.CalcularSaldoEsperado(1000m, new DateOnly(2024, 1, 15), new DateOnly(2024, 3, 15));
            Assert.Equal(1010.43m, saldo);
        }

        [Fact]
        public void ObterAliquota_MenosDeUmAno_DeveSer22_5()
        {
            var aliquota = CalculadoraRendimento.ObterAliquota(new DateOnly(2024, 1, 15), new DateOnly(2024, 12, 15));
            Assert.Equal(0.225m, aliquota);
        }

        [Fact]
        public void ObterAliquota_EntreUmEDoisAnos_DeveSer18_5()
        {
            var aliquota = CalculadoraRendimento.ObterAliquota(new DateOnly(2023, 1, 15), new DateOnly(2024, 1, 15));
            Assert.Equal(0.185m, aliquota);
        }

        [Fact]
        public void ObterAliquota_DoisAnosOuMais_DeveSer15()
        {
            var aliquota = CalculadoraRendimento.ObterAliquota(new DateOnly(2022, 1, 15), new DateOnly(2024, 1, 15));
            Assert.Equal(0.15m, aliquota);
        }

        [Fact]
        public void CalcularImposto_ExemploDoEnunciado_DeveSer45()
        {
            var imposto = CalculadoraRendimento.CalcularImposto(200m, 0.225m);
            Assert.Equal(45.00m, imposto);
        }

        [Fact]
        public void CalcularValorLiquido_ExemploDoEnunciado_DeveSer1155()
        {
            var liquido = CalculadoraRendimento.CalcularValorLiquido(1200m, 45m);
            Assert.Equal(1155.00m, liquido);
        }
    }
}
