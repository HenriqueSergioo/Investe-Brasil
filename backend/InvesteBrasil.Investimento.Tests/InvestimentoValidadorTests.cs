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
                Proprietario = "Ana",
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
                Proprietario = "Ana",
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
                Proprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = 100,
                Cpf = "529.982.247-25",
                TipoInvestimento = "CDB",
                Instituicao = "Nubank",
                Liquidez = "Diária",
                Objetivo = "Reserva de emergência"
            };

            Assert.False(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void IncluirInvestimento_CpfIncompleto_DeveFalhar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                Proprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = 100,
                Cpf = "123.456.789",
                TipoInvestimento = "CDB",
                Instituicao = "Nubank",
                Liquidez = "Diária",
                Objetivo = "Reserva de emergência"
            };

            Assert.True(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void IncluirInvestimento_VencimentoAnteriorAAplicacao_DeveFalhar()
        {
            var request = new IncluirInvestimentoRequestEntity
            {
                Proprietario = "Ana",
                DataCriacao = hoje,
                ValorInicial = 100,
                Cpf = "529.982.247-25",
                TipoInvestimento = "CDB",
                Instituicao = "Nubank",
                Liquidez = "Diária",
                Objetivo = "Reserva de emergência",
                DataVencimento = hoje.AddDays(-1)
            };

            Assert.True(validador.IncluirInvestimento(request, hoje).OcorreuErro);
        }

        [Fact]
        public void ResgatarInvestimento_DataAnteriorACriacao_DeveFalhar()
        {
            var investimento = new InvestimentoEntity
            {
                Id = 1,
                DataCriacao = new DateOnly(2024, 5, 10),
                Situacao = SituacaoInvestimento.Investido
            };

            var request = new ResgatarInvestimentoRequestEntity
            {
                Id = 1,
                DataResgate = new DateOnly(2024, 5, 9)
            };

            Assert.True(validador.ResgatarInvestimento(request, investimento, hoje).OcorreuErro);
        }

        [Fact]
        public void ResgatarInvestimento_JaResgatado_DeveFalhar()
        {
            var investimento = new InvestimentoEntity
            {
                Id = 1,
                DataCriacao = new DateOnly(2024, 1, 1),
                Situacao = SituacaoInvestimento.Resgatado
            };

            var request = new ResgatarInvestimentoRequestEntity
            {
                Id = 1,
                DataResgate = hoje
            };

            Assert.True(validador.ResgatarInvestimento(request, investimento, hoje).OcorreuErro);
        }
    }
}
