using Xunit;
using FluentAssertions;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Tests.Helpers;

namespace SysBiblioteca.API.Tests.Services.PRS;

public class PrestamosServiceTests
{
    [Fact]
    public void Create_GetById_Y_Read_RetornanPrestamosNoFinalizados()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        service.Create(new Prestamos { IdPrestamo = 99, IdUsuario = 1, IdEjemplar = 1, Entregado = false, Finalizado = false });

        service.getById(99).Should().NotBeNull();
        service.Read().Should().Contain(x => x.IdPrestamo == 99).And.NotContain(x => x.Finalizado == true);
    }

    [Fact]
    public void GetPendingLoans_Y_GetOngoingLoans_FiltranPorEstadoDelFlujo()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        service.GetPendingLoans().Should().ContainSingle(x => x.IdPrestamo == 1);
        service.GetOngoingLoans().Should().ContainSingle(x => x.IdPrestamo == 2);
    }

    [Fact]
    public void GetMyBooks_RetornaPrestamosEntregadosNoFinalizadosDelUsuario()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        var result = service.GetMyBooks(1);

        result.Should().ContainSingle(x => x.IdPrestamo == 2);
    }

    [Fact]
    public void GetUserForLoans_RetornaUsuariosActivosSinCargo()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        var result = service.getUserForLoans();

        result.Should().ContainSingle(x => x.IdUsuario == 1);
    }

    [Fact]
    public void ValidatePrestamo_CuandoExistePrestamoActivo_RetornaPrestamo()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        var result = service.validatePrestamo(1, 1);

        result.Should().NotBeNull();
        result.IdPrestamo.Should().Be(2);
    }

    [Fact]
    public void LoanBook_MarcaPrestamoComoEntregado()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        service.LoanBook(1, 2);

        var prestamo = context.Prestamos.First(x => x.IdPrestamo == 1);
        prestamo.Entregado.Should().BeTrue();
        prestamo.IdUsuarioEntrego.Should().Be(2);
        prestamo.FechaPrestamo.Should().NotBeNull();
    }

    [Fact]
    public void MarkAsFinished_FinalizaPrestamoYRegistraUsuarioRecibio()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        service.MarkAsFinished(2, 3);

        var prestamo = context.Prestamos.First(x => x.IdPrestamo == 2);
        prestamo.Finalizado.Should().BeTrue();
        prestamo.IdUsuarioRecibio.Should().Be(3);
        prestamo.FechaDevolucion.Should().NotBeNull();
    }

    [Fact]
    public void GetFinishedLoans_RetornaPrestamosFinalizadosEntregados()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        var result = service.GetFinishedLoans();

        result.Should().ContainSingle(x => x.IdPrestamo == 3);
    }

    [Fact]
    public void GetFinishedLoans_ConFechas_RetornaPrestamosFinalizadosDentroDelRangoSegunImplementacionActual()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        var result = service.GetFinishedLoans(DateTime.Today.AddDays(-2), DateTime.Today.AddDays(-1));

        result.Should().ContainSingle(x => x.IdPrestamo == 3);
    }

    [Fact]
    public void GetLastLoanByEjemplar_RetornaPrestamoMasReciente()
    {
        using var context = TestDbContextFactory.Create();
        SeedPrestamos(context);
        var service = new PrestamosService(context);

        var result = service.getLastLoanByEjemplar(1);

        result.Should().NotBeNull();
        result.IdPrestamo.Should().Be(3);
    }

    private static void SeedPrestamos(SysBiblioteca.API.dbContext.DataContext context)
    {
        context.DatosPersonales.AddRange(
            new DatosPersonales { IdDatosPersonales = 1, Nombres = "Lector", DUI = "00000000-0", Correo = "lector@test.com", Telefono = "7777-7777" },
            new DatosPersonales { IdDatosPersonales = 2, Nombres = "Empleado" },
            new DatosPersonales { IdDatosPersonales = 3, Nombres = "Recibidor" }
        );
        context.Usuarios.AddRange(
            new Usuarios { IdUsuario = 1, Usuario = "lector", IdEstado = 1, IdDatosPersonales = 1, IdCargo = null },
            new Usuarios { IdUsuario = 2, Usuario = "empleado", IdEstado = 1, IdDatosPersonales = 2, IdCargo = 1 },
            new Usuarios { IdUsuario = 3, Usuario = "recibidor", IdEstado = 1, IdDatosPersonales = 3, IdCargo = 1 }
        );
        context.Libros.Add(new Libros { IdLibro = 1, Libro = "Clean Code", IdEstado = 1 });
        context.Ejemplares.Add(new Ejemplares { IdEjemplar = 1, IdLibro = 1, Estado = true });
        context.Prestamos.AddRange(
            new Prestamos { IdPrestamo = 1, IdUsuario = 1, IdEjemplar = 1, Entregado = false, Finalizado = false, FechaPrestamo = DateTime.Today.AddDays(-1) },
            new Prestamos { IdPrestamo = 2, IdUsuario = 1, IdEjemplar = 1, Entregado = true, Finalizado = false, FechaPrestamo = DateTime.Today.AddDays(-2), IdUsuarioEntrego = 2 },
            new Prestamos { IdPrestamo = 3, IdUsuario = 1, IdEjemplar = 1, Entregado = true, Finalizado = true, FechaPrestamo = DateTime.Today.AddDays(-4), FechaDevolucion = DateTime.Today.AddDays(-1), IdUsuarioEntrego = 2, IdUsuarioRecibio = 3 }
        );
        context.SaveChanges();
    }
}
