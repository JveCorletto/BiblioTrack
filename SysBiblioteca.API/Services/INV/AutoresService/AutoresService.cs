using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.AutoresService
{
    public class AutoresService : iAutoresService
    {
        private readonly DataContext context;
        public AutoresService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Autores entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Autores entity)
        {
            throw new NotImplementedException();
        }

        public Autores getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<Autores> Read()
        {
            return context.Autores.ToList();
        }

        public void Update(Autores entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}