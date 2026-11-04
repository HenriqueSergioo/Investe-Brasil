import { Component, HostListener } from '@angular/core';
import { AlertService } from './shared/alert.service';
import { LoadingService } from './shared/loading.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(
    public alertService: AlertService,
    public loading: LoadingService
  ) {}

  fechar(id: number): void {
    this.alertService.fechar(id);
  }

  aceitarConfirmacao(): void {
    this.alertService.responderConfirmacao(true);
  }

  cancelarConfirmacao(): void {
    this.alertService.responderConfirmacao(false);
  }

  @HostListener('document:keydown.escape')
  aoEscapar(): void {
    this.cancelarConfirmacao();
  }
}
