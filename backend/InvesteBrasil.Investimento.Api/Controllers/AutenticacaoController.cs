using InvesteBrasil.Infra.Controller;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesteBrasil.Investimento.Api.Controllers
{
    public class AutenticacaoController : ApiController
    {
        private readonly IAutenticacaoService autenticacaoService;

        public AutenticacaoController(IAutenticacaoService autenticacaoService)
        {
            this.autenticacaoService = autenticacaoService;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Entrar([FromBody] EntrarRequestEntity request)
        {
            var resultado = autenticacaoService.Entrar(request);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }
    }
}
