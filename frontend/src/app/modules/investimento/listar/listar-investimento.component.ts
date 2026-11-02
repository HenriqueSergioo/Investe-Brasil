import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { formatarData, formatarMoeda } from '../../../shared/formatacao';
import { InvestimentoListagemEntity } from '../entities/investimento-listagem.entity';
import { PaginacaoEntity } from '../entities/paginacao.entity';
import { PainelMercadoEntity } from '../entities/painel-mercado.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-listar-investimento',
  templateUrl: './listar-investimento.component.html',
  styleUrls: ['./listar-investimento.component.scss']
})
export class ListarInvestimentoComponent implements OnInit {
  saldo = 0;
  proprietario = '';
  dataInicial = '';
  dataFinal = '';
  pagina = 1;
  tamanhoPagina = 5;
  paginacao = new PaginacaoEntity<InvestimentoListagemEntity>();
  painel?: PainelMercadoEntity;

  constructor(
    private investimentoService: InvestimentoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.carregar();
  }

  carregar(): void {
    this.investimentoService.consultarSaldoGeral()
      .pipe(take(1))
      .subscribe(retorno => this.saldo = retorno.saldo);

    this.investimentoService.consultarInvestimentos(
      this.proprietario,
      this.dataInicial,
      this.dataFinal,
      this.pagina,
      this.tamanhoPagina
    )
      .pipe(take(1))
      .subscribe(retorno => this.paginacao = retorno);

    this.investimentoService.consultarMercado()
      .pipe(take(1))
      .subscribe({
        next: painel => this.painel = painel,
        error: () => this.painel = undefined
      });
  }

  filtrar(): void {
    this.pagina = 1;
    this.carregar();
  }

  irParaPagina(pagina: number): void {
    if (pagina < 1 || pagina > this.paginacao.totalPaginas) {
      return;
    }

    this.pagina = pagina;
    this.carregar();
  }

  novo(): void {
    this.router.navigate(['/investimentos/novo']);
  }

  visualizar(item: InvestimentoListagemEntity): void {
    this.router.navigate(['/investimentos', item.id]);
  }

  resgatar(item: InvestimentoListagemEntity): void {
    this.router.navigate(['/investimentos', item.id, 'resgatar']);
  }

  abrirMercado(): void {
    this.router.navigate(['/investimentos/mercado']);
  }

  formatarMoeda = formatarMoeda;
  formatarData = formatarData;

  formatarPontos(valor?: number | null): string {
    if (valor == null) {
      return '-';
    }

    const sinal = valor > 0 ? '+' : '';
    return `${sinal}${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })} p.p. vs Selic`;
  }

  formatarDolar(valor?: number | null): string {
    if (valor == null) {
      return '-';
    }

    return `US$ ${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}`;
  }
}
