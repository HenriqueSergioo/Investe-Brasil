namespace InvesteBrasil.Investimento.Entities
{
    public class UsuarioEntity
    {
        public long CodigoUsuario { get; set; }

        public string NomeUsuario { get; set; } = string.Empty;

        public string DescricaoUsuario { get; set; } = string.Empty;

        public string DescricaoSenha { get; set; } = string.Empty;

        public string DescricaoEmail { get; set; } = string.Empty;
    }
}
