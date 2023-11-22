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
            return context.Libros.FirstOrDefault(l => l.IdLibro == id);
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

        public void Update(Libros oldEntity, Libros newEntity)
        {
            oldEntity.FotoLibro = newEntity.FotoLibro;
            oldEntity.Libro = newEntity.Libro;
            oldEntity.Version = newEntity.Version;
            oldEntity.ISBN = newEntity.ISBN;
            oldEntity.AnioPublicacion = newEntity.AnioPublicacion;
            oldEntity.Descripcion = newEntity.Descripcion;
            oldEntity.Cantidad = newEntity.Cantidad;
            oldEntity.IdEditorial = newEntity.IdEditorial;
            oldEntity.UsuarioModificacion = newEntity.UsuarioModificacion;
            oldEntity.FechaModificacion = newEntity.FechaModificacion;
            context.SaveChanges();
        }
    }
}