using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.LibrosService
{
    public interface iLibrosService : CRUD<Libros> 
    {
        List<Libros> search(String? Libro, Int64? IdAutor, Int64? IdGenero, Boolean? prestamo);
        List<Libros> searchInactivos(String? Libro, Int64? IdAutor, Int64? IdGenero);
        void Update(Libros oldEntity, Libros newEntity);
        void Activate(Int64? IdLibro);
        void deactivate(Int64? IdLibro);
    }
}