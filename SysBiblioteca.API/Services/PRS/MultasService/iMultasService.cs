using SysBiblioteca.API.Models.PRS;

namespace SysBiblioteca.API.Services.PRS.MultasService
{
    public interface iMultasService : CRUD<Multas> 
    {
        void cargarComprobante(Multas comprobante);
        List<Multas> GetMyFines(Int64? IdUsuario);
        List<Multas> GetMyPaidFines(Int64? IdUsuario);
        List<Multas> getWaiting();
        List<Multas> getPaid();
    }
}