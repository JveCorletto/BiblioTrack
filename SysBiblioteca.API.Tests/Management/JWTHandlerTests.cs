using Xunit;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Management;

public class JWTHandlerTests
{
    [Fact]
    public void GenerateToken_UsuarioConRol_RetornaJwtValido()
    {
        var handler = new JWT_Handler(TestConfigurationFactory.CreateJwtConfiguration());
        var usuario = new Usuarios
        {
            IdUsuario = 1,
            Usuario = "admin",
            Rol = new Roles { IdRol = 1, Rol = "Administrador" }
        };

        var token = handler.GenerateToken(usuario);

        token.Should().NotBeNullOrWhiteSpace();
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        jwt.Claims.Should().Contain(c => c.Value == "admin");
    }
}
