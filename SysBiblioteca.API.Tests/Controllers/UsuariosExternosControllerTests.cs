using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.DTO;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class UsuariosExternosControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iDatosPersonalesService> _datos = new();

    private UsuariosExternosController CreateController() => new(_usuarios.Object, _permisos.Object, _datos.Object);

    [Fact]
    public void GetMyData_UsuarioAutenticado_RetornaDatosUsuario()
    {
        var request = new Usuarios { Token = "token-valido", ActualRute = "/perfil" };
        var currentUser = TestEntities.AuthenticatedUser(userId: 1);
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/perfil")).Returns(TestEntities.Permission(read: true));
        _usuarios.Setup(s => s.getById(1)).Returns(new Usuarios { IdUsuario = 1, Usuario = "lector", DatosPersonales = new DatosPersonales { Nombres = "Lector" } });

        var result = CreateController().GetMyData(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeOfType<Usuarios>();
    }

    [Fact]
    public void ChangeMyPassword_PasswordAnteriorCorrecto_CambiaPassword()
    {
        var encryptedPassword = crypto.Encrypt("anterior");
        var currentUser = TestEntities.AuthenticatedUser(userId: 1);
        currentUser.Contrasenia = encryptedPassword;
        var request = new PasswordDTO { Token = "token-valido", ActualRute = "/perfil", OldPassword = "anterior", NewPassword = "nuevo" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/perfil")).Returns(TestEntities.Permission(update: true));
        _usuarios.Setup(s => s.getById(1)).Returns(currentUser);

        var result = CreateController().ChangeMyPassword(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _usuarios.Verify(s => s.changePassword(1, It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void ChangeMyPassword_PasswordAnteriorIncorrecto_NoCambiaPassword()
    {
        var currentUser = TestEntities.AuthenticatedUser(userId: 1);
        currentUser.Contrasenia = crypto.Encrypt("anterior");
        var request = new PasswordDTO { Token = "token-valido", ActualRute = "/perfil", OldPassword = "incorrecto", NewPassword = "nuevo" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(currentUser);
        _permisos.Setup(s => s.validateVista(1, "/perfil")).Returns(TestEntities.Permission(update: true));
        _usuarios.Setup(s => s.getById(1)).Returns(currentUser);

        var result = CreateController().ChangeMyPassword(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _usuarios.Verify(s => s.changePassword(It.IsAny<long?>(), It.IsAny<string>()), Times.Never);
    }
}
