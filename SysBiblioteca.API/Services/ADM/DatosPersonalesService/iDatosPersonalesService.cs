using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.DatosPersonalesService
{
    public interface iDatosPersonalesService : CRUD<DatosPersonales>
    {
        void UpdateMyData(DatosPersonales myData);
        DatosPersonales getByDUI(String DUI);
    }
}
