using Microsoft.EntityFrameworkCore;
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
            context.Editoriales.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Editoriales entity)
        {
            context.Editoriales.Remove(entity);
            context.SaveChanges();
        }

        public Editoriales getById(long? id)
        {
            return context.Editoriales.FirstOrDefault(e => e.IdEditorial == id);
        }

        public List<Editoriales> Read()
        {
            return context.Editoriales.ToList();
        }

        public void Update(Editoriales entity)
        {
            context.Editoriales.Update(entity);
            context.SaveChanges();
        }

        #endregion

        public Editoriales GetByName(string Editorial)
        {
            return context.Editoriales.FirstOrDefault(e => e.Editorial.ToUpper().Trim().Contains(Editorial.ToUpper().Trim()));
        }
    }
}
