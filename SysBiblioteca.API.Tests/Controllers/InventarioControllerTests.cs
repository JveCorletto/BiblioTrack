using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.EstanteriasService;
using SysBiblioteca.API.Services.INV.NivelesService;
using SysBiblioteca.API.Services.INV.SeccionesService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class InventarioControllerTests
{
    private readonly Mock<iNivelesService> _niveles = new();
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iSeccionesService> _secciones = new();
    private readonly Mock<iEstanteriasService> _estanterias = new();

    private InventarioController CreateController() => new(_niveles.Object, _usuarios.Object, _permisos.Object, _secciones.Object, _estanterias.Object);

    [Fact]
    public void GetSecciones_UsuarioConPermisoYDatos_RetornaOkConSecciones()
    {
        var request = new Secciones { Token = "token-valido", ActualRute = "/inventario" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/inventario")).Returns(TestEntities.Permission(read: true));
        _secciones.Setup(s => s.Read()).Returns(new List<Secciones> { new() { IdSeccion = 1, Seccion = "A" } });

        var result = CreateController().GetSecciones(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeAssignableTo<IEnumerable<Secciones>>();
    }

    [Fact]
    public void CreateSeccion_NombreDuplicado_RetornaOkConResultadoCero()
    {
        var request = new Secciones { Seccion = "General", Token = "token-valido", ActualRute = "/inventario" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/inventario")).Returns(TestEntities.Permission(create: true));
        _secciones.Setup(s => s.getByName("General")).Returns(new Secciones { IdSeccion = 1, Seccion = "General" });

        var result = CreateController().CreateSeccion(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _secciones.Verify(s => s.Create(It.IsAny<Secciones>()), Times.Never);
    }

    [Fact]
    public void DeleteNivel_NivelNoExiste_RetornaOkConResultadoCero()
    {
        var request = new Niveles { IdNivel = 99, Token = "token-valido", ActualRute = "/inventario" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/inventario")).Returns(TestEntities.Permission(delete: true));
        _niveles.Setup(s => s.getById(99)).Returns((Niveles?)null);

        var result = CreateController().DeleteNivel(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _niveles.Verify(s => s.Delete(It.IsAny<Niveles>()), Times.Never);
    }
}
