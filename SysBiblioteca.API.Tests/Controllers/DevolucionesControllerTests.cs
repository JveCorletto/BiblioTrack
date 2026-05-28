using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.PRS.MultasService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class DevolucionesControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iPrestamosService> _prestamos = new();
    private readonly Mock<iMultasService> _multas = new();
    private readonly Mock<iLibrosService> _libros = new();

    private DevolucionesController CreateController() => new(_usuarios.Object, _permisos.Object, _prestamos.Object, _multas.Object, _libros.Object);

    [Fact]
    public void FinishLoan_PrestamoVigente_FinalizaPrestamo()
    {
        var request = new Prestamos { IdPrestamo = 1, Token = "token-valido", ActualRute = "/devoluciones" };
        var currentUser = TestEntities.AuthenticatedUser(userId: 99);
        var prestamo = TestEntities.Prestamo(1, entregado: true, finalizado: false);
        prestamo.FechaPrestamo = DateTime.Now.AddDays(-1);
        prestamo.DiasPrestamo = 7;
        prestamo.Usuario = new Usuarios { IdUsuario = 2, Usuario = "lector" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/devoluciones")).Returns(TestEntities.Permission(update: true));
        _prestamos.Setup(s => s.getById(1)).Returns(prestamo);

        var result = CreateController().FinishLoan(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _prestamos.Verify(s => s.MarkAsFinished(1, 99), Times.Once);
    }

    [Fact]
    public void FinishLoan_PrestamoDemorado_RetornaResultadoDosSinFinalizar()
    {
        var request = new Prestamos { IdPrestamo = 1, Token = "token-valido", ActualRute = "/devoluciones" };
        var prestamo = TestEntities.Prestamo(1, entregado: true, finalizado: false);
        prestamo.FechaPrestamo = DateTime.Now.AddDays(-10);
        prestamo.DiasPrestamo = 3;
        prestamo.Usuario = new Usuarios { IdUsuario = 2, Usuario = "lector" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(userId: 99));
        _permisos.Setup(s => s.validateVista(1, "/devoluciones")).Returns(TestEntities.Permission(update: true));
        _prestamos.Setup(s => s.getById(1)).Returns(prestamo);

        var result = CreateController().FinishLoan(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(2);
        _prestamos.Verify(s => s.MarkAsFinished(It.IsAny<long?>(), It.IsAny<long?>()), Times.Never);
    }

    [Fact]
    public void PayFine_PrestamoExistenteYMultaRegistrada_FinalizaPrestamo()
    {
        var request = new Multas { IdPrestamo = 1, Monto = 2.5m, DiasRetraso = 5, Token = "token-valido", ActualRute = "/devoluciones" };
        var currentUser = TestEntities.AuthenticatedUser(userId: 99);
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/devoluciones")).Returns(TestEntities.Permission(create: true));
        _prestamos.Setup(s => s.getById(1)).Returns(TestEntities.Prestamo(1));
        _multas.Setup(s => s.Create(It.IsAny<Multas>())).Callback<Multas>(m => m.IdMulta = 10);

        var result = CreateController().PayFine(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _prestamos.Verify(s => s.MarkAsFinished(1, 99), Times.Once);
    }
}
