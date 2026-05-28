using Microsoft.EntityFrameworkCore;
using SysBiblioteca.API.dbContext;

namespace SysBiblioteca.API.Tests.Helpers;

public static class TestDbContextFactory
{
    public static DataContext Create(string? databaseName = null)
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

        var context = new DataContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        return context;
    }
}