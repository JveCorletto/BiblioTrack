using SysBiblioteca.API.Models.PRS;

namespace SysBiblioteca.API.Services.PRS.PrestamosService
{
    public interface iPrestamosService : CRUD<Prestamos>
    {
        List<Prestamos> GetMyBooks(Int64? IdUsuario);
    }
}