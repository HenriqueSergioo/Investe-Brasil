using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using InvesteBrasil.Infra.Services.Abstracts;
using InvesteBrasil.Infra.Shared;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Services.Interfaces;
using InvesteBrasil.Investimento.Services.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InvesteBrasil.Investimento.Services
{
    public class AutenticacaoService : ServiceBase<AutenticacaoValidador>, IAutenticacaoService
    {
        private readonly IConfiguration configuration;
        private static readonly IReadOnlyList<UsuarioEntity> Usuarios = new List<UsuarioEntity>
        {
            new() { Id = 1, Nome = "Henrique Santos", Usuario = "henrique", Senha = GerarHash("123456") },
            new() { Id = 2, Nome = "Administrador", Usuario = "admin", Senha = GerarHash("admin123") }
        };

        public AutenticacaoService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IResultadoValidacao<UsuarioAutenticadoEntity> Entrar(EntrarRequestEntity request)
        {
            try
            {
                var validacao = Validator.Entrar(request);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<UsuarioAutenticadoEntity>(validacao);

                var senha = GerarHash(request.Senha);
                var usuario = Usuarios.FirstOrDefault(x =>
                    x.Usuario.Equals(request.Usuario.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    x.Senha == senha);

                if (usuario == null)
                    return new ResultadoValidacao<UsuarioAutenticadoEntity>("Usuário ou senha inválidos.");

                return new ResultadoValidacao<UsuarioAutenticadoEntity>(new UsuarioAutenticadoEntity
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Usuario = usuario.Usuario,
                    Token = GerarToken(usuario)
                });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<UsuarioAutenticadoEntity>(ex, "[AutenticacaoService] - Erro ao tentar autenticar o usuário.");
            }
        }

        private string GerarToken(UsuarioEntity usuario)
        {
            var jwt = configuration.GetSection("Jwt");
            var chave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key não configurada.")));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim("usuario", usuario.Usuario)
                },
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private static string GerarHash(string senha)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(senha));
            return Convert.ToHexString(bytes);
        }
    }
}
