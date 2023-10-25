using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers
{
    public class SysBibliotecaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}