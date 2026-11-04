export function formatarMoeda(valor: number | null | undefined): string {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL'
  }).format(valor ?? 0);
}

export function mascararMoeda(texto: string | null | undefined): string {
  const numeros = (texto ?? '').replace(/\D/g, '').slice(0, 13);
  if (!numeros) {
    return '';
  }

  return formatarMoeda(Number(numeros) / 100);
}

export function parsearMoeda(texto: string | null | undefined): number | null {
  const numeros = (texto ?? '').replace(/\D/g, '');
  if (!numeros) {
    return null;
  }

  return Number(numeros) / 100;
}

export function formatarData(data: string | null | undefined): string {
  if (!data) {
    return '-';
  }

  const [ano, mes, dia] = data.split('-');
  return `${dia}/${mes}/${ano}`;
}

export function hojeIso(): string {
  const agora = new Date();
  const mes = String(agora.getMonth() + 1).padStart(2, '0');
  const dia = String(agora.getDate()).padStart(2, '0');
  return `${agora.getFullYear()}-${mes}-${dia}`;
}

export function formatarPercentual(valor: number): string {
  return `${(valor * 100).toLocaleString('pt-BR', { minimumFractionDigits: 1, maximumFractionDigits: 1 })}%`;
}
