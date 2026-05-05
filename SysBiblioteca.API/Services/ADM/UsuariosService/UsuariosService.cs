using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using Microsoft.EntityFrameworkCore;

namespace SysBiblioteca.API.Services.ADM.UsuariosService
{
    public class UsuariosService : iUsuariosService
    {
        private readonly DataContext context;
        public UsuariosService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void activateUser(Usuarios usuario)
        {
            usuario.IdEstado = 1;
            context.SaveChanges();
        }

        public void deactivateUser(Usuarios usuario)
        {
            usuario.IdEstado = 2;
            context.SaveChanges();
        }

        public void Create(Usuarios entity)
        {
            context.Usuarios.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Usuarios entity)
        {
            throw new NotImplementedException();
        }

        public List<Usuarios> Read()
        {
            throw new NotImplementedException();
        }

        public Usuarios getById(long? id)
        {
            return context.Usuarios
                .Include(c => c.Cargo)
                .Include(r => r.Rol)
                .Include(g => g.DatosPersonales.Genero)
                .FirstOrDefault(x => x.IdUsuario == id);
        }

        public void Update(Usuarios entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Session Management

        public Usuarios getTokenActual(string token)
        {
            return context.Usuarios
                .Include(r => r.Rol)
                .FirstOrDefault(e => e.Token == token);
        }

        public Usuarios getUserInfo(string UserName)
        {
            return context.Usuarios
                .Include(r => r.Rol)
                .FirstOrDefault(e => e.Usuario == UserName);
        }

        public Usuarios LogIn(String usuario, String Contrasenia, IConfiguration configuration)
        {
            JWT_Handler _jwtHandler = new JWT_Handler(configuration);
            Usuarios _user = context.Usuarios
                .Include(r => r.Rol)
                .FirstOrDefault(e => e.Usuario == usuario && e.Contrasenia == crypto.Encrypt(Contrasenia) && e.IdEstado == 1);

            if (_user != null)
            {
                _user.ConteoIntentos = 0;
                _user.UltimoAcceso = DateTime.Now;
                _user.Token = _jwtHandler.GenerateToken(_user);
                context.SaveChanges();
                return _user;
            }
            else
            {
                Usuarios _counter = context.Usuarios
                    .FirstOrDefault(e => e.Usuario == usuario && e.IdEstado == 1);

                //Este fue el ultimo intento, el usuario se bloquea
                if (_counter.ConteoIntentos == 2)
                {
                    _counter.ConteoIntentos = 3;
                    _counter.IdEstado = 2;
                    context.SaveChanges();
                }
                //solo se suma un intento de logeo
                else
                {
                    _counter.ConteoIntentos = (_counter.ConteoIntentos != null ? _counter.ConteoIntentos : 0) + 1;
                    context.SaveChanges();
                }
                return _counter;
            }
        }

        public void LogOut(Usuarios usuario)
        {
            usuario.Token = null;
            context.SaveChanges();
        }

        #endregion

        #region Empleados

        public List<Usuarios> getEmpleados()
        {
            return context.Usuarios
                .Include(r => r.Rol)
                .Include(c => c.Cargo)
                .Include(g => g.DatosPersonales.Genero)
                .Where(u => u.IdCargo != null && u.IdEstado == 1)
                .ToList();
        }

        public List<Usuarios> getEmpleadosInactivos()
        {
            return context.Usuarios
                .Include(r => r.Rol)
                .Include(c => c.Cargo)
                .Include(g => g.DatosPersonales.Genero)
                .Where(u => u.IdCargo != null && u.IdEstado == 2)
                .ToList();
        } 

        public void UpdateEmpleado(Usuarios newData, Usuarios oldData)
        {
            oldData.IdRol = newData.IdRol;
            oldData.IdCargo = newData.IdCargo;

            DatosPersonales oldPersonalData = context.DatosPersonales.FirstOrDefault(d => d.IdDatosPersonales == oldData.IdDatosPersonales);

            oldPersonalData.IdGenero = newData.DatosPersonales.IdGenero;
            oldPersonalData.Nombres = newData.DatosPersonales.Nombres;
            oldPersonalData.Apellidos = newData.DatosPersonales.Apellidos;
            oldPersonalData.DUI = newData.DatosPersonales.DUI;
            oldPersonalData.Correo = newData.DatosPersonales.Correo;
            oldPersonalData.Direccion = newData.DatosPersonales.Direccion;
            oldPersonalData.Telefono = newData.DatosPersonales.Telefono;
            oldPersonalData.FechaNacimiento = newData.DatosPersonales.FechaNacimiento;

            context.SaveChanges();
        }

        #endregion

        #region Usuarios
        public List<Usuarios> getUsuarios()
        {
            return context.Usuarios
                .Include(r => r.Rol)
                .Include(c => c.Cargo)
                .Include(g => g.DatosPersonales.Genero)
                .Where(u => u.IdCargo == null && u.IdEstado == 1)
                .ToList();
        }

        public List<Usuarios> getUsuariosInactivos()
        {
            return context.Usuarios
                .Include(r => r.Rol)
                .Include(c => c.Cargo)
                .Include(g => g.DatosPersonales.Genero)
                .Where(u => u.IdCargo == null && u.IdEstado == 2)
                .ToList();
        }

        public void UpdateUsuario(Usuarios newData, Usuarios oldData)
        {
            oldData.IdRol = newData.IdRol;
            oldData.IdCargo = newData.IdCargo;

            DatosPersonales oldPersonalData = context.DatosPersonales.FirstOrDefault(d => d.IdDatosPersonales == oldData.IdDatosPersonales);
            oldPersonalData.IdGenero = newData.DatosPersonales.IdGenero;
            oldPersonalData.Nombres = newData.DatosPersonales.Nombres;
            oldPersonalData.Apellidos = newData.DatosPersonales.Apellidos;
            oldPersonalData.DUI = newData.DatosPersonales.DUI;
            oldPersonalData.Correo = newData.DatosPersonales.Correo;
            oldPersonalData.Direccion = newData.DatosPersonales.Direccion;
            oldPersonalData.Telefono = newData.DatosPersonales.Telefono;
            oldPersonalData.FechaNacimiento = newData.DatosPersonales.FechaNacimiento;

            context.SaveChanges();
        }

        public void changePassword(long? IdUsuario, string newPassword)
        {
            Usuarios user = context.Usuarios.FirstOrDefault(u => u.IdUsuario == IdUsuario);
            user.Contrasenia = newPassword;
            context.SaveChanges();
        }

        #endregion
    }
}