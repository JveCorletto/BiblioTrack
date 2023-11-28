using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.PRS;

namespace SysBiblioteca.API.Services.PRS.PrestamosService
{
    public interface iPrestamosService : CRUD<Prestamos>
    {
        void LoanBook(Int64? IdPrestamo, Int64? IdUsuarioEntrego);
        List<Prestamos> GetMyBooks(Int64? IdUsuario);
        Prestamos validatePrestamo(Int64? IdLibro, Int64? IdUsuario);
        List<Usuarios> getUserForLoans();
        List<Prestamos> GetPendingLoans();
        List<Prestamos> GetOngoingLoans();
    }
}