using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.AutoresLibrosService
{
    public interface iAutoresLibrosService : CRUD<AutoresLibros> 
    {
        List<Autores> getAutoresLibro(Int64? IdLibro);
    }
}