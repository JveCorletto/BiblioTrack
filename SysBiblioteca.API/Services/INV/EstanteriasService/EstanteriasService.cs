using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.EstanteriasService
{
    public class EstanteriasService : iEstanteriasService
    {
        private readonly DataContext context;
        public EstanteriasService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Estanterias entity)
        {
            context.Estanterias.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Estanterias entity)
        {
            context.Estanterias.Remove(entity);
            context.SaveChanges();
        }

        public Estanterias getById(long? id)
        {
            return context.Estanterias.FirstOrDefault(e => e.IdEstanteria == id);
        }

        public List<Estanterias> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Estanterias entity)
        {
            Estanterias oldData = context.Estanterias.FirstOrDefault(e => e.IdEstanteria == entity.IdEstanteria);

            oldData.Estanteria = entity.Estanteria;
            oldData.UsuarioModificacion = entity.UsuarioModificacion;
            oldData.FechaModificacion = entity.FechaModificacion;
            context.SaveChanges();
        }

        #endregion
        
        public List<Estanterias> getBySeccion(long? IdSeccion)
        {
            return context.Estanterias.Where(e => e.IdSeccion == IdSeccion).ToList();
        }
    }
}