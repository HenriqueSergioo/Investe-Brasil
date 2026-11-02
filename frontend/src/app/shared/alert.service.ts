import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  sucesso(mensagem: string): void {
    window.alert(mensagem);
  }

  erro(mensagem: string): void {
    window.alert(mensagem);
  }
}
