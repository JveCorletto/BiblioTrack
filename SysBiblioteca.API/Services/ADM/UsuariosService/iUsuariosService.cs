using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.UsuariosService
{
    public interface iUsuariosService : CRUD<Usuarios> 
    {
        Usuarios getUserInfo(string UserName);
        Usuarios LogIn(String usuario, String Contrasenia, IConfiguration configuration);
        Usuarios LogOut(Usuarios usuario);
    }
}