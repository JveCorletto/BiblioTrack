using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json.Linq;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using System.Reflection;
using Xunit;

namespace SysBiblioteca.API.Tests.Controllers;

public class PriorityControllersCoverageTests
{
    private const string ValidToken = "valid-token";

    public static IEnumerable<object[]> PriorityControllerTypes()
    {
        yield return new object[] { typeof(LibrosController) };
        yield return new object[] { typeof(SeguridadController) };
        yield return new object[] { typeof(InventarioController) };
        yield return new object[] { typeof(MultasController) };
        yield return new object[] { typeof(PrestamosController) };
        yield return new object[] { typeof(EjemplaresController) };
        yield return new object[] { typeof(AuthenticationController) };
        yield return new object[] { typeof(DevolucionesController) };
    }

    [Theory]
    [MemberData(nameof(PriorityControllerTypes))]
    public void AccionesConToken_TokenNulo_RetornanOkConReplySinResultado(Type controllerType)
    {
        var controller = CreateController(controllerType, out _);
        var failures = new List<string>();

        foreach (var method in GetActionMethods(controllerType).Where(CanCreateArguments).Where(IsTokenGuardedAction))
        {
            var args = CreateArguments(method, token: null, route: "/Ruta/Prueba");
            var result = InvokeAction(controller, method, args, failures);
            if (result is null) continue;
            
            result.Should().BeAssignableTo<ObjectResult>(method.Name);
            
            var objectResult = (ObjectResult)result;
            var reply = objectResult.Value.Should().BeOfType<Reply>().Subject;
            reply.Resultado.Should().NotBe(1, $"{controllerType.Name}.{method.Name} no debe aceptar token nulo");
        }

        failures.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(PriorityControllerTypes))]
    public void AccionesConToken_TokenNoAsociadoAUsuario_RetornanOkConReplySinResultado(Type controllerType)
    {
        var controller = CreateController(controllerType, out var mocks);
        SetupUsuarioNoAutenticado(mocks);
        var failures = new List<string>();

        foreach (var method in GetActionMethods(controllerType).Where(CanCreateArguments).Where(IsTokenGuardedAction))
        {
            var args = CreateArguments(method, ValidToken, route: "/Ruta/Prueba");
            var result = InvokeAction(controller, method, args, failures);
            if (result is null) continue;

            result.Should().BeAssignableTo<ObjectResult>(method.Name);

            var objectResult = (ObjectResult)result;
            var reply = objectResult.Value.Should().BeOfType<Reply>().Subject;
            reply.Resultado.Should().NotBe(1, $"{controllerType.Name}.{method.Name} no debe aceptar usuario no autenticado");
        }

        failures.Should().BeEmpty();
    }

    [Theory]
    [MemberData(nameof(PriorityControllerTypes))]
    public void AccionesConToken_UsuarioSinPermisos_RetornanOkConReplySinResultado(Type controllerType)
    {
        var controller = CreateController(controllerType, out var mocks);
        SetupUsuarioAutenticado(mocks);
        SetupPermisos(mocks, null);
        var failures = new List<string>();

        foreach (var method in GetActionMethods(controllerType).Where(CanCreateArguments).Where(IsTokenGuardedAction).Where(m => m.Name != "validateToken"))
        {
            var args = CreateArguments(method, ValidToken, route: "/Ruta/SinPermisos");
            var result = InvokeAction(controller, method, args, failures);
            if (result is null) continue;

            result.Should().BeAssignableTo<ObjectResult>(method.Name);

            var objectResult = (ObjectResult)result;
            var reply = objectResult.Value.Should().BeOfType<Reply>().Subject;
            reply.Resultado.Should().NotBe(1, $"{controllerType.Name}.{method.Name} no debe aceptar falta de permisos");
        }

        failures.Should().BeEmpty();
    }

    [Fact]
    public void Authentication_LogIn_UsuarioInexistente_RetornaBadRequest()
    {
        var controller = (AuthenticationController)CreateController(typeof(AuthenticationController), out var mocks);
        var usuarios = GetMock<iUsuariosService>(mocks);
        usuarios.Setup(s => s.getUserInfo("noexiste")).Returns((Usuarios)null!);

        var result = controller.LogIn(JObject.Parse("{ 'Usuario': 'noexiste', 'Contrasenia': '123456' }"));

        result.Should().BeOfType<BadRequestObjectResult>();
        var reply = ((BadRequestObjectResult)result).Value.Should().BeOfType<Reply>().Subject;
        reply.Resultado.Should().Be(0);
    }

    [Fact]
    public void Authentication_LogIn_UsuarioActivoSinRol_RetornaOkResultadoDos()
    {
        var controller = (AuthenticationController)CreateController(typeof(AuthenticationController), out var mocks);
        var usuarios = GetMock<iUsuariosService>(mocks);
        var usuario = new Usuarios { IdUsuario = 1, Usuario = "lector", IdEstado = 1, ConteoIntentos = 0, IdRol = null };
        usuarios.Setup(s => s.getUserInfo("lector")).Returns(usuario);
        usuarios.Setup(s => s.LogIn("lector", "123456", It.IsAny<IConfiguration>())).Returns(usuario);

        var result = controller.LogIn(JObject.Parse("{ 'Usuario': 'lector', 'Contrasenia': '123456' }"));

        result.Should().BeOfType<OkObjectResult>();
        var reply = ((OkObjectResult)result).Value.Should().BeOfType<Reply>().Subject;
        reply.Resultado.Should().Be(2);
    }

    private static object CreateController(Type controllerType, out Dictionary<Type, object> mocks)
    {
        mocks = new Dictionary<Type, object>();
        var constructor = controllerType.GetConstructors().OrderByDescending(c => c.GetParameters().Length).First();
        var args = new List<object?>();

        foreach (var parameter in constructor.GetParameters())
        {
            if (parameter.ParameterType == typeof(IConfiguration))
            {
                args.Add(new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        ["JWT:JWT_SECRET_KEY"] = "0123456789ABCDEF0123456789ABCDEF",
                        ["JWT:JWT_ISSUER_TOKEN"] = "SysBiblioteca.Tests",
                        ["JWT:JWT_AUDIENCE_TOKEM"] = "SysBiblioteca.Tests",
                        ["JWT:JWT_SUBJECT_TOKEN"] = "SysBiblioteca.Tests",
                        ["JWT:JWT_EXPIRE_MINUTES"] = "60",
                        ["JWT:JWT_EXPIRE_DAYS_SERVICES"] = "1"
                    })
                    .Build());
                continue;
            }

            if (parameter.ParameterType.IsInterface)
            {
                var mockType = typeof(Mock<>).MakeGenericType(parameter.ParameterType);
                var mock = Activator.CreateInstance(mockType, MockBehavior.Loose)!;
                mocks[parameter.ParameterType] = mock;

                // Solución: Usar BindingFlags.DeclaredOnly para evitar la herencia
                var propertyInfo = mockType.GetProperty("Object", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                args.Add(propertyInfo!.GetValue(mock));
                continue;
            }

            args.Add(Activator.CreateInstance(parameter.ParameterType));
        }

        return Activator.CreateInstance(controllerType, args.ToArray())!;
    }

    private static IEnumerable<MethodInfo> GetActionMethods(Type controllerType)
    {
        return controllerType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Where(m => typeof(IActionResult).IsAssignableFrom(m.ReturnType));
    }

    private static bool CanCreateArguments(MethodInfo method)
    {
        return method.GetParameters().All(p => p.ParameterType == typeof(object)
            || p.ParameterType == typeof(bool)
            || p.ParameterType.GetConstructor(Type.EmptyTypes) != null);
    }

    private static bool IsTokenGuardedAction(MethodInfo method)
    {
        if (method.Name is "LogIn" or "LogOut" or "Register") return false;
        var first = method.GetParameters().FirstOrDefault();
        return first != null && first.ParameterType.GetProperty("Token") != null;
    }

    private static object?[] CreateArguments(MethodInfo method, string? token, string route)
    {
        return method.GetParameters().Select(p => CreateArgument(p.ParameterType, token, route)).ToArray();
    }

    private static object? CreateArgument(Type type, string? token, string route)
    {
        if (type == typeof(bool)) return false;
        if (type == typeof(object)) return JObject.Parse("{ 'Usuario': 'test', 'Contrasenia': '123456' }");

        var instance = Activator.CreateInstance(type)!;
        SetProperty(instance, "Token", token);
        SetProperty(instance, "ActualRute", route);
        SetProperty(instance, "IdUsuario", 1L);
        SetProperty(instance, "IdRol", 1);
        SetProperty(instance, "IdLibro", 1L);
        SetProperty(instance, "IdAutor", 1L);
        SetProperty(instance, "IdGenero", 1L);
        SetProperty(instance, "IdEditorial", 1L);
        SetProperty(instance, "IdEjemplar", 1L);
        SetProperty(instance, "IdPrestamo", 1L);
        SetProperty(instance, "IdMulta", 1L);
        SetProperty(instance, "Libro", "Libro de prueba");
        SetProperty(instance, "Autor", "Autor de prueba");
        SetProperty(instance, "Editorial", "Editorial de prueba");
        SetProperty(instance, "Genero", "Genero de prueba");
        SetProperty(instance, "CodigoEjemplar", "QR-001");
        SetProperty(instance, "Titulo", "Libro");
        SetProperty(instance, "FechaDesde", DateTime.Today.AddDays(-7));
        SetProperty(instance, "FechaHasta", DateTime.Today);
        return instance;
    }

    private static void SetProperty(object instance, string propertyName, object? value)
    {
        var property = instance.GetType().GetProperty(propertyName);
        if (property == null || !property.CanWrite) return;
        if (value == null)
        {
            property.SetValue(instance, null);
            return;
        }

        var targetType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
        if (targetType == typeof(string)) property.SetValue(instance, value.ToString());
        else if (targetType == typeof(long)) property.SetValue(instance, Convert.ToInt64(value));
        else if (targetType == typeof(int)) property.SetValue(instance, Convert.ToInt32(value));
        else if (targetType == typeof(bool)) property.SetValue(instance, Convert.ToBoolean(value));
        else if (targetType == typeof(DateTime)) property.SetValue(instance, Convert.ToDateTime(value));
    }

    private static IActionResult? InvokeAction(object controller, MethodInfo method, object?[] args, List<string> failures)
    {
        try
        {
            return (IActionResult?)method.Invoke(controller, args);
        }
        catch (TargetInvocationException ex)
        {
            failures.Add($"{controller.GetType().Name}.{method.Name}: {ex.InnerException?.GetType().Name} - {ex.InnerException?.Message}");
            return null;
        }
    }

    private static Mock<T> GetMock<T>(Dictionary<Type, object> mocks) where T : class
    {
        return (Mock<T>)mocks[typeof(T)];
    }

    private static void SetupUsuarioNoAutenticado(Dictionary<Type, object> mocks)
    {
        if (mocks.TryGetValue(typeof(iUsuariosService), out var mock))
        {
            ((Mock<iUsuariosService>)mock).Setup(s => s.getTokenActual(It.IsAny<string>())).Returns((Usuarios)null!);
        }
    }

    private static void SetupUsuarioAutenticado(Dictionary<Type, object> mocks)
    {
        if (mocks.TryGetValue(typeof(iUsuariosService), out var mock))
        {
            ((Mock<iUsuariosService>)mock)
                .Setup(s => s.getTokenActual(It.IsAny<string>()))
                .Returns(new Usuarios { IdUsuario = 1, Usuario = "admin", IdRol = 1, IdEstado = 1 });
        }
    }

    private static void SetupPermisos(Dictionary<Type, object> mocks, Link_Rol_Menu? permisos)
    {
        if (mocks.TryGetValue(typeof(iLinkRolMenuService), out var mock))
        {
            ((Mock<iLinkRolMenuService>)mock)
                .Setup(s => s.validateVista(It.IsAny<int?>(), It.IsAny<string>()))
                .Returns(permisos!);
        }
    }
}
