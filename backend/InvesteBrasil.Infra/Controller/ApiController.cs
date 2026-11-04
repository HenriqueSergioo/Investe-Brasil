using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace InvesteBrasil.Infra.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiController : ControllerBase
    {
        protected bool TentarObterCodigoUsuario(out long codigoUsuario)
        {
            codigoUsuario = 0;
            var valor = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return long.TryParse(valor, out codigoUsuario) && codigoUsuario > 0;
        }
    }
}
