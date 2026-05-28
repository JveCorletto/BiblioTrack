using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Models.ADM;
using Microsoft.EntityFrameworkCore;

namespace SysBiblioteca.API.Services.PRS.PrestamosService
{
    public class PrestamosService : iPrestamosService
    {
        private readonly DataContext context;
        public PrestamosService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Prestamos entity)
        {
            context.Prestamos.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Prestamos entity)
        {
            throw new NotImplementedException();
        }

        public Prestamos getById(long? id)
        {
            return context.Prestamos
                .Include(l => l.Ejemplar.Libro)
                .Include(l => l.Usuario.DatosPersonales)
                .Include(l => l.UsuarioEntrego)
                .FirstOrDefault(p => p.IdPrestamo == id);
        }

        public List<Prestamos> Read()
        {
            return context.Prestamos
                .Include(x => x.Ejemplar.Libro)
                .Include(u => u.Usuario)
                .Where(p => p.Finalizado == false)
                .ToList();
        }

        public void Update(Prestamos entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Prestamos> GetMyBooks(long? IdUsuario)
        {
            return context.Prestamos
                .Include(x => x.Ejemplar.Libro)
                .Where(p => p.Finalizado == false && p.Entregado == true && p.IdUsuario == IdUsuario)
                .ToList();
        }

        public List<Prestamos> GetPendingLoans()
        {
            return context.Prestamos
                .Include(p => p.Ejemplar)
                    .ThenInclude(e => e.Libro)
                .Include(p => p.Usuario)
                .Where(p => p.Finalizado == false && p.Entregado == false)
                .ToList();
        }

        public List<Prestamos> GetOngoingLoans()
        {
            return context.Prestamos
                .Include(x => x.Ejemplar.Libro)
                .Include(u => u.Usuario)
                .Include(u => u.UsuarioEntrego)
                .Where(p => p.Finalizado == false && p.Entregado == true)
                .ToList();
        }

        public List<Usuarios> getUserForLoans()
        {
            return context.Usuarios
                .Include(d => d.DatosPersonales)
                .Include(d => d.DatosPersonales.Genero)
                .Where(u => u.IdEstado == 1 && u.Cargo == null)
                .ToList();
        }

        public Prestamos validatePrestamo(Int64? IdLibro, Int64? IdUsuario)
        {
            return context.Prestamos
                .OrderByDescending(p => p.IdPrestamo)
                .FirstOrDefault(p => p.Ejemplar.IdLibro == IdLibro && p.IdUsuario == IdUsuario && p.Finalizado == false);
        }

        public void LoanBook(Int64? IdPrestamo, Int64? IdUsuarioEntrego)
        {
            Prestamos prestamo = context.Prestamos.FirstOrDefault(p => p.IdPrestamo == IdPrestamo);
            prestamo.Entregado = true;
            prestamo.IdUsuarioEntrego = IdUsuarioEntrego;
            prestamo.FechaPrestamo = DateTime.Now;
            context.SaveChanges();
        }

        public void MarkAsFinished(long? IdPrestamo, long? IdUsuario)
        {
            Prestamos prestamo = context.Prestamos.FirstOrDefault(p => p.IdPrestamo == IdPrestamo);

            prestamo.Finalizado = true;
            prestamo.FechaDevolucion = DateTime.Now;
            prestamo.IdUsuarioRecibio = IdUsuario;
            context.SaveChanges();
        }

        public List<Prestamos> GetFinishedLoans()
        {
            return context.Prestamos
                .Include(x => x.Ejemplar.Libro)
                .Include(u => u.Usuario)
                .Include(u => u.UsuarioEntrego)
                .Include(u => u.UsuarioRecibio)
                .Where(p => p.Finalizado == true && p.Entregado == true)
                .ToList();
        }

        public List<Prestamos> GetFinishedLoans(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            return context.Prestamos
               .Include(x => x.Ejemplar.Libro)
               .Include(u => u.Usuario)
               .Include(u => u.UsuarioEntrego)
               .Include(u => u.UsuarioRecibio)
               .Where(p => p.Finalizado == true && p.Entregado == true
                    && Convert.ToDateTime(p.FechaDevolucion.Value.ToShortDateString()) >= Convert.ToDateTime(FechaDesde.Value.ToShortDateString())
                    && Convert.ToDateTime(p.FechaDevolucion.Value.ToShortDateString()) >= Convert.ToDateTime(FechaHasta.Value.ToShortDateString()))
               .ToList();
        }

        public Prestamos getLastLoanByEjemplar(long? IdEjemplar)
        {
            return context.Prestamos
                .Include(e => e.Ejemplar.Libro)
                .Include(e => e.Usuario.DatosPersonales)
                .Where(p => p.IdEjemplar == IdEjemplar)
                .OrderByDescending(p => p.IdPrestamo)
                .FirstOrDefault();
        }
    }
}