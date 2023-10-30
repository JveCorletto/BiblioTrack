using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.MenusService
{
    public interface iMenusService : CRUD<Menus> 
    {
        IEnumerable<Menus> getMenuByRol(int? IdRol);
    }
}