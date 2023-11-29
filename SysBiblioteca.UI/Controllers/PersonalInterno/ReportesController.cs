using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers.PersonalInterno
{
    public class ReportesController : Controller
    {
        public IActionResult Usuarios()
        {
            return View();
        }

        public IActionResult Libros()
        {
            return View();
        }

        public IActionResult ReportView()
        {
            return View();
        }
    }
}