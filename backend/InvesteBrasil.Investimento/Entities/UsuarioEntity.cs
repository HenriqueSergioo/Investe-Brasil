namespace InvesteBrasil.Investimento.Entities
{
    public class UsuarioEntity
    {
        public long Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;
    }
}
