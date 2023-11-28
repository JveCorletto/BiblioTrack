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

        public List<Libros> search(String? Libro, Int64? IdAutor, Int64? IdGenero, Boolean? prestamo)
        {
            if (prestamo.Value)
            {
                var librosBuscados = context.Libros
                    .Join(context.GenerosLibros, l => l.IdLibro, gl => gl.IdLibro, (l, gl) => new { Libro = l, GeneroLibro = gl })
                    .Join(context.AutoresLibros, ll => ll.Libro.IdLibro, al => al.IdLibro, (ll, al) => new { ll.Libro, ll.GeneroLibro, AutorLibro = al })
                    .Where(result =>
                        result.Libro.Libro.ToUpper().Contains(Libro.ToUpper()) && 
                        result.Libro.IdEstado == 1 && result.Libro.Cantidad > 0 &&
                        (IdAutor == 0 || result.AutorLibro.IdAutor == IdAutor) &&
                        (IdGenero == 0 || result.GeneroLibro.IdGenero == IdGenero))
                    .Select(result => result.Libro).Distinct().ToList();

                return librosBuscados;
            }
            else
            {
                var librosBuscados = context.Libros
                    .Join(context.GenerosLibros, l => l.IdLibro, gl => gl.IdLibro, (l, gl) => new { Libro = l, GeneroLibro = gl })
                    .Join(context.AutoresLibros, ll => ll.Libro.IdLibro, al => al.IdLibro, (ll, al) => new { ll.Libro, ll.GeneroLibro, AutorLibro = al })
                    .Where(result =>
                        result.Libro.Libro.ToUpper().Contains(Libro.ToUpper()) && result.Libro.IdEstado == 1 &&
                        (IdAutor == 0 || result.AutorLibro.IdAutor == IdAutor) &&
                        (IdGenero == 0 || result.GeneroLibro.IdGenero == IdGenero))
                    .Select(result => result.Libro).Distinct().ToList();

                return librosBuscados;
            }
        }

        public List<Libros> searchInactivos(String? Libro, Int64? IdAutor, Int64? IdGenero)
        {
            var librosBuscados = context.Libros
                    .Join(context.GenerosLibros, l => l.IdLibro, gl => gl.IdLibro, (l, gl) => new { Libro = l, GeneroLibro = gl })
                    .Join(context.AutoresLibros, ll => ll.Libro.IdLibro, al => al.IdLibro, (ll, al) => new { ll.Libro, ll.GeneroLibro, AutorLibro = al })
                    .Where(result =>
                        result.Libro.Libro.ToUpper().Contains(Libro.ToUpper()) && result.Libro.IdEstado == 2 &&
                        (IdAutor == 0 || result.AutorLibro.IdAutor == IdAutor) &&
                        (IdGenero == 0 || result.GeneroLibro.IdGenero == IdGenero))
                    .Select(result => result.Libro).Distinct().ToList();

            return librosBuscados;
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

        public void Activate(long? IdLibro)
        {
            Libros libro = context.Libros.FirstOrDefault(l => l.IdLibro == IdLibro);
            libro.IdEstado = 1;
            context.SaveChanges();
        }

        public void deactivate(long? IdLibro)
        {
            Libros libro = context.Libros.FirstOrDefault(l => l.IdLibro == IdLibro);
            libro.IdEstado = 2;
            context.SaveChanges();
        }

        public void restarUnidad(long? IdLibro)
        {
            Libros libros = context.Libros.FirstOrDefault(l => l.IdLibro == IdLibro);

            libros.Cantidad--;
            context.SaveChanges();
        }

        public void agregarUnidad(long? IdLibro)
        {
            Libros libros = context.Libros.FirstOrDefault(l => l.IdLibro == IdLibro);

            libros.Cantidad++;
            context.SaveChanges();
        }
    }
}