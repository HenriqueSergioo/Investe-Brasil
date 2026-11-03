using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InvesteBrasil.Infra.Services.Abstracts;
using InvesteBrasil.Infra.Shared;
using InvesteBrasil.Infra.Shared.Interfaces;
using InvesteBrasil.Investimento.Entities;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Repositories.Postgres.Interfaces;
using InvesteBrasil.Investimento.Security;
using InvesteBrasil.Investimento.Services.Interfaces;
using InvesteBrasil.Investimento.Services.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InvesteBrasil.Investimento.Services
{
    public class AutenticacaoService : ServiceBase<AutenticacaoValidador>, IAutenticacaoService
    {
        private readonly IConfiguration configuration;
        private readonly IUsuarioRepository usuarioRepository;

        public AutenticacaoService(IConfiguration configuration, IUsuarioRepository usuarioRepository)
        {
            this.configuration = configuration;
            this.usuarioRepository = usuarioRepository;
        }

        public async Task<IResultadoValidacao<UsuarioAutenticadoEntity>> EntrarAsync(EntrarRequestEntity request)
        {
            try
            {
                var validacao = Validator.Entrar(request);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<UsuarioAutenticadoEntity>(validacao);

                var usuario = await usuarioRepository.ConsultarPorUsuarioAsync(request.Usuario);
                var senha = HashSenha.Gerar(request.Senha);
                if (usuario == null || usuario.DescricaoSenha != senha)
                    return new ResultadoValidacao<UsuarioAutenticadoEntity>("Usuário ou senha inválidos.");

                return new ResultadoValidacao<UsuarioAutenticadoEntity>(new UsuarioAutenticadoEntity
                {
                    CodigoUsuario = usuario.CodigoUsuario,
                    NomeUsuario = usuario.NomeUsuario,
                    DescricaoUsuario = usuario.DescricaoUsuario,
                    Token = GerarToken(usuario)
                });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<UsuarioAutenticadoEntity>(ex, "[AutenticacaoService] - Erro ao tentar autenticar o usuário.");
            }
        }

        public async Task<IResultadoValidacao<UsuarioAutenticadoEntity>> CadastrarUsuarioAsync(CadastrarUsuarioRequestEntity request)
        {
            try
            {
                var validacao = Validator.CadastrarUsuario(request);
                if (validacao.OcorreuErro)
                    return new ResultadoValidacao<UsuarioAutenticadoEntity>(validacao);

                var existente = await usuarioRepository.ConsultarPorUsuarioAsync(request.Usuario);
                if (existente != null)
                    return new ResultadoValidacao<UsuarioAutenticadoEntity>("Este usuário já está cadastrado.");

                var usuario = await usuarioRepository.IncluirAsync(new UsuarioEntity
                {
                    NomeUsuario = request.Nome.Trim(),
                    DescricaoUsuario = request.Usuario.Trim(),
                    DescricaoSenha = HashSenha.Gerar(request.Senha),
                    DescricaoEmail = request.Email.Trim()
                });

                return new ResultadoValidacao<UsuarioAutenticadoEntity>(new UsuarioAutenticadoEntity
                {
                    CodigoUsuario = usuario.CodigoUsuario,
                    NomeUsuario = usuario.NomeUsuario,
                    DescricaoUsuario = usuario.DescricaoUsuario,
                    Token = GerarToken(usuario)
                });
            }
            catch (Exception ex)
            {
                return new ResultadoValidacao<UsuarioAutenticadoEntity>(ex, "[AutenticacaoService] - Erro ao tentar cadastrar o usuário.");
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
                claims:
                [
                    new Claim(ClaimTypes.NameIdentifier, usuario.CodigoUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                    new Claim("usuario", usuario.DescricaoUsuario)
                ],
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
