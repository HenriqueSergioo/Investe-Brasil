import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { AlertService } from '../../../shared/alert.service';
import { hojeIso } from '../../../shared/formatacao';
import { IncluirInvestimentoRequestEntity } from '../entities/requests/incluir-investimento.request.entity';
import { InvestimentoService } from '../services/investimento.service';

@Component({
  selector: 'app-criar-investimento',
  templateUrl: './criar-investimento.component.html',
  styleUrls: ['./criar-investimento.component.scss']
})
export class CriarInvestimentoComponent {
  proprietario = '';
  cpf = '';
  tipoInvestimento = '';
  instituicao = '';
  liquidez = '';
  objetivo = '';
  observacao = '';
  dataCriacao = hojeIso();
  dataVencimento = '';
  hoje = hojeIso();
  valorInicial: number | null = null;

  tipos = ['CDB', 'LCI', 'LCA', 'Tesouro Selic', 'Tesouro IPCA', 'Fundo', 'Ações', 'Poupança'];
  liquideces = ['Diária', 'D+30', 'No vencimento'];
  objetivos = ['Reserva de emergência', 'Aposentadoria', 'Compra de imóvel', 'Educação', 'Outros'];

  constructor(
    private investimentoService: InvestimentoService,
    private alertService: AlertService,
    private router: Router
  ) {}

  mascararCpf(valor: string): void {
    const numeros = valor.replace(/\D/g, '').slice(0, 11);
    if (numeros.length <= 3) {
      this.cpf = numeros;
    } else if (numeros.length <= 6) {
      this.cpf = `${numeros.slice(0, 3)}.${numeros.slice(3)}`;
    } else if (numeros.length <= 9) {
      this.cpf = `${numeros.slice(0, 3)}.${numeros.slice(3, 6)}.${numeros.slice(6)}`;
    } else {
      this.cpf = `${numeros.slice(0, 3)}.${numeros.slice(3, 6)}.${numeros.slice(6, 9)}-${numeros.slice(9)}`;
    }
  }

  salvar(): void {
    if (!this.proprietario.trim()) {
      this.alertService.erro('Informe o proprietário.');
      return;
    }

    if (this.cpf.replace(/\D/g, '').length !== 11) {
      this.alertService.erro('Informe um CPF válido.');
      return;
    }

    if (!this.tipoInvestimento) {
      this.alertService.erro('Informe o tipo do investimento.');
      return;
    }

    if (!this.instituicao.trim()) {
      this.alertService.erro('Informe a instituição.');
      return;
    }

    if (!this.dataCriacao) {
      this.alertService.erro('Informe a data da aplicação.');
      return;
    }

    if (!this.valorInicial || this.valorInicial <= 0) {
      this.alertService.erro('O valor deve ser maior que zero.');
      return;
    }

    if (!this.liquidez) {
      this.alertService.erro('Informe a liquidez.');
      return;
    }

    if (!this.objetivo) {
      this.alertService.erro('Informe o objetivo.');
      return;
    }

    const request = new IncluirInvestimentoRequestEntity({
      proprietario: this.proprietario.trim(),
      cpf: this.cpf,
      tipoInvestimento: this.tipoInvestimento,
      instituicao: this.instituicao.trim(),
      dataCriacao: this.dataCriacao,
      valorInicial: this.valorInicial,
      liquidez: this.liquidez,
      dataVencimento: this.dataVencimento || null,
      objetivo: this.objetivo,
      observacao: this.observacao.trim()
    });

    this.investimentoService.incluirInvestimento(request)
      .pipe(take(1))
      .subscribe(() => {
        this.alertService.sucesso('Investimento criado com sucesso.');
        this.router.navigate(['/investimentos']);
      });
  }
}
