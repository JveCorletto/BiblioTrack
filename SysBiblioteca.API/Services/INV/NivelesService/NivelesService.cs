using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.NivelesService
{
    public class NivelesService : iNivelesService
    {
        private readonly DataContext context;
        public NivelesService(DataContext context)
        {
            this.context = context;
        }


        #region CRUD

        public void Create(Niveles entity)
        {
            context.Niveles.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Niveles entity)
        {
            context.Niveles.Remove(entity);
            context.SaveChanges();
        }

        public Niveles getById(long? id)
        {
            return context.Niveles.FirstOrDefault(n => n.IdNivel == id);
        }

        public List<Niveles> Read()
        {
            throw new NotImplementedException();
        }

        public void Update(Niveles entity)
        {
            Niveles oldData = context.Niveles.FirstOrDefault(n => n.IdNivel == entity.IdNivel);

            oldData.Nivel = entity.Nivel;
            oldData.FechaModificacion = entity.FechaModificacion;
            oldData.UsuarioModificacion = entity.UsuarioModificacion;
            context.SaveChanges();
        }

        #endregion

        public List<Niveles> getByEstanteria(long? IdEstanteria)
        {
            return context.Niveles.Where(n => n.IdEstanteria == IdEstanteria).ToList();
        }
    }
}