using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.CTL;
using SysBiblioteca.API.Services.ADM.RolesService;
using SysBiblioteca.API.Services.ADM.CargosService;
using SysBiblioteca.API.Services.CTL.GenerosService;
using SysBiblioteca.API.Services.CTL.EstadosService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class CatalogosController : Controller
    {
        private readonly iRolesService iRoles;
        private readonly iCargosService iCargos;
        private readonly iEstadosService iEstados;
        private readonly iGenerosService iGeneros;
        private readonly iUsuariosService iUsuarios;
        private readonly iLinkRolMenuService iLinkRolMenuService;

        public CatalogosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iGenerosService generosService, iEstadosService estadosService,
            iRolesService rolesService, iCargosService cargosService)
        {
            iRoles = rolesService;
            iCargos = cargosService;
            iEstados = estadosService; 
            iGeneros = generosService;
            iUsuarios = usuariosService;
            iLinkRolMenuService = linkRolMenuService;
        }

        [HttpPost]
        [Route("GetGeneros")]
        // SysBiblioteca/API/Catalogos/GetGeneros
        // Método que obtiene el listado de Generos
        public IActionResult GetGeneros([FromBody] Generos _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Generos> generos = iGeneros.Read();
                            if (generos.Count() > 0)
                            {
                                _rp.Datos = generos;
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
        [Route("GetEstados")]
        // SysBiblioteca/API/Catalogos/GetEstados
        // Método que obtiene el listado de Estados
        public IActionResult GetEstados([FromBody] Generos _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Estados> estados = iEstados.Read();
                            if (estados.Count() > 0)
                            {
                                _rp.Datos = estados;
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
        [Route("GetRoles")]
        // SysBiblioteca/API/Catalogos/GetRoles
        // Método que obtiene el listado de Roles
        public IActionResult GetRoles([FromBody] Generos _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Roles> roles = iRoles.Read();
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
        [Route("GetCargos")]
        // SysBiblioteca/API/Catalogos/GetCargos
        // Método que obtiene el listado de Cargos
        public IActionResult GetCargos([FromBody] Generos _generos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_generos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_generos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _generos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Cargos> cargos = iCargos.Read();
                            if (cargos.Count() > 0)
                            {
                                _rp.Datos = cargos;
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