namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class PaginacaoEntity<T>
    {
        public IEnumerable<T> Itens { get; set; } = Enumerable.Empty<T>();

        public int Pagina { get; set; }

        public int TamanhoPagina { get; set; }

        public int TotalItens { get; set; }

        public int TotalPaginas { get; set; }
    }
}
