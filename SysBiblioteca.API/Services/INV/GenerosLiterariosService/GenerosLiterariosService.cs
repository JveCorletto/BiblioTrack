using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.GenerosLiterariosService
{
    public class GenerosLiterariosService : iGenerosLiterariosService
    {
        private readonly DataContext context;
        public GenerosLiterariosService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(GenerosLiterarios entity)
        {
            context.GenerosLiterarios.Add(entity);
            context.SaveChanges();
        }

        public void Delete(GenerosLiterarios entity)
        {
            throw new NotImplementedException();
        }

        public GenerosLiterarios getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<GenerosLiterarios> Read()
        {
            return context.GenerosLiterarios.ToList();
        }

        public void Update(GenerosLiterarios entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}