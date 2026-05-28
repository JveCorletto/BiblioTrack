using Xunit;
using FluentAssertions;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.AutoresService;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Services.INV;

public class AutoresServiceTests
{
    [Fact]
    public void Create_AutorValido_GuardaAutor()
    {
        using var context = TestDbContextFactory.Create();
        var service = new AutoresService(context);
        var autor = new Autores { IdAutor = 1, Autor = "Robert C. Martin" };

        service.Create(autor);

        context.Autores.Should().ContainSingle(a => a.Autor == "Robert C. Martin");
    }

    [Fact]
    public void Read_ConAutores_RetornaTodosLosAutores()
    {
        using var context = TestDbContextFactory.Create();
        context.Autores.AddRange(
            new Autores { IdAutor = 1, Autor = "Autor 1" },
            new Autores { IdAutor = 2, Autor = "Autor 2" });
        context.SaveChanges();
        var service = new AutoresService(context);

        var result = service.Read();

        result.Should().HaveCount(2);
    }

    [Fact]
    public void GetById_IdExistente_RetornaAutor()
    {
        using var context = TestDbContextFactory.Create();
        context.Autores.Add(new Autores { IdAutor = 10, Autor = "Autor encontrado" });
        context.SaveChanges();
        var service = new AutoresService(context);

        var result = service.getById(10);

        result.Should().NotBeNull();
        result!.Autor.Should().Be("Autor encontrado");
    }

    [Fact]
    public void GetByName_CoincidenciaParcialIgnoraMayusculas_RetornaAutor()
    {
        using var context = TestDbContextFactory.Create();
        context.Autores.Add(new Autores { IdAutor = 1, Autor = "Gabriel García Márquez" });
        context.SaveChanges();
        var service = new AutoresService(context);

        var result = service.getByName("garcía");

        result.Should().NotBeNull();
        result!.Autor.Should().Contain("García");
    }

    [Fact]
    public void Update_AutorExistente_ActualizaAutor()
    {
        using var context = TestDbContextFactory.Create();
        context.Autores.Add(new Autores { IdAutor = 1, Autor = "Nombre anterior" });
        context.SaveChanges();
        var service = new AutoresService(context);

        service.Update(new Autores { IdAutor = 1, Autor = "Nombre actualizado" });

        context.Autores.Single(a => a.IdAutor == 1).Autor.Should().Be("Nombre actualizado");
    }
}
