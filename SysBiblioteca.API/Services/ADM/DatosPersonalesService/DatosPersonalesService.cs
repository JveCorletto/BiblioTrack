using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.DatosPersonalesService
{
    public class DatosPersonalesService : iDatosPersonalesService
    {
        private readonly DataContext context;
        public DatosPersonalesService(DataContext context)
        {
            this.context = context;
        }

        public void Create(DatosPersonales entity)
        {
            context.DatosPersonales.Add(entity);
            context.SaveChanges();
        }

        public void Delete(DatosPersonales entity)
        {
            throw new NotImplementedException();
        }

        public List<DatosPersonales> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(DatosPersonales entity)
        {
            throw new NotImplementedException();
        }

        public DatosPersonales getByDUI(string DUI)
        {
            return context.DatosPersonales.FirstOrDefault(d => d.DUI == DUI);
        }

        public DatosPersonales getById(long? id)
        {
            throw new NotImplementedException();
        }
    }
}
