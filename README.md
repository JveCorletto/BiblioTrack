# BiblioTrack / SysBiblioteca

Sistema web para la gestión de una biblioteca, desarrollado con **ASP.NET Core 7**, **ASP.NET Core MVC**, **API REST**, **Entity Framework Core** y **SQL Server**. El proyecto permite administrar usuarios, roles, permisos, inventario bibliográfico, ejemplares, préstamos, devoluciones, multas, pagos y consultas de libros para usuarios externos.

---

## Descripción general

**BiblioTrack** es una solución orientada a digitalizar procesos bibliotecarios que normalmente se gestionan de forma manual o en hojas de cálculo. El sistema separa la lógica de negocio en una **API REST** y la interfaz de usuario en una aplicación **MVC**, lo que facilita el mantenimiento, la escalabilidad y la integración con otros servicios.

El proyecto incluye:

- API REST protegida con JWT.
- Interfaz web MVC con vistas Razor.
- Gestión de sesión en la UI mediante `HttpContext.Session`.
- Middleware personalizado para controlar acceso a vistas según sesión activa.
- Control de roles: Administrador, Empleado y Usuario.
- Menú dinámico basado en permisos por rol.
- Módulo de seguridad para usuarios, empleados, roles, permisos y estados.
- Módulo de inventario bibliográfico: autores, editoriales, géneros literarios, libros, ejemplares, secciones, estanterías, niveles y ubicaciones.
- Módulo de préstamos y devoluciones.
- Módulo de multas, comprobantes y pagos físicos/digitales.
- Módulo de usuario externo para consulta de perfil, libros, reservas y pagos.
- Módulo RFID/serial con SignalR para lectura de etiquetas.
- Generación de códigos QR para ejemplares.
- Exportación y utilidades frontend mediante librerías JavaScript adicionales.
- Scripts SQL para crear y poblar la base de datos.
- Respaldo `.bak` de la base de datos.
- Diagrama de base de datos incluido como imagen.
- Proyecto de pruebas unitarias para API.
- Reporte de cobertura generado previamente.

---

## Tecnologías utilizadas

| Componente | Tecnología / librería |
|---|---|
| Backend API | ASP.NET Core Web API 7.0 |
| Frontend web | ASP.NET Core MVC 7.0 + Razor Views |
| Base de datos | SQL Server |
| ORM | Entity Framework Core 7.0 |
| Autenticación | JWT Bearer |
| Documentación API | Swagger / Swashbuckle / OpenAPI |
| Serialización JSON | Newtonsoft.Json |
| UI base | Bootstrap, jQuery, DataTables |
| Alertas y notificaciones | SweetAlert, AlertifyJS |
| Comunicación en tiempo real | SignalR |
| Lectura serial/RFID | System.IO.Ports / RJCP.SerialPortStream |
| Generación QR | QRCoder + System.Drawing.Common |
| Exportación a Excel / archivos | ExcelJS, XLSX, JSZip, FileSaver |
| Fechas y formularios | Moment.js, Bootstrap Datepicker, jQuery Mask, jQuery UI |
| Gráficos y visualización | CanvasJS, Flot, jqvmap, Leaflet, gmaps |
| Editores enriquecidos incluidos como assets | CKEditor, TinyMCE, Summernote |
| Carga/edición de archivos incluidos como assets | Uppy, Cropper |
| Resaltado de sintaxis incluido como asset | PrismJS |
| Calendario frontend incluido como asset | FullCalendar |
| Pruebas | xUnit, Moq, FluentAssertions, coverlet.collector, Microsoft.NET.Test.Sdk |

---

## Dependencias NuGet principales

### `SysBiblioteca.API`

| Paquete | Versión | Uso |
|---|---:|---|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 7.0.10 | Autenticación JWT |
| `Microsoft.AspNetCore.OpenApi` | 7.0.10 | Soporte OpenAPI |
| `Microsoft.EntityFrameworkCore` | 7.0.10 | ORM |
| `Microsoft.EntityFrameworkCore.Design` | 7.0.10 | Herramientas de diseño EF Core |
| `Microsoft.EntityFrameworkCore.SqlServer` | 7.0.10 | Provider SQL Server |
| `Microsoft.EntityFrameworkCore.Tools` | 7.0.10 | Herramientas EF Core |
| `Newtonsoft.Json` | 13.0.3 | Serialización/deserialización JSON |
| `QRCoder` | 1.6.0 | Generación de códigos QR |
| `Swashbuckle.AspNetCore` | 6.5.0 | Swagger |
| `Swashbuckle.AspNetCore.Swagger` | 6.5.0 | Swagger |
| `System.Drawing.Common` | 9.0.0 | Generación/procesamiento gráfico asociado a QR |

