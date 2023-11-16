using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.LibrosService
{
    public class LibrosService : iLibrosService
    {
        private readonly DataContext context;
        public LibrosService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Libros entity)
        {
            context.Libros.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Libros entity)
        {
            throw new NotImplementedException();
        }

        public Libros getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<Libros> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Libros entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Libros> searchByName(String? Libro)
        {
            return context.Libros.Where(l => l.IdEstado == 1 && l.Libro.ToUpper().Contains(Libro.ToUpper())).ToList();
        }
    }
}