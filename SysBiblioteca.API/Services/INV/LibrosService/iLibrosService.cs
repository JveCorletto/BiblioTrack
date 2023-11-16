using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.LibrosService
{
    public interface iLibrosService : CRUD<Libros> 
    {
        List<Libros> searchByName(String? Libro);
    }
}