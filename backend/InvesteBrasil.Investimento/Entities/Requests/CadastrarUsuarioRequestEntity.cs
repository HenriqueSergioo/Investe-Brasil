namespace InvesteBrasil.Investimento.Entities.Requests
{
    public class CadastrarUsuarioRequestEntity
    {
        public string Nome { get; set; } = string.Empty;

        public string Senha { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
    }
}
