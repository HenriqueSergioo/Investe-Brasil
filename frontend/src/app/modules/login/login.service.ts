import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { ServiceBase } from '../../shared/service-base';
import { UsuarioAutenticadoEntity } from './entities/usuario-autenticado.entity';

@Injectable({
  providedIn: 'root'
})
export class LoginService extends ServiceBase {
  constructor(private http: HttpClient) {
    super();
  }

  entrar(usuario: string, senha: string): Observable<UsuarioAutenticadoEntity> {
    return this.http.post<UsuarioAutenticadoEntity>(
      `${environment.apiInvestimentos}Autenticacao/Entrar`,
      { usuario, senha }
    );
  }
}
