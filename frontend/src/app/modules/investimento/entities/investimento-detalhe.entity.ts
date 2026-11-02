export class InvestimentoDetalheEntity {
  id = 0;
  proprietario = '';
  dataCriacao = '';
  dataResgate?: string | null;
  valorInicial = 0;
  cpf = '';
  tipoInvestimento = '';
  instituicao = '';
  liquidez = '';
  dataVencimento?: string | null;
  objetivo = '';
  observacao = '';
  ganhos = 0;
  saldoEsperado = 0;
  aliquotaImposto = 0;
  valorImposto = 0;
  valorLiquido = 0;
  situacao = '';

  constructor(init?: Partial<InvestimentoDetalheEntity>) {
    Object.assign(this, init);
  }
}
