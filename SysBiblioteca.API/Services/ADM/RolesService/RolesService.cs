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
            throw new NotImplementedException();
        }

        public void Delete(Roles entity)
        {
            throw new NotImplementedException();
        }

        public Roles getById(long? id)
        {
            throw new NotImplementedException();
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
    }
}