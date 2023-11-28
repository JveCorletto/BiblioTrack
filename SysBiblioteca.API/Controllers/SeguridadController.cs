using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.NivelesService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class SeguridadController : Controller
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        private readonly iRolesService iRolesService;

        public SeguridadController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iRolesService iRolesService)
        {
            iUsuarios = usuariosService;
            iLinkRolMenuService = linkRolMenuService;
            this.iRolesService = iRolesService;
        }

        #region Usuarios

        [HttpPost]
        [Route("ActivateUser")]
        // SysBiblioteca/API/Seguridad/ActivateUser
        // Método que activa un usuario en el sistema
        public IActionResult ActivateUser([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Usuarios usuario = iUsuarios.getById(_usuarios.IdUsuario);
                            if (usuario != null)
                            {
                                iUsuarios.activateUser(usuario);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Usuario activado con éxito.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "El usuario seleccionado no pudo ser encontrado.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("DeactivateUser")]
        // SysBiblioteca/API/Seguridad/DeactivateUser
        // Método que activa un usuario en el sistema
        public IActionResult DeactivateUser([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Usuarios usuario = iUsuarios.getById(_usuarios.IdUsuario);
                            if (usuario != null)
                            {
                                iUsuarios.deactivateUser(usuario);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Usuario desactivado con éxito.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "El usuario seleccionado no pudo ser encontrado.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetUserById")]
        // SysBiblioteca/API/Seguridad/GetUserById
        // Método que activa un usuario en el sistema
        public IActionResult GetUserById([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Usuarios usuario = iUsuarios.getById(_usuarios.IdUsuario);
                            if (usuario != null)
                            {
                                _rp.Resultado = 1;
                                _rp.Datos = usuario;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "El usuario seleccionado no pudo ser encontrado.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        #endregion

        #region Empleados

        [HttpPost]
        [Route("CreateEmpleado")]
        // SysBiblioteca/API/Seguridad/CreateEmpleado
        // Método que Ingresa un nuevo empleado al sistema
        public IActionResult CreateEmpleado([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Usuarios validation = iUsuarios.getUserInfo(_usuarios.Usuario);
                            if (validation == null)
                            {
                                iUsuarios.Create(new Usuarios
                                {
                                    IdEstado = 1,
                                    IdRol = _usuarios.IdRol,
                                    IdCargo = _usuarios.IdCargo,
                                    Usuario = _usuarios.Usuario,
                                    Contrasenia = crypto.Encrypt("1234"),

                                    DatosPersonales = new DatosPersonales
                                    {
                                        IdGenero = _usuarios.DatosPersonales.IdGenero,
                                        Nombres = _usuarios.DatosPersonales.Nombres,
                                        Apellidos = _usuarios.DatosPersonales.Apellidos,
                                        DUI = _usuarios.DatosPersonales.DUI,
                                        Correo = _usuarios.DatosPersonales.Correo,
                                        Direccion = _usuarios.DatosPersonales.Direccion,
                                        Telefono = _usuarios.DatosPersonales.Telefono,
                                        FechaNacimiento = _usuarios.DatosPersonales.FechaNacimiento
                                    },

                                    UsuarioCreacion = user.Usuario,
                                    FechaCreacion = DateTime.Now
                                });

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Empleado creado con éxito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Ya existe un usuario con el mismo nombre de usuario.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetEmpleados")]
        // SysBiblioteca/API/Seguridad/GetEmpleados
        // Método que obtiene el listado de los empleados Activos
        public IActionResult GetEmpleados([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Usuarios> usuarios = iUsuarios.getEmpleados();
                            if (usuarios.Count() > 0)
                            {
                                _rp.Datos = usuarios;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetEmpleadosInactivos")]
        // SysBiblioteca/API/Seguridad/GetEmpleadosInactivos
        // Método que obtiene el listado de los empleados Activos
        public IActionResult GetEmpleadosInactivos([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Usuarios> usuarios = iUsuarios.getEmpleadosInactivos();
                            if (usuarios.Count() > 0)
                            {
                                _rp.Datos = usuarios;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetEmpleadoById")]
        // SysBiblioteca/API/Seguridad/GetEmpleadoById
        // Método que obtiene los datos del usuario seleccionado
        public IActionResult GetEmpleadoById([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Usuarios usuario = iUsuarios.getById(_usuarios.IdUsuario);
                            if (usuario != null)
                            {
                                _rp.Datos = usuario;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("UpdateEmpleado")]
        // SysBiblioteca/API/Seguridad/UpdateEmpleado
        // Método que modifica los datos del usuario
        public IActionResult UpdateEmpleado([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Usuarios oldData = iUsuarios.getById(_usuarios.IdUsuario);
                            if (oldData != null) 
                            {
                                iUsuarios.UpdateEmpleado(_usuarios, oldData);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Datos modificados con éxito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "El usuario seleccionado no pudo ser encontrado.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        #endregion

        #region UsuariosSistema

        [HttpPost]
        [Route("CreateUsuario")]
        // SysBiblioteca/API/Seguridad/CreateEmpleado
        // Método que Ingresa un nuevo empleado al sistema
        public IActionResult CreateUsuario([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Usuarios validation = iUsuarios.getUserInfo(_usuarios.Usuario);
                            if (validation == null)
                            {
                                iUsuarios.Create(new Usuarios
                                {
                                    IdEstado = 1,
                                    IdRol = _usuarios.IdRol,
                                    IdCargo = _usuarios.IdCargo,
                                    Usuario = _usuarios.Usuario,
                                    Contrasenia = crypto.Encrypt("1234"),

                                    DatosPersonales = new DatosPersonales
                                    {
                                        IdGenero = _usuarios.DatosPersonales.IdGenero,
                                        Nombres = _usuarios.DatosPersonales.Nombres,
                                        Apellidos = _usuarios.DatosPersonales.Apellidos,
                                        DUI = _usuarios.DatosPersonales.DUI,
                                        Correo = _usuarios.DatosPersonales.Correo,
                                        Direccion = _usuarios.DatosPersonales.Direccion,
                                        Telefono = _usuarios.DatosPersonales.Telefono,
                                        FechaNacimiento = _usuarios.DatosPersonales.FechaNacimiento
                                    },

                                    UsuarioCreacion = user.Usuario,
                                    FechaCreacion = DateTime.Now
                                });

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Empleado creado con éxito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Ya existe un usuario con el mismo nombre de usuario.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetUsuarios")]
        // SysBiblioteca/API/Seguridad/GetEmpleados
        // Método que obtiene el listado de los empleados Activos
        public IActionResult GetUsuarios([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Usuarios> usuarios = iUsuarios.getUsuarios();
                            if (usuarios.Count() > 0)
                            {
                                _rp.Datos = usuarios;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetUsuariosInactivos")]
        // SysBiblioteca/API/Seguridad/GetEmpleadosInactivos
        // Método que obtiene el listado de los empleados Activos
        public IActionResult GetUsuariosInactivos([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Usuarios> usuarios = iUsuarios.getUsuariosInactivos();
                            if (usuarios.Count() > 0)
                            {
                                _rp.Datos = usuarios;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetUsuarioById")]
        // SysBiblioteca/API/Seguridad/GetEmpleadoById
        // Método que obtiene los datos del usuario seleccionado
        public IActionResult GetUsuarioById([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Usuarios usuario = iUsuarios.getById(_usuarios.IdUsuario);
                            if (usuario != null)
                            {
                                _rp.Datos = usuario;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("UpdateUsuario")]
        // SysBiblioteca/API/Seguridad/UpdateEmpleado
        // Método que modifica los datos del usuario
        public IActionResult UpdateUsuario([FromBody] Usuarios _usuarios)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_usuarios.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_usuarios.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _usuarios.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Usuarios oldData = iUsuarios.getById(_usuarios.IdUsuario);
                            if (oldData != null)
                            {
                                iUsuarios.UpdateUsuario(_usuarios, oldData);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Datos modificados con éxito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "El usuario seleccionado no pudo ser encontrado.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        #endregion

        #region Roles


        [HttpPost]
        [Route("CreateRol")]
        // SysBiblioteca/API/Seguridad/CreateEmpleado
        // Método que Ingresa un nuevo empleado al sistema
        public IActionResult CreateRol([FromBody] Roles _Roles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_Roles.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_Roles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _Roles.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            iRolesService.Create(new Roles { Rol = _Roles.Rol, FechaCreacion = DateTime.Now });

                            _rp.Resultado = 1;
                            _rp.Mensaje = "Rol creado con Éxito";

                            return Ok(_rp);
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de escritura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetRoles")]
        // SysBiblioteca/API/Seguridad/GetEmpleados
        // Método que obtiene el listado de los empleados Activos
        public IActionResult GetRoles([FromBody] Roles _Roles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_Roles.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_Roles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _Roles.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Roles> roles = iRolesService.Read();
                                _rp.Datos = roles;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetRolesInactivos")]
        // SysBiblioteca/API/Seguridad/GetEmpleadosInactivos
        // Método que obtiene el listado de los empleados Activos
        public IActionResult GetRolesInactivos([FromBody] Roles _roles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_roles.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_roles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _roles.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Roles> roles = iRolesService.ReadInactivos();
                            if (roles.Count() > 0)
                            {
                                _rp.Datos = roles;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("GetRolById")]
        // SysBiblioteca/API/Seguridad/GetEmpleadoById
        // Método que obtiene los datos del usuario seleccionado
        public IActionResult GetRolById([FromBody] Roles _roles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_roles.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_roles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _roles.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Roles rol = iRolesService.getById(_roles.IdRol);
                            if (rol != null)
                            {
                                _rp.Datos = rol;
                                _rp.Resultado = 1;
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Sin datos que mostrar.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de lectura en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        [HttpPost]
        [Route("UpdateRol")]
        // SysBiblioteca/API/Seguridad/UpdateEmpleado
        // Método que modifica los datos del usuario
        public IActionResult UpdateRol([FromBody] Roles _roles)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_roles.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_roles.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _roles.ActualRute);
                        if (permisos != null && permisos.Update)
                        {
                            Roles rol = iRolesService.getById(_roles.IdRol);
                            if (rol != null)
                            {
                                iRolesService.Update(_roles);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Datos modificados con éxito";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "El usuario seleccionado no pudo ser encontrado.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edición en ésta pantalla.";
                            return Ok(_rp);
                        }
                    }
                    else
                    {
                        _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                        return Ok(_rp);
                    }
                }
                else
                {
                    _rp.Mensaje = "Usuario no autenticado, inicie sesión nuevamente.";
                    return Ok(_rp);
                }
            }
            catch (Exception ex)
            {
                _rp.Mensaje = ex.Message;
                return Ok(_rp);
            }
        }

        #endregion
    }
}