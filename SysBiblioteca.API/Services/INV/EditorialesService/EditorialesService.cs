using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.EditorialesService
{
    public class EditorialesService : iEditorialesService
    {
        private readonly DataContext context;
        public EditorialesService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Editoriales entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Editoriales entity)
        {
            throw new NotImplementedException();
        }

        public Editoriales getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<Editoriales> Read()
        {
            return context.Editoriales.ToList();
        }

        public void Update(Editoriales entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
