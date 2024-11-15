using Microsoft.EntityFrameworkCore;
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

        public List<Libros> search(string? libro, long? idAutor, long? idGenero, bool? prestamo)
        {
            libro = libro?.ToUpper() ?? string.Empty;

            IQueryable<Libros> query;

            if (prestamo == true)
            {
                // Buscar libros que tengan ejemplares disponibles para préstamo (incluyendo los que nunca han sido prestados)
                query = context.Ejemplares
                    .Include(l => l.Libro.Estado)
                    .Include(l => l.Libro.Editorial)
                    .Where(e => !context.Prestamos.Any(p => p.IdEjemplar == e.IdEjemplar && p.FechaDevolucion == null))
                    .Join(context.Libros,
                          e => e.IdLibro,
                          l => l.IdLibro,
                          (e, l) => l)
                    .Where(l => l.IdEstado == 1)
                    .Distinct();
            }
            else
            {
                // Buscar todos los libros disponibles en el inventario
                query = context.Libros
                    .Include(l => l.Estado)
                    .Include(l => l.Editorial)
                    .Where(l => l.IdEstado == 1);
            }

            // Aplicar el filtro por nombre del libro
            if (!string.IsNullOrEmpty(libro))
            {
                query = query.Where(l => l.Libro.ToUpper().Contains(libro));
            }

            // Aplicar el filtro por género, si se especificó
            if (idGenero.HasValue && idGenero > 0)
            {
                query = query
                    .Join(context.GenerosLibros,
                          l => l.IdLibro,
                          gl => gl.IdLibro,
                          (l, gl) => new { Libro = l, GeneroLibro = gl })
                    .Where(result => result.GeneroLibro.IdGenero == idGenero)
                    .Select(result => result.Libro);
            }

            // Aplicar el filtro por autor, si se especificó
            if (idAutor.HasValue && idAutor > 0)
            {
                query = query
                    .Join(context.AutoresLibros,
                          l => l.IdLibro,
                          al => al.IdLibro,
                          (l, al) => new { Libro = l, AutorLibro = al })
                    .Where(result => result.AutorLibro.IdAutor == idAutor)
                    .Select(result => result.Libro);
            }

            // Convertir a lista y retornar los resultados
            return query.Distinct().ToList();
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
    }
}