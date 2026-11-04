import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  private ativos = 0;
  private readonly subject = new BehaviorSubject<boolean>(false);
  readonly visivel$ = this.subject.asObservable();

  iniciar(): void {
    this.ativos++;
    this.subject.next(true);
  }

  terminar(): void {
    this.ativos = Math.max(0, this.ativos - 1);
    if (this.ativos === 0) {
      this.subject.next(false);
    }
  }
}
