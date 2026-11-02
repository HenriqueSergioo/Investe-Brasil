import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AlertService } from './alert.service';
import { SessionService } from './session.service';

@Injectable()
export class HttpRequestInterceptor implements HttpInterceptor {
  constructor(
    private alertService: AlertService,
    private session: SessionService,
    private router: Router
  ) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.session.token;
    const requisicao = token
      ? request.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
      : request;

    return next.handle(requisicao).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          this.session.limpar();
          this.router.navigate(['/login']);
        }

        const mensagem = typeof error.error === 'string' && error.error
          ? error.error
          : 'Não foi possível concluir a operação.';

        if (!request.url.includes('Autenticacao/Entrar') && !request.url.includes('ConsultarMercado')) {
          this.alertService.erro(mensagem);
        }

        return throwError(() => error);
      })
    );
  }
}
