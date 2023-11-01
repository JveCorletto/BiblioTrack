using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.CargosService
{
    public class CargosService : iCargosService
    {
        private readonly DataContext context;
        public CargosService(DataContext context)
        {
            this.context = context;
        }
        public void Create(Cargos entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Cargos entity)
        {
            throw new NotImplementedException();
        }

        public Cargos getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<Cargos> Read()
        {
            return context.Cargos.Where(c => c.IdEstado == 1).ToList();
        }

        public void Update(Cargos entity)
        {
            throw new NotImplementedException();
        }
    }
}
