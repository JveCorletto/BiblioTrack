using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.PRS;

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
            throw new NotImplementedException();
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

        public List<Multas> getUnPaid()
        {
            return context.Multas
                .Include(e => e.EstadoMulta)
                .Include(e => e.Prestamo)
                .Include(e => e.Prestamo.Usuario)
                .Include(e => e.UsuarioValidacion)
                .Where(m => m.IdEstadoMulta == 1).ToList();
        }

        public List<Multas> getWaiting()
        {
            return context.Multas
                .Include(e => e.EstadoMulta)
                .Include(e => e.Prestamo)
                .Include(e => e.Prestamo.Usuario)
                .Include(e => e.UsuarioValidacion)
                .Where(m => m.IdEstadoMulta == 2).ToList();
        }

        public List<Multas> getPaid()
        {
            return context.Multas
                .Include(e => e.EstadoMulta)
                .Include(e => e.Prestamo)
                .Include(e => e.Prestamo.Usuario)
                .Include(e => e.UsuarioValidacion)
                .Where(m => m.IdEstadoMulta == 3).ToList();
        }
    }
}