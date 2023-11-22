using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.LibrosService
{
    public interface iLibrosService : CRUD<Libros> 
    {
        List<Libros> searchByName(String? Libro);
        List<Libros> searchByNameInactivos(String? Libro);
        void Update(Libros oldEntity, Libros newEntity);
        void Activate(Int64? IdLibro);
        void deactivate(Int64? IdLibro);
    }
}