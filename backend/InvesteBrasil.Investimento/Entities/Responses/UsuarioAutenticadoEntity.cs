namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class UsuarioAutenticadoEntity
    {
        public long Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Usuario { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
