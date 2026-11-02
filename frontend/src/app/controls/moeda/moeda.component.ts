import { Component, EventEmitter, forwardRef, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { formatarMoeda, mascararMoeda, parsearMoeda } from '../../shared/formatacao';

@Component({
  selector: 'app-moeda',
  templateUrl: './moeda.component.html',
  styleUrls: ['./moeda.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MoedaComponent),
      multi: true
    }
  ]
})
export class MoedaComponent implements ControlValueAccessor {
  @Input() titulo = '';
  @Input() name = 'valor';
  @Input() placeholder = 'R$ 0,00';
  @Input() required = false;
  @Input() disabled = false;
  @Output() valorAlterado = new EventEmitter<number | null>();

  texto = '';

  private onChange: (valor: number | null) => void = () => undefined;
  private onTouched: () => void = () => undefined;

  writeValue(valor: number | null | undefined): void {
    this.texto = valor == null || Number.isNaN(valor) ? '' : formatarMoeda(valor);
  }

  registerOnChange(fn: (valor: number | null) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(desabilitado: boolean): void {
    this.disabled = desabilitado;
  }

  aoDigitar(campo: HTMLInputElement): void {
    this.texto = mascararMoeda(campo.value);
    campo.value = this.texto;
    this.aplicar(parsearMoeda(this.texto));
  }

  aoSair(): void {
    this.onTouched();
  }

  private aplicar(valor: number | null): void {
    this.onChange(valor);
    this.valorAlterado.emit(valor);
  }
}
