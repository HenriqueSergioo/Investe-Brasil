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
  nomeProprietario = '';
  numeroCpf = '';
  descricaoTipoInvestimento = '';
  nomeInstituicao = '';
  descricaoLiquidez = '';
  descricaoObjetivo = '';
  descricaoObservacao = '';
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
      this.numeroCpf = numeros;
    } else if (numeros.length <= 6) {
      this.numeroCpf = `${numeros.slice(0, 3)}.${numeros.slice(3)}`;
    } else if (numeros.length <= 9) {
      this.numeroCpf = `${numeros.slice(0, 3)}.${numeros.slice(3, 6)}.${numeros.slice(6)}`;
    } else {
      this.numeroCpf = `${numeros.slice(0, 3)}.${numeros.slice(3, 6)}.${numeros.slice(6, 9)}-${numeros.slice(9)}`;
    }
  }

  salvar(): void {
    if (!this.nomeProprietario.trim()) {
      this.alertService.erro('Informe o proprietário.');
      return;
    }

    if (this.numeroCpf.replace(/\D/g, '').length !== 11) {
      this.alertService.erro('Informe um CPF válido.');
      return;
    }

    if (!this.descricaoTipoInvestimento) {
      this.alertService.erro('Informe o tipo do investimento.');
      return;
    }

    if (!this.nomeInstituicao.trim()) {
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

    if (!this.descricaoLiquidez) {
      this.alertService.erro('Informe a liquidez.');
      return;
    }

    if (!this.descricaoObjetivo) {
      this.alertService.erro('Informe o objetivo.');
      return;
    }

    const request = new IncluirInvestimentoRequestEntity({
      nomeProprietario: this.nomeProprietario.trim(),
      numeroCpf: this.numeroCpf,
      descricaoTipoInvestimento: this.descricaoTipoInvestimento,
      nomeInstituicao: this.nomeInstituicao.trim(),
      dataCriacao: this.dataCriacao,
      valorInicial: this.valorInicial,
      descricaoLiquidez: this.descricaoLiquidez,
      dataVencimento: this.dataVencimento || null,
      descricaoObjetivo: this.descricaoObjetivo,
      descricaoObservacao: this.descricaoObservacao.trim()
    });

    this.investimentoService.incluirInvestimento(request)
      .pipe(take(1))
      .subscribe(() => {
        this.alertService.sucesso('Investimento criado com sucesso.');
        this.router.navigate(['/investimentos']);
      });
  }
}
