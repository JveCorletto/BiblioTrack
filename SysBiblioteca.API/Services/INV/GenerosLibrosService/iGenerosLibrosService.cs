using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.GenerosLibrosService
{
    public interface iGenerosLibrosService : CRUD<GenerosLibros> 
    {
        List<GenerosLiterarios> getGenerosLibro(Int64? IdLibro);
    }
}