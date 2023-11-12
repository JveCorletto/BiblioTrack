using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers.PersonalInterno
{
    public class PrestamosDevolucionesController : Controller
    {
        public IActionResult Devoluciones()
        {
            return View();
        }
    }
}
