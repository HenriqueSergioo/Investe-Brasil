using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Enums;
using InvesteBrasil.Investimento.Services.Validators;

namespace InvesteBrasil.Investimento.Tests
{
    public class InvestimentoValidadorTests
    {
        private readonly InvestimentoValidador validador = new();
        private readonly DateOnly hoje = new(2024, 10, 31);

        [Fact]
        public void IncluirInvestimento_ValorNegativo_DeveFalhar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                NomeProprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = -10
            };

            Assert.True(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void IncluirInvestimento_DataFutura_DeveFalhar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                NomeProprietario = "Ana",
                DataCriacao = hoje.AddDays(1),
                ValorInicial = 100
            };

            Assert.True(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void IncluirInvestimento_DadosValidos_DevePassar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                NomeProprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = 100,
                NumeroCpf = "529.982.247-25",
                DescricaoTipoInvestimento = "CDB",
                NomeInstituicao = "Nubank",
                DescricaoLiquidez = "Diária",
                DescricaoObjetivo = "Reserva de emergência"
            };

            Assert.False(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void IncluirInvestimento_CpfIncompleto_DeveFalhar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                NomeProprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = 100,
                NumeroCpf = "123.456.789",
                DescricaoTipoInvestimento = "CDB",
                NomeInstituicao = "Nubank",
                DescricaoLiquidez = "Diária",
                DescricaoObjetivo = "Reserva de emergência"
            };

            Assert.True(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void IncluirInvestimento_VencimentoAnteriorAAplicacao_DeveFalhar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                NomeProprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = 100,
                NumeroCpf = "529.982.247-25",
                DescricaoTipoInvestimento = "CDB",
                NomeInstituicao = "Nubank",
                DescricaoLiquidez = "Diária",
                DescricaoObjetivo = "Reserva de emergência",
                DataVencimento = hoje.AddDays(-1)
            };

            Assert.True(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void ConsultarInvestimento_IdInvalido_DeveFalhar()
        {
            Assert.True(validador.ConsultarInvestimento(0).OcorreuErro);
        }

        [Fact]
        public void ResgatarInvestimento_DadosValidos_DevePassar()
        {
            var investimento = new InvestimentoEntity
            {
                CodigoInvestimento = 1,
                DataCriacao = new DateOnly(2024, 5, 10),
                SituacaoInvestimento = SituacaoInvestimento.Investido
            };

            var request = new ResgatarInvestimentoRequestEntity
            {
                CodigoInvestimento = 1,
                DataResgate = hoje
            };

            Assert.False(validador.ResgatarInvestimento(request, investimento, hoje).OcorreuErro);
        }

        [Fact]
        public void ResgatarInvestimento_DataAnteriorACriacao_DeveFalhar()
        {
            var investimento = new InvestimentoEntity
            {
                CodigoInvestimento = 1,
                DataCriacao = new DateOnly(2024, 5, 10),
                SituacaoInvestimento = SituacaoInvestimento.Investido
            };

            var request = new ResgatarInvestimentoRequestEntity
            {
                CodigoInvestimento = 1,
                DataResgate = new DateOnly(2024, 5, 9)
            };

            Assert.True(validador.ResgatarInvestimento(request, investimento, hoje).OcorreuErro);
        }

        [Fact]
        public void ResgatarInvestimento_JaResgatado_DeveFalhar()
        {
            var investimento = new InvestimentoEntity
            {
                CodigoInvestimento = 1,
                DataCriacao = new DateOnly(2024, 1, 1),
                SituacaoInvestimento = SituacaoInvestimento.Resgatado
            };

            var request = new ResgatarInvestimentoRequestEntity
            {
                CodigoInvestimento = 1,
                DataResgate = hoje
            };

            Assert.True(validador.ResgatarInvestimento(request, investimento, hoje).OcorreuErro);
        }
    }
}
