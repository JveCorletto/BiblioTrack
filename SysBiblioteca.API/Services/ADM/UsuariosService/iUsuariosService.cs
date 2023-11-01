using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.UsuariosService
{
    public interface iUsuariosService : CRUD<Usuarios> 
    {
        Usuarios getTokenActual(string token);
        Usuarios getUserInfo(string UserName);
        Usuarios LogIn(String usuario, String Contrasenia, IConfiguration configuration);
        void LogOut(Usuarios usuario);


        List<Usuarios> getEmpleados();
    }
}