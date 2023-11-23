using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.PRS.MultasService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class MultasController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iMultasService iMultasService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        public MultasController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iMultasService multasService)
        {
            iUsuarios = usuariosService;
            iMultasService = multasService;
            iLinkRolMenuService = linkRolMenuService;

        }

        [HttpPost]
        [Route("GetMultasNoPagadas")]
        // SysBiblioteca/API/Multas/GetMultasNoPagadas
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetMultasNoPagadas([FromBody] Multas _multas)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_multas.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_multas.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _multas.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Multas> multas = iMultasService.getUnPaid();
                            if (multas.Count() > 0)
                            {
                                _rp.Datos = multas;
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
        [Route("GetMultasPendientes")]
        // SysBiblioteca/API/Multas/GetMultasPendientes
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetMultasPendientes([FromBody] Multas _multas)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_multas.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_multas.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _multas.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Multas> multas = iMultasService.getWaiting();
                            if (multas.Count() > 0)
                            {
                                _rp.Datos = multas;
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
        [Route("GetMultasPagadas")]
        // SysBiblioteca/API/Multas/GetMultasPagadas
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetMultasPagadas([FromBody] Multas _multas)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_multas.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_multas.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _multas.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Multas> multas = iMultasService.getPaid();
                            if (multas.Count() > 0)
                            {
                                _rp.Datos = multas;
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