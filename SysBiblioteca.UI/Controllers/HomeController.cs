using SysBiblioteca.UI.Models;
using Microsoft.AspNetCore.Mvc;
using SysBiblioteca.UI.Management;
using Microsoft.Extensions.Options;

namespace SysBiblioteca.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly API_Configs _configs;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IOptions<API_Configs> options)
        {
            _logger = logger;
            _configs = options.Value;
        }

        public IActionResult Index()
        {
            ViewBag.apiURL = _configs.URL.ToString();
            ViewBag.IsAvailable = HttpContext.Session.IsAvailable;
            if (HttpContext.Session.GetString("Rol") != null)
            {
                return RedirectToAction("setProfile", "Home");
            }
            else
            {
                return View();
            }
        }

        [Route("setProfile")]
        public IActionResult setProfile()
        {
            string Rol = HttpContext.Session.GetString("Rol");
            if (Rol != null)
            {
                return RedirectToAction("Index", "SysBiblioteca");

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [Route("activateSesion")]
        public IActionResult activateSesion([FromBody] Usuarios _usuario)
        {
            if (_usuario != null)
            {
                HttpContext.Session.SetString("Rol", crypto.Decrypt(_usuario.Rol));
                HttpContext.Session.SetString("Usuario", _usuario.Usuario);
                return Ok(new { Resultado = 1 });
            }
            else
            {
                string Mensaje = "Solicitud no procesada";
                int Resultado = 0;
                return Ok(new { Mensaje, Resultado });
            }
        }

        [HttpGet]
        [Route("deactivateSesion")]
        public IActionResult deactivateSesion()
        {
            HttpContext.Session.Remove("Rol");
            HttpContext.Session.Remove("Usuario");
            return Ok(new { Resultado = 1 });
        }
    }
}