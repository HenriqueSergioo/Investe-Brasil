export class InvestimentoListagemEntity {
  codigoInvestimento = 0;
  nomeProprietario = '';
  descricaoTipoInvestimento = '';
  descricaoObjetivo = '';
  nomeInstituicao = '';
  dataCriacao = '';
  valorInicial = 0;
  valorSaldoAtual = 0;
  situacaoInvestimento = '';

  constructor(init?: Partial<InvestimentoListagemEntity>) {
    Object.assign(this, init);
  }
}
