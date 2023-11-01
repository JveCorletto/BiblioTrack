using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.MenusService
{
    public class MenusService : iMenusService
    {
        private readonly DataContext context;
        public MenusService(DataContext context)
        {
            this.context = context;
        }

        public void Create(Menus entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Menus entity)
        {
            throw new NotImplementedException();
        }

        public List<Menus> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Menus entity)
        {
            throw new NotImplementedException();
        }

        public Menus getById(long? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Menus> getMenuByRol(int? IdRol)
        {
            return context.Link_Rol_Menu
                .Where(x => x.IdRol == IdRol)
                .OrderBy(x => x.IdMenu)
                .Select(s => s.Menu).ToList();
        }
    }
}