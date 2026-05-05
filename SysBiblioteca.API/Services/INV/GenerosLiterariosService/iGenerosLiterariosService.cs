using SysBiblioteca.API.Models.INV;

namespace SysBiblioteca.API.Services.INV.GenerosLiterariosService
{
    public interface iGenerosLiterariosService : CRUD<GenerosLiterarios> 
    {
        GenerosLiterarios GetByname(String Genero);
    }
}