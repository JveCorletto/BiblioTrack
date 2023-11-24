using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.RolesService
{
    public interface iRolesService : CRUD<Roles> 
    {
        List<Roles> Read();
        List<Roles> ReadInactivos();
    }

}