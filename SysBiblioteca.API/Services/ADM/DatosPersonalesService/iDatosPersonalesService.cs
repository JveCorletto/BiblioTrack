using SysBiblioteca.API.Models.ADM;

namespace SysBiblioteca.API.Services.ADM.DatosPersonalesService
{
    public interface iDatosPersonalesService : CRUD<DatosPersonales>
    {
        DatosPersonales getByDUI(String DUI);
    }
}
