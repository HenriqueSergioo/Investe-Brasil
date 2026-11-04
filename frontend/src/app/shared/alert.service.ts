import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type TipoAlerta = 'sucesso' | 'alerta';

export interface Alerta {
  id: number;
  tipo: TipoAlerta;
  mensagem: string;
}

export interface Confirmacao {
  titulo: string;
  mensagem: string;
  textoConfirmar: string;
  perigo: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AlertService {
  private sequencia = 0;
  private resolverConfirmacao?: (confirmou: boolean) => void;
  private readonly subject = new BehaviorSubject<Alerta[]>([]);
  private readonly confirmacaoSubject = new BehaviorSubject<Confirmacao | null>(null);

  readonly alertas$ = this.subject.asObservable();
  readonly confirmacao$ = this.confirmacaoSubject.asObservable();

  sucesso(mensagem: string): void {
    this.adicionar('sucesso', mensagem);
  }

  alerta(mensagem: string): void {
    this.adicionar('alerta', mensagem);
  }

  confirmar(
    mensagem: string,
    opcoes?: { titulo?: string; textoConfirmar?: string; perigo?: boolean }
  ): Promise<boolean> {
    this.encerrarConfirmacao(false);

    return new Promise(resolve => {
      this.resolverConfirmacao = resolve;
      this.confirmacaoSubject.next({
        titulo: opcoes?.titulo ?? 'Confirmar',
        mensagem,
        textoConfirmar: opcoes?.textoConfirmar ?? 'Confirmar',
        perigo: opcoes?.perigo ?? false
      });
    });
  }

  responderConfirmacao(confirmou: boolean): void {
    this.encerrarConfirmacao(confirmou);
  }

  fechar(id: number): void {
    this.subject.next(this.subject.value.filter(item => item.id !== id));
  }

  private encerrarConfirmacao(confirmou: boolean): void {
    const resolver = this.resolverConfirmacao;
    this.resolverConfirmacao = undefined;
    this.confirmacaoSubject.next(null);
    resolver?.(confirmou);
  }

  private adicionar(tipo: TipoAlerta, mensagem: string): void {
    const id = ++this.sequencia;
    this.subject.next([...this.subject.value, { id, tipo, mensagem }]);
    window.setTimeout(() => this.fechar(id), 4500);
  }
}