### `SysBiblioteca.UI`

| Paquete | Versión | Uso |
|---|---:|---|
| `Microsoft.VisualStudio.Web.CodeGeneration.Design` | 7.0.12 | Scaffolding/generación de código MVC |
| `RJCP.SerialPortStream` | 3.0.1 | Comunicación serial alternativa |
| `System.IO.Ports` | 9.0.0 | Comunicación con puerto serial |

### `SysBiblioteca.API.Tests`

| Paquete | Uso |
|---|---|
| `xUnit` | Framework de pruebas unitarias |
| `Moq` | Mocking de dependencias |
| `FluentAssertions` | Aserciones expresivas |
| `Microsoft.NET.Test.Sdk` | Ejecución de pruebas .NET |
| `coverlet.collector` | Recolección de cobertura |

---

## Estructura del proyecto

```text
BiblioTrack/
├── SysBiblioteca.sln
├── README.md
├── SysBiblioteca.API/
│   ├── Controllers/
│   ├── DTO/
│   ├── Management/
│   ├── Models/
│   │   ├── ADM/
│   │   ├── CTL/
│   │   ├── INV/
│   │   └── PRS/
│   ├── Services/
│   │   ├── ADM/
│   │   ├── CTL/
│   │   ├── INV/
│   │   └── PRS/
│   ├── dbContext/
│   ├── Program.cs
│   ├── appsettings.json
│   └── SysBiblioteca.API.csproj
├── SysBiblioteca.API.Tests/
│   ├── Controllers/
│   ├── Services/
│   ├── TestData/
│   ├── TestHelpers/
│   └── SysBiblioteca.API.Tests.csproj
├── SysBiblioteca.UI/
│   ├── Controllers/
│   │   ├── PersonalExterno/
│   │   └── PersonalInterno/
│   ├── Management/
│   ├── Middlewares/
│   ├── Models/
│   ├── RDIF_Module/
│   ├── Views/
│   ├── wwwroot/
│   ├── Program.cs
│   ├── appsettings.json
│   └── SysBiblioteca.UI.csproj
├── coveragereport/
├── SysBiblioteca_Schema.sql
├── SysBiblioteca_Data.sql
├── SysBiblioteca_BooksData.sql
├── SysBiblioteca.bak
└── Diagrama DB_01.png
```

> Nota: La carpeta se llama `RDIF_Module`, aunque funcionalmente corresponde a un módulo RFID.

---

## Arquitectura del sistema

El proyecto está dividido en tres componentes principales:

### 1. `SysBiblioteca.API`

Proyecto backend encargado de exponer servicios REST, conectarse a SQL Server y ejecutar la lógica de negocio.

Responsabilidades principales:

- Validar credenciales de usuarios.
- Generar y validar tokens JWT.
- Administrar usuarios, roles, permisos, cargos, estados y menús.
- Administrar catálogos generales.
- Gestionar autores, editoriales, géneros literarios, libros y ejemplares.
- Gestionar secciones, estanterías, niveles y ubicaciones físicas.
- Gestionar préstamos, reservas, devoluciones y multas.
- Generar códigos QR para ejemplares.
- Consultar ejemplares escaneados.
- Exponer documentación de endpoints mediante Swagger.
- Exponer utilidades internas de cifrado/descifrado mediante `ValuesController`.

### 2. `SysBiblioteca.UI`

Proyecto frontend MVC encargado de renderizar las vistas del sistema y consumir la API mediante peticiones AJAX.

Responsabilidades principales:

- Mostrar login y registro de usuarios.
- Administrar sesión mediante `HttpContext.Session`.
- Redirigir usuarios autenticados/no autenticados mediante middleware personalizado.
- Renderizar menú dinámico según rol.
- Consumir endpoints de la API.
- Presentar módulos internos y externos.
- Escuchar lecturas RFID mediante SignalR.
- Ejecutar lógica frontend con jQuery, AJAX, DataTables y librerías de apoyo.
- Exportar información mediante utilidades JavaScript.

### 3. `SysBiblioteca.API.Tests`

Proyecto de pruebas unitarias orientado a validar controladores, servicios y lógica de negocio del API.

Responsabilidades principales:

- Probar controladores API.
- Probar servicios de dominio.
- Usar mocks para dependencias.
- Generar cobertura con coverlet.

---

## Módulos funcionales

### Autenticación y sesión

