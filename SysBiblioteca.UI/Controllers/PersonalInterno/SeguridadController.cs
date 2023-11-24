using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers.PersonalInterno
{
    public class SeguridadController : Controller
    {
        public IActionResult Usuarios()
        {
            return View();
        }

        public IActionResult Empleados()
        {
            return View();
        }

        public IActionResult Permisos()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View();
        }
    }
}