using FluentAssertions;
using Moq;
using Xunit;
using Newtonsoft.Json.Linq;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.DatosPersonalesService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.MenusService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Tests.Helpers;
using Microsoft.Extensions.Configuration;

namespace SysBiblioteca.API.Tests.Controllers;

public class AuthenticationControllerTests
{
    private readonly Mock<iMenusService> _menus = new();
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iDatosPersonalesService> _datosPersonales = new();
    private readonly Mock<iLinkRolMenuService> _linkRolMenu = new();

    private AuthenticationController CreateController() => new(
        TestConfigurationFactory.CreateJwtConfiguration(),
        _menus.Object,
        _usuarios.Object,
        _datosPersonales.Object,
        _linkRolMenu.Object);

    [Fact]
    public void LogIn_CredencialesValidas_RetornaOkConToken()
    {
        var request = JObject.Parse("{\"Usuario\":\"admin\",\"Contrasenia\":\"123\"}");
        var userInfo = new Usuarios { IdUsuario = 1, Usuario = "admin", IdEstado = 1 };
        var loggedUser = new Usuarios
        {
            IdUsuario = 1,
            Usuario = "admin",
            IdEstado = 1,
            IdRol = 1,
            Token = "jwt-token",
            ConteoIntentos = 0,
            Rol = new Roles { IdRol = 1, Rol = "Administrador" }
        };
        _usuarios.Setup(s => s.getUserInfo("admin")).Returns(userInfo);
        _usuarios.Setup(s => s.LogIn("admin", "123", It.IsAny<IConfiguration>())).Returns(loggedUser);

        var result = CreateController().LogIn(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().NotBeNull();
    }

    [Fact]
    public void LogIn_UsuarioNoExiste_RetornaBadRequest()
    {
        var request = JObject.Parse("{\"Usuario\":\"nadie\",\"Contrasenia\":\"123\"}");
        _usuarios.Setup(s => s.getUserInfo("nadie")).Returns((Usuarios?)null);

        var result = CreateController().LogIn(request);

        var reply = result.ShouldBeBadRequestReply();
        reply.Resultado.Should().Be(0);
        reply.Mensaje.Should().Contain("Datos incorrectos");
    }

    [Fact]
    public void LogOut_UsuarioExistente_RetornaOkYLLamaLogOut()
    {
        var request = JObject.Parse("{\"Usuario\":\"admin\"}");
        var usuario = new Usuarios { IdUsuario = 1, Usuario = "admin" };
        _usuarios.Setup(s => s.getUserInfo("admin")).Returns(usuario);

        var result = CreateController().LogOut(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _usuarios.Verify(s => s.LogOut(usuario), Times.Once);
    }

    [Fact]
    public void Register_UsuarioDuplicado_RetornaBadRequest()
    {
        var request = new Usuarios
        {
            Usuario = "lector",
            Contrasenia = "123",
            DatosPersonales = new DatosPersonales { DUI = "00000000-0" }
        };
        _usuarios.Setup(s => s.getUserInfo("lector")).Returns(new Usuarios { IdUsuario = 1, Usuario = "lector" });
        _datosPersonales.Setup(s => s.getByDUI("00000000-0")).Returns((DatosPersonales?)null);

        var result = CreateController().Register(request);

        var reply = result.ShouldBeBadRequestReply();
        reply.Resultado.Should().Be(0);
        reply.Mensaje.Should().Contain("usuario");
    }
}
