import { Component, EventEmitter, forwardRef, HostListener, Input, Output } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

const MESES = [
  'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
  'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'
];

@Component({
  selector: 'app-data',
  templateUrl: './data.component.html',
  styleUrls: ['./data.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DataComponent),
      multi: true
    }
  ]
})
export class DataComponent implements ControlValueAccessor {
  @Input() titulo = '';
  @Input() name = 'data';
  @Input() placeholder = 'dd/mm/aaaa';
  @Input() required = false;
  @Input() disabled = false;
  @Input() minimo?: string;
  @Input() maximo?: string;
  @Output() valorAlterado = new EventEmitter<string>();

  valorIso = '';
  texto = '';
  aberto = false;
  mesVisivel = new Date().getMonth();
  anoVisivel = new Date().getFullYear();
  diasSemana = ['D', 'S', 'T', 'Q', 'Q', 'S', 'S'];

  private onChange: (valor: string) => void = () => undefined;
  private onTouched: () => void = () => undefined;

  get meses(): string[] {
    return MESES;
  }

  get diasDoCalendario(): Array<{ iso: string; rotulo: number; outroMes: boolean; selecionado: boolean; desabilitado: boolean }> {
    const primeiro = new Date(this.anoVisivel, this.mesVisivel, 1);
    const inicio = new Date(primeiro);
    inicio.setDate(primeiro.getDate() - primeiro.getDay());

    return Array.from({ length: 42 }, (_, indice) => {
      const data = new Date(inicio);
      data.setDate(inicio.getDate() + indice);
      const iso = this.paraIso(data);
      return {
        iso,
        rotulo: data.getDate(),
        outroMes: data.getMonth() !== this.mesVisivel,
        selecionado: iso === this.valorIso,
        desabilitado: this.foraDoLimite(iso)
      };
    });
  }

  writeValue(valor: string): void {
    this.valorIso = valor || '';
    this.texto = this.valorIso ? this.isoParaTexto(this.valorIso) : '';
    if (this.valorIso) {
      const [ano, mes] = this.valorIso.split('-').map(Number);
      this.anoVisivel = ano;
      this.mesVisivel = mes - 1;
    }
  }

  registerOnChange(fn: (valor: string) => void): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: () => void): void {
    this.onTouched = fn;
  }

  setDisabledState(desabilitado: boolean): void {
    this.disabled = desabilitado;
  }

  abrir(): void {
    if (this.disabled) {
      return;
    }

    this.aberto = true;
    this.onTouched();
  }

  fechar(): void {
    this.aberto = false;
  }

  selecionar(iso: string): void {
    if (this.foraDoLimite(iso)) {
      return;
    }

    this.aplicar(iso);
    this.fechar();
  }

  aoDigitar(texto: string): void {
    const apenasNumeros = texto.replace(/\D/g, '').slice(0, 8);
    if (apenasNumeros.length <= 2) {
      this.texto = apenasNumeros;
    } else if (apenasNumeros.length <= 4) {
      this.texto = `${apenasNumeros.slice(0, 2)}/${apenasNumeros.slice(2)}`;
    } else {
      this.texto = `${apenasNumeros.slice(0, 2)}/${apenasNumeros.slice(2, 4)}/${apenasNumeros.slice(4)}`;
    }

    if (apenasNumeros.length === 8) {
      const iso = this.textoParaIso(this.texto);
      if (iso && !this.foraDoLimite(iso)) {
        this.aplicar(iso);
        return;
      }
    }

    if (!apenasNumeros) {
      this.aplicar('');
    }
  }

  mesAnterior(): void {
    if (this.mesVisivel === 0) {
      this.mesVisivel = 11;
      this.anoVisivel -= 1;
      return;
    }

    this.mesVisivel -= 1;
  }

  proximoMes(): void {
    if (this.mesVisivel === 11) {
      this.mesVisivel = 0;
      this.anoVisivel += 1;
      return;
    }

    this.mesVisivel += 1;
  }

  limpar(): void {
    this.aplicar('');
    this.fechar();
  }

  hoje(): void {
    const iso = this.paraIso(new Date());
    if (!this.foraDoLimite(iso)) {
      this.aplicar(iso);
    }
    this.fechar();
  }

  @HostListener('document:click', ['$event'])
  fecharFora(evento: MouseEvent): void {
    const alvo = evento.target as HTMLElement;
    if (!alvo.closest('app-data')) {
      this.fechar();
    }
  }

  private aplicar(iso: string): void {
    this.valorIso = iso;
    this.texto = iso ? this.isoParaTexto(iso) : '';
    this.onChange(iso);
    this.valorAlterado.emit(iso);
  }

  private foraDoLimite(iso: string): boolean {
    if (this.minimo && iso < this.minimo) {
      return true;
    }

    return !!this.maximo && iso > this.maximo;
  }

  private paraIso(data: Date): string {
    const mes = String(data.getMonth() + 1).padStart(2, '0');
    const dia = String(data.getDate()).padStart(2, '0');
    return `${data.getFullYear()}-${mes}-${dia}`;
  }

  private isoParaTexto(iso: string): string {
    const [ano, mes, dia] = iso.split('-');
    return `${dia}/${mes}/${ano}`;
  }

  private textoParaIso(texto: string): string {
    const partes = texto.split('/');
    if (partes.length !== 3) {
      return '';
    }

    const [dia, mes, ano] = partes.map(Number);
    const data = new Date(ano, mes - 1, dia);
    if (data.getFullYear() !== ano || data.getMonth() !== mes - 1 || data.getDate() !== dia) {
      return '';
    }

    return this.paraIso(data);
  }
}
