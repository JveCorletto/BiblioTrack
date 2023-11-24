using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iPrestamosService iPrestamosService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        public PrestamosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iPrestamosService prestamosService)
        {
            iUsuarios = usuariosService;
            iPrestamosService = prestamosService;
            iLinkRolMenuService = linkRolMenuService;
        }

        [HttpPost]
        [Route("GetMyBooks")]
        // SysBiblioteca/API/Prestamos/GetMyBooks
        // Método que obtiene el listado de Prestamos Activos para el Usuario
        public IActionResult GetMyBooks([FromBody] Prestamos _prestamos)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_prestamos.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_prestamos.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _prestamos.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Prestamos> prestamos = iPrestamosService.GetMyBooks(user.IdUsuario);
                            if (prestamos.Count() > 0)
                            {
                                List<MisPrestamosDTO> misPrestamos = new List<MisPrestamosDTO>();
                                foreach (var item in prestamos)
                                {
                                    misPrestamos.Add(new MisPrestamosDTO
                                    {
                                        IdPrestamo = item.IdPrestamo,
                                        Libro = item.Libro,
                                        DiasPrestamo = item.DiasPrestamo,
                                        FechaPrestamo = item.FechaPrestamo,
                                        Estado = getStatus(item.DiasPrestamo, item.FechaPrestamo)
                                    });
                                }

                                _rp.Datos = misPrestamos;
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

        private string getStatus(int? diasPrestamo, DateTime? fechaPrestamo)
        {
            DateTime fechaActual = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            DateTime fechaDevolucion = fechaPrestamo.Value.AddDays((double)diasPrestamo);

            return fechaActual > fechaDevolucion ? "A tiempo" : "Demorado";
        }
    }
}