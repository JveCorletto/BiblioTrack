using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.NivelesService
{
    public interface iNivelesService : CRUD<Niveles>
    {
        List<Niveles> getByEstanteria(long? IdEstanteria);
    }
}