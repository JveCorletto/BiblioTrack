using Xunit;
using FluentAssertions;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Services.INV;

public class LibrosServiceTests
{
    [Fact]
    public void Search_SinFiltros_RetornaSoloLibrosActivos()
    {
        using var context = TestDbContextFactory.Create();
        SeedLibros(context);
        var service = new LibrosService(context);

        var result = service.search(null, 0, 0, false);

        result.Should().ContainSingle(x => x.IdLibro == 1);
        result.Should().NotContain(x => x.IdEstado == 2);
    }

    [Fact]
    public void Search_ConTituloAutorYGenero_RetornaCoincidenciaActiva()
    {
        using var context = TestDbContextFactory.Create();
        SeedLibros(context);
        var service = new LibrosService(context);

        var result = service.search("clean", 10, 20, false);

        result.Should().ContainSingle();
        result[0].Libro.Should().Be("Clean Code");
    }

    [Fact]
    public void Search_ParaPrestamo_ExcluyeEjemplarConPrestamoActivo()
    {
        using var context = TestDbContextFactory.Create();
        SeedLibros(context);
        var service = new LibrosService(context);

        var result = service.search(null, 0, 0, true);

        result.Should().Contain(x => x.IdLibro == 1);
        result.Should().NotContain(x => x.IdLibro == 3);
    }

    [Fact]
    public void SearchInactivos_ConFiltros_RetornaLibroInactivo()
    {
        using var context = TestDbContextFactory.Create();
        SeedLibros(context);
        var service = new LibrosService(context);

        var result = service.searchInactivos("legacy", 11, 21);

        result.Should().ContainSingle(x => x.IdLibro == 2);
    }

    [Fact]
    public void Update_LibroExistente_ModificaCamposEsperados()
    {
        using var context = TestDbContextFactory.Create();
        SeedLibros(context);
        var service = new LibrosService(context);
        var oldEntity = context.Libros.First(x => x.IdLibro == 1);
        var newEntity = new Libros
        {
            FotoLibro = "foto2",
            Libro = "Clean Architecture",
            Version = "2",
            ISBN = "ISBN-2",
            AnioPublicacion = 2020,
            Descripcion = "Actualizado",
            IdEditorial = 99,
            UsuarioModificacion = "tester",
            FechaModificacion = DateTime.Today
        };

        service.Update(oldEntity, newEntity);

        oldEntity.Libro.Should().Be("Clean Architecture");
        oldEntity.IdEditorial.Should().Be(99);
        oldEntity.UsuarioModificacion.Should().Be("tester");
    }

    [Fact]
    public void Activate_Y_Deactivate_CambianEstadoDelLibro()
    {
        using var context = TestDbContextFactory.Create();
        SeedLibros(context);
        var service = new LibrosService(context);

        service.deactivate(1);
        context.Libros.First(x => x.IdLibro == 1).IdEstado.Should().Be(2);

        service.Activate(1);
        context.Libros.First(x => x.IdLibro == 1).IdEstado.Should().Be(1);
    }

    [Fact]
    public void Create_Y_GetById_GuardanYObtienenLibro()
    {
        using var context = TestDbContextFactory.Create();
        var service = new LibrosService(context);

        service.Create(new Libros { IdLibro = 100, Libro = "Nuevo", IdEstado = 1 });

        service.getById(100).Should().NotBeNull();
    }

    private static void SeedLibros(SysBiblioteca.API.dbContext.DataContext context)
    {
        context.Libros.AddRange(
            new Libros { IdLibro = 1, Libro = "Clean Code", IdEstado = 1, IdEditorial = 1 },
            new Libros { IdLibro = 2, Libro = "Legacy Code", IdEstado = 2, IdEditorial = 1 },
            new Libros { IdLibro = 3, Libro = "Busy Book", IdEstado = 1, IdEditorial = 1 }
        );
        context.Ejemplares.AddRange(
            new Ejemplares { IdEjemplar = 1, IdLibro = 1, Estado = true },
            new Ejemplares { IdEjemplar = 2, IdLibro = 3, Estado = true }
        );
        context.Prestamos.Add(new Prestamos { IdPrestamo = 1, IdEjemplar = 2, FechaDevolucion = null, Finalizado = false });
        context.AutoresLibros.AddRange(
            new AutoresLibros { IdAutorLibro = 1, IdLibro = 1, IdAutor = 10 },
            new AutoresLibros { IdAutorLibro = 2, IdLibro = 2, IdAutor = 11 }
        );
        context.GenerosLibros.AddRange(
            new GenerosLibros { IdGeneroLibro = 1, IdLibro = 1, IdGenero = 20 },
            new GenerosLibros { IdGeneroLibro = 2, IdLibro = 2, IdGenero = 21 }
        );
        context.SaveChanges();
    }
}
