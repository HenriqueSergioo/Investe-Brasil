export class PaginacaoEntity<T> {
  itens: T[] = [];
  pagina = 1;
  tamanhoPagina = 10;
  totalItens = 0;
  totalPaginas = 0;
}
