using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.LinkRolMenuService
{
    public class LinkRolMenuService : iLinkRolMenuService
    {
        private readonly DataContext context;
        public LinkRolMenuService(DataContext context)
        {
            this.context = context;
        }

        public void Create(Link_Rol_Menu entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Link_Rol_Menu entity)
        {
            throw new NotImplementedException();
        }

        public List<Link_Rol_Menu> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Link_Rol_Menu entity)
        {
            throw new NotImplementedException();
        }

        public Link_Rol_Menu validateVista(int? IdRol, string URL)
        {
            Menus menu = context.Menus.FirstOrDefault(m => m.Url == URL);

            if (menu != null)
            {
                return context.Link_Rol_Menu
                    .Include(m => m.Menu)
                    .FirstOrDefault(l => l.IdRol == IdRol && l.IdMenu == menu.IdMenu);
            }
            else
            {
                return null;
            }
        }
    }
}