using SysBiblioteca.API.dbContext;
using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.SeccionesService
{
    public class SeccionesService : iSeccionesService
    {
        private readonly DataContext context;
        public SeccionesService(DataContext context)
        {
            this.context = context;
        }

        #region CRUD

        public void Create(Secciones entity)
        {
            context.Secciones.Add(entity);
            context.SaveChanges();
        }

        public void Delete(Secciones entity)
        {
            context.Secciones.Remove(entity);
            context.SaveChanges();
        }

        public Secciones getById(long? id)
        {
            return context.Secciones.FirstOrDefault(s => s.IdSeccion == id);
        }

        public List<Secciones> Read()
        {
            return context.Secciones.ToList();
        }

        public void Update(Secciones entity)
        {
            Secciones oldData = context.Secciones.FirstOrDefault(s => s.IdSeccion == entity.IdSeccion);

            oldData.Seccion = entity.Seccion;
            oldData.UsuarioModificacion = entity.UsuarioModificacion;
            oldData.FechaModificacion = entity.FechaModificacion;
            context.SaveChanges();
        }

        #endregion

        public Secciones getByName(string name)
        {
            return context.Secciones.FirstOrDefault(s => s.Seccion.ToUpper().Equals(name));
        }
    }
}