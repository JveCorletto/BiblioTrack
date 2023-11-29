using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Services.PRS.MultasService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.INV.LibrosService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class MultasController : ControllerBase
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLibrosService iLibrosService;
        private readonly iMultasService iMultasService;
        private readonly iPrestamosService iPrestamosService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        public MultasController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, 
            iMultasService multasService, iPrestamosService prestamosService, iLibrosService librosService)
        {
            iUsuarios = usuariosService;
            iLibrosService = librosService;
            iMultasService = multasService;
            iPrestamosService = prestamosService;
            iLinkRolMenuService = linkRolMenuService;
        }

        #region Gestión de Multas para Personal Interno

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
                                List<MultasDTO> multasDTO = new List<MultasDTO>();
                                foreach (var item in multas)
                                {
                                    multasDTO.Add(new MultasDTO
                                    {
                                        IdMulta = item.IdMulta,
                                        FotoLibro = item.Prestamo.Libro.FotoLibro,
                                        Libro = item.Prestamo.Libro.Libro,
                                        Usuario = item.Prestamo.Usuario.Usuario,
                                        DiasRetraso = item.DiasRetraso,
                                        Monto = item.Monto,
                                        Estado = item.EstadoMulta.EstadoMulta
                                    });
                                }

                                _rp.Datos = multasDTO;
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
                                List<MultasDTO> multasDTO = new List<MultasDTO>();
                                foreach (var item in multas)
                                {
                                    multasDTO.Add(new MultasDTO
                                    {
                                        FotoLibro = item.Prestamo.Libro.FotoLibro,
                                        Libro = item.Prestamo.Libro.Libro,
                                        Usuario = item.Prestamo.Usuario.Usuario,
                                        DiasRetraso = item.DiasRetraso,
                                        Monto = item.Monto,
                                        FechaValidacion = item.FechaValidacion,
                                        UsuarioValidacion = item.UsuarioValidacion.Usuario
                                    });
                                }

                                _rp.Datos = multasDTO;
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
        [Route("GenerateInvoce")]
        // SysBiblioteca/API/Multas/GenerateInvoce
        // Método que Genera el cobro a un Usuario
        public IActionResult GenerateInvoce([FromBody] Multas _multas)
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
                        if (permisos != null && permisos.Create)
                        {
                            Multas newMulta = new Multas
                            {
                                IdPrestamo = _multas.IdPrestamo,
                                DiasRetraso = _multas.DiasRetraso,
                                Monto = _multas.Monto,
                                IdEstadoMulta = 1
                            };

                            iMultasService.Create(newMulta);

                            if (newMulta.IdMulta > 0)
                            {
                                _rp.Resultado = 1;
                                _rp.Mensaje = "Solicitud de cobro generada correctamente, usuario puede realizar el pago de forma virtual.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo generar la solicitud de cobro, intente nuevamente";
                                return Ok(_rp);
                            }
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
        [Route("GetUnpaidInvoce")]
        // SysBiblioteca/API/Multas/GetUnpaidInvoce
        // Método que Genera el cobro a un Usuario
        public IActionResult GetUnpaidInvoce([FromBody] Multas _multas)
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
                            Multas multa = iMultasService.getById(_multas.IdMulta);
                            if (multa != null)
                            {
                                _rp.Datos = new
                                {
                                    multa.IdMulta,
                                    multa.Monto,
                                    multa.DiasRetraso,
                                    multa.ComprobantePago,
                                    multa.Prestamo.Libro.Libro,
                                    multa.Prestamo.Libro.FotoLibro,
                                    multa.Prestamo.Usuario.Usuario
                                };
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
        [Route("ApproveInvoce")]
        // SysBiblioteca/API/Multas/ApproveInvoce
        // Método que aprueba el pago de una multa
        public IActionResult ApproveInvoce([FromBody] Multas _multas)
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
                            Multas multa = iMultasService.getById(_multas.IdMulta);
                            if (multa != null)
                            {
                                multa.FechaValidacion = DateTime.Now;
                                multa.IdUsuarioValidacion = user.IdUsuario;
                                multa.IdEstadoMulta = 3;
                                iMultasService.cargarComprobante(multa);
                                iPrestamosService.MarkAsFinished(multa.IdPrestamo, user.IdUsuario);
                                iLibrosService.agregarUnidad(multa.Prestamo.IdLibro);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Pago aprobado y se marc&oacute; el prestamo como finalizado";
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
        [Route("PhysicalPayment")]
        // SysBiblioteca/API/Multas/PhysicalPayment
        // Método que realiza un cobro físico
        public IActionResult PhysicalPayment([FromBody] Multas _multas)
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
                            Multas multa = iMultasService.getById(_multas.IdMulta);
                            if (multa != null)
                            {
                                multa.PagoFisico = true;
                                multa.IdEstadoMulta = 3;
                                multa.FechaValidacion = DateTime.Now;
                                multa.IdUsuarioValidacion = user.IdUsuario;
                                iMultasService.cargarComprobante(multa);
                                iPrestamosService.MarkAsFinished(multa.IdPrestamo, user.IdUsuario);
                                iLibrosService.agregarUnidad(multa.Prestamo.IdLibro);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Se realiz&oacute; el cobro y se marc&oacute; el prestamo como finalizado";
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

        #endregion

        #region Gestión de Multas para el Usuario Externo

        [HttpPost]
        [Route("GetMyFines")]
        // SysBiblioteca/API/Multas/GetMyFines
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetMyFines([FromBody] Multas _multas)
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
                            IEnumerable<Multas> multas = iMultasService.GetMyFines(user.IdUsuario);
                            if (multas.Count() > 0)
                            {
                                List<MultasDTO> multasDTO = new List<MultasDTO>();
                                foreach (var item in multas)
                                {
                                    multasDTO.Add(new MultasDTO
                                    {
                                        IdMulta = item.IdMulta,
                                        FotoLibro = item.Prestamo.Libro.FotoLibro,
                                        Libro = item.Prestamo.Libro.Libro,
                                        DiasRetraso = item.DiasRetraso,
                                        Monto = item.Monto,
                                        Estado = item.EstadoMulta.EstadoMulta
                                    });
                                }
                                _rp.Datos = multasDTO;
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
        [Route("GetMyPaidFines")]
        // SysBiblioteca/API/Multas/GetMyPaidFines
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetMyPaidFines([FromBody] Multas _multas)
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
                            IEnumerable<Multas> multas = iMultasService.GetMyPaidFines(user.IdUsuario);
                            if (multas.Count() > 0)
                            {
                                List<MultasDTO> multasDTO = new List<MultasDTO>();
                                foreach (var item in multas)
                                {
                                    multasDTO.Add(new MultasDTO
                                    {
                                        FotoLibro = item.Prestamo.Libro.FotoLibro,
                                        Libro = item.Prestamo.Libro.Libro,
                                        DiasRetraso = item.DiasRetraso,
                                        Monto = item.Monto,
                                        FechaValidacion = item.FechaValidacion,
                                        UsuarioValidacion = item.UsuarioValidacion.Usuario
                                    });
                                }
                                _rp.Datos = multasDTO;
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
        [Route("GetInvoce")]
        // SysBiblioteca/API/Multas/GetInvoce
        // Método que obtiene la solicitud de cobro seleccionada
        public IActionResult GetInvoce([FromBody] Multas _multas)
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
                            Multas multa = iMultasService.getById(_multas.IdMulta);
                            if (multa != null)
                            {
                                _rp.Datos = new {
                                    multa.IdMulta,
                                    multa.Monto,
                                    multa.DiasRetraso,
                                    multa.Prestamo.Libro.Libro,
                                    multa.Prestamo.Libro.FotoLibro
                                };
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
        [Route("UploadInvoce")]
        // SysBiblioteca/API/Multas/UploadInvoce
        // Método que carga el comprobante de pago
        public IActionResult UploadInvoce([FromBody] Multas _multas)
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
                        if (permisos != null && permisos.Update)
                        {
                            Multas multa = iMultasService.getById(_multas.IdMulta);
                            if (multa != null) 
                            {
                                multa.ComprobantePago = _multas.ComprobantePago;
                                multa.IdEstadoMulta = 2;
                                multa.PagoFisico = false;
                                iMultasService.cargarComprobante(multa);

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Comprobante cargado con &eacute;xito, un administrador aprobará su pago en breve";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo encontrar los datos de la multa seleccionada, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El usuario no tiene permisos de edicion en ésta pantalla.";
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