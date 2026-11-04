using InvesteBrasil.Infra.Services.Validators.Abstracts;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Enums;

namespace InvesteBrasil.Investimento.Services.Validators
{
    public class InvestimentoValidador : Validator
    {
        public IResultadoValidacao IncluirInvestimento(IncluirInvestimentoRequestEntity request, DateOnly hoje)
        {
            if (request == null)
                return Erro("Os dados do investimento são obrigatórios.");

            if (string.IsNullOrWhiteSpace(request.NomeProprietario))
                return Erro("O proprietário é obrigatório.");

            if (request.NomeProprietario.Trim().Length > 200)
                return Erro("O proprietário deve ter no máximo 200 caracteres.");

            if (request.ValorInicial <= 0)
                return Erro("O valor do investimento deve ser maior que zero.");

            if (request.DataCriacao > hoje)
                return Erro("A data de criação não pode ser futura.");

            if (string.IsNullOrWhiteSpace(request.DescricaoObjetivo))
                return Erro("Informe o objetivo do investimento.");

            if (!request.DataVencimento.HasValue)
                return Erro("Informe a data de retirada.");

            if (request.DataVencimento.Value < request.DataCriacao)
                return Erro("A data de retirada não pode ser anterior à aplicação.");

            return Sucesso();
        }

        public IResultadoValidacao ConsultarInvestimento(long codigoInvestimento)
        {
            if (codigoInvestimento <= 0)
                return Erro("Identificador do investimento inválido.");

            return Sucesso();
        }

        public IResultadoValidacao ResgatarInvestimento(ResgatarInvestimentoRequestEntity request, InvestimentoEntity? investimento, DateOnly hoje)
        {
            if (request == null)
                return Erro("Os dados do resgate são obrigatórios.");

            if (request.CodigoInvestimento <= 0)
                return Erro("Identificador do investimento inválido.");

            if (investimento == null)
                return Erro("Investimento não encontrado.");

            if (investimento.SituacaoInvestimento == SituacaoInvestimento.Resgatado)
                return Erro("Este investimento já foi resgatado.");

            if (request.DataResgate > hoje)
                return Erro("A data de resgate não pode ser futura.");

            if (request.DataResgate < investimento.DataCriacao)
                return Erro("A data de resgate não pode ser anterior à criação do investimento.");

            return Sucesso();
        }

        private static string SomenteNumeros(string valor) =>
            new(valor.Where(char.IsDigit).ToArray());
    }
}
