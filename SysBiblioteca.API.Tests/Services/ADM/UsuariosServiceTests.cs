using Xunit;
using FluentAssertions;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.CTL;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Tests.Helpers;
using Microsoft.Extensions.Configuration;

namespace SysBiblioteca.API.Tests.Services.ADM;

public class UsuariosServiceTests
{
    [Fact]
    public void Create_GetById_GetTokenActual_Y_GetUserInfo_RetornanUsuarioEsperado()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var service = new UsuariosService(context);

        service.Create(new Usuarios { IdUsuario = 99, Usuario = "nuevo", Token = "token-nuevo", IdEstado = 1, IdRol = 1, IdDatosPersonales = 1 });

        service.getById(99).Should().NotBeNull();
        service.getTokenActual("token-nuevo")!.Usuario.Should().Be("nuevo");
        service.getUserInfo("nuevo")!.IdUsuario.Should().Be(99);
    }

    [Fact]
    public void ActivateDeactivateLogoutYChangePassword_ModificanEstadoDelUsuario()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var service = new UsuariosService(context);
        var usuario = context.Usuarios.First(x => x.IdUsuario == 1);

        service.deactivateUser(usuario);
        usuario.IdEstado.Should().Be(2);

        service.activateUser(usuario);
        usuario.IdEstado.Should().Be(1);

        service.changePassword(1, "nuevo-password");
        usuario.Contrasenia.Should().Be("nuevo-password");

        service.LogOut(usuario);
        usuario.Token.Should().BeNull();
    }

    [Fact]
    public void GetEmpleadosYUsuarios_FiltraPorCargoYEstado()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var service = new UsuariosService(context);

        service.getEmpleados().Should().ContainSingle(x => x.IdUsuario == 1);
        service.getEmpleadosInactivos().Should().ContainSingle(x => x.IdUsuario == 2);
        service.getUsuarios().Should().ContainSingle(x => x.IdUsuario == 3);
        service.getUsuariosInactivos().Should().ContainSingle(x => x.IdUsuario == 4);
    }

    [Fact]
    public void UpdateEmpleado_ActualizaRolCargoYDatosPersonales()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var service = new UsuariosService(context);
        var oldData = context.Usuarios.First(x => x.IdUsuario == 1);
        var newData = new Usuarios
        {
            IdRol = 2,
            IdCargo = 2,
            DatosPersonales = new DatosPersonales
            {
                IdGenero = 2,
                Nombres = "Nombre actualizado",
                Apellidos = "Apellido actualizado",
                DUI = "99999999-9",
                Correo = "actualizado@test.com",
                Direccion = "San Salvador",
                Telefono = "2222-2222",
                FechaNacimiento = "01/01/2000"
            }
        };

        service.UpdateEmpleado(newData, oldData);

        oldData.IdRol.Should().Be(2);
        oldData.IdCargo.Should().Be(2);
        context.DatosPersonales.First(x => x.IdDatosPersonales == oldData.IdDatosPersonales).Nombres.Should().Be("Nombre actualizado");
    }

    [Fact]
    public void UpdateUsuario_ActualizaRolYDatosPersonales()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var service = new UsuariosService(context);
        var oldData = context.Usuarios.First(x => x.IdUsuario == 3);
        var newData = new Usuarios
        {
            IdRol = 3,
            IdCargo = null,
            DatosPersonales = new DatosPersonales
            {
                IdGenero = 1,
                Nombres = "Lector actualizado",
                Apellidos = "Prueba",
                DUI = "11111111-1",
                Correo = "lector@test.com",
                Direccion = "Santa Tecla",
                Telefono = "7777-7777",
                FechaNacimiento = "02/02/2001"
            }
        };

        service.UpdateUsuario(newData, oldData);

        oldData.IdRol.Should().Be(3);
        context.DatosPersonales.First(x => x.IdDatosPersonales == oldData.IdDatosPersonales).Nombres.Should().Be("Lector actualizado");
    }

    [Fact]
    public void LogIn_CredencialesInvalidas_IncrementaIntentos()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var service = new UsuariosService(context);

        var result = service.LogIn("empleado", "incorrecta", BuildConfiguration());

        result.Should().NotBeNull();
        result.ConteoIntentos.Should().Be(1);
    }

    [Fact]
    public void LogIn_TercerIntentoInvalido_BloqueaUsuario()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var usuario = context.Usuarios.First(x => x.IdUsuario == 1);
        usuario.ConteoIntentos = 2;
        context.SaveChanges();
        var service = new UsuariosService(context);

        var result = service.LogIn("empleado", "incorrecta", BuildConfiguration());

        result.ConteoIntentos.Should().Be(3);
        result.IdEstado.Should().Be(2);
    }

    [Fact]
    public void LogIn_CredencialesValidas_GeneraTokenYReiniciaIntentos()
    {
        using var context = TestDbContextFactory.Create();
        SeedUsuarios(context);
        var encrypted = crypto.Encrypt("correcta");
        var usuario = context.Usuarios.First(x => x.IdUsuario == 1);
        usuario.Contrasenia = encrypted;
        usuario.ConteoIntentos = 2;
        context.SaveChanges();
        var service = new UsuariosService(context);

        var result = service.LogIn("empleado", "correcta", BuildConfiguration());

        result.ConteoIntentos.Should().Be(0);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    private static Microsoft.Extensions.Configuration.IConfiguration BuildConfiguration()
    {
        return new Microsoft.Extensions.Configuration.ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JWT:JWT_SECRET_KEY"] = "0123456789ABCDEF0123456789ABCDEF",
                ["JWT:JWT_ISSUER_TOKEN"] = "SysBiblioteca.Tests",
                ["JWT:JWT_AUDIENCE_TOKEM"] = "SysBiblioteca.Tests",
                ["JWT:JWT_SUBJECT_TOKEN"] = "SysBiblioteca.Tests",
                ["JWT:JWT_EXPIRE_MINUTES"] = "60",
                ["JWT:JWT_EXPIRE_DAYS_SERVICES"] = "1"
            })
            .Build();
    }

    private static void SeedUsuarios(SysBiblioteca.API.dbContext.DataContext context)
    {
        context.Generos.Add(new Generos { IdGenero = 1, Genero = "Masculino" });
        context.Roles.AddRange(
            new Roles { IdRol = 1, Rol = "Admin", IdEstado = 1 },
            new Roles { IdRol = 2, Rol = "Empleado", IdEstado = 1 },
            new Roles { IdRol = 3, Rol = "Lector", IdEstado = 1 }
        );
        context.Cargos.AddRange(
            new Cargos { IdCargo = 1, Cargo = "Bibliotecario", IdEstado = 1, UsuarioCreacion = "test", FechaCreacion = DateTime.Today },
            new Cargos { IdCargo = 2, Cargo = "Auxiliar", IdEstado = 1, UsuarioCreacion = "test", FechaCreacion = DateTime.Today }
        );
        context.DatosPersonales.AddRange(
            new DatosPersonales { IdDatosPersonales = 1, Nombres = "Empleado", IdGenero = 1 },
            new DatosPersonales { IdDatosPersonales = 2, Nombres = "Empleado inactivo", IdGenero = 1 },
            new DatosPersonales { IdDatosPersonales = 3, Nombres = "Lector", IdGenero = 1 },
            new DatosPersonales { IdDatosPersonales = 4, Nombres = "Lector inactivo", IdGenero = 1 }
        );
        context.Usuarios.AddRange(
            new Usuarios { IdUsuario = 1, Usuario = "empleado", Contrasenia = "hash", Token = "t1", IdEstado = 1, IdRol = 1, IdCargo = 1, IdDatosPersonales = 1, ConteoIntentos = 0 },
            new Usuarios { IdUsuario = 2, Usuario = "empleado_inactivo", Token = "t2", IdEstado = 2, IdRol = 1, IdCargo = 1, IdDatosPersonales = 2, ConteoIntentos = 0 },
            new Usuarios { IdUsuario = 3, Usuario = "lector", Token = "t3", IdEstado = 1, IdRol = 3, IdCargo = null, IdDatosPersonales = 3, ConteoIntentos = 0 },
            new Usuarios { IdUsuario = 4, Usuario = "lector_inactivo", Token = "t4", IdEstado = 2, IdRol = 3, IdCargo = null, IdDatosPersonales = 4, ConteoIntentos = 0 }
        );
        context.SaveChanges();
    }
}
