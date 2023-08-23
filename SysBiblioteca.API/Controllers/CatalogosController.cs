using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.API.Models;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogosController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetStates()
        {
            var allStates = new List<Estados>() { 
                new Estados
                {
                    IdEstado = 1,
                    Estado = "Activo"
                }
            };

            return Ok(allStates);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Estados>> getAState(int id)
        {
            var allStates = new List<Estados>() {
                new Estados
                {
                    IdEstado = 1,
                    Estado = "Activo"
                }
            };

            return Ok(allStates);
        }
    }
}