using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.UsuariosService
{
    public interface iUsuariosService : CRUD<Usuarios> 
    {
        void activateUser(Usuarios usuario);
        void deactivateUser(Usuarios usuario);
        Usuarios getTokenActual(string token);
        Usuarios getUserInfo(string UserName);
        void changePassword(Int64? IdUsuario, String newPassword);
        Usuarios LogIn(String usuario, String Contrasenia, IConfiguration configuration);
        void LogOut(Usuarios usuario);

        #region Empleados

        List<Usuarios> getEmpleados();
        List<Usuarios> getEmpleadosInactivos();
        void UpdateEmpleado(Usuarios newData, Usuarios oldData);

        #endregion

        #region Usuarios

        List<Usuarios> getUsuarios();
        List<Usuarios> getUsuariosInactivos();
        void UpdateUsuario(Usuarios newData, Usuarios oldData);

        #endregion
    }
}