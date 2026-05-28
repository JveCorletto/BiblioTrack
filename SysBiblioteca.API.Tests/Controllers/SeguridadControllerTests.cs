using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class SeguridadControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iRolesService> _roles = new();

    private SeguridadController CreateController() => new(_usuarios.Object, _permisos.Object, _roles.Object);

    [Fact]
    public void ActivateUser_UsuarioConPermisoYRegistroExistente_RetornaOkConResultadoExitoso()
    {
        var request = new Usuarios { IdUsuario = 2, Token = "token-valido", ActualRute = "/seguridad" };
        var currentUser = TestEntities.AuthenticatedUser(roleId: 1);
        var targetUser = new Usuarios { IdUsuario = 2, IdEstado = 2 };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/seguridad")).Returns(TestEntities.Permission(update: true));
        _usuarios.Setup(s => s.getById(2)).Returns(targetUser);

        var result = CreateController().ActivateUser(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _usuarios.Verify(s => s.activateUser(targetUser), Times.Once);
    }

    [Fact]
    public void ActivateUser_SinToken_RetornaOkConResultadoCero()
    {
        var result = CreateController().ActivateUser(new Usuarios { IdUsuario = 2 });

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        reply.Mensaje.Should().Contain("Usuario no autenticado");
        _usuarios.Verify(s => s.activateUser(It.IsAny<Usuarios>()), Times.Never);
    }

    [Fact]
    public void GetUsuarios_ServicioRetornaDatos_RetornaOkConListaUsuarios()
    {
        var request = new Usuarios { Token = "token-valido", ActualRute = "/seguridad" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(roleId: 1));
        _permisos.Setup(s => s.validateVista(1, "/seguridad")).Returns(TestEntities.Permission(read: true));
        _usuarios.Setup(s => s.getUsuarios()).Returns(new List<Usuarios> { new() { IdUsuario = 1, Usuario = "lector" } });

        var result = CreateController().GetUsuarios(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeAssignableTo<IEnumerable<Usuarios>>();
    }

    [Fact]
    public void CreateRol_RolYaExiste_RetornaOkConResultadoCero()
    {
        var request = new Roles { Rol = "Administrador", Token = "token-valido", ActualRute = "/seguridad" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser(roleId: 1));
        _permisos.Setup(s => s.validateVista(1, "/seguridad")).Returns(TestEntities.Permission(create: true));
        _roles.Setup(s => s.getByName("Administrador")).Returns(new Roles { IdRol = 1, Rol = "Administrador" });

        var result = CreateController().CreateRol(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _roles.Verify(s => s.Create(It.IsAny<Roles>()), Times.Never);
    }
}
