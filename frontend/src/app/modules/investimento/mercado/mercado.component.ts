import { Component, OnInit } from '@angular/core';
import { formatarMoeda } from '../../../shared/formatacao';
import { PainelMercadoEntity } from '../entities/painel-mercado.entity';
import { InvestimentoService } from '../services/investimento.service';
import { take } from 'rxjs';

@Component({
  selector: 'app-mercado',
  templateUrl: './mercado.component.html',
  styleUrls: ['./mercado.component.scss']
})
export class MercadoComponent implements OnInit {
  painel = new PainelMercadoEntity();
  carregado = false;

  constructor(private investimentoService: InvestimentoService) {}

  ngOnInit(): void {
    this.investimentoService.consultarMercado()
      .pipe(take(1))
      .subscribe({
        next: painel => {
          this.painel = painel;
          this.carregado = true;
        },
        error: () => this.carregado = true
      });
  }

  formatarMoeda = formatarMoeda;

  formatarTaxa(valor?: number | null, sufixo = '% a.a.'): string {
    if (valor == null) {
      return '-';
    }

    return `${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}${sufixo}`;
  }

  formatarPontos(valor?: number | null): string {
    if (valor == null) {
      return '-';
    }

    const sinal = valor > 0 ? '+' : '';
    return `${sinal}${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })} p.p.`;
  }

  formatarVariacao(valor?: number | null): string {
    if (valor == null) {
      return '';
    }

    const sinal = valor > 0 ? '+' : '';
    return `${sinal}${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}% no dia`;
  }

  formatarDolar(valor?: number | null): string {
    if (valor == null) {
      return '-';
    }

    return `US$ ${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  }

  formatarEuro(valor?: number | null): string {
    if (valor == null) {
      return '-';
    }

    return `€ ${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  }

  formatarBitcoin(valor?: number | null): string {
    if (valor == null) {
      return '-';
    }

    return `${valor.toLocaleString('pt-BR', { minimumFractionDigits: 6, maximumFractionDigits: 8 })} BTC`;
  }

  classeVariacao(valor?: number | null): string {
    if (valor == null || valor === 0) {
      return '';
    }

    return valor > 0 ? 'alta' : 'baixa';
  }
}