- Inicio de sesión.
- Cierre de sesión.
- Registro de usuarios externos.
- Validación de token.
- Renderizado de menú según permisos.
- Middleware de sesión en la aplicación MVC.
- Redirección automática según sesión activa.
- Activación y desactivación de variables de sesión desde rutas auxiliares de UI.

Controlador principal API:

```text
SysBiblioteca.API/Controllers/AuthenticationController.cs
```

Controlador principal UI:

```text
SysBiblioteca.UI/Controllers/HomeController.cs
```

Endpoints API relevantes:

```text
POST SysBiblioteca/API/Authentication/LogIn
POST SysBiblioteca/API/Authentication/LogOut
POST SysBiblioteca/API/Authentication/Register
POST SysBiblioteca/API/Authentication/GetMenu
POST SysBiblioteca/API/Authentication/ValidateView
POST SysBiblioteca/API/Authentication/validateToken
```

Rutas UI auxiliares:

```text
GET  /setProfile
POST /activateSesion
GET  /deactivateSesion
```

---

### Seguridad y administración

Permite administrar usuarios internos, usuarios externos, empleados, roles, cargos, estados y permisos.

Controlador principal:

```text
SysBiblioteca.API/Controllers/SeguridadController.cs
```

Funciones principales:

- Crear empleados.
- Crear usuarios.
- Activar y desactivar usuarios.
- Consultar usuarios activos e inactivos.
- Crear, actualizar, activar y desactivar roles.
- Consultar información de usuarios por ID.
- Gestionar permisos asociados al menú dinámico mediante `Link_Rol_Menu`.

Roles incluidos en la base de datos:

| ID | Rol |
|---:|---|
| 1 | Administrador |
| 2 | Empleado |
| 3 | Usuario |

---

### Inventario bibliográfico

Permite administrar la estructura física y lógica del inventario.

Controladores principales:

```text
SysBiblioteca.API/Controllers/InventarioController.cs
SysBiblioteca.API/Controllers/LibrosController.cs
SysBiblioteca.API/Controllers/EjemplaresController.cs
```

Funciones principales:

- Gestión de autores.
- Gestión de editoriales.
- Gestión de géneros literarios.
- Gestión de libros.
- Activación y desactivación de libros.
- Gestión de ejemplares.
- Generación de QR para ejemplares.
- Consulta de ejemplares escaneados.
- Gestión de secciones, estanterías, niveles y ubicaciones.
- Manejo de imagen/foto de libro en formato codificado.

---

### Préstamos y devoluciones

Gestiona el flujo operativo de préstamo de libros, reservas, devolución y finalización de préstamos.

Controladores principales:

```text
SysBiblioteca.API/Controllers/PrestamosController.cs
SysBiblioteca.API/Controllers/DevolucionesController.cs
```

Funciones principales:

- Procesar préstamos.
- Procesar préstamos mediante RFID.
- Consultar préstamos pendientes.
- Consultar préstamos activos.
- Buscar usuarios para préstamo.
- Prestar libros.
- Consultar libros prestados por usuario.
- Finalizar préstamos.
- Consultar préstamos finalizados.
- Registrar pagos de multa desde devolución.

---

### Multas y pagos

Permite consultar, generar, aprobar y pagar multas asociadas a préstamos.

Controlador principal:

```text
SysBiblioteca.API/Controllers/MultasController.cs
```

Funciones principales:

- Consultar multas pendientes.
- Consultar multas pagadas.
- Generar comprobantes o facturas de multa.
- Aprobar comprobantes.
- Registrar pagos físicos.
- Consultar multas del usuario autenticado.
- Consultar multas pagadas del usuario autenticado.
- Consultar comprobante/factura.
- Subir comprobantes de pago.
- Registrar usuario validador y fecha de validación.

---

### Usuario externo

Permite que un lector consulte su información y gestione operaciones personales dentro del sistema.

Controlador principal:

```text
SysBiblioteca.API/Controllers/UsuariosExternosController.cs
```

Funciones principales:

- Consultar datos personales.
- Actualizar datos personales.
- Cambiar contraseña.
- Buscar y reservar libros desde la UI.
- Consultar libros prestados.
- Consultar pagos.
- Consultar pantalla principal del módulo de usuario externo.

---

### RFID / puerto serial

La UI incluye un módulo para escuchar lecturas desde un puerto serial y enviarlas a la interfaz en tiempo real mediante SignalR.

Archivos principales:

```text
SysBiblioteca.UI/RDIF_Module/RfidHub.cs
SysBiblioteca.UI/RDIF_Module/SerialPortConfig.cs
SysBiblioteca.UI/RDIF_Module/SerialPortHostedService.cs
SysBiblioteca.UI/RDIF_Module/SerialPortListener.cs
SysBiblioteca.UI/wwwroot/js/rfid_listener.js
SysBiblioteca.UI/wwwroot/js/rfid_module/rfid.js
```

El servicio serial está registrado como servicio hospedado:

```text
SerialPortHostedService : IHostedService
```

Esto implica que la UI intenta iniciar la escucha del puerto serial al levantar la aplicación.

> Importante: si el puerto configurado no existe, está ocupado o no corresponde al lector conectado, el módulo RFID puede fallar durante la inicialización o ejecución.

---

### Utilidades internas de cifrado

Existe un controlador auxiliar:

```text
SysBiblioteca.API/Controllers/ValuesController.cs
```

Este controlador expone endpoints para cifrar y descifrar cadenas mediante la clase `crypto`.

Rutas:

```text
POST api/Values/encriptString
POST api/Values/decriptString
```

> Observación técnica: este controlador no sigue la ruta base del resto del API (`SysBiblioteca/API/[controller]`), sino `api/[controller]`. Además, el endpoint de descifrado requiere rol `Administrador`.

---

## Servicios y lógica core

La lógica de negocio está organizada principalmente en la carpeta:

```text
SysBiblioteca.API/Services/
```

Submódulos principales:

| Carpeta | Propósito |
|---|---|
| `ADM` | Servicios de usuarios, roles, menús, cargos, datos personales y permisos |
| `CTL` | Servicios de catálogos generales y estados |
| `INV` | Servicios de inventario, libros, autores, editoriales, géneros, ejemplares, ubicaciones |
| `PRS` | Servicios de préstamos, devoluciones, multas y estados de multas |

Además, existe un servicio base genérico:

```text
SysBiblioteca.API/Services/CRUD.cs
```

Este servicio centraliza operaciones CRUD reutilizables por servicios específicos.

La carpeta `Management` contiene lógica transversal de autenticación, token y utilidades, incluyendo:

```text
SysBiblioteca.API/Management/JWT_Handler.cs
SysBiblioteca.API/Management/TokenManager.cs
SysBiblioteca.API/Management/crypto.cs
SysBiblioteca.UI/Management/API_Configs.cs
```

---

## Base de datos

El proyecto utiliza SQL Server y una base de datos llamada:

```text
SysBiblioteca
```

Archivos incluidos:

| Archivo | Descripción |
|---|---|
| `SysBiblioteca_Schema.sql` | Script para crear la base de datos y sus tablas. |
| `SysBiblioteca_Data.sql` | Script con datos base: roles, usuarios, catálogos, menús, etc. |
| `SysBiblioteca_BooksData.sql` | Script con datos de libros, autores-libros y géneros-libros. |
| `SysBiblioteca.bak` | Respaldo de base de datos. |
| `Diagrama DB_01.png` | Diagrama visual de la base de datos. |

Tablas principales detectadas:

```text
Autores
AutoresLibros
Cargos
DatosPersonales
Editoriales
Ejemplares
Estados
EstadosMultas
Estanterias
Generos
GenerosLibros
GenerosLiterarios
Libros
Link_Rol_Menu
Menus
Multas
Niveles
Prestamos
Roles
Secciones
Ubicaciones
Usuarios
```

### Campos críticos relevantes

| Tabla / entidad | Campo | Propósito |
|---|---|---|
| `Usuarios` | `Token` | Almacena token activo o referencia de sesión del usuario. |
| `Usuarios` | `UltimoAcceso` | Guarda fecha/hora de último acceso. |
| `Usuarios` | `ConteoIntentos` | Controla intentos de inicio de sesión. |
| `Usuarios` | `ActualRute` | Campo presente en el modelo C#. Revisar consistencia con el script SQL. |
| `Link_Rol_Menu` | `Create`, `Read`, `Update`, `Delete` | Permisos CRUD por rol y menú. |
| `Menus` | `IdParent`, `IdSubParent`, `Url`, `Icono` | Soporte para menú dinámico jerárquico. |
| `Multas` | `ComprobantePago`, `PagoFisico`, `FechaValidacion`, `IdUsuarioValidacion` | Flujo de comprobantes, pagos físicos y validación. |
| `Libros` | `FotoLibro` | Imagen del libro codificada o almacenada como texto. |
| `Ejemplares` | `CodigoEjemplar` | Código operativo usado para QR/RFID/escaneo. |

### Observación de consistencia de modelo

En el modelo C# `Ubicaciones.cs` aparece una propiedad llamada:

```text
IdAutorLibro
```

Sin embargo, en el script SQL la tabla `Ubicaciones` define:

