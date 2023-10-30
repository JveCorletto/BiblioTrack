using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.LinkRolMenuService
{
    public interface iLinkRolMenuService : CRUD<Link_Rol_Menu> 
    {
        Link_Rol_Menu validateVista(int? IdRol, string URL);
    }
}