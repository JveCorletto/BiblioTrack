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

        public void UpdateMyData(DatosPersonales myData)
        {
            DatosPersonales myOldData = context.DatosPersonales.FirstOrDefault(d => d.IdDatosPersonales == myData.IdDatosPersonales);
            myOldData.Nombres = myData.Nombres;
            myOldData.Apellidos = myData.Apellidos;
            myOldData.DUI = myData.DUI;
            myOldData.Correo = myData.Correo;
            myOldData.Direccion = myData.Direccion;
            myOldData.Telefono = myData.Telefono;
            myOldData.FechaNacimiento = myData.FechaNacimiento;
            myOldData.IdGenero = myData.IdGenero;
            context.SaveChanges();
        }
    }
}
