using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Enums;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;

namespace InvesteBrasil.Investimento.Repositories.Memoria
{
    public class InvestimentoMemoriaRepository : IInvestimentoRepository
    {
        private readonly List<InvestimentoEntity> investimentos = new();
        private readonly object locker = new();
        private long proximoId = 1;

        public InvestimentoMemoriaRepository()
        {
            Semear();
        }

        public Task GarantirEsquemaAsync() => Task.CompletedTask;

        public Task<InvestimentoEntity> IncluirAsync(InvestimentoEntity entity)
        {
            lock (locker)
            {
                entity.Id = proximoId++;
                investimentos.Add(Clone(entity));
                return Task.FromResult(Clone(entity));
            }
        }

        public Task<InvestimentoEntity?> ConsultarPorIdAsync(long id)
        {
            lock (locker)
            {
                var item = investimentos.FirstOrDefault(x => x.Id == id);
                return Task.FromResult(item == null ? null : Clone(item));
            }
        }

        public Task<(IEnumerable<InvestimentoEntity> Itens, int Total)> ConsultarAsync(
            string? proprietario,
            DateOnly? dataInicial,
            DateOnly? dataFinal,
            int pagina,
            int tamanhoPagina)
        {
            lock (locker)
            {
                var consulta = investimentos.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(proprietario))
                    consulta = consulta.Where(x => x.Proprietario.Contains(proprietario.Trim(), StringComparison.OrdinalIgnoreCase));

                if (dataInicial.HasValue)
                    consulta = consulta.Where(x => x.DataCriacao >= dataInicial.Value);

                if (dataFinal.HasValue)
                    consulta = consulta.Where(x => x.DataCriacao <= dataFinal.Value);

                var filtrado = consulta
                    .OrderByDescending(x => x.DataCriacao)
                    .ThenByDescending(x => x.Id)
                    .ToList();

                var itens = filtrado
                    .Skip((pagina - 1) * tamanhoPagina)
                    .Take(tamanhoPagina)
                    .Select(Clone)
                    .ToList();

                return Task.FromResult<(IEnumerable<InvestimentoEntity>, int)>((itens, filtrado.Count));
            }
        }

        public Task<IEnumerable<InvestimentoEntity>> ConsultarAtivosAsync()
        {
            lock (locker)
            {
                var ativos = investimentos
                    .Where(x => x.Situacao == SituacaoInvestimento.Investido)
                    .Select(Clone)
                    .ToList();

                return Task.FromResult<IEnumerable<InvestimentoEntity>>(ativos);
            }
        }

        public Task ResgatarAsync(long id, DateOnly dataResgate)
        {
            lock (locker)
            {
                var item = investimentos.First(x => x.Id == id);
                item.DataResgate = dataResgate;
                item.Situacao = SituacaoInvestimento.Resgatado;
                return Task.CompletedTask;
            }
        }

        private void Semear()
        {
            IncluirSemente("Ana Souza", "52998224725", "CDB", "Nubank", "Diária", "Reserva de emergência", new DateOnly(2022, 11, 15), 2300m, null);
            IncluirSemente("Bruno Lima", "39053344705", "Tesouro Selic", "XP Investimentos", "Diária", "Aposentadoria", new DateOnly(2022, 8, 10), 670m, null);
            IncluirSemente("Carla Dias", "15350946056", "LCI", "Itaú", "No vencimento", "Compra de imóvel", new DateOnly(2021, 3, 16), 2340m, new DateOnly(2023, 3, 16));
            IncluirSemente("Diego Alves", "11144477735", "CDB", "Banco Inter", "D+30", "Educação", new DateOnly(2021, 3, 1), 5000m, null);
            IncluirSemente("Elisa Martins", "86421538010", "Fundo", "BTG Pactual", "D+30", "Outros", new DateOnly(2020, 1, 15), 2300m, new DateOnly(2021, 2, 15));
            IncluirSemente("Felipe Rocha", "07312965094", "Poupança", "Caixa", "Diária", "Reserva de emergência", new DateOnly(2019, 1, 1), 280m, null);
        }

        private void IncluirSemente(
            string proprietario,
            string cpf,
            string tipo,
            string instituicao,
            string liquidez,
            string objetivo,
            DateOnly dataCriacao,
            decimal valor,
            DateOnly? dataResgate)
        {
            investimentos.Add(new InvestimentoEntity
            {
                Id = proximoId++,
                Proprietario = proprietario,
                Cpf = cpf,
                TipoInvestimento = tipo,
                Instituicao = instituicao,
                Liquidez = liquidez,
                Objetivo = objetivo,
                DataCriacao = dataCriacao,
                ValorInicial = valor,
                DataResgate = dataResgate,
                Situacao = dataResgate.HasValue ? SituacaoInvestimento.Resgatado : SituacaoInvestimento.Investido
            });
        }

        private static InvestimentoEntity Clone(InvestimentoEntity entity)
        {
            return new InvestimentoEntity
            {
                Id = entity.Id,
                Proprietario = entity.Proprietario,
                DataCriacao = entity.DataCriacao,
                ValorInicial = entity.ValorInicial,
                Cpf = entity.Cpf,
                TipoInvestimento = entity.TipoInvestimento,
                Instituicao = entity.Instituicao,
                Liquidez = entity.Liquidez,
                DataVencimento = entity.DataVencimento,
                Objetivo = entity.Objetivo,
                Observacao = entity.Observacao,
                DataResgate = entity.DataResgate,
                Situacao = entity.Situacao
            };
        }
    }
}
