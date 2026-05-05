using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.CTL;

namespace SysBiblioteca.API.Services.CTL.GenerosService
{
    public class GenerosService : iGenerosService
    {
        private readonly DataContext context;
        public GenerosService(DataContext context)
        {
            this.context = context;
        }
        public void Create(Generos entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Generos entity)
        {
            throw new NotImplementedException();
        }

        public Generos getById(long? id)
        {
            throw new NotImplementedException();
        }

        public List<Generos> Read()
        {
            return context.Generos.ToList();
        }

        public void Update(Generos entity)
        {
            throw new NotImplementedException();
        }
    }
}
