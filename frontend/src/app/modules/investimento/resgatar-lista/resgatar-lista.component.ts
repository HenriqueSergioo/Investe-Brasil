import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { formatarData, formatarMoeda } from '../../../shared/formatacao';
import { InvestimentoListagemEntity } from '../entities/investimento-listagem.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-resgatar-lista',
  templateUrl: './resgatar-lista.component.html',
  styleUrls: ['./resgatar-lista.component.scss']
})
export class ResgatarListaComponent implements OnInit {
  investimentos: InvestimentoListagemEntity[] = [];

  constructor(
    private investimentoService: InvestimentoService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.investimentoService.consultarInvestimentos('', '', '', 1, 100)
      .pipe(take(1))
      .subscribe(retorno => {
        this.investimentos = retorno.itens.filter(item => item.situacao === 'Investido');
      });
  }

  resgatar(item: InvestimentoListagemEntity): void {
    this.router.navigate(['/investimentos', item.id, 'resgatar']);
  }

  formatarMoeda = formatarMoeda;
  formatarData = formatarData;
}
