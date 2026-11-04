export class ResgatarInvestimentoRequestEntity {
  codigoInvestimento = 0;
  dataResgate = '';

  constructor(init?: Partial<ResgatarInvestimentoRequestEntity>) {
    Object.assign(this, init);
  }
}
