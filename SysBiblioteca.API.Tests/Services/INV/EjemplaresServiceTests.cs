using Xunit;
using FluentAssertions;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.EjemplaresService;
using SysBiblioteca.API.Tests.Builders;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Services.INV;

public class EjemplaresServiceTests
{
    [Fact]
    public void GetStock_LibroExistente_RetornaEjemplaresDelLibro()
    {
        using var context = TestDbContextFactory.Create();
        context.Ejemplares.AddRange(
            TestEntities.Ejemplar(1, 10),
            TestEntities.Ejemplar(2, 10),
            TestEntities.Ejemplar(3, 99));
        context.SaveChanges();
        var service = new EjemplaresService(context);

        var result = service.getStock(10);

        result.Should().HaveCount(2);
    }

    [Fact]
    public void GetAvailables_LibroConEjemplaresDisponibles_RetornaCantidadDisponible()
    {
        using var context = TestDbContextFactory.Create();
        context.Ejemplares.AddRange(
            TestEntities.Ejemplar(1, 10, true),
            TestEntities.Ejemplar(2, 10, false),
            TestEntities.Ejemplar(3, 10, true));
        context.SaveChanges();
        var service = new EjemplaresService(context);

        var result = service.getAvailables(10);

        result.Should().Be(2);
    }

    [Fact]
    public void GetEjemplarToLoan_LibroConDisponible_RetornaPrimerEjemplarDisponible()
    {
        using var context = TestDbContextFactory.Create();
        context.Ejemplares.AddRange(
            TestEntities.Ejemplar(1, 20, false),
            TestEntities.Ejemplar(2, 20, true));
        context.SaveChanges();
        var service = new EjemplaresService(context);

        var result = service.getEjemplarToLoan(20);

        result.Should().NotBeNull();
        result!.IdEjemplar.Should().Be(2);
    }

    [Fact]
    public void GetByCodigo_CodigoExistente_RetornaEjemplar()
    {
        using var context = TestDbContextFactory.Create();
        context.Ejemplares.Add(new Ejemplares { IdEjemplar = 5, CodigoEjemplar = "QR-ABC", Estado = true, IdLibro = 1 });
        context.SaveChanges();
        var service = new EjemplaresService(context);

        var result = service.getByCodigo("QR-ABC");

        result.Should().NotBeNull();
        result!.IdEjemplar.Should().Be(5);
    }
}
