using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.GenerosLibrosService
{
    public class GenerosLibrosService : iGenerosLibrosService
    {
        private readonly DataContext context;
        public GenerosLibrosService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(GenerosLibros entity)
        {
            context.GenerosLibros.Add(entity);
            context.SaveChanges();
        }

        public void Delete(GenerosLibros entity)
        {
            throw new NotImplementedException();
        }

        public GenerosLibros getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<GenerosLibros> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(GenerosLibros entity)
        {
            throw new NotImplementedException();
        }

        #endregion

        public List<GenerosLiterarios> getGenerosLibro(Int64? IdLibro)
        {
            return context.GenerosLibros
                .Include(g => g.GeneroLiterario)
                .Where(gl => gl.IdLibro == IdLibro)
                .Select(gl => gl.GeneroLiterario)
                .ToList();
        }
    }
}