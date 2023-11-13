using Microsoft.AspNetCore.Mvc;

namespace SysBiblioteca.UI.Controllers.PersonalInterno
{
    public class Inventario : Controller
    {
        public IActionResult Estanteria()
        {
            return View();
        }

        public IActionResult Libros()
        {
            return View();
        }

        public IActionResult RFID()
        {
            return View();
        }
    }
}