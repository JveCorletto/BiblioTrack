using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Management
{
    public class Modulo
    {
        public Menus Padre { get; set; }
        public List<Menu> Hijos { get; set; }
    }
}
