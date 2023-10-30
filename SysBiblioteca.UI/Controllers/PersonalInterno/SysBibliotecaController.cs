using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers.PersonalInterno
{
    public class SysBibliotecaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}