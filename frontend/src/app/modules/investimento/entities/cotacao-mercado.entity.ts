export class CotacaoMercadoEntity {
  codigo = '';
  nome = '';
  valor = 0;
  variacaoPercentual?: number | null;
  unidade = 'moeda';
  fonte = '';
  atualizadoEm = '';

  constructor(init?: Partial<CotacaoMercadoEntity>) {
    Object.assign(this, init);
  }
}
