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
            context.GenerosLiterarios.Remove(entity);
            context.SaveChanges();
        }

        public GenerosLiterarios getById(long? id)
        {
            return context.GenerosLiterarios.FirstOrDefault(gl => gl.IdGenero == id);
        }

        public List<GenerosLiterarios> Read()
        {
            return context.GenerosLiterarios.ToList();
        }

        public void Update(GenerosLiterarios entity)
        {
            context.GenerosLiterarios.Update(entity);
            context.SaveChanges();
        }

        #endregion

        public GenerosLiterarios GetByname(string Genero)
        {
            return context.GenerosLiterarios.FirstOrDefault(gl => gl.Genero.ToUpper().Trim().Contains(Genero.ToUpper().Trim()));
        }
    }
}