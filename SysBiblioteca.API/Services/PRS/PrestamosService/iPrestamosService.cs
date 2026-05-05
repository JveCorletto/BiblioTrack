using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.PRS;

namespace SysBiblioteca.API.Services.PRS.PrestamosService
{
    public interface iPrestamosService : CRUD<Prestamos>
    {
        Prestamos validatePrestamo(Int64? IdLibro, Int64? IdUsuario);
        Prestamos getLastLoanByEjemplar(Int64? IdEjemplar);
        void LoanBook(Int64? IdPrestamo, Int64? IdUsuarioEntrego);
        void MarkAsFinished(Int64? IdPrestamo, Int64? IdUsuario);
        List<Prestamos> GetMyBooks(Int64? IdUsuario);
        List<Usuarios> getUserForLoans();
        List<Prestamos> GetPendingLoans();
        List<Prestamos> GetOngoingLoans();
        List<Prestamos> GetFinishedLoans();
        List<Prestamos> GetFinishedLoans(DateTime? FechaDesde, DateTime? FechaHasta);
    }
}