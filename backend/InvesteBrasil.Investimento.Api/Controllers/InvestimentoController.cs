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
            if (!TentarObterCodigoUsuario(out var codigoUsuario))
                return Unauthorized();

            var resultado = await investimentoService.IncluirInvestimentoAsync(codigoUsuario, request);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConsultarInvestimentos(
            [FromQuery] string? nomeProprietario,
            [FromQuery] DateOnly? dataInicial,
            [FromQuery] DateOnly? dataFinal,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            if (!TentarObterCodigoUsuario(out var codigoUsuario))
                return Unauthorized();

            var resultado = await investimentoService.ConsultarInvestimentosAsync(
                codigoUsuario,
                nomeProprietario,
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
            if (!TentarObterCodigoUsuario(out var codigoUsuario))
                return Unauthorized();

            var resultado = await investimentoService.ConsultarInvestimentoAsync(codigoUsuario, id, dataReferencia);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ResgatarInvestimento([FromBody] ResgatarInvestimentoRequestEntity request)
        {
            if (!TentarObterCodigoUsuario(out var codigoUsuario))
                return Unauthorized();

            var resultado = await investimentoService.ResgatarInvestimentoAsync(codigoUsuario, request);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ConsultarSaldoGeral()
        {
            if (!TentarObterCodigoUsuario(out var codigoUsuario))
                return Unauthorized();

            var resultado = await investimentoService.ConsultarSaldoGeralAsync(codigoUsuario);
            if (resultado.OcorreuErro)
                return BadRequest(resultado.MensagemErro);

            return Ok(resultado.Retorno);
        }
    }
}
