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
  cadastrando = false;
  nome = '';
  usuario = '';
  email = '';
  senha = '';
  confirmarSenha = '';
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

  mostrarCadastro(): void {
    this.cadastrando = true;
    this.carregando = false;
    this.senha = '';
    this.confirmarSenha = '';
  }

  mostrarLogin(): void {
    this.cadastrando = false;
    this.carregando = false;
    this.senha = '';
    this.confirmarSenha = '';
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
          this.alertService.erro(this.mensagemErro(erro, 'Não foi possível entrar.'));
        }
      });
  }

  cadastrar(): void {
    if (!this.nome.trim() || !this.usuario.trim() || !this.email.trim() || !this.senha) {
      this.alertService.erro('Preencha nome, usuário, e-mail e senha.');
      return;
    }

    if (this.senha !== this.confirmarSenha) {
      this.alertService.erro('A confirmação da senha não confere.');
      return;
    }

    this.carregando = true;
    this.loginService.cadastrar(this.nome.trim(), this.usuario.trim(), this.email.trim(), this.senha)
      .pipe(take(1))
      .subscribe({
        next: retorno => {
          this.session.gravar(retorno);
          this.router.navigate(['/investimentos']);
        },
        error: erro => {
          this.carregando = false;
          this.alertService.erro(this.mensagemErro(erro, 'Não foi possível cadastrar.'));
        }
      });
  }

  private mensagemErro(erro: { error?: unknown }, padrao: string): string {
    return typeof erro.error === 'string' && erro.error ? erro.error : padrao;
  }
}
