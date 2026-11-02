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

            if (string.IsNullOrWhiteSpace(request.Proprietario))
                return Erro("O proprietário é obrigatório.");

            if (request.Proprietario.Trim().Length > 200)
                return Erro("O proprietário deve ter no máximo 200 caracteres.");

            if (request.ValorInicial <= 0)
                return Erro("O valor do investimento deve ser maior que zero.");

            if (request.DataCriacao > hoje)
                return Erro("A data de criação não pode ser futura.");

            if (string.IsNullOrWhiteSpace(request.Cpf) || SomenteNumeros(request.Cpf).Length != 11)
                return Erro("Informe um CPF válido.");

            if (string.IsNullOrWhiteSpace(request.TipoInvestimento))
                return Erro("Informe o tipo do investimento.");

            if (string.IsNullOrWhiteSpace(request.Instituicao))
                return Erro("Informe a instituição.");

            if (string.IsNullOrWhiteSpace(request.Liquidez))
                return Erro("Informe a liquidez.");

            if (string.IsNullOrWhiteSpace(request.Objetivo))
                return Erro("Informe o objetivo do investimento.");

            if (request.DataVencimento.HasValue && request.DataVencimento.Value < request.DataCriacao)
                return Erro("A data de vencimento não pode ser anterior à aplicação.");

            return Sucesso();
        }

        public IResultadoValidacao ConsultarInvestimento(long id)
        {
            if (id <= 0)
                return Erro("Identificador do investimento inválido.");

            return Sucesso();
        }

        public IResultadoValidacao ResgatarInvestimento(ResgatarInvestimentoRequestEntity request, InvestimentoEntity? investimento, DateOnly hoje)
        {
            if (request == null)
                return Erro("Os dados do resgate são obrigatórios.");

            if (request.Id <= 0)
                return Erro("Identificador do investimento inválido.");

            if (investimento == null)
                return Erro("Investimento não encontrado.");

            if (investimento.Situacao == SituacaoInvestimento.Resgatado)
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
