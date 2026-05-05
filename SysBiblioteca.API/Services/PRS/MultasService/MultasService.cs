using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.PRS;
using Microsoft.EntityFrameworkCore;

namespace SysBiblioteca.API.Services.PRS.MultasService
{
    public class MultasService : iMultasService
    {
        private readonly DataContext context;
        public MultasService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Multas entity)
        {
            context.Multas.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Multas entity)
        {
            throw new NotImplementedException();
        }

        public Multas getById(long? id)
        {
            return context.Multas
                .Include(p => p.Prestamo.Ejemplar.Libro)
                .Include(p => p.Prestamo.Usuario)
                .FirstOrDefault(m => m.IdMulta == id);
        }

        public List<Multas> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Multas entity)
        {
            throw new NotImplementedException();
        }

        #endregion
        
        public List<Multas> getWaiting()
        {
            return context.Multas
                .Include(e => e.EstadoMulta)
                .Include(e => e.Prestamo.Ejemplar.Libro)
                .Include(e => e.Prestamo.Usuario)
                .Where(m => (new[] { 1, 2 }).Contains(m.IdEstadoMulta ?? 0)).ToList();
        }

        public List<Multas> getPaid()
        {
            return context.Multas
                .Include(e => e.EstadoMulta)
                .Include(e => e.Prestamo.Ejemplar.Libro)
                .Include(e => e.Prestamo.Usuario)
                .Include(e => e.UsuarioValidacion)
                .Where(m => m.IdEstadoMulta == 3).ToList();
        }

        public List<Multas> GetMyFines(long? IdUsuario)
        {
            return context.Multas
                .Include(e => e.EstadoMulta)
                .Include(e => e.Prestamo.Ejemplar.Libro)
                .Include(e => e.Prestamo.Usuario)
                .Where(m => (new[] { 1, 2 }).Contains(m.IdEstadoMulta ?? 0) && m.Prestamo.Usuario.IdUsuario == IdUsuario)
                .ToList();
        }

        public List<Multas> GetMyPaidFines(long? IdUsuario)
        {
            return context.Multas
                .Include(e => e.Prestamo.Ejemplar.Libro)
                .Include(e => e.Prestamo.Usuario)
                .Include(e => e.UsuarioValidacion)
                .Where(m => m.IdEstadoMulta == 3 && m.Prestamo.Usuario.IdUsuario == IdUsuario)
                .ToList();
        }

        public void cargarComprobante(Multas comprobante)
        {
            context.Multas.Update(comprobante);
            context.SaveChanges();
        }
    }
}