using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.CTL;
using SysBiblioteca.API.Services.ADM.CargosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.CTL.EstadosService;
using SysBiblioteca.API.Services.CTL.GenerosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class CatalogosControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iGenerosService> _generos = new();
    private readonly Mock<iEstadosService> _estados = new();
    private readonly Mock<iRolesService> _roles = new();
    private readonly Mock<iCargosService> _cargos = new();

    private CatalogosController CreateController() => new(_usuarios.Object, _permisos.Object, _generos.Object, _estados.Object, _roles.Object, _cargos.Object);

    [Fact]
    public void GetGeneros_UsuarioConPermisoYDatos_RetornaOkConGeneros()
    {
        var request = new Generos { Token = "token-valido", ActualRute = "/catalogos" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/catalogos")).Returns(TestEntities.Permission(read: true));
        _generos.Setup(s => s.Read()).Returns(new List<Generos> { new() { IdGenero = 1, Genero = "Masculino" } });

        var result = CreateController().GetGeneros(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeAssignableTo<IEnumerable<Generos>>();
    }

    [Fact]
    public void GetGeneros_SinDatos_RetornaOkConMensajeSinDatos()
    {
        var request = new Generos { Token = "token-valido", ActualRute = "/catalogos" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/catalogos")).Returns(TestEntities.Permission(read: true));
        _generos.Setup(s => s.Read()).Returns(new List<Generos>());

        var result = CreateController().GetGeneros(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        reply.Mensaje.Should().Contain("Sin datos");
    }

    [Fact]
    public void GetEstados_UsuarioSinPermisoLectura_RetornaOkConResultadoCero()
    {
        var request = new Generos { Token = "token-valido", ActualRute = "/catalogos" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/catalogos")).Returns(TestEntities.Permission(read: false));

        var result = CreateController().GetEstados(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _estados.Verify(s => s.Read(), Times.Never);
    }
}
