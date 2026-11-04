import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from '../shared/alert.service';
import { RecursosMenu } from '../shared/recursos-menu';
import { SessionService } from '../shared/session.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  menu = RecursosMenu;
  menuAberto = false;

  constructor(
    private session: SessionService,
    private router: Router,
    private alertService: AlertService
  ) {}

  tituloPagina(): string {
    return 'Investimentos';
  }

  alternarMenu(): void {
    this.menuAberto = !this.menuAberto;
  }

  fecharMenu(): void {
    this.menuAberto = false;
  }

  async sair(): Promise<void> {
    const confirmou = await this.alertService.confirmar('Deseja mesmo sair da conta?');
    if (!confirmou) {
      return;
    }

    this.fecharMenu();
    this.session.limpar();
    this.router.navigate(['/login']);
  }
}
