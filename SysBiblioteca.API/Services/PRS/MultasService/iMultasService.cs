using SysBiblioteca.API.Models.PRS;

namespace SysBiblioteca.API.Services.PRS.MultasService
{
    public interface iMultasService : CRUD<Multas> 
    {
        List<Multas> getUnPaid();
        List<Multas> getWaiting();
        List<Multas> getPaid();
    }
}