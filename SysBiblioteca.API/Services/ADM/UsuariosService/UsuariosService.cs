using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.UsuariosService
{
    public class UsuariosService : iUsuariosService
    {
        private readonly DataContext context;
        public UsuariosService(DataContext context)
        {
            this.context = context;
        }

        public void Create(Usuarios entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Usuarios entity)
        {
            throw new NotImplementedException();
        }

        public List<Usuarios> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Usuarios entity)
        {
            throw new NotImplementedException();
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

        public Usuarios LogOut(Usuarios usuario)
        {
            throw new NotImplementedException();
        }
    }
}
