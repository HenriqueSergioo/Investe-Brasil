import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { take } from 'rxjs';
import { AlertService } from '../../shared/alert.service';
import { SessionService } from '../../shared/session.service';
import { LoginService } from './login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  usuario = '';
  senha = '';
  carregando = false;

  constructor(
    private loginService: LoginService,
    private session: SessionService,
    private alertService: AlertService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.session.autenticado) {
      this.router.navigate(['/investimentos']);
    }
  }

  acessar(): void {
    if (!this.usuario.trim() || !this.senha) {
      this.alertService.erro('Informe usuário e senha.');
      return;
    }

    this.carregando = true;
    this.loginService.entrar(this.usuario.trim(), this.senha)
      .pipe(take(1))
      .subscribe({
        next: retorno => {
          this.session.gravar(retorno);
          this.router.navigate(['/investimentos']);
        },
        error: erro => {
          this.carregando = false;
          const mensagem = typeof erro.error === 'string' && erro.error
            ? erro.error
            : 'Não foi possível entrar.';
          this.alertService.erro(mensagem);
        }
      });
  }
}
