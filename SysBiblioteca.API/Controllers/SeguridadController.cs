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
    }
}