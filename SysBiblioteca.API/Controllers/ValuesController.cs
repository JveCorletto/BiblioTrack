using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.API.Management;
using Microsoft.AspNetCore.Authorization;

namespace SysBiblioteca.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        [Route("encriptString")]
        public IActionResult encriptString([FromBody] String _cadena)
        {
            try
            {
                Reply _rp = new Reply
                {
                    Resultado = 0,
                    Datos = crypto.Encrypt(_cadena)
                };

                return Ok(_rp);
            }
            catch (Exception ex)
            {
                Reply _rp = new Reply { Resultado = 0 };
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("decriptString")]
        [Authorize(Roles = "Administrador")]
        public IActionResult decriptString([FromBody] String _cadena)
        {
            try
            {
                Reply _rp = new Reply
                {
                    Resultado = 0,
                    Datos = crypto.Decrypt(_cadena)
                };

                return Ok(_rp);
            }
            catch (Exception ex)
            {
                Reply _rp = new Reply { Resultado = 0 };
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }
    }
}