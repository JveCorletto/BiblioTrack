using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.DTO;
using SysBiblioteca.API.Models.CTL;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.PRS.MultasService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class MultasControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iMultasService> _multas = new();
    private readonly Mock<iPrestamosService> _prestamos = new();
    private readonly Mock<iLibrosService> _libros = new();

    private MultasController CreateController() => new(_usuarios.Object, _permisos.Object, _multas.Object, _prestamos.Object, _libros.Object);

    [Fact]
    public void GetMyFines_UsuarioConMultas_RetornaDTOsDeMultas()
    {
        var request = new Multas { Token = "token-valido", ActualRute = "/multas" };
        var currentUser = TestEntities.AuthenticatedUser(userId: 10);
        var multa = new Multas
        {
            IdMulta = 1,
            DiasRetraso = 3,
            Monto = 1.5m,
            EstadoMulta = new EstadosMultas { IdEstadoMulta = 1, EstadoMulta = "Pendiente" },
            Prestamo = new Prestamos
            {
                IdPrestamo = 1,
                Ejemplar = new Ejemplares { IdEjemplar = 1, Libro = TestEntities.Book(1, "Clean Code") }
            }
        };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/multas")).Returns(TestEntities.Permission(read: true));
        _multas.Setup(s => s.GetMyFines(10)).Returns(new List<Multas> { multa });

        var result = CreateController().GetMyFines(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeAssignableTo<IEnumerable<MultasDTO>>();
    }

    [Fact]
    public void GetMyFines_SinMultas_RetornaMensajeSinDatos()
    {
        var request = new Multas { Token = "token-valido", ActualRute = "/multas" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(userId: 10));
        _permisos.Setup(s => s.validateVista(1, "/multas")).Returns(TestEntities.Permission(read: true));
        _multas.Setup(s => s.GetMyFines(10)).Returns(new List<Multas>());

        var result = CreateController().GetMyFines(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        reply.Mensaje.Should().Contain("Sin datos");
    }

    [Fact]
    public void UploadInvoce_SinToken_RetornaOkConResultadoCero()
    {
        var result = CreateController().UploadInvoce(new Multas { IdMulta = 1 });

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _multas.Verify(s => s.cargarComprobante(It.IsAny<Multas>()), Times.Never);
    }
}
