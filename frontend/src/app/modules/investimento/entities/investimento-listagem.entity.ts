export class InvestimentoListagemEntity {
  codigoInvestimento = 0;
  nomeProprietario = '';
  descricaoObjetivo = '';
  dataCriacao = '';
  valorInicial = 0;
  valorSaldoAtual = 0;
  situacaoInvestimento = '';

  constructor(init?: Partial<InvestimentoListagemEntity>) {
    Object.assign(this, init);
  }
}
