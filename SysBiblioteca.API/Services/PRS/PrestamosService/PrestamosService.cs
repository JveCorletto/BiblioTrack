using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.PRS;

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
            throw new NotImplementedException();
        }

        public void Delete(Prestamos entity)
        {
            throw new NotImplementedException();
        }

        public Prestamos getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<Prestamos> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Prestamos entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<Prestamos> GetMyBooks(long? IdUsuario)
        {
            return context.Prestamos
                .Include(x => x.Libro)
                .Where(p => p.Finalizado == false && p.Entregado == true && p.IdUsuario == IdUsuario)
                .ToList();
        }
    }
}