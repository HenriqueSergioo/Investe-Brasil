export class IncluirInvestimentoRequestEntity {
  proprietario = '';
  dataCriacao = '';
  valorInicial = 0;
  cpf = '';
  tipoInvestimento = '';
  instituicao = '';
  liquidez = '';
  dataVencimento?: string | null;
  objetivo = '';
  observacao = '';

  constructor(init?: Partial<IncluirInvestimentoRequestEntity>) {
    Object.assign(this, init);
  }
}
