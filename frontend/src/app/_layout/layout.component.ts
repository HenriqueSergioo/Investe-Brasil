import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { RecursosMenu } from '../shared/recursos-menu';
import { SessionService } from '../shared/session.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  menu = RecursosMenu;

  constructor(
    private session: SessionService,
    private router: Router
  ) {}

  get nome(): string {
    return this.session.sessao?.nome ?? '';
  }

  tituloPagina(): string {
    if (this.router.url.includes('/novo')) {
      return 'Novo investimento';
    }

    if (this.router.url.includes('/resgatar')) {
      return 'Resgatar';
    }

    if (this.router.url.includes('/mercado')) {
      return 'Mercado';
    }

    return 'Investimentos';
  }

  sair(): void {
    this.session.limpar();
    this.router.navigate(['/login']);
  }
}
