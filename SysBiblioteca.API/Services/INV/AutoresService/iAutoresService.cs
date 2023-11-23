using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.AutoresService
{
    public interface iAutoresService : CRUD<Autores>
    {
        Autores getByName(string? name);
    }
}