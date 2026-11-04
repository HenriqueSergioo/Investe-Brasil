import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs';
import { AlertService } from '../../../shared/alert.service';
import { formatarMoeda, hojeIso } from '../../../shared/formatacao';
import { InvestimentoDetalheEntity } from '../entities/investimento-detalhe.entity';
import { ResgatarInvestimentoRequestEntity } from '../entities/requests/resgatar-investimento.request.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-resgatar-investimento',
  templateUrl: './resgatar-investimento.component.html',
  styleUrls: ['./resgatar-investimento.component.scss']
})
export class ResgatarInvestimentoComponent implements OnInit {
  detalhe?: InvestimentoDetalheEntity;
  dataResgate = hojeIso();
  hoje = hojeIso();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private investimentoService: InvestimentoService,
    private alertService: AlertService
  ) {}

  ngOnInit(): void {
    this.carregarSimulacao();
  }

  carregarSimulacao(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.investimentoService.consultarInvestimento(id, this.dataResgate)
      .pipe(take(1))
      .subscribe(detalhe => this.detalhe = detalhe);
  }

  async salvar(): Promise<void> {
    if (!this.detalhe) {
      return;
    }

    const confirmou = await this.alertService.confirmar(
      'Deseja mesmo resgatar este investimento? O resgate é total e não pode ser desfeito.',
      { titulo: 'Resgatar', textoConfirmar: 'Resgatar', perigo: true }
    );
    if (!confirmou) {
      return;
    }

    const request = new ResgatarInvestimentoRequestEntity({
      codigoInvestimento: this.detalhe.codigoInvestimento,
      dataResgate: this.dataResgate
    });

    this.investimentoService.resgatarInvestimento(request)
      .pipe(take(1))
      .subscribe(resultado => {
        this.alertService.sucesso(
          `Resgate concluído. Valor líquido: ${formatarMoeda(resultado.valorLiquido)}`
        );
        this.voltar();
      });
  }

  voltar(): void {
    this.router.navigate(['/investimentos']);
  }

  formatarMoeda = formatarMoeda;
}
