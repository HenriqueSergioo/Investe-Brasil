import { CotacaoMercadoEntity } from './cotacao-mercado.entity';

export class PainelMercadoEntity {
  saldoCarteira = 0;
  taxaMensalCarteira = 0;
  taxaAnualCarteira = 0;
  selicAnual?: number | null;
  diferencaPontos?: number | null;
  comparativoSelic = '';
  saldoEmDolar?: number | null;
  saldoEmEuro?: number | null;
  saldoEmBitcoin?: number | null;
  cotacaoDolar?: number | null;
  variacaoDolar?: number | null;
  cotacaoEuro?: number | null;
  variacaoEuro?: number | null;
  cotacaoBitcoin?: number | null;
  variacaoBitcoin?: number | null;
  leituraCambio = '';
  cotacoes: CotacaoMercadoEntity[] = [];

  constructor(init?: Partial<PainelMercadoEntity>) {
    Object.assign(this, init);
  }
}
