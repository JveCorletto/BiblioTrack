using FluentAssertions;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.CTL;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.PRS.MultasService;
using SysBiblioteca.API.Tests.Helpers;
using Xunit;

namespace SysBiblioteca.API.Tests.Services.PRS;

public class MultasServiceTests
{
    [Fact]
    public void Create_Y_GetById_GuardanYObtienenMulta()
    {
        using var context = TestDbContextFactory.Create();
        SeedMultas(context);
        var service = new MultasService(context);

        service.Create(new Multas { IdMulta = 99, IdPrestamo = 1, IdEstadoMulta = 1, Monto = 2.5m });

        service.getById(99).Should().NotBeNull();
    }

    [Fact]
    public void GetWaiting_RetornaMultasPendientesYEnRevision()
    {
        using var context = TestDbContextFactory.Create();
        SeedMultas(context);
        var service = new MultasService(context);

        var result = service.getWaiting();

        result.Should().HaveCount(2);
        result.Should().OnlyContain(x => x.IdEstadoMulta == 1 || x.IdEstadoMulta == 2);
    }

    [Fact]
    public void GetPaid_RetornaSoloMultasPagadas()
    {
        using var context = TestDbContextFactory.Create();
        SeedMultas(context);
        var service = new MultasService(context);

        var result = service.getPaid();

        result.Should().ContainSingle(x => x.IdEstadoMulta == 3);
    }

    [Fact]
    public void GetMyFines_Y_GetMyPaidFines_FiltranPorUsuario()
    {
        using var context = TestDbContextFactory.Create();
        SeedMultas(context);
        var service = new MultasService(context);

        service.GetMyFines(1).Should().HaveCount(2);
        service.GetMyPaidFines(1).Should().ContainSingle(x => x.IdEstadoMulta == 3);
    }

    [Fact]
    public void CargarComprobante_MultaExistente_ActualizaComprobanteYPagoFisico()
    {
        using var context = TestDbContextFactory.Create();
        SeedMultas(context);
        var service = new MultasService(context);

        service.cargarComprobante(new Multas { IdMulta = 1, ComprobantePago = "base64", PagoFisico = false });

        var multa = context.Multas.First(x => x.IdMulta == 1);
        multa.ComprobantePago.Should().Be("base64");
        multa.PagoFisico.Should().BeFalse();
    }

    private static void SeedMultas(SysBiblioteca.API.dbContext.DataContext context)
    {
        context.EstadosMultas.AddRange(
            new EstadosMultas { IdEstadoMulta = 1, EstadoMulta = "Pendiente" },
            new EstadosMultas { IdEstadoMulta = 2, EstadoMulta = "En revisión" },
            new EstadosMultas { IdEstadoMulta = 3, EstadoMulta = "Pagada" }
        );
        context.Usuarios.AddRange(
            new Usuarios { IdUsuario = 1, Usuario = "lector" },
            new Usuarios { IdUsuario = 2, Usuario = "validador" }
        );
        context.Libros.Add(new Libros { IdLibro = 1, Libro = "Clean Code", IdEstado = 1 });
        context.Ejemplares.Add(new Ejemplares { IdEjemplar = 1, IdLibro = 1, Estado = true });
        context.Prestamos.Add(new Prestamos { IdPrestamo = 1, IdUsuario = 1, IdEjemplar = 1, Entregado = true, Finalizado = true });
        context.Multas.AddRange(
            new Multas { IdMulta = 1, IdPrestamo = 1, IdEstadoMulta = 1, Monto = 1.5m },
            new Multas { IdMulta = 2, IdPrestamo = 1, IdEstadoMulta = 2, Monto = 2.5m },
            new Multas { IdMulta = 3, IdPrestamo = 1, IdEstadoMulta = 3, Monto = 3.5m, IdUsuarioValidacion = 2 }
        );
        context.SaveChanges();
    }
}
