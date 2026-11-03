import { Injectable } from '@angular/core';

const CHAVE = 'investebrasil.sessao';

export interface SessaoUsuario {
  codigoUsuario: number;
  nomeUsuario: string;
  descricaoUsuario: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class SessionService {
  get sessao(): SessaoUsuario | null {
    const json = sessionStorage.getItem(CHAVE);
    return json ? JSON.parse(json) : null;
  }

  get token(): string {
    return this.sessao?.token ?? '';
  }

  get autenticado(): boolean {
    return !!this.token;
  }

  gravar(sessao: SessaoUsuario): void {
    sessionStorage.setItem(CHAVE, JSON.stringify(sessao));
  }

  limpar(): void {
    sessionStorage.removeItem(CHAVE);
  }
}
