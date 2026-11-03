export class InvestimentoDetalheEntity {
  codigoInvestimento = 0;
  nomeProprietario = '';
  dataCriacao = '';
  dataResgate?: string | null;
  valorInicial = 0;
  numeroCpf = '';
  descricaoTipoInvestimento = '';
  nomeInstituicao = '';
  descricaoLiquidez = '';
  dataVencimento?: string | null;
  descricaoObjetivo = '';
  descricaoObservacao = '';
  valorGanhos = 0;
  valorSaldoEsperado = 0;
  valorAliquotaImposto = 0;
  valorImposto = 0;
  valorLiquido = 0;
  situacaoInvestimento = '';

  constructor(init?: Partial<InvestimentoDetalheEntity>) {
    Object.assign(this, init);
  }
}
