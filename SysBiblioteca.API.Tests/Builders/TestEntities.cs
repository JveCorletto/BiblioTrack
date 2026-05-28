using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;

namespace SysBiblioteca.API.Tests.Builders;

public static class TestEntities
{
    public static Usuarios AuthenticatedUser(int? roleId = 1, long? userId = 1) => new()
    {
        IdUsuario = userId,
        Usuario = "admin.test",
        IdRol = roleId,
        Token = "token-valido",
        ActualRute = "/tests"
    };

    public static Link_Rol_Menu Permission(bool create = true, bool read = true, bool update = true, bool delete = true) => new()
    {
        IdLinkRolMenu = 1,
        IdRol = 1,
        IdMenu = 1,
        Create = create,
        Read = read,
        Update = update,
        Delete = delete
    };

    public static Libros Book(long id = 1, string title = "Clean Code", int estado = 1) => new()
    {
        IdLibro = id,
        Libro = title,
        IdEstado = estado,
        IdEditorial = 1,
        ISBN = $"ISBN-{id}",
        UsuarioCreacion = "tests",
        FechaCreacion = DateTime.UtcNow
    };

    public static Ejemplares Ejemplar(long id = 1, long bookId = 1, bool disponible = true) => new()
    {
        IdEjemplar = id,
        IdLibro = bookId,
        CodigoEjemplar = $"QR-{id}",
        Estado = disponible,
        UsuarioCreacion = "tests",
        FechaCreacion = DateTime.UtcNow
    };

    public static Prestamos Prestamo(long id = 1, long userId = 1, long ejemplarId = 1, bool entregado = false, bool finalizado = false) => new()
    {
        IdPrestamo = id,
        IdUsuario = userId,
        IdEjemplar = ejemplarId,
        DiasPrestamo = 7,
        Entregado = entregado,
        Finalizado = finalizado
    };

    public static Prestamos Prestamos(long id = 1, long userId = 1, long ejemplarId = 1, bool entregado = false, bool finalizado = false, Usuarios usuario = null, Ejemplares ejemplar = null) => new()
    {
        IdPrestamo = id,
        IdUsuario = userId,
        IdEjemplar = ejemplarId,
        DiasPrestamo = 7,
        Entregado = entregado,
        Finalizado = finalizado,
        Usuario = usuario ?? TestEntities.AuthenticatedUser(userId: userId),
        Ejemplar = ejemplar ?? TestEntities.Ejemplar(id: ejemplarId)
    };

    public static Usuarios Usuario(long id = 1, string nombre = "admin.test") => new()
    {
        IdUsuario = id,
        Usuario = nombre,
        Token = "token-valido"
    };
}
