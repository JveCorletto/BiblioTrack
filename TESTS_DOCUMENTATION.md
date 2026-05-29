# Documentación de Tests Unitarios — BiblioTrack

> Generado el 2026-05-28 · **101 tests** · Todos superados ✅

---

## Índice

1. [Resumen de Cobertura](#resumen-de-cobertura)
2. [Cobertura por Clase](#cobertura-por-clase)
3. [Infraestructura de Tests](#infraestructura-de-tests)
4. [Tests de Management (Seguridad y JWT)](#tests-de-management)
5. [Tests de Servicios](#tests-de-servicios)
   - [UsuariosService (ADM)](#usuariosservice)
   - [LibrosService (INV)](#librosservice)
   - [EjemplaresService (INV)](#ejemplaresservice)
   - [AutoresService (INV)](#autoresservice)
   - [PrestamosService (PRS)](#prestamosservice)
   - [MultasService (PRS)](#multasservice)
6. [Tests de Controladores](#tests-de-controladores)
   - [AuthenticationController](#authenticationcontroller)
   - [LibrosController](#libroscontroller)
   - [PrestamosController](#prestamoscontroller)
   - [MultasController](#multascontroller)
   - [EjemplaresController](#ejemplarescontroller)
   - [DevolucionesController](#devolucionescontroller)
   - [UsuariosExternosController](#usuariosexternoscontroller)
   - [SeguridadController](#seguridadcontroller)
   - [CatalogosController](#catalogoscontroller)
   - [InventarioController](#inventariocontroller)
   - [PriorityControllersCoverage (Tests transversales)](#prioritycontrollerscoverage)

---

## Resumen de Cobertura

| Métrica          | Valor    |
|-----------------|---------|
| **Líneas cubiertas**  | 2 938 / 5 508 |
| **Cobertura de líneas** | **53.34 %** |
| **Ramas cubiertas**   | 716 / 1 232 |
| **Cobertura de ramas** | **58.11 %** |
| **Total de tests**    | **101**  |
| **Tests pasados**     | 101 ✅  |
| **Tests fallidos**    | 0       |

### Interpretación

La cobertura global del **53 %** en líneas y **58 %** en ramas indica que los tests ejercen más de la mitad del código productivo. Los servicios de negocio clave (`PrestamosService`, `UsuariosService`, `LibrosService`) tienen cobertura superior al **85 %**, mientras que los controladores, que contienen mucha lógica de autorización y ramificaciones, bajan el promedio global.

---

## Cobertura por Clase

### Servicios de negocio

| Clase | Líneas | Ramas |
|------|--------|-------|
| `PrestamosService` | **96.07 %** | 100 % |
| `UsuariosService` | **95.55 %** | 75 % |
| `MultasService` | **90.00 %** | 100 % |
| `LibrosService` | **93.81 %** | 87.5 % |
| `AutoresService` | **86.20 %** | 100 % |
| `EjemplaresService` | **50.00 %** | 100 % |
| `RolesService` | 0 % | — |
| `UbicacionesService` | 0 % | — |
| `SeccionesService` | 0 % | — |
| `NivelesService` | 0 % | — |
| `GenerosLiterariosService` | 0 % | — |
| `EditorialesService` | 0 % | — |

### Controladores

| Controlador | Líneas | Ramas |
|------------|--------|-------|
| `InventarioController` | 62.38 % | 70.00 % |
| `LibrosController` | 57.87 % | 62.25 % |
| `SeguridadController` | 57.54 % | 62.50 % |
| `DevolucionesController` | 57.43 % | 62.50 % |
| `MultasController` | 52.72 % | 60.90 % |
| `PrestamosController` | 51.31 % | 50.83 % |
| `EjemplaresController` | 49.23 % | 50.00 % |
| `AuthenticationController` | 43.96 % | 44.23 % |
| `UsuariosExternosController` | 38.09 % | 37.50 % |
| `CatalogosController` | 30.82 % | 25.00 % |
| `ValuesController` | 0 % | — |

### Management y utilidades

| Clase | Líneas | Ramas |
|------|--------|-------|
| `JWT_Handler` | 100 % | 100 % |
| `Reply` | 100 % | 100 % |
| `JWT` (model) | 100 % | 100 % |
| `TokenManager` | 100 % | 100 % |
| `crypto` | 70 % | 100 % |
| `DataContext` | 88.57 % | 50 % |

---

## Infraestructura de Tests

**Ubicación:** `SysBiblioteca.API.Tests/`

**Stack de testing:**

| Herramienta | Versión | Uso |
|------------|---------|-----|
| xUnit | 2.6.6 | Framework de tests |
| Moq | 4.20.70 | Mocking de dependencias |
| FluentAssertions | 6.12.0 | Aserciones legibles |
| EF Core InMemory | 7.0.10 | Base de datos aislada por test |
| Mvc.Testing | 7.0.10 | Pruebas de integración HTTP |

### Archivos de apoyo

#### `Builders/TestEntities.cs`
Fábrica estática de entidades de prueba con valores predeterminados consistentes.

| Método | Entidad creada |
|--------|---------------|
| `AuthenticatedUser()` | Usuario autenticado con rol |
| `Permission()` | Objeto de permisos CRUD completo |
| `Book()` | Entidad `Libros` |
| `Ejemplar()` | Entidad `Ejemplares` (copia física) |
| `Prestamo()` | Entidad `Prestamos` (préstamo) |
| `Usuario()` | Entidad `Usuarios` básica |

#### `Helpers/TestDbContextFactory.cs`
Crea instancias de `DataContext` en memoria (EF Core InMemory) con nombre único por test, garantizando aislamiento total entre tests.

#### `Helpers/TestConfigurationFactory.cs`
Provee configuración JWT para tests:
- Expiración: 60 minutos
- Issuer: `SysBiblioteca.Tests`
- Clave secreta: cadena de prueba > 32 caracteres

#### `Helpers/ReplyResultExtensions.cs`
Extensiones de aserción para respuestas HTTP del tipo `Reply`:

| Método | Verifica |
|--------|---------|
| `ShouldBeOkReply()` | HTTP 200 con objeto `Reply` |
| `ShouldBeBadRequestReply()` | HTTP 400 |
| `ShouldBeConflictReply()` | HTTP 409 |
| `ShouldBeNotFoundReply()` | HTTP 404 |

---

## Tests de Management

**Ubicación:** `SysBiblioteca.API.Tests/Management/`

### CryptoTests.cs

**Clase bajo prueba:** `SysBiblioteca.API.Management.crypto`

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `Encrypt_TextoPlano_RetornaTextoDistinto` | Verifica que al cifrar un texto plano, el resultado es diferente al original (cifrado funciona). |
| 2 | `Decrypt_TextoEncriptado_RetornaTextoOriginal` | Verifica que descifrar el texto cifrado devuelve exactamente el texto original (reversibilidad del cifrado). |

---

### JWTHandlerTests.cs

**Clase bajo prueba:** `SysBiblioteca.API.Management.JWT_Handler`

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GenerateToken_UsuarioConRol_RetornaJwtValido` | Genera un token JWT para un usuario con rol y valida que el claim del nombre de usuario esté presente en el token resultante. |

---

## Tests de Servicios

**Ubicación:** `SysBiblioteca.API.Tests/Services/`

Los tests de servicios usan **base de datos en memoria** (EF Core InMemory), sin mocks de repositorio. Cada test opera sobre una instancia de `DataContext` independiente.

---

### UsuariosService

**Archivo:** `Services/ADM/UsuariosServiceTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Services.ADM.UsuariosService.UsuariosService`  
**Cobertura:** 95.55 % líneas · 75 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `Create_GetById_GetTokenActual_Y_GetUserInfo_RetornanUsuarioEsperado` | Crea un usuario, lo recupera por ID, obtiene su token actual y verifica su información completa. Valida el flujo CRUD básico del servicio. |
| 2 | `ActivateDeactivateLogoutYChangePassword_ModificanEstadoDelUsuario` | Prueba los cambios de estado del usuario: activación, desactivación, cierre de sesión (logout) y cambio de contraseña. Verifica que cada operación modifica correctamente el estado en la BD. |
| 3 | `GetEmpleadosYUsuarios_FiltraPorCargoYEstado` | Verifica que `GetEmpleados` y `GetUsuarios` filtran correctamente por cargo (solo empleados / solo lectores) y por estado activo/inactivo. |
| 4 | `UpdateEmpleado_ActualizaRolCargoYDatosPersonales` | Actualiza el rol, cargo y datos personales de un empleado y verifica que los cambios persisten correctamente. |
| 5 | `UpdateUsuario_ActualizaRolYDatosPersonales` | Actualiza el rol y datos personales de un usuario lector y verifica que los cambios persisten. |
| 6 | `LogIn_CredencialesInvalidas_IncrementaIntentos` | Con credenciales incorrectas, verifica que el contador de intentos fallidos aumenta en cada intento. |
| 7 | `LogIn_TercerIntentoInvalido_BloqueaUsuario` | Verifica que al tercer intento fallido consecutivo, el usuario queda bloqueado (`Estado = false`). |
| 8 | `LogIn_CredencialesValidas_GeneraTokenYReiniciaIntentos` | Con credenciales válidas, verifica que se genera un token JWT y el contador de intentos fallidos se reinicia a cero. |

---

### LibrosService

**Archivo:** `Services/INV/LibrosServiceTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Services.INV.LibrosService.LibrosService`  
**Cobertura:** 93.81 % líneas · 87.5 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `Search_SinFiltros_RetornaSoloLibrosActivos` | Busca sin filtros y verifica que solo se retornan libros con `Estado = true` (activos). |
| 2 | `Search_ConTituloAutorYGenero_RetornaCoincidenciaActiva` | Busca con filtros de título, autor y género literario, y verifica que se retorna el libro activo que coincide con los tres criterios. |
| 3 | `Search_ParaPrestamo_ExcluyeEjemplarConPrestamoActivo` | Al buscar libros disponibles para préstamo, verifica que se excluyen los libros cuyos únicos ejemplares tienen un préstamo activo. |
| 4 | `SearchInactivos_ConFiltros_RetornaLibroInactivo` | Verifica que la búsqueda de libros inactivos con filtros retorna correctamente los libros desactivados. |
| 5 | `Update_LibroExistente_ModificaCamposEsperados` | Actualiza campos de un libro (título, ISBN, editorial, páginas, año) y verifica que los cambios persisten en la BD. |
| 6 | `Activate_Y_Deactivate_CambianEstadoDelLibro` | Verifica que las operaciones de activar y desactivar un libro cambian correctamente el campo `Estado`. |
| 7 | `Create_Y_GetById_GuardanYObtienenLibro` | Crea un libro y lo recupera por ID, verificando que todos sus datos se persistieron correctamente. |

---

### EjemplaresService

**Archivo:** `Services/INV/EjemplaresServiceTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Services.INV.EjemplaresService.EjemplaresService`  
**Cobertura:** 50 % líneas · 100 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GetStock_LibroExistente_RetornaEjemplaresDelLibro` | Verifica que `GetStock` retorna todos los ejemplares (copias físicas) asociadas a un libro específico. |
| 2 | `GetAvailables_LibroConEjemplaresDisponibles_RetornaCantidadDisponible` | Verifica que `GetAvailables` cuenta correctamente los ejemplares sin préstamo activo (disponibles para prestar). |
| 3 | `GetEjemplarToLoan_LibroConDisponible_RetornaPrimerEjemplarDisponible` | Verifica que se devuelve el primer ejemplar disponible del libro para asignarlo a un nuevo préstamo. |
| 4 | `GetByCodigo_CodigoExistente_RetornaEjemplar` | Verifica que `GetByCodigo` retorna el ejemplar correcto dado un código QR/identificador. |

---

### AutoresService

**Archivo:** `Services/INV/AutoresServiceTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Services.INV.AutoresService.AutoresService`  
**Cobertura:** 86.20 % líneas · 100 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `Create_AutorValido_GuardaAutor` | Crea un nuevo autor y verifica que se persiste en la BD. |
| 2 | `Read_ConAutores_RetornaTodosLosAutores` | Verifica que `Read` retorna todos los autores registrados en el sistema. |
| 3 | `GetById_IdExistente_RetornaAutor` | Recupera un autor por su ID y verifica que los datos son correctos. |
| 4 | `GetByName_CoincidenciaParcialIgnoraMayusculas_RetornaAutor` | Busca autores por nombre con coincidencia parcial y sin distinción de mayúsculas/minúsculas. |
| 5 | `Update_AutorExistente_ActualizaAutor` | Actualiza el nombre de un autor y verifica que el cambio persiste. |

---

### PrestamosService

**Archivo:** `Services/PRS/PrestamosServiceTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Services.PRS.PrestamosService.PrestamosService`  
**Cobertura:** 96.07 % líneas · 100 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `Create_GetById_Y_Read_RetornanPrestamosNoFinalizados` | Crea un préstamo, lo recupera por ID y verifica que `Read` solo retorna préstamos no finalizados. |
| 2 | `GetPendingLoans_Y_GetOngoingLoans_FiltranPorEstadoDelFlujo` | Verifica que `GetPendingLoans` retorna préstamos pendientes (no entregados aún al usuario) y `GetOngoingLoans` retorna los que ya fueron entregados pero no devueltos. |
| 3 | `GetMyBooks_RetornaPrestamosEntregadosNoFinalizadosDelUsuario` | Verifica que un usuario solo ve sus propios préstamos activos (entregados, no finalizados). |
| 4 | `GetUserForLoans_RetornaUsuariosActivosSinCargo` | Verifica que se retornan solo los usuarios activos sin cargo (lectores) como elegibles para recibir préstamos. |
| 5 | `ValidatePrestamo_CuandoExistePrestamoActivo_RetornaPrestamo` | Verifica que si un usuario ya tiene un préstamo activo para un ejemplar, `ValidatePrestamo` lo detecta y lo retorna. |
| 6 | `LoanBook_MarcaPrestamoComoEntregado` | Verifica que `LoanBook` marca el préstamo como entregado al usuario (`Entregado = true`). |
| 7 | `MarkAsFinished_FinalizaPrestamoYRegistraUsuarioRecibio` | Finaliza un préstamo y registra qué empleado recibió la devolución del libro. |
| 8 | `GetFinishedLoans_RetornaPrestamosFinalizadosEntregados` | Verifica que `GetFinishedLoans` retorna únicamente préstamos que están finalizados y entregados. |
| 9 | `GetFinishedLoans_ConFechas_RetornaPrestamosFinalizadosDentroDelRangoSegunImplementacionActual` | Verifica el filtrado de préstamos finalizados dentro de un rango de fechas. |
| 10 | `GetLastLoanByEjemplar_RetornaPrestamoMasReciente` | Verifica que se retorna el préstamo más reciente asociado a un ejemplar específico. |

---

### MultasService

**Archivo:** `Services/PRS/MultasServiceTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Services.PRS.MultasService.MultasService`  
**Cobertura:** 90 % líneas · 100 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `Create_Y_GetById_GuardanYObtienenMulta` | Crea una multa y la recupera por ID, verificando que todos sus datos persisten correctamente. |
| 2 | `GetWaiting_RetornaMultasPendientesYEnRevision` | Verifica que `GetWaiting` retorna multas en estado pendiente y en revisión (no pagadas). |
| 3 | `GetPaid_RetornaSoloMultasPagadas` | Verifica que `GetPaid` retorna únicamente multas en estado pagado. |
| 4 | `GetMyFines_Y_GetMyPaidFines_FiltranPorUsuario` | Verifica que `GetMyFines` y `GetMyPaidFines` filtran las multas por el usuario autenticado, retornando solo las propias. |
| 5 | `CargarComprobante_MultaExistente_ActualizaComprobanteYPagoFisico` | Sube un comprobante de pago a una multa existente y verifica que se actualiza el campo del comprobante y el flag de pago físico. |

---

## Tests de Controladores

**Ubicación:** `SysBiblioteca.API.Tests/Controllers/`

Los tests de controladores usan **Moq** para simular todos los servicios. El controlador se instancia directamente (sin `WebApplicationFactory`), y se usa `HttpContext` simulado para inyectar tokens y permisos.

---

### AuthenticationController

**Archivo:** `Controllers/AuthenticationControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.AuthenticationController`  
**Cobertura:** 43.96 % líneas · 44.23 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `LogIn_CredencialesValidas_RetornaOkConToken` | Login con credenciales válidas devuelve HTTP 200 con token JWT en la respuesta. |
| 2 | `LogIn_UsuarioNoExiste_RetornaBadRequest` | Login de un usuario inexistente devuelve HTTP 400 Bad Request. |
| 3 | `LogOut_UsuarioExistente_RetornaOkYLlamaLogOut` | Logout de un usuario autenticado devuelve HTTP 200 y llama al método `LogOut` del servicio. |
| 4 | `Register_UsuarioDuplicado_RetornaBadRequest` | Intento de registro con un correo ya existente devuelve HTTP 400 Bad Request. |

---

### LibrosController

**Archivo:** `Controllers/LibrosControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.LibrosController`  
**Cobertura:** 57.87 % líneas · 62.25 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `CreateAutor_AutorNuevoYPermisoCrear_RetornaOkConResultadoExitoso` | Con permiso de creación y un autor nuevo (no duplicado), crea el autor y devuelve resultado exitoso. |
| 2 | `CreateAutor_AutorDuplicado_NoCreaRegistro` | Si el autor ya existe (búsqueda por nombre retorna resultado), no crea un nuevo registro (resultado = 0). |
| 3 | `SearchLibros_ServicioRetornaLibros_RetornaDTOsConAutoresGenerosYCantidad` | La búsqueda de libros retorna DTOs con información de autores, géneros y cantidad de ejemplares disponibles. |
| 4 | `DeactivateBook_LibroExistenteYPermisoUpdate_LlamaServicioDeactivate` | Con permiso de actualización, desactiva un libro llamando al método `Deactivate` del servicio. |

---

### PrestamosController

**Archivo:** `Controllers/PrestamosControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.PrestamosController`  
**Cobertura:** 51.31 % líneas · 50.83 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `ProcesarPrestamo_SinToken_RetornaBadRequest` | Sin token de autenticación, el procesamiento de préstamo devuelve resultado 0 (fallo). |
| 2 | `ProcesarPrestamo_PrestamoActivoExistente_RetornaConflict` | Si el usuario ya tiene un préstamo activo para ese libro, devuelve HTTP 409 Conflict. |
| 3 | `ProcesarPrestamo_SinEjemplaresDisponibles_RetornaNotFound` | Si no hay ejemplares disponibles del libro, devuelve HTTP 404 Not Found. |
| 4 | `GetPendingLoans_UsuarioConPermiso_RetornaOkConPrestamosPendientes` | Con permiso de lectura, devuelve los préstamos pendientes como lista de DTOs. |

---

### MultasController

**Archivo:** `Controllers/MultasControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.MultasController`  
**Cobertura:** 52.72 % líneas · 60.90 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GetMyFines_UsuarioConMultas_RetornaDTOsDeMultas` | El usuario autenticado con multas recibe una lista de DTOs con su información. |
| 2 | `GetMyFines_SinMultas_RetornaMensajeSinDatos` | Si el usuario no tiene multas, devuelve un mensaje indicando que no hay datos. |
| 3 | `UploadInvoce_SinToken_RetornaOkConResultadoCero` | Sin token válido, subir un comprobante de pago devuelve resultado 0. |

---

### EjemplaresController

**Archivo:** `Controllers/EjemplaresControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.EjemplaresController`  
**Cobertura:** 49.23 % líneas · 50 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GetEjemplares_UsuarioConPermiso_RetornaStockDelLibro` | Con permiso de lectura, retorna el inventario de ejemplares (copias) de un libro. |
| 2 | `DeactivateEjemplar_EjemplarExistente_LlamaUpdate` | Desactiva un ejemplar existente llamando a `Update` en el servicio y guarda los cambios en el contexto. |

---

### DevolucionesController

**Archivo:** `Controllers/DevolucionesControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.DevolucionesController`  
**Cobertura:** 57.43 % líneas · 62.5 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `FinishLoan_PrestamoVigente_FinalizaPrestamo` | Un préstamo dentro de plazo se finaliza correctamente (llama a `MarkAsFinished`). |
| 2 | `FinishLoan_PrestamoDemorado_RetornaResultadoDosSinFinalizar` | Un préstamo con retraso retorna resultado = 2 sin finalizarlo (se genera una multa pendiente). |
| 3 | `PayFine_PrestamoExistenteYMultaRegistrada_FinalizaPrestamo` | Al pagar una multa existente asociada a un préstamo demorado, el préstamo se finaliza. |

---

### UsuariosExternosController

**Archivo:** `Controllers/UsuariosExternosControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.UsuariosExternosController`  
**Cobertura:** 38.09 % líneas · 37.5 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GetMyData_UsuarioAutenticado_RetornaDatosUsuario` | El usuario autenticado puede recuperar su propio perfil de datos personales. |
| 2 | `ChangeMyPassword_PasswordAnteriorCorrecto_CambiaPassword` | Con la contraseña anterior correcta, el cambio de contraseña se procesa exitosamente. |
| 3 | `ChangeMyPassword_PasswordAnteriorIncorrecto_NoCambiaPassword` | Con la contraseña anterior incorrecta, el cambio se rechaza (no modifica nada). |

---

### SeguridadController

**Archivo:** `Controllers/SeguridadControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.SeguridadController`  
**Cobertura:** 57.54 % líneas · 62.5 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `ActivateUser_UsuarioConPermisoYRegistroExistente_RetornaOkConResultadoExitoso` | Con permiso de actualización y usuario existente, la activación de cuenta devuelve resultado exitoso. |
| 2 | `ActivateUser_SinToken_RetornaOkConResultadoCero` | Sin token de autenticación, la activación devuelve resultado 0 (no autorizado). |
| 3 | `GetUsuarios_ServicioRetornaDatos_RetornaOkConListaUsuarios` | Con permiso de lectura, devuelve la lista de usuarios como DTOs. |
| 4 | `CreateRol_RolYaExiste_RetornaOkConResultadoCero` | Si el rol ya existe, no crea un duplicado y devuelve resultado 0. |

---

### CatalogosController

**Archivo:** `Controllers/CatalogosControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.CatalogosController`  
**Cobertura:** 30.82 % líneas · 25 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GetGeneros_UsuarioConPermisoYDatos_RetornaOkConGeneros` | Con permiso de lectura y géneros registrados, retorna la lista de géneros literarios. |
| 2 | `GetGeneros_SinDatos_RetornaOkConMensajeSinDatos` | Sin géneros registrados, devuelve un mensaje indicando que no hay datos disponibles. |
| 3 | `GetEstados_UsuarioSinPermisoLectura_RetornaOkConResultadoCero` | Sin permiso de lectura, la consulta de estados devuelve resultado 0 (acceso denegado). |

---

### InventarioController

**Archivo:** `Controllers/InventarioControllerTests.cs`  
**Clase bajo prueba:** `SysBiblioteca.API.Controllers.InventarioController`  
**Cobertura:** 62.38 % líneas · 70 % ramas

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `GetSecciones_UsuarioConPermisoYDatos_RetornaOkConSecciones` | Con permiso de lectura y secciones registradas, retorna la lista de secciones del inventario. |
| 2 | `CreateSeccion_NombreDuplicado_RetornaOkConResultadoCero` | Si ya existe una sección con el mismo nombre, no la duplica y devuelve resultado 0. |
| 3 | `DeleteNivel_NivelNoExiste_RetornaOkConResultadoCero` | Intentar eliminar un nivel inexistente devuelve resultado 0 (no hay nada que eliminar). |

---

### PriorityControllersCoverage

**Archivo:** `Controllers/PriorityControllersCoverageTests.cs`  
**Tipo:** Tests transversales parametrizados (`[Theory]` con `[MemberData]`)  
**Controladores cubiertos:** LibrosController, SeguridadController, InventarioController, MultasController, PrestamosController, EjemplaresController, AuthenticationController, DevolucionesController

Estos tests parametrizados ejecutan el mismo escenario de validación de token/permisos sobre **múltiples acciones de múltiples controladores** en una sola definición de test.

| # | Nombre del test | Descripción |
|---|----------------|------------|
| 1 | `AccionesConToken_TokenNulo_RetornanOkConReplySinResultado` | `[Theory]` — Para todas las acciones protegidas de los 8 controladores: sin token (null), la respuesta es resultado = 0 (acceso denegado). |
| 2 | `AccionesConToken_TokenNoAsociadoAUsuario_RetornanOkConReplySinResultado` | `[Theory]` — Con token que no corresponde a ningún usuario activo, la respuesta es resultado = 0. |
| 3 | `AccionesConToken_UsuarioSinPermisos_RetornanOkConReplySinResultado` | `[Theory]` — Con token válido pero sin permisos CRUD para la acción, la respuesta es resultado = 0. |
| 4 | `Authentication_LogIn_UsuarioInexistente_RetornaBadRequest` | `[Fact]` — Login con correo que no existe en el sistema devuelve HTTP 400. |
| 5 | `Authentication_LogIn_UsuarioActivoSinRol_RetornaOkResultadoDos` | `[Fact]` — Login de un usuario activo pero sin rol asignado devuelve resultado = 2 (requiere asignación de rol). |

---

## Patrones de testing utilizados

| Patrón | Descripción |
|--------|-------------|
| **Arrange-Act-Assert** | Estructura estándar en todos los tests |
| **In-Memory Database** | Tests de servicios usan EF Core InMemory para aislamiento total |
| **Mocking con Moq** | Tests de controladores mockean todos los servicios |
| **FluentAssertions** | Aserciones descriptivas y legibles |
| **Test Data Builders** | `TestEntities` como fábrica centralizada de datos de prueba |
| **Parameterized Tests** | `[Theory]` + `[MemberData]` para cobertura transversal de controladores |
| **Extension Methods** | `ReplyResultExtensions` para aserciones de tipo HTTP reutilizables |
