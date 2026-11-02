export class ResgatarInvestimentoRequestEntity {
  id = 0;
  dataResgate = '';

  constructor(init?: Partial<ResgatarInvestimentoRequestEntity>) {
    Object.assign(this, init);
  }
}
