import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { formatarData, formatarMoeda, formatarPercentual } from '../../../shared/formatacao';
import { InvestimentoDetalheEntity } from '../entities/investimento-detalhe.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-detalhe-investimento',
  templateUrl: './detalhe-investimento.component.html',
  styleUrls: ['./detalhe-investimento.component.scss']
})
export class DetalheInvestimentoComponent implements OnInit {
  detalhe?: InvestimentoDetalheEntity;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private investimentoService: InvestimentoService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.investimentoService.consultarInvestimento(id)
      .pipe(take(1))
      .subscribe(detalhe => this.detalhe = detalhe);
  }

  resgatar(): void {
    if (!this.detalhe) {
      return;
    }

    this.router.navigate(['/investimentos', this.detalhe.codigoInvestimento, 'resgatar']);
  }

  voltar(): void {
    this.router.navigate(['/investimentos']);
  }

  formatarMoeda = formatarMoeda;
  formatarData = formatarData;
  formatarPercentual = formatarPercentual;
}
