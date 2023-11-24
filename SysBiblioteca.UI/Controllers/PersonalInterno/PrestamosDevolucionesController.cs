using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers.PersonalInterno
{
    public class PrestamosDevolucionesController : Controller
    {
        public IActionResult Prestamos()
        {
            return View();
        }

        public IActionResult Devoluciones()
        {
            return View();
        }

        public IActionResult Pagos()
        {
            return View();
        }
    }
}
