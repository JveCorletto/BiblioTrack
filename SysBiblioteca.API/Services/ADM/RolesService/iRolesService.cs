using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.ADM.RolesService
{
    public interface iRolesService : CRUD<Roles> 
    {
        List<Roles> Read();
        List<Roles> ReadInactivos();

        Roles getByName(string name);

        void UpdateRol(Roles newData, Roles oldData);

        void activateRol(Roles rol);
        void deactivateRol(Roles rol);
    }

}