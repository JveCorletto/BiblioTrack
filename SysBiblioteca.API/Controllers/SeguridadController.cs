using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class SeguridadController : Controller
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLinkRolMenuService iLinkRolMenuService;

        public SeguridadController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService)
        {
            iUsuarios = usuariosService;
            iLinkRolMenuService = linkRolMenuService;
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
    }
}