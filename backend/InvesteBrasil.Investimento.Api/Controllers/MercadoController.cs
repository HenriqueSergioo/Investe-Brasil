using InvesteBrasil.Infra.Controller;
using InvesteBrasil.Investimento.Entities.Responses;
using InvesteBrasil.Investimento.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesteBrasil.Investimento.Api.Controllers
{
    [Authorize]
    public class MercadoController : ApiController
    {
        private readonly IMercadoService mercadoService;

        public MercadoController(IMercadoService mercadoService)
        {
            this.mercadoService = mercadoService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConsultarMercado()
        {
            var resultado = await mercadoService.ConsultarMercadoAsync();
            if (resultado.OcorreuErro)
                return Ok(new PainelMercadoEntity());

            return Ok(resultado.Retorno);
        }
    }
}