```text
IdUbicacion
IdLibro
IdNivel
```

Se recomienda revisar este mapeo porque puede generar inconsistencias con Entity Framework Core si se realizan operaciones directas sobre `Ubicaciones`.

### Orden recomendado para preparar la base de datos

Opción 1: usando scripts SQL:

```text
1. Ejecutar SysBiblioteca_Schema.sql
2. Ejecutar SysBiblioteca_Data.sql
3. Ejecutar SysBiblioteca_BooksData.sql
```

Opción 2: usando respaldo:

```text
Restaurar SysBiblioteca.bak desde SQL Server Management Studio o Azure Data Studio.
```

---

## Configuración requerida

El proyecto usa archivos `appsettings.json` en la API y en la UI.

> Recomendación: para publicar o compartir el proyecto, no incluir credenciales reales en `appsettings.json`. Usar `appsettings.Development.json`, variables de entorno o User Secrets.

### `SysBiblioteca.API/appsettings.json`

Crear o ajustar el archivo en:

```text
SysBiblioteca.API/appsettings.json
```

Ejemplo recomendado para desarrollo local con autenticación integrada:

```json
{
  "ConnectionStrings": {
    "SysBiblioteca_Context": "Server=localhost;Database=SysBiblioteca;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "JWT": {
    "JWT_EXPIRE_MINUTES": "120",
    "JWT_EXPIRE_DAYS_SERVICES": "365",
    "JWT_ISSUER_TOKEN": "ufg.edu.sv",
    "JWT_AUDIENCE_TOKEM": "ufg.edu.sv",
    "JWT_SUBJECT_TOKEN": "SysBiblioteca_Auth",
    "JWT_SECRET_KEY": "CAMBIAR_ESTA_CLAVE_POR_UNA_CLAVE_SEGURA_DE_AL_MENOS_32_CARACTERES"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Si se usa SQL Server con usuario y contraseña:

```json
"SysBiblioteca_Context": "Server=localhost;Database=SysBiblioteca;User Id=sa;Password=TU_PASSWORD;TrustServerCertificate=True;"
```

> Importante: En el código aparece la clave `JWT_AUDIENCE_TOKEM` con la palabra `TOKEM`. Se debe respetar ese nombre porque así está escrito en la configuración del proyecto.

Claves JWT detectadas:

| Clave | Descripción |
|---|---|
| `JWT_SECRET_KEY` | Clave usada para firmar tokens. |
| `JWT_ISSUER_TOKEN` | Emisor del token. |
| `JWT_AUDIENCE_TOKEM` | Audiencia del token. Mantener el nombre con `TOKEM`. |
| `JWT_SUBJECT_TOKEN` | Subject del token. |
| `JWT_EXPIRE_MINUTES` | Expiración en minutos para tokens normales. |
| `JWT_EXPIRE_DAYS_SERVICES` | Expiración en días para tokens de servicio o flujos extendidos. |

### `SysBiblioteca.UI/appsettings.json`

Crear o ajustar el archivo en:

```text
SysBiblioteca.UI/appsettings.json
```

Ejemplo para API en HTTPS:

```json
{
  "API_Configs": {
    "URL": "https://localhost:7174/SysBiblioteca/API/"
  },
  "SerialPortConfig": {
    "PortName": "COM4",
    "BaudRate": 9600
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Ejemplo para API en HTTP:

```json
{
  "API_Configs": {
    "URL": "http://localhost:5037/SysBiblioteca/API/"
  },
  "SerialPortConfig": {
    "PortName": "COM4",
    "BaudRate": 9600
  },
  "AllowedHosts": "*"
}
```

La clase `API_Configs` contiene las propiedades:

```text
URL
AppToken
```

En el archivo real detectado solo se configura `URL`. `AppToken` existe como propiedad, pero no se observa configurado de forma efectiva en `appsettings.json`.

### Configuración serial

| Clave | Ejemplo | Uso |
|---|---|---|
| `SerialPortConfig:PortName` | `COM4` | Puerto serial del lector RFID. |
| `SerialPortConfig:BaudRate` | `9600` | Velocidad de comunicación serial. |

---

## Ejecución del proyecto

### 1. Clonar o descomprimir el repositorio

```bash
git clone <url-del-repositorio>
cd BiblioTrack
```

O descomprimir el archivo ZIP y entrar a la carpeta del proyecto.

### 2. Restaurar dependencias

```bash
dotnet restore SysBiblioteca.sln
```

### 3. Compilar solución

```bash
dotnet build SysBiblioteca.sln
```

### 4. Ejecutar la API

```bash
cd SysBiblioteca.API
dotnet run
```

Puertos configurados en `launchSettings.json`:

```text
HTTPS: https://localhost:7174
HTTP:  http://localhost:5037
Swagger: /swagger
```

URL Swagger esperada:

```text
https://localhost:7174/swagger
```

### 5. Ejecutar la UI

En otra terminal:

```bash
cd SysBiblioteca.UI
dotnet run
```

Puertos configurados en `launchSettings.json`:

```text
HTTPS: https://localhost:7271
HTTP:  http://localhost:5201
```

URL esperada:

```text
https://localhost:7271
```

---

## Ejecución de pruebas y cobertura

El repositorio incluye un proyecto de pruebas:

```text
SysBiblioteca.API.Tests
```

Para ejecutar las pruebas:

```bash
dotnet test SysBiblioteca.API.Tests/SysBiblioteca.API.Tests.csproj
```

Para ejecutar pruebas con cobertura:

```bash
dotnet test SysBiblioteca.API.Tests/SysBiblioteca.API.Tests.csproj --collect:"XPlat Code Coverage"
```

El repositorio también contiene una carpeta `coveragereport/` con un reporte HTML generado previamente.

---

## Credenciales de prueba

En el script `SysBiblioteca_Data.sql` se detectaron usuarios base. La contraseña almacenada aparece codificada en Base64 Unicode como:

```text
MQAyADMANAA=
```

Esto corresponde a:

```text
1234
```

Usuarios detectados:

| Usuario | Rol aproximado | Contraseña |
|---|---|---|
| `jvemartinez` | Administrador | `1234` |
| `angie` | Empleado | `1234` |
| `gabriela` | Administrador | `1234` |
| `diego` | Usuario | `1234` |
| `adriana` | Usuario | `1234` |

> Recomendación: cambiar estas contraseñas al preparar un ambiente real o de demostración pública.

---

## Principales rutas de la UI

| Ruta | Módulo |
|---|---|
| `/Home/Index` | Login |
| `/Home/Register` | Registro |
| `/setProfile` | Redirección según perfil/sesión |
| `/activateSesion` | Activación de sesión MVC |
| `/deactivateSesion` | Cierre/limpieza de sesión MVC |
| `/SysBiblioteca/Index` | Panel principal |
| `/Inventario/Estanteria` | Gestión de estanterías |
| `/Inventario/Catalogos` | Catálogos de inventario |
| `/Inventario/Libros` | Gestión de libros |
| `/Inventario/RFID` | Módulo RFID |
| `/PrestamosDevoluciones/Prestamos` | Préstamos |
| `/PrestamosDevoluciones/Devoluciones` | Devoluciones |
| `/PrestamosDevoluciones/Pagos` | Pagos de mora |
| `/PrestamosDevoluciones/EventHandler` | Vista auxiliar de eventos/flujo operativo |
| `/Reportes/Usuarios` | Reporte de usuarios |
| `/Reportes/Libros` | Reporte de libros |
| `/Seguridad/Usuarios` | Administración de usuarios |
| `/Seguridad/Empleados` | Administración de empleados |
| `/Seguridad/Roles` | Administración de roles |
| `/Seguridad/Permisos` | Administración de permisos |
| `/Usuarios/Index` | Inicio del módulo de usuario externo |
| `/Usuarios/BusquedaReserva` | Búsqueda y reserva de libros |
| `/Usuarios/MisLibros` | Libros del usuario |
| `/Usuarios/MiPerfil` | Perfil del usuario |
| `/Usuarios/Pagos` | Pagos del usuario |

> Observación: se detectó la acción `/Seguridad/Permisos`, pero debe verificarse que exista su vista Razor correspondiente antes de exponerla en navegación.

---

## Endpoints principales de la API

Base URL recomendada en desarrollo:

```text
https://localhost:7174/SysBiblioteca/API/
```

### Autenticación

```text
POST Authentication/LogIn
POST Authentication/LogOut
POST Authentication/Register
POST Authentication/GetMenu
POST Authentication/ValidateView
POST Authentication/validateToken
```

### Catálogos

```text
POST Catalogos/GetGeneros
POST Catalogos/GetEstados
POST Catalogos/GetRoles
POST Catalogos/GetCargos
```

### Inventario

```text
POST Inventario/GetSecciones
POST Inventario/GetSeccion
POST Inventario/CreateSeccion
POST Inventario/UpdateSeccion
POST Inventario/DeleteSeccion
POST Inventario/GetEstanterias
POST Inventario/GetEstanteria
POST Inventario/CreateEstanteria
POST Inventario/UpdateEstanteria
POST Inventario/DeleteEstanteria
POST Inventario/GetNiveles
POST Inventario/GetNivel
POST Inventario/CreateNivel
POST Inventario/UpdateNivel
POST Inventario/DeleteNivel
```

### Libros

```text
POST Libros/GetAutores
POST Libros/GetAutor
POST Libros/CreateAutor
POST Libros/UpdateAutor
POST Libros/DeleteAutor
POST Libros/GetEditoriales
POST Libros/GetEditorial
POST Libros/CreateEditorial
POST Libros/UpdateEditorial
POST Libros/DeleteEditorial
POST Libros/GetGeneros
POST Libros/GetGenero
POST Libros/CreateGenero
POST Libros/UpdateGenero
POST Libros/DeleteGenero
POST Libros/CreateLibro
POST Libros/SearchLibros
POST Libros/SearchLibrosInactivos
POST Libros/GetLibro
POST Libros/GetAutoresLibro
POST Libros/GetGenerosLibro
POST Libros/UpdateLibro
POST Libros/AddAutor
POST Libros/RemoveAutor
POST Libros/AddGender
POST Libros/RemoveGender
POST Libros/ActivateBook
POST Libros/DeactivateBook
```

### Ejemplares

```text
POST Ejemplares/CreateEjemplar
POST Ejemplares/GetEjemplares
POST Ejemplares/GetScannedEjemplar
POST Ejemplares/GetQREjemplar
POST Ejemplares/DeactivateEjemplar
```

### Préstamos

```text
POST Prestamos/ProcesarPrestamo
POST Prestamos/PrestamoRFID
POST Prestamos/GetPendingLoans
POST Prestamos/GetOngoingLoans
POST Prestamos/GetUserForLoans
POST Prestamos/GetLoan
POST Prestamos/LoanBook
POST Prestamos/GetMyBooks
```

### Devoluciones

```text
POST Devoluciones/GetLoan
POST Devoluciones/FinishLoan
POST Devoluciones/PayFine
POST Devoluciones/GetFinishedLoans
```

### Multas

```text
POST Multas/GetMultasPendientes
POST Multas/GetMultasPagadas
POST Multas/GenerateInvoce
POST Multas/GetUnpaidInvoce
POST Multas/ApproveInvoce
POST Multas/PhysicalPayment
POST Multas/GetMyFines
POST Multas/GetMyPaidFines
POST Multas/GetInvoce
POST Multas/UploadInvoce
```

### Seguridad

```text
POST Seguridad/ActivateUser
POST Seguridad/DeactivateUser
POST Seguridad/GetUserById
POST Seguridad/CreateEmpleado
POST Seguridad/GetEmpleados
POST Seguridad/GetEmpleadosInactivos
POST Seguridad/GetEmpleadoById
POST Seguridad/UpdateEmpleado
POST Seguridad/CreateUsuario
POST Seguridad/GetUsuarios
POST Seguridad/GetUsuariosInactivos
POST Seguridad/GetUsuarioById
POST Seguridad/UpdateUsuario
POST Seguridad/CreateRol
POST Seguridad/GetRoles
POST Seguridad/GetRolesInactivos
POST Seguridad/GetRolById
POST Seguridad/ActivateRol
POST Seguridad/DeactivateRol
POST Seguridad/UpdateRol
```

### Usuarios externos

```text
POST UsuariosExternos/GetMyData
POST UsuariosExternos/UpdateMyData
POST UsuariosExternos/ChangeMyPassword
```

### Utilidades internas / Values

Estos endpoints usan una ruta base distinta:

```text
POST api/Values/encriptString
POST api/Values/decriptString
```

> Observación: `ValuesController` no está bajo `SysBiblioteca/API/[controller]`, sino bajo `api/[controller]`.

---

## Middleware y flujo de sesión UI

La UI contiene un middleware personalizado:

```text
SysBiblioteca.UI/Middlewares/AuthenticationMiddleware.cs
```

Comportamiento general:

- Si existe sesión activa y el usuario intenta entrar a `/Home`, lo redirige a `/SysBiblioteca`.
- Si no existe sesión activa y el usuario intenta entrar a una ruta distinta de `/Home`, lo redirige a `/Home`.
- Usa la variable de sesión `Rol` para validar si hay sesión activa.

Variables de sesión relevantes:

```text
Rol
Usuario
```

---

## Flujo básico de uso

1. El usuario abre la UI.
2. Inicia sesión desde `/Home/Index`.
3. La UI envía credenciales a la API.
4. La API valida usuario, estado, contraseña y rol.
5. La API genera token JWT.
6. La UI activa la sesión MVC mediante `/activateSesion`.
7. La UI guarda datos necesarios en sesión/localStorage.
8. El sistema redirige al panel correspondiente mediante `/setProfile`.
9. La UI renderiza el menú según rol y permisos.
10. El usuario accede a módulos autorizados.
11. La UI consume endpoints de la API usando AJAX.
12. El usuario cierra sesión mediante `/deactivateSesion` y se limpia la sesión/localStorage.

---

## Consideraciones de seguridad

Este proyecto funciona como base académica o prototipo funcional. Antes de usarlo en producción, se recomienda corregir los siguientes puntos:

1. **Contraseñas**: actualmente se codifican con Base64 Unicode, no se cifran ni se hashean de forma segura. Se recomienda usar `PasswordHasher<TUser>`, BCrypt o Argon2.
2. **JWT secret**: debe almacenarse en variables de entorno, User Secrets o un gestor de secretos, no directamente en `appsettings.json` para producción.
3. **Clave JWT débil en ambiente local**: el archivo real contiene una clave de ejemplo corta. Para producción se requiere una clave robusta de al menos 32 caracteres.
4. **CORS**: la API y la UI permiten cualquier origen, método y encabezado. En producción se deben restringir los orígenes permitidos.
5. **Tokens en localStorage**: la UI guarda información de autenticación en `localStorage`. Para mayor seguridad, evaluar cookies HTTP-only y SameSite.
6. **Validación de entrada**: varios endpoints reciben `Object` o `dynamic`. Se recomienda usar DTOs tipados y validaciones con DataAnnotations o FluentValidation.
7. **Manejo de errores**: algunos endpoints devuelven mensajes técnicos de excepción. En producción se deben usar mensajes controlados y logging interno.
8. **Endpoints de cifrado/descifrado**: `ValuesController` expone utilidades criptográficas por HTTP. Se recomienda restringir, eliminar o documentar su uso interno.
9. **Archivos `.csproj.user` y `.slnLaunch.user`**: son archivos locales de Visual Studio. Se recomienda excluirlos del repositorio.
10. **Archivo `.bak`**: evitar subir respaldos con datos sensibles a repositorios públicos.
11. **appsettings con credenciales reales**: no subir cadenas de conexión con usuario y contraseña reales.
12. **Puerto serial**: validar manejo de errores cuando el puerto configurado no existe o está ocupado.

---

## Recomendaciones de mejora técnica

- Crear archivos `appsettings.Development.json` y `appsettings.Production.json`.
- Mover secretos a User Secrets, variables de entorno o un gestor de secretos.
- Agregar migraciones de EF Core o documentar formalmente el uso de scripts SQL.
- Separar DTOs de entidades de base de datos en todos los endpoints.
- Implementar repositorios o servicios de dominio con responsabilidades más claras.
- Fortalecer pruebas unitarias para servicios críticos.
- Agregar pruebas de integración para autenticación, préstamos, devoluciones y multas.
- Mantener o regenerar reportes de cobertura desde `SysBiblioteca.API.Tests`.
- Normalizar nombres de rutas y métodos en español o inglés, pero no mezclarlos.
- Corregir nombres como `GenerateInvoce`, `GetUnpaidInvoce`, `ApproveInvoce` a `GenerateInvoice`, `GetUnpaidInvoice`, `ApproveInvoice`.
- Corregir carpeta `RDIF_Module` a `RFID_Module`.
- Evaluar si `ValuesController` debe eliminarse, protegerse más o moverse bajo la ruta estándar `SysBiblioteca/API/Values`.
- Revisar inconsistencia entre `Ubicaciones.cs` y la tabla `Ubicaciones` del script SQL.
- Verificar existencia de vista Razor para `/Seguridad/Permisos`.
- Documentar el flujo completo de préstamo y devolución con diagramas.
- Agregar colección Postman o archivo `.http` para probar la API.
- Reducir assets frontend no utilizados si provienen de una plantilla base.

---

## Estado del análisis

Este README fue actualizado a partir de la revisión del proyecto comprimido, incluyendo API, UI, modelos, scripts SQL, configuración, dependencias, servicios, rutas auxiliares, proyecto de pruebas y reporte de cobertura incluido en el repositorio.

No se ejecutó una compilación completa ni pruebas dentro del entorno de análisis. El contenido se basa en inspección estática del código fuente y archivos de configuración.

---

## Licencia

No se encontró un archivo de licencia en el proyecto. Se recomienda agregar uno antes de publicar el repositorio.

---

## Autoría

Proyecto académico desarrollado como sistema web de gestión bibliotecaria.
