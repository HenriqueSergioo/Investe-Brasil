export class InvestimentoListagemEntity {
  id = 0;
  proprietario = '';
  tipoInvestimento = '';
  instituicao = '';
  dataCriacao = '';
  valorInicial = 0;
  saldoAtual = 0;
  situacao = '';

  constructor(init?: Partial<InvestimentoListagemEntity>) {
    Object.assign(this, init);
  }
}
