import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { AlertService } from '../../../shared/alert.service';
import { hojeIso } from '../../../shared/formatacao';
import { SessionService } from '../../../shared/session.service';
import { IncluirInvestimentoRequestEntity } from '../entities/requests/incluir-investimento.request.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-criar-investimento',
  templateUrl: './criar-investimento.component.html',
  styleUrls: ['./criar-investimento.component.scss']
})
export class CriarInvestimentoComponent {
  descricaoObjetivo = '';
  descricaoObservacao = '';
  hoje = hojeIso();
  dataCriacao = hojeIso();
  dataVencimento = '';
  valorInicial: number | null = null;
  salvando = false;
  objetivos = ['Reserva de emergência', 'Aposentadoria', 'Compra de imóvel', 'Educação', 'Outros'];

  constructor(
    private investimentoService: InvestimentoService,
    private alertService: AlertService,
    private session: SessionService,
    private router: Router
  ) {}

  async salvar(): Promise<void> {
    if (!this.descricaoObjetivo) {
      this.alertService.alerta('Informe o objetivo.');
      return;
    }

    if (!this.dataCriacao) {
      this.alertService.alerta('Informe a data da aplicação.');
      return;
    }

    if (this.dataCriacao > this.hoje) {
      this.alertService.alerta('A data da aplicação não pode ser futura.');
      return;
    }

    if (!this.valorInicial || this.valorInicial <= 0) {
      this.alertService.alerta('O valor deve ser maior que zero.');
      return;
    }

    if (!this.dataVencimento) {
      this.alertService.alerta('Informe a data de retirada.');
      return;
    }

    const confirmou = await this.alertService.confirmar('Deseja mesmo criar este investimento?');
    if (!confirmou || this.salvando) {
      return;
    }

    this.salvando = true;
    const request = new IncluirInvestimentoRequestEntity({
      nomeProprietario: this.session.sessao?.nomeUsuario ?? '',
      dataCriacao: this.dataCriacao,
      valorInicial: this.valorInicial,
      dataVencimento: this.dataVencimento,
      descricaoObjetivo: this.descricaoObjetivo,
      descricaoObservacao: this.descricaoObservacao.trim()
    });

    this.investimentoService.incluirInvestimento(request)
      .pipe(take(1))
      .subscribe({
        next: () => {
          this.alertService.sucesso('Investimento criado com sucesso.');
          this.voltar();
        },
        error: () => {
          this.salvando = false;
        }
      });
  }

  voltar(): void {
    this.router.navigate(['/investimentos']);
  }
}
