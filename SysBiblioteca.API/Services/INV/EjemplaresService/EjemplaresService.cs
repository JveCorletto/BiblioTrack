using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.EjemplaresService
{
    public class EjemplaresService : iEjemplaresService
    {
        private readonly DataContext context;
        public EjemplaresService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Ejemplares entity)
        {
            context.Ejemplares.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Ejemplares entity)
        {
            entity.Estado = false;
            entity.FechaModificacion = DateTime.Now;
            context.SaveChanges();
        }

        public Ejemplares getById(long? id)
        {
            return context.Ejemplares.FirstOrDefault(e => e.IdEjemplar == id);
        }

        public List<Ejemplares> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Ejemplares entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Ejemplares> getStock(long? IdLibro)
        {
            return context.Ejemplares.Where(e => e.IdLibro == IdLibro && e.Estado == true)
                .ToList();
        }

        public int getAvailables(long? IdLibro)
        {
            return context.Ejemplares
                .Where(e => e.IdLibro == IdLibro && e.Estado &&
                            (!context.Prestamos.Any(p => p.IdEjemplar == e.IdEjemplar) ||
                             context.Prestamos.Any(p => p.IdEjemplar == e.IdEjemplar && p.Finalizado == true)))
                .Count();
        }

        public Ejemplares getEjemplarToLoan(long? IdLibro)
        {
            return context.Ejemplares
                .Where(e => e.IdLibro == IdLibro && e.Estado &&
                            (!context.Prestamos.Any(p => p.IdEjemplar == e.IdEjemplar) ||
                             context.Prestamos.Any(p => p.IdEjemplar == e.IdEjemplar && p.Finalizado == true)))
                .FirstOrDefault();
        }

        public Ejemplares getByCodigo(string? CodigoEjemplar)
        {
            return context.Ejemplares
                .Include(e => e.Libro)
                .FirstOrDefault(e => e.CodigoEjemplar == CodigoEjemplar);
        }
    }
}