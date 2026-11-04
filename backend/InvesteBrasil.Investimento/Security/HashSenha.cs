using System.Security.Cryptography;
using System.Text;

namespace InvesteBrasil.Investimento.Security
{
    public static class HashSenha
    {
        public static string Gerar(string senha)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
            return Convert.ToHexString(bytes);
        }
    }
}
