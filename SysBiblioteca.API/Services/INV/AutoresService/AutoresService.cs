using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.AutoresService
{
    public class AutoresService : iAutoresService
    {
        private readonly DataContext context;
        public AutoresService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Autores entity)
        {
            context.Autores.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Autores entity)
        {
            context.Autores.Add(entity);
            context.SaveChanges();
        }

        public Autores getById(long? id)
        {
            return context.Autores.FirstOrDefault(a => a.IdAutor == id);
        }

        public List<Autores> Read()
        {
            return context.Autores.ToList();
        }

        public void Update(Autores entity)
        {
            context.Autores.Update(entity);
            context.SaveChanges();
        }

        #endregion

        public Autores getByName(string? name)
        {
            return context.Autores.FirstOrDefault(a => a.Autor.ToUpper().Trim().Contains(name.ToUpper().Trim()));
        }
    }
}