using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.RolesService
{
    public class RolesService : iRolesService
    {
        private readonly DataContext context;
        public RolesService(DataContext context)
        {
            this.context = context;
        }
        public void Create(Roles entity)
        {
            context.Roles.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Roles entity)
        {
            throw new NotImplementedException();
        }

        public Roles getById(long? id)
        {
            return context.Roles.FirstOrDefault(r => r.IdRol == id);
        }

        public List<Roles> Read()
        {
            return context.Roles.Where(r => r.IdEstado == 1).ToList();
        }

        public List<Roles> ReadInactivos()
        {
            return context.Roles.Where(r => r.IdEstado == 2).ToList();
        }

        public void Update(Roles entity)
        {
            throw new NotImplementedException();
        }

        public void activateRol(Roles rol)
        {
            rol.IdEstado = 1;
            context.SaveChanges();
        }

        public void deactivateRol(Roles rol)
        {
            rol.IdEstado = 2;
            context.SaveChanges();
        }

        public void UpdateRol(Roles newData, Roles oldData)
        {
            oldData.Rol = newData.Rol;
            oldData.UsuarioModificacion = newData.UsuarioModificacion;
            oldData.FechaModificacion = DateTime.Now;
            context.SaveChanges();
        }

        public Roles getByName(string name)
        {
            return context.Roles.FirstOrDefault(r => r.Rol.ToUpper()==name.ToUpper());
        }
    }
}