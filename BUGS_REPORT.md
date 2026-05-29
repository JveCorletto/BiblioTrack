# Reporte de Bugs — BiblioTrack

> Generado el 2026-05-28 · Revisión: rama `testing`  
> **13 bugs corregidos** · **101/101 tests pasan** ✅  
> 1 debilidad de seguridad documentada (requiere migración de datos)

---

## Índice

1. [Resumen ejecutivo](#resumen-ejecutivo)
2. [Bugs corregidos](#bugs-corregidos)
   - [BUG-01 — `Delete` inserta en lugar de eliminar (AutoresService)](#bug-01)
   - [BUG-02 — Filtro de fechas invertido (PrestamosService)](#bug-02)
   - [BUG-03 — NullReferenceException en LogIn con usuario inexistente](#bug-03)
   - [BUG-04 — NullReferenceException en UpdateEmpleado](#bug-04)
   - [BUG-05 — NullReferenceException en UpdateUsuario](#bug-05)
   - [BUG-06 — NullReferenceException en changePassword](#bug-06)
   - [BUG-07 — NullReferenceException en LoanBook](#bug-07)
   - [BUG-08 — NullReferenceException en MarkAsFinished](#bug-08)
   - [BUG-09 — Operador `||` incorrecto bloquea a todos los roles en ProcesarPrestamo](#bug-09)
   - [BUG-10 — Vista `Permisos.cshtml` faltante causa error 500 al navegar a Seguridad/Permisos](#bug-10)
   - [BUG-11 — Vista `RFID.cshtml` faltante causa error 500 al navegar a Inventario/RFID](#bug-11)
   - [BUG-12 — Etiquetas `<head>` y `<body>` anidadas en `SysBiblioteca/Index.cshtml` rompen el DOM e impiden que el menú cargue](#bug-12)
   - [BUG-13 — Acción `SysBiblioteca/Inicio` inexistente causa 404 al cargar el dashboard desde el menú](#bug-13)
3. [Debilidad de seguridad documentada (sin corrección automática)](#debilidad-de-seguridad)

---

## Resumen ejecutivo

| ID | Severidad | Archivo | Línea | Estado |
|----|-----------|---------|-------|--------|
| BUG-01 | 🔴 Crítico | `Services/INV/AutoresService/AutoresService.cs` | 22–26 | ✅ Corregido |
| BUG-02 | 🔴 Crítico | `Services/PRS/PrestamosService/PrestamosService.cs` | 137 | ✅ Corregido |
| BUG-03 | 🟠 Alto | `Services/ADM/UsuariosService/UsuariosService.cs` | 99 | ✅ Corregido |
| BUG-04 | 🟠 Alto | `Services/ADM/UsuariosService/UsuariosService.cs` | 150 | ✅ Corregido |
| BUG-05 | 🟠 Alto | `Services/ADM/UsuariosService/UsuariosService.cs` | 192 | ✅ Corregido |
| BUG-06 | 🟠 Alto | `Services/ADM/UsuariosService/UsuariosService.cs` | 207 | ✅ Corregido |
| BUG-07 | 🟠 Alto | `Services/PRS/PrestamosService/PrestamosService.cs` | 100 | ✅ Corregido |
| BUG-08 | 🟠 Alto | `Services/PRS/PrestamosService/PrestamosService.cs` | 109 | ✅ Corregido |
| BUG-09 | 🔴 Crítico | `Controllers/PrestamosController.cs` | 143 | ✅ Corregido |
| BUG-10 | 🔴 Crítico | `SysBiblioteca.UI/Views/Seguridad/` | — | ✅ Corregido |
| BUG-11 | 🔴 Crítico | `SysBiblioteca.UI/Views/Inventario/` | — | ✅ Corregido |
| BUG-12 | 🔴 Crítico | `SysBiblioteca.UI/Views/SysBiblioteca/Index.cshtml` | 5–14 | ✅ Corregido |
| BUG-13 | 🔴 Crítico | `SysBiblioteca.UI/Controllers/PersonalInterno/SysBibliotecaController.cs` | — | ✅ Corregido |
| SEC-01 | 🟡 Medio | `Management/crypto.cs` | 5–11 | 📋 Documentado |

---

## Bugs corregidos

---

### BUG-01

**Severidad:** 🔴 Crítico — Corrupción de datos  
**Archivo:** [`Services/INV/AutoresService/AutoresService.cs`](SysBiblioteca.API/Services/INV/AutoresService/AutoresService.cs)  
**Línea:** 22–26

#### Descripción

El método `Delete` llamaba a `context.Autores.Add(entity)` en lugar de `context.Autores.Remove(entity)`. Esto significa que al intentar eliminar un autor, en realidad se **insertaba un duplicado** en la base de datos.

#### Código antes

```csharp
public void Delete(Autores entity)
{
    context.Autores.Add(entity);   // ← BUG: debería ser Remove
    context.SaveChanges();
}
```

#### Código después

```csharp
public void Delete(Autores entity)
{
    context.Autores.Remove(entity);
    context.SaveChanges();
}
```

#### Escenario de fallo

Cualquier llamada a "Eliminar autor" desde la interfaz creaba un registro duplicado en la tabla `Autores` en lugar de borrar el original. Efecto acumulativo: la base de datos se llenaba de autores repetidos con cada operación de eliminación.

---

### BUG-02

**Severidad:** 🔴 Crítico — Resultados incorrectos  
**Archivo:** [`Services/PRS/PrestamosService/PrestamosService.cs`](SysBiblioteca.API/Services/PRS/PrestamosService/PrestamosService.cs)  
**Línea:** 137

#### Descripción

El filtro de préstamos finalizados por rango de fechas usaba `>=` en ambas condiciones. La condición de fecha final debería ser `<=` para filtrar préstamos **dentro** del rango, no después del límite superior.

#### Código antes

```csharp
.Where(p => p.Finalizado == true && p.Entregado == true
     && Convert.ToDateTime(p.FechaDevolucion.Value.ToShortDateString()) >= Convert.ToDateTime(FechaDesde.Value.ToShortDateString())
     && Convert.ToDateTime(p.FechaDevolucion.Value.ToShortDateString()) >= Convert.ToDateTime(FechaHasta.Value.ToShortDateString()))  // ← BUG
```

#### Código después

```csharp
.Where(p => p.Finalizado == true && p.Entregado == true
     && Convert.ToDateTime(p.FechaDevolucion.Value.ToShortDateString()) >= Convert.ToDateTime(FechaDesde.Value.ToShortDateString())
     && Convert.ToDateTime(p.FechaDevolucion.Value.ToShortDateString()) <= Convert.ToDateTime(FechaHasta.Value.ToShortDateString()))
```

#### Escenario de fallo

Búsqueda de préstamos devueltos entre `01/01/2025` y `31/01/2025`. Un préstamo con `FechaDevolucion = 15/01/2025` fallaba la segunda condición (`15/01 >= 31/01` → false) y quedaba **excluido del resultado** aunque estaba dentro del rango pedido. Solo aparecían préstamos con fecha posterior al límite superior.

---

### BUG-03

**Severidad:** 🟠 Alto — Crash en runtime  
**Archivo:** [`Services/ADM/UsuariosService/UsuariosService.cs`](SysBiblioteca.API/Services/ADM/UsuariosService/UsuariosService.cs)  
**Línea:** 99

#### Descripción

En el flujo de `LogIn` fallido, se consultaba el usuario activo (`IdEstado == 1`) para incrementar el contador de intentos. Si el usuario no existía o usaba un nombre de usuario incorrecto, `FirstOrDefault` retornaba `null` y la siguiente línea `_counter.ConteoIntentos == 2` lanzaba `NullReferenceException`.

#### Código antes

```csharp
Usuarios _counter = context.Usuarios
    .FirstOrDefault(e => e.Usuario == usuario && e.IdEstado == 1);

// Sin null check → crash si el usuario no existe
if (_counter.ConteoIntentos == 2)
{
    ...
}
```

#### Código después

```csharp
Usuarios _counter = context.Usuarios
    .FirstOrDefault(e => e.Usuario == usuario && e.IdEstado == 1);

if (_counter == null)
    return null;

if (_counter.ConteoIntentos == 2)
{
    ...
}
```

#### Escenario de fallo

Cualquier intento de login con un nombre de usuario que no existe en la base de datos causaba una excepción no controlada (`NullReferenceException`) en lugar de retornar una respuesta de error amigable. Afectaba también a usuarios con estado bloqueado (`IdEstado == 2`) ya que la query los excluye.

---

### BUG-04

**Severidad:** 🟠 Alto — Crash en runtime  
**Archivo:** [`Services/ADM/UsuariosService/UsuariosService.cs`](SysBiblioteca.API/Services/ADM/UsuariosService/UsuariosService.cs)  
**Línea:** 150

#### Descripción

En `UpdateEmpleado`, se usaba `FirstOrDefault` para obtener los datos personales del empleado y luego se accedía directamente a sus propiedades sin verificar si el resultado era `null`.

#### Código antes

```csharp
DatosPersonales oldPersonalData = context.DatosPersonales
    .FirstOrDefault(d => d.IdDatosPersonales == oldData.IdDatosPersonales);

oldPersonalData.IdGenero = newData.DatosPersonales.IdGenero;  // ← NullReferenceException si no existe
```

#### Código después

```csharp
DatosPersonales oldPersonalData = context.DatosPersonales
    .FirstOrDefault(d => d.IdDatosPersonales == oldData.IdDatosPersonales);

if (oldPersonalData != null)
{
    oldPersonalData.IdGenero = newData.DatosPersonales.IdGenero;
    // ...
}
```

#### Escenario de fallo

Si el registro de `DatosPersonales` asociado al empleado no existía en la base de datos (por datos inconsistentes o eliminación parcial), la actualización causaba `NullReferenceException` sin guardar ningún cambio.

---

### BUG-05

**Severidad:** 🟠 Alto — Crash en runtime  
**Archivo:** [`Services/ADM/UsuariosService/UsuariosService.cs`](SysBiblioteca.API/Services/ADM/UsuariosService/UsuariosService.cs)  
**Línea:** 192 (antes de corrección)

#### Descripción

Mismo patrón que BUG-04 pero en el método `UpdateUsuario`. La misma falta de null-check aplicaba a la actualización de datos personales de usuarios lectores.

#### Código antes

```csharp
DatosPersonales oldPersonalData = context.DatosPersonales
    .FirstOrDefault(d => d.IdDatosPersonales == oldData.IdDatosPersonales);
oldPersonalData.IdGenero = ...;  // ← NullReferenceException si no existe
```

#### Código después

```csharp
DatosPersonales oldPersonalData = context.DatosPersonales
    .FirstOrDefault(d => d.IdDatosPersonales == oldData.IdDatosPersonales);

if (oldPersonalData != null)
{
    oldPersonalData.IdGenero = ...;
    // ...
}
```

#### Escenario de fallo

Idéntico a BUG-04 pero afectando a usuarios lectores en lugar de empleados.

---

### BUG-06

**Severidad:** 🟠 Alto — Crash en runtime  
**Archivo:** [`Services/ADM/UsuariosService/UsuariosService.cs`](SysBiblioteca.API/Services/ADM/UsuariosService/UsuariosService.cs)  
**Línea:** 207

#### Descripción

En `changePassword`, si el `IdUsuario` no correspondía a ningún registro, `FirstOrDefault` retornaba `null` y la asignación `user.Contrasenia = newPassword` lanzaba `NullReferenceException`.

#### Código antes

```csharp
Usuarios user = context.Usuarios.FirstOrDefault(u => u.IdUsuario == IdUsuario);
user.Contrasenia = newPassword;  // ← NullReferenceException si user es null
```

#### Código después

```csharp
Usuarios user = context.Usuarios.FirstOrDefault(u => u.IdUsuario == IdUsuario);
if (user == null) return;
user.Contrasenia = newPassword;
```

#### Escenario de fallo

Si se invocaba el cambio de contraseña con un ID de usuario inválido o eliminado, el endpoint lanzaba una excepción en lugar de retornar un error controlado.

---

### BUG-07

**Severidad:** 🟠 Alto — Crash en runtime  
**Archivo:** [`Services/PRS/PrestamosService/PrestamosService.cs`](SysBiblioteca.API/Services/PRS/PrestamosService/PrestamosService.cs)  
**Línea:** 100

#### Descripción

En `LoanBook`, si el `IdPrestamo` no existía en la base de datos, `FirstOrDefault` retornaba `null` y la siguiente línea `prestamo.Entregado = true` causaba `NullReferenceException`.

#### Código antes

```csharp
Prestamos prestamo = context.Prestamos.FirstOrDefault(p => p.IdPrestamo == IdPrestamo);
prestamo.Entregado = true;  // ← NullReferenceException si no existe
```

#### Código después

```csharp
Prestamos prestamo = context.Prestamos.FirstOrDefault(p => p.IdPrestamo == IdPrestamo);
if (prestamo == null) return;
prestamo.Entregado = true;
```

#### Escenario de fallo

Si el RFID scanner o la interfaz enviaba un `IdPrestamo` inválido (por error de lectura del código QR o por un préstamo ya eliminado), el endpoint de entrega de libro crasheaba.

---

### BUG-08

**Severidad:** 🟠 Alto — Crash en runtime  
**Archivo:** [`Services/PRS/PrestamosService/PrestamosService.cs`](SysBiblioteca.API/Services/PRS/PrestamosService/PrestamosService.cs)  
**Línea:** 109

#### Descripción

Mismo patrón que BUG-07 en el método `MarkAsFinished`. Si el préstamo no existía, acceder a sus propiedades causaba `NullReferenceException`.

#### Código antes

```csharp
Prestamos prestamo = context.Prestamos.FirstOrDefault(p => p.IdPrestamo == IdPrestamo);

prestamo.Finalizado = true;  // ← NullReferenceException si no existe
```

#### Código después

```csharp
Prestamos prestamo = context.Prestamos.FirstOrDefault(p => p.IdPrestamo == IdPrestamo);
if (prestamo == null) return;
prestamo.Finalizado = true;
```

#### Escenario de fallo

Durante la devolución de un libro, si el `IdPrestamo` era inválido, el proceso de finalización crasheaba dejando el préstamo en estado inconsistente (ni finalizado ni marcado con error).

---

### BUG-09

**Severidad:** 🔴 Crítico — Lógica de autorización siempre bloquea  
**Archivo:** [`Controllers/PrestamosController.cs`](SysBiblioteca.API/Controllers/PrestamosController.cs)  
**Línea:** 143

#### Descripción

La condición de chequeo de rol usaba el operador `||` en lugar de `&&`. La expresión `(rol != "Administrador" || rol != "Empleado")` es **lógicamente siempre verdadera** para cualquier valor de rol, porque ningún string puede ser simultáneamente igual a dos valores distintos.

Esto significaba que cualquier petición que no viniera del EventHandler del RFID era bloqueada, incluidos los Administradores y Empleados.

#### Código antes

```csharp
if (_prestamo.ActualRute != "/PrestamosDevoluciones/EventHandler"
    && (user.Rol?.Rol != "Administrador" || user.Rol?.Rol != "Empleado"))  // ← ||: siempre true
{
    _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
    return Ok(_rp);
}
```

#### Código después

```csharp
if (_prestamo.ActualRute != "/PrestamosDevoluciones/EventHandler"
    && (user.Rol?.Rol != "Administrador" && user.Rol?.Rol != "Empleado"))  // && correcto
{
    _rp.Mensaje = "El usuario no tiene permisos de escritura en esta pantalla.";
    return Ok(_rp);
}
```

#### Tabla de verdad

| Rol | Antes (`||`) | Después (`&&`) |
|-----|-------------|----------------|
| `"Administrador"` | ❌ Bloqueado | ✅ Permitido |
| `"Empleado"` | ❌ Bloqueado | ✅ Permitido |
| `"Lector"` | ❌ Bloqueado | ✅ Bloqueado (correcto) |

#### Escenario de fallo

Cualquier empleado o administrador que intentara procesar un préstamo manualmente desde la interfaz web recibía el error "no tiene permisos", aunque tuviera el rol correcto. Solo funcionaba el flujo automático del RFID.

---

---

### BUG-10

**Severidad:** 🔴 Crítico — Página inaccesible (error 500)  
**Proyecto:** `SysBiblioteca.UI`  
**Archivo faltante:** [`Views/Seguridad/Permisos.cshtml`](SysBiblioteca.UI/Views/Seguridad/Permisos.cshtml)  
**Controller:** [`Controllers/PersonalInterno/SeguridadController.cs:17`](SysBiblioteca.UI/Controllers/PersonalInterno/SeguridadController.cs)

#### Descripción

El método de acción `Permisos()` existía en el controlador MVC del proyecto UI y retornaba `return View()` correctamente, pero **no existía el archivo de vista correspondiente** `Views/Seguridad/Permisos.cshtml`. Cuando ASP.NET Core MVC no encuentra la vista solicitada, lanza una excepción `InvalidOperationException` que resulta en un **error HTTP 500** para el usuario.

#### Causa raíz

La acción fue declarada en el controlador durante el desarrollo pero la vista nunca fue creada, posiblemente porque se dejó pendiente o se olvidó después de definir la ruta.

```csharp
// SeguridadController.cs — el método existe pero la vista no
public IActionResult Permisos()
{
    return View();   // ← ASP.NET buscaba Permisos.cshtml y no la encontraba → 500
}
```

ASP.NET Core MVC busca la vista en estas ubicaciones (en orden):
1. `Views/Seguridad/Permisos.cshtml`  ← **no existía**
2. `Views/Shared/Permisos.cshtml`     ← **no existía**

#### Archivos JS que ya existían (la lógica estaba lista)

Los scripts de la página **sí estaban implementados** en `wwwroot/js/Permisos/`:

| Archivo | Función |
|---------|---------|
| `rls.js` | Inicialización, validación de sesión, reset de formulario de Roles |
| `rls.logs.js` | Carga de tablas de roles activos/inactivos desde la API |
| `rls.actions.js` | Crear, editar, activar, desactivar roles; abrir modal de permisos |
| `asg.js` | Gestión de checkboxes CRUD, mostrar/ocultar panel de permisos |
| `asg.logs.js` | Navegación del árbol de menús (Raíz → Padre → Hijo), carga de selects |
| `asg.actions.js` | Asignar, editar y desasignar menús a roles via API |

#### Solución

Se creó el archivo [`Views/Seguridad/Permisos.cshtml`](SysBiblioteca.UI/Views/Seguridad/Permisos.cshtml) con la estructura HTML completa que los scripts ya existentes esperaban.

La vista incluye:

**1. Tabla de Roles (tabs Activos / Inactivos)**
- `#tableActivos` / `#tActivos` — tabla de roles activos con botones Editar, Asignar Permisos y Desactivar
- `#tableInactivos` / `#tInactivos` — tabla de roles inactivos con botón Activar

**2. Modal `#staticNewRol` — Crear / Editar Rol**

Contiene todos los elementos que `rls.js` y `rls.actions.js` manipulan:

| ID HTML | Tipo | Uso |
|---------|------|-----|
| `#tituloModal` | `<h5>` | Título dinámico del modal |
| `#dataRol` | `<form>` | Formulario de datos del rol |
| `#IdRol` | `<input hidden>` | ID del rol al editar |
| `#Rol` | `<input text>` | Nombre del rol |
| `#editionMode` / `#btnEdition` | `<div>` / `<button>` | Panel que aparece en modo lectura |
| `#btnSave` | `<button>` | Guardar nuevo rol |
| `#btnEdit` | `<button>` | Confirmar edición |
| `#btnCancel` | `<button>` | Cerrar modal |

**3. Modal `#staticPermisions` — Asignación de Permisos a un Rol**

El modal más complejo, con todos los elementos que `asg.js`, `asg.logs.js` y `asg.actions.js` necesitan:

| ID HTML | Tipo | Uso |
|---------|------|-----|
| `#IdRolPermiso` | `<input hidden>` | ID del rol al que se asignan permisos |
| `#NombreRol` | `<input readonly>` | Nombre del rol (informativo) |
| `#IdPlataforma` | `<select>` | Selector de aplicación; al cambiar llama `validateParms()` |
| `#formPermisos` | `<div hidden>` | Panel completo de asignación, se muestra al elegir aplicación |
| `#rootMenu` | `<div>` | Lista dinámica del árbol de menús asignados al rol |
| `#navegacion` | `<ol>` | Breadcrumb de navegación dentro del árbol |
| `#MenuRaiz` | `<select>` | Selecciona menú raíz; al cambiar llama `GetMenus()` |
| `#Menu` | `<select>` | Selecciona menú padre; al cambiar llama `GetSons()` |
| `#MenuHijo` | `<select>` | Selecciona sub-menú (hoja) |
| `#Create` / `#lbCreate` | `<input checkbox>` / `<small>` | Permiso Crear + label Sí/No |
| `#Read` / `#lbRead` | `<input checkbox>` / `<small>` | Permiso Leer + label Sí/No |
| `#Update` / `#lbUpdate` | `<input checkbox>` / `<small>` | Permiso Editar + label Sí/No |
| `#Delete` / `#lbDelete` | `<input checkbox>` / `<small>` | Permiso Eliminar + label Sí/No |
| `#dataMenu` | `<div hidden>` | Sección que aparece al editar un menú ya asignado |
| `#nameMenu` | `<input readonly>` | Nombre del menú seleccionado para editar |
| `#IdLinkRolMenuPais` | `<input hidden>` | ID del registro `Link_Rol_Menu` a modificar |
| `#btnSaveAssign` | `<button>` | Guardar nueva asignación |
| `#btnEditAssign` | `<button hidden>` | Actualizar asignación existente |
| `#btnCancelAssign` | `<button hidden>` | Limpiar formulario de asignación |
| `#btnTabs` | `<div hidden>` | Navegación de nivel del árbol |
| `#btnCancelRol` | `<button>` | Cerrar modal de permisos |

#### Scripts incluidos en la vista (en orden)

```html
<script src="~/js/Permisos/rls.js"></script>
<script src="~/js/Permisos/rls.logs.js"></script>
<script src="~/js/Permisos/rls.actions.js"></script>
<script src="~/js/Permisos/asg.js"></script>
<script src="~/js/Permisos/asg.logs.js"></script>
<script src="~/js/Permisos/asg.actions.js"></script>
```

#### Verificación

El proyecto UI compiló sin errores tras crear la vista. Los 101 tests unitarios del proyecto API siguen pasando sin cambios.

---

### BUG-11

**Severidad:** 🔴 Crítico — HTTP 500 / página no carga  
**Archivo creado:** [`SysBiblioteca.UI/Views/Inventario/RFID.cshtml`](SysBiblioteca.UI/Views/Inventario/RFID.cshtml)  
**Controlador:** [`SysBiblioteca.UI/Controllers/PersonalInterno/Inventario.cs`](SysBiblioteca.UI/Controllers/PersonalInterno/Inventario.cs) — línea 22

#### Descripción

El método `RFID()` del controlador `Inventario` ejecuta `return View()`, lo que instruye a ASP.NET Core MVC a buscar la vista en:

1. `Views/Inventario/RFID.cshtml`  ← **no existía**
2. `Views/Shared/RFID.cshtml`     ← **no existía**

Al no encontrar ninguna, el framework lanzaba una excepción `InvalidOperationException` que se traducía en **HTTP 500** para el usuario. El enlace de navegación "Inventario → RFID" era completamente inaccesible.

#### Arquitectura RFID del sistema

El sistema implementa una arquitectura de dos capas para RFID:

| Componente | Archivo | Función |
|------------|---------|---------|
| Listener SignalR | `wwwroot/js/rfid_listener.js` | Se conecta al hub `/rfidHub` y abre un popup al recibir un tag |
| Popup de procesamiento | `Views/PrestamosDevoluciones/EventHandler.cshtml` | Ventana emergente (1040×600) que gestiona el préstamo/devolución |
| **Monitor del personal** | **`Views/Inventario/RFID.cshtml`** ← **se crea aquí** | Página siempre abierta que muestra estado y actividad del lector |

`rfid_listener.js` incluye la conexión SignalR como `const` local (no global), por lo que la vista no puede reutilizar esa instancia. En su lugar, la vista crea su propia conexión al hub.

#### Solución

Se creó `Views/Inventario/RFID.cshtml` con las siguientes características:

- **Layout:** `_emptyLayout.cshtml` (consistente con el resto de páginas operativas)
- **Conexión SignalR propia:** la vista crea su propio `HubConnection` con `.withAutomaticReconnect()` para que el monitor no quede desconectado silenciosamente
- **Indicador de estado:** dos estados visuales (`label-success` / `label-danger`) que cambian con los eventos `onreconnected` / `onreconnecting` / `onclose`
- **Registro de actividad:** log con scroll que muestra timestamp + código de tag de cada lectura; botón "Limpiar"
- **Última lectura:** tarjeta que muestra el tag más reciente y su hora
- **Apertura del popup:** al recibir un tag, la vista abre la misma ventana emergente `EventHandler` (mismo comportamiento que `rfid_listener.js`)

#### Scripts en la vista

La vista no incluye `rfid_listener.js` para evitar abrir dos popups por cada tag leído (uno del listener y otro de la vista). Toda la lógica SignalR se implementa inline en el bloque `<script>` de la vista.

#### Verificación

Los 101 tests unitarios del proyecto API siguen pasando sin cambios.

---

### BUG-12

**Severidad:** 🔴 Crítico — Dashboard no carga (menú, usuario y scripts del layout no se ejecutan)  
**Archivo:** [`SysBiblioteca.UI/Views/SysBiblioteca/Index.cshtml`](SysBiblioteca.UI/Views/SysBiblioteca/Index.cshtml)  
**Líneas:** 5–14 (antes de la corrección)

#### Descripción

La vista `SysBiblioteca/Index.cshtml` (la página principal tras el login) incluía etiquetas `<head>` y `<body>` propias dentro del contenido de la vista:

```razor
@{
    ViewData["Title"] = "Inicio";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
<head>                           ← segundo <head> dentro del <body> del layout
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Página con Fondo</title>
</head>
<body>                           ← segundo <body> (inválido)
    <div id="inicio">
    </div>
</body>                          ← cierra el body del layout prematuramente
```

Al renderizarse dentro de `_Layout.cshtml`, el HTML final tenía esta estructura inválida:

```html
<body style="background-color: #BFD6E5;">   ← body del layout
    <!-- sidebar, header, contenido del layout... -->
    <div id="mainContainer">
        <head>...</head>   ← INVÁLIDO: <head> dentro de <body>
        <body>             ← INVÁLIDO: <body> anidado; navegadores lo ignoran
            <div id="inicio"></div>
        </body>            ← el parser puede interpretar esto como cierre del body del layout
    </div>
    <!-- scripts: src.js, rfid_listener.js, etc. -->
</body>
```

Según el algoritmo de parsing de HTML5, un `</body>` encontrado dentro del body activo cierra el body. Los scripts que vienen después (`src.js` con `setMenu()` y `setUserInfo()`, `rfid_listener.js`) quedan fuera del documento activo y **no se ejecutan**. Resultado: el menú lateral nunca se renderiza, el nombre de usuario no aparece, y el dashboard queda completamente en blanco.

#### Código antes

```razor
@{
    ViewData["Title"] = "Inicio";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Página con Fondo</title>
</head>
<body>
    <div id="inicio">
    </div>
</body>

<style>
    body {
        background-image: url('media/imgs/inicio4.jpg');
        ...
    }
</style>
```

#### Código después

```razor
@{
    ViewData["Title"] = "Inicio";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<style>
    #inicio {
        background-image: url('/media/imgs/inicio4.jpg');
        background-size: cover;
        background-position: center;
        min-height: calc(100vh - 130px);
    }
</style>

<div id="inicio"></div>
```

#### Cambios aplicados

1. Se eliminaron las etiquetas inválidas `<head>`, `</head>`, `<body>`, `</body>` del contenido de la vista.
2. El CSS que estaba en `body { background-image... }` se movió a un selector `#inicio { }` para aplicar solo al `div`, no al body completo.
3. La ruta de la imagen se corrigió de ruta relativa (`'media/imgs/...'`) a ruta absoluta (`'/media/imgs/...'`) para que funcione independientemente del path actual.

#### Verificación

Los 101 tests unitarios del proyecto API siguen pasando sin cambios.

---

## Debilidad de seguridad

### SEC-01

**Severidad:** 🟡 Medio — Seguridad de contraseñas  
**Archivo:** [`Management/crypto.cs`](SysBiblioteca.API/Management/crypto.cs)  
**Línea:** 5–11  
**Estado:** 📋 Documentado — requiere migración de datos para corregir

#### Descripción

La clase `crypto` usa codificación Base64 (Unicode → Base64) como mecanismo de "cifrado" para las contraseñas. Base64 **no es cifrado**: es una codificación reversible sin clave. Cualquier persona con acceso a la base de datos puede decodificar todas las contraseñas instantáneamente.

```csharp
public static string Encrypt(string _cadenaAencriptar)
{
    byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
    result = Convert.ToBase64String(encryted);  // ← Solo es codificación, no cifrado
    return result;
}
```

#### Impacto

- Un atacante con acceso a la BD obtiene todas las contraseñas en texto plano.
- No se usa salt ni hash: dos usuarios con la misma contraseña tienen el mismo valor almacenado.

#### Recomendación de corrección

Reemplazar por **BCrypt** o **PBKDF2** (disponibles en el ecosistema .NET):

```csharp
// Ejemplo con BCrypt.Net-Next
public static string Encrypt(string password)
    => BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);

public static bool Verify(string password, string hash)
    => BCrypt.Net.BCrypt.Verify(password, hash);
```

> **Nota:** La corrección requiere una **migración de datos** para re-hashear las contraseñas existentes, ya que el hash con BCrypt no es reversible. Los usuarios deberían ser forzados a restablecer su contraseña o se debe implementar un proceso de migración progresiva en el primer login.

---

## Metodología de revisión

La revisión se realizó desde **3 ángulos independientes**:

| Ángulo | Foco | Hallazgos |
|--------|------|-----------|
| **A — Lógica de servicios** | Null derefs, condiciones invertidas, operaciones CRUD incorrectas | BUG-01, BUG-02, BUG-03, BUG-04, BUG-05, BUG-06, BUG-07, BUG-08 |
| **B — Seguridad en controladores** | Autorización, cifrado, precedencia de operadores | BUG-09, SEC-01 |
| **C — Integridad de datos transversal** | Flujos de préstamo/multa, condiciones de carrera | Confirmó BUG-02, BUG-03 |

Todos los bugs listados fueron **verificados en código** antes de aplicar la corrección. Los 101 tests unitarios existentes siguen pasando tras los cambios.
