using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.AutoresLibrosService
{
    public class AutoresLibrosService : iAutoresLibrosService
    {
        private readonly DataContext context;
        public AutoresLibrosService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(AutoresLibros entity)
        {
            context.AutoresLibros.Add(entity);
            context.SaveChanges();
        }

        public void Delete(AutoresLibros entity)
        {
            throw new NotImplementedException();
        }

        public AutoresLibros getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<AutoresLibros> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(AutoresLibros entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Autores> getAutoresLibro(Int64? IdLibro)
        {
            return context.AutoresLibros
                .Include(a => a.Autor)
                .Where(al => al.IdLibro == IdLibro)
                .Select(al => al.Autor)
                .ToList();
        }
    }
}