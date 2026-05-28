using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.EjemplaresService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class EjemplaresControllerTests
{
    private readonly Mock<iEjemplaresService> _ejemplares = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLibrosService> _libros = new();
    private readonly Mock<iPrestamosService> _prestamos = new();

    private EjemplaresController CreateController() => new(_ejemplares.Object, _permisos.Object, _usuarios.Object, _libros.Object, _prestamos.Object);

    [Fact]
    public void GetEjemplares_UsuarioConPermiso_RetornaStockDelLibro()
    {
        var request = new Ejemplares { IdLibro = 1, Token = "token-valido", ActualRute = "/ejemplares" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/ejemplares")).Returns(TestEntities.Permission(read: true));
        _ejemplares.Setup(s => s.getStock(1)).Returns(new List<Ejemplares> { TestEntities.Ejemplar(1, 1) });

        var result = CreateController().GetEjemplares(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeAssignableTo<IEnumerable<Ejemplares>>();
    }

    [Fact]
    public void DeactivateEjemplar_EjemplarExistente_LlamaUpdate()
    {
        var request = new Ejemplares { IdEjemplar = 5, Token = "token-valido", ActualRute = "/ejemplares" };
        var existing = TestEntities.Ejemplar(5, 1, true);
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/ejemplares")).Returns(TestEntities.Permission(update: true));
        _ejemplares.Setup(s => s.getById(5)).Returns(existing);
        _ejemplares.Setup(s => s.deactivateEjemplar(It.IsAny<Ejemplares>())).Callback<Ejemplares>(e => e.Estado = false);

        var result = CreateController().DeactivateEjemplar(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        existing.Estado.Should().BeFalse();
        _ejemplares.Verify(s => s.updateContext(existing), Times.Once);
    }
}
