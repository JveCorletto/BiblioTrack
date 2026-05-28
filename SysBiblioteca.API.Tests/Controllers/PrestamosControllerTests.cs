using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.DTO;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.AutoresLibrosService;
using SysBiblioteca.API.Services.INV.EjemplaresService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class PrestamosControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iPrestamosService> _prestamos = new();
    private readonly Mock<iLibrosService> _libros = new();
    private readonly Mock<iAutoresLibrosService> _autoresLibros = new();
    private readonly Mock<iEjemplaresService> _ejemplares = new();

    private PrestamosController CreateController() => new(_usuarios.Object, _permisos.Object, _prestamos.Object, _libros.Object, _autoresLibros.Object, _ejemplares.Object);

    [Fact]
    public void ProcesarPrestamo_SinToken_RetornaBadRequest()
    {
        var result = CreateController().ProcesarPrestamo(new PrestamosDTO { IdLibro = 1, IdUsuario = 2 }, false);

        var reply = result.ShouldBeBadRequestReply();
        reply.Resultado.Should().Be(0);
        reply.Mensaje.Should().Contain("Usuario no autenticado");
    }

    [Fact]
    public void ProcesarPrestamo_PrestamoActivoExistente_RetornaConflict()
    {
        var request = new PrestamosDTO { Token = "token-valido", ActualRute = "/prestamos", IdLibro = 1, IdUsuario = 2, DiasPrestamo = 7 };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(roleId: 1));
        _permisos.Setup(s => s.validateVista(1, "/prestamos")).Returns(TestEntities.Permission(create: true));
        _prestamos.Setup(s => s.validatePrestamo(1, 2)).Returns(TestEntities.Prestamo(1));

        var result = CreateController().ProcesarPrestamo(request, false);

        var reply = result.ShouldBeConflictReply();
        reply.Resultado.Should().Be(0);
        _prestamos.Verify(s => s.Create(It.IsAny<Prestamos>()), Times.Never);
    }

    [Fact]
    public void ProcesarPrestamo_SinEjemplaresDisponibles_RetornaNotFound()
    {
        var request = new PrestamosDTO { Token = "token-valido", ActualRute = "/prestamos", IdLibro = 1, IdUsuario = 2, DiasPrestamo = 7 };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(roleId: 1));
        _permisos.Setup(s => s.validateVista(1, "/prestamos")).Returns(TestEntities.Permission(create: true));
        _prestamos.Setup(s => s.validatePrestamo(1, 2)).Returns((Prestamos?)null);
        _ejemplares.Setup(s => s.getEjemplarToLoan(1)).Returns((Ejemplares?)null);

        var result = CreateController().ProcesarPrestamo(request, false);

        result.ShouldBeNotFoundReply().Mensaje.Should().Contain("no quedan unidades");
    }

    [Fact]
    public void GetPendingLoans_UsuarioConPermiso_RetornaOkConPrestamosPendientes()
    {
        var request = new Prestamos { Token = "token-valido", ActualRute = "/prestamos" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(roleId: 1));
        _permisos.Setup(s => s.validateVista(1, "/prestamos")).Returns(TestEntities.Permission(read: true));

        // Crear un libro para los ejemplares
        var libro = TestEntities.Book(id: 1, title: "Clean Code");

        // Crear los 3 préstamos con sus propiedades navegacionales
        var prestamosMock = new List<Prestamos> {
            TestEntities.Prestamos(
                id: 2,
                entregado: false,
                finalizado: false,
                ejemplar: TestEntities.Ejemplar(id: 2, bookId: 1),
                usuario: TestEntities.Usuario(id: 1, nombre: "usuario1")
            ),
            TestEntities.Prestamos(
                id: 19,
                entregado: false,
                finalizado: false,
                ejemplar: TestEntities.Ejemplar(id: 19, bookId: 1),
                usuario: TestEntities.Usuario(id: 2, nombre: "usuario2")
            ),
            TestEntities.Prestamos(
                id: 20,
                entregado: false,
                finalizado: false,
                ejemplar: TestEntities.Ejemplar(id: 20, bookId: 1),
                usuario: TestEntities.Usuario(id: 3, nombre: "usuario3")
            )
        };
        
        foreach (var prestamo in prestamosMock)
        {
            prestamo.Ejemplar.Libro = libro;
        }

        _prestamos.Setup(s => s.GetPendingLoans()).Returns(prestamosMock);
        var result = CreateController().GetPendingLoans(request);

        // Assert
        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);

        var misPrestamos = reply.Datos as IEnumerable<MisPrestamosDTO>;
        misPrestamos.Count().Should().Be(3);
    }
}
