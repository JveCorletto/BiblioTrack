# SysBiblioteca.API.Tests

Proyecto de pruebas unitarias para `SysBiblioteca.API`.

## Stack usado

- xUnit
- Moq
- FluentAssertions
- Microsoft.EntityFrameworkCore.InMemory
- Microsoft.AspNetCore.Mvc.Testing
- coverlet.collector

## Estructura

```text
SysBiblioteca.API.Tests/
├── Controllers/
├── Services/
│   ├── INV/
│   └── PRS/
├── Management/
├── Helpers/
└── Builders/
```

## Comandos de ejecución

Desde la carpeta raíz donde estén `SysBiblioteca.API` y `SysBiblioteca.API.Tests`:

```bash
dotnet restore
dotnet build
dotnet test
```

## Cobertura de código

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Para reporte HTML, instala ReportGenerator:

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:**/coverage.cobertura.xml -targetdir:coveragereport -reporttypes:Html
```

## Nota técnica importante

Para que EF Core InMemory funcione, se aplicó una refactorización mínima en `DataContext`:

```csharp
if (!optionsBuilder.IsConfigured && !String.IsNullOrWhiteSpace(connectionString))
{
    optionsBuilder.UseSqlServer(connectionString);
}
```

Sin este cambio, `DataContext` fuerza `UseSqlServer` incluso cuando las pruebas configuran `UseInMemoryDatabase`, provocando conflicto de proveedores.

## Métodos cubiertos inicialmente

### Controladores

- `AuthenticationController`: `LogIn`, `LogOut`, `Register`
- `CatalogosController`: `GetGeneros`, `GetEstados`
- `SeguridadController`: `ActivateUser`, `GetUsuarios`, `CreateRol`
- `InventarioController`: `GetSecciones`, `CreateSeccion`, `DeleteNivel`
- `LibrosController`: `CreateAutor`, `SearchLibros`, `DeactivateBook`
- `EjemplaresController`: `GetEjemplares`, `DeactivateEjemplar`
- `PrestamosController`: `ProcesarPrestamo`, `GetPendingLoans`
- `DevolucionesController`: `FinishLoan`, `PayFine`
- `MultasController`: `GetMyFines`, `UploadInvoce`
- `UsuariosExternosController`: `GetMyData`, `ChangeMyPassword`
- `ValuesController`: `encriptString`, `decriptString`

### Servicios y Management

- `AutoresService`
- `EjemplaresService`
- `PrestamosService`
- `MultasService`
- `JWT_Handler`
- `crypto`

## Observaciones de testabilidad

1. Varios controladores retornan `OkObjectResult` aunque el escenario sea error de negocio. Esto dificulta diferenciar errores HTTP reales de errores funcionales.
2. `DataContext` estaba acoplado a SQL Server dentro de `OnConfiguring`. Para pruebas unitarias con EF InMemory se requiere respetar opciones ya configuradas.
3. Algunos métodos de servicios tienen `throw new NotImplementedException()`. Esos métodos no deben probarse como comportamiento funcional hasta implementarse.
4. En varios servicios, algunas consultas dependen de propiedades de navegación cargadas. Para InMemory, los datos de prueba deben sembrar correctamente entidades relacionadas.
5. Hay métodos `Delete` que usan `Add` en lugar de `Remove` en algunas clases CRUD. Conviene revisarlos antes de ampliar cobertura de eliminación.
