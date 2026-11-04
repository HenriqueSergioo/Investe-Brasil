import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { formatarData, formatarMoeda } from '../../../shared/formatacao';
import { InvestimentoListagemEntity } from '../entities/investimento-listagem.entity';
import { PaginacaoEntity } from '../entities/paginacao.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-listar-investimento',
  templateUrl: './listar-investimento.component.html',
  styleUrls: ['./listar-investimento.component.scss']
})
export class ListarInvestimentoComponent implements OnInit {
  saldo = 0;
  pagina = 1;
  tamanhoPagina = 5;
  paginacao = new PaginacaoEntity<InvestimentoListagemEntity>();

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
      '',
      '',
      '',
      this.pagina,
      this.tamanhoPagina
    )
      .pipe(take(1))
      .subscribe(retorno => this.paginacao = retorno);
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
    this.router.navigate(['/investimentos', item.codigoInvestimento]);
  }

  resgatar(item: InvestimentoListagemEntity): void {
    this.router.navigate(['/investimentos', item.codigoInvestimento, 'resgatar']);
  }

  formatarMoeda = formatarMoeda;
  formatarData = formatarData;
}
