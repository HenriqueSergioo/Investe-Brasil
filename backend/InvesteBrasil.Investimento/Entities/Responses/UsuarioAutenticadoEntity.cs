namespace InvesteBrasil.Investimento.Entities.Responses
{
    public class UsuarioAutenticadoEntity
    {
        public long CodigoUsuario { get; set; }

        public string NomeUsuario { get; set; } = string.Empty;

        public string DescricaoUsuario { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;
    }
}
