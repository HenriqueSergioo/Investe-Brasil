using InvesteBrasil.Infra.Controller;
using InvesteBrasil.Investimento.Entities.Requests;
using InvesteBrasil.Investimento.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvesteBrasil.Investimento.Api.Controllers
{
    [Authorize]
    public class InvestimentoController : ApiController
    {
        private readonly IInvestimentoService investimentoService;

        public InvestimentoController(IInvestimentoService investimentoService)
        {
            this.investimentoService = investimentoService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> IncluirInvestimento([FromBody] IncluirInvestimentoRequestEntity request)
        {
            var resultado = await investimentoService.IncluirInvestimentoAsync(request);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConsultarInvestimentos(
            [FromQuery] string? proprietario,
            [FromQuery] DateOnly? dataInicial,
            [FromQuery] DateOnly? dataFinal,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            var resultado = await investimentoService.ConsultarInvestimentosAsync(
                proprietario,
                dataInicial,
                dataFinal,
                pagina,
                tamanhoPagina);

            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpGet("[action]/{id:long}")]
        public async Task<IActionResult> ConsultarInvestimento([FromRoute] long id, [FromQuery] DateOnly? dataReferencia)
        {
            var resultado = await investimentoService.ConsultarInvestimentoAsync(id, dataReferencia);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResgatarInvestimento([FromBody] ResgatarInvestimentoRequestEntity request)
        {
            var resultado = await investimentoService.ResgatarInvestimentoAsync(request);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConsultarSaldoGeral()
        {
            var resultado = await investimentoService.ConsultarSaldoGeralAsync();
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }
    }
}
