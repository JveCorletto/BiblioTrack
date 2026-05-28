using FluentAssertions;
using Moq;
using Xunit;
using SysBiblioteca.API.Controllers;
using SysBiblioteca.API.DTO;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.INV.AutoresLibrosService;
using SysBiblioteca.API.Services.INV.AutoresService;
using SysBiblioteca.API.Services.INV.EditorialesService;
using SysBiblioteca.API.Services.INV.EjemplaresService;
using SysBiblioteca.API.Services.INV.GenerosLibrosService;
using SysBiblioteca.API.Services.INV.GenerosLiterariosService;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Controllers;

public class LibrosControllerTests
{
    private readonly Mock<iUsuariosService> _usuarios = new();
    private readonly Mock<iLinkRolMenuService> _permisos = new();
    private readonly Mock<iAutoresService> _autores = new();
    private readonly Mock<iGenerosLiterariosService> _generosLiterarios = new();
    private readonly Mock<iLibrosService> _libros = new();
    private readonly Mock<iAutoresLibrosService> _autoresLibros = new();
    private readonly Mock<iGenerosLibrosService> _generosLibros = new();
    private readonly Mock<iEditorialesService> _editoriales = new();
    private readonly Mock<iEjemplaresService> _ejemplares = new();

    private LibrosController CreateController() => new(
        _usuarios.Object,
        _permisos.Object,
        _autores.Object,
        _generosLiterarios.Object,
        _libros.Object,
        _autoresLibros.Object,
        _generosLibros.Object,
        _editoriales.Object,
        _ejemplares.Object);

    [Fact]
    public void CreateAutor_AutorNuevoYPermisoCrear_RetornaOkConResultadoExitoso()
    {
        var request = new Autores { Autor = "Autor Nuevo", Token = "token-valido", ActualRute = "/libros" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/libros")).Returns(TestEntities.Permission(create: true));
        _autores.Setup(s => s.getByName("Autor Nuevo")).Returns((Autores?)null);

        var result = CreateController().CreateAutor(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _autores.Verify(s => s.Create(It.Is<Autores>(a => a.Autor == "Autor Nuevo")), Times.Once);
    }

    [Fact]
    public void CreateAutor_AutorDuplicado_NoCreaRegistro()
    {
        var request = new Autores { Autor = "Autor Existente", Token = "token-valido", ActualRute = "/libros" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/libros")).Returns(TestEntities.Permission(create: true));
        _autores.Setup(s => s.getByName("Autor Existente")).Returns(new Autores { IdAutor = 1, Autor = "Autor Existente" });

        var result = CreateController().CreateAutor(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(0);
        _autores.Verify(s => s.Create(It.IsAny<Autores>()), Times.Never);
    }

    [Fact]
    public void SearchLibros_ServicioRetornaLibros_RetornaDTOsConAutoresGenerosYCantidad()
    {
        var request = new searchQuery { Titulo = "Clean", Token = "token-valido", ActualRute = "/libros" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/libros")).Returns(TestEntities.Permission(read: true));
        _libros.Setup(s => s.search("Clean", null, null, null)).Returns(new List<Libros> { TestEntities.Book(1, "Clean Code") });
        _autoresLibros.Setup(s => s.getAutoresLibro(1)).Returns(new List<Autores> { new() { IdAutor = 1, Autor = "Robert C. Martin" } });
        _generosLibros.Setup(s => s.getGenerosLibro(1)).Returns(new List<GenerosLiterarios> { new() { IdGenero = 1, Genero = "Software" } });
        _ejemplares.Setup(s => s.getAvailables(1)).Returns(3);

        var result = CreateController().SearchLibros(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        reply.Datos.Should().BeAssignableTo<IEnumerable<LibrosDTO>>();
    }

    [Fact]
    public void DeactivateBook_LibroExistenteYPermisoUpdate_LlamaServicioDeactivate()
    {
        var request = new Libros { IdLibro = 7, Token = "token-valido", ActualRute = "/libros" };
        _usuarios.Setup(s => s.getTokenActual("token-valido")).Returns(TestEntities.AuthenticatedUser());
        _permisos.Setup(s => s.validateVista(1, "/libros")).Returns(TestEntities.Permission(update: true));
        _libros.Setup(s => s.getById(7)).Returns(TestEntities.Book(7));

        var result = CreateController().DeactivateBook(request);

        var reply = result.ShouldBeOkReply();
        reply.Resultado.Should().Be(1);
        _libros.Verify(s => s.deactivate(7), Times.Once);
    }
}
