import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UsuarioAutenticadoEntity } from './entities/usuario-autenticado.entity';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  constructor(private http: HttpClient) {}

  entrar(usuario: string, senha: string): Observable<UsuarioAutenticadoEntity> {
    return this.http.post<UsuarioAutenticadoEntity>(
      `${environment.apiInvestimentos}Autenticacao/Entrar`,
      { usuario, senha }
    );
  }

  cadastrar(nome: string, email: string, senha: string): Observable<UsuarioAutenticadoEntity> {
    return this.http.post<UsuarioAutenticadoEntity>(
      `${environment.apiInvestimentos}Autenticacao/CadastrarUsuario`,
      { nome, email, senha }
    );
  }
}
