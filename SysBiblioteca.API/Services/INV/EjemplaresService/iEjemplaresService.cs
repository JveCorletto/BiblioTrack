using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.EjemplaresService
{
    public interface iEjemplaresService : CRUD<Ejemplares>
    {
        int getAvailables(long? IdLibro);

        List<Ejemplares> getStock(long? IdLibro);
    }
}