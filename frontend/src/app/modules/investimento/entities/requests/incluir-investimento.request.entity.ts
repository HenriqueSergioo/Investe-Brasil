export class IncluirInvestimentoRequestEntity {
  nomeProprietario = '';
  dataCriacao = '';
  valorInicial = 0;
  numeroCpf = '';
  descricaoTipoInvestimento = '';
  nomeInstituicao = '';
  descricaoLiquidez = '';
  dataVencimento?: string | null;
  descricaoObjetivo = '';
  descricaoObservacao = '';

  constructor(init?: Partial<IncluirInvestimentoRequestEntity>) {
    Object.assign(this, init);
  }
}
