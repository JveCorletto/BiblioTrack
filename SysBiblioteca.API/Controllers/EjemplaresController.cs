using QRCoder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.PRS;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.PRS.PrestamosService;
using SysBiblioteca.API.Services.INV.EjemplaresService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class EjemplaresController : Controller
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLibrosService iLibrosService;
        private readonly iPrestamosService iPrestamosService;
        private readonly iEjemplaresService iEjemplaresService;
        private readonly iLinkRolMenuService iLinkRolMenuService;

        public EjemplaresController(iEjemplaresService EjemplaresService, iLinkRolMenuService LinkRolMenuService, 
            iUsuariosService UsuariosService, iLibrosService LibrosService, iPrestamosService PrestamosService)
        {
            iUsuarios = UsuariosService;
            iLibrosService = LibrosService;
            iPrestamosService = PrestamosService;
            iEjemplaresService = EjemplaresService;
            iLinkRolMenuService = LinkRolMenuService;
        }

        [HttpPost]
        [Route("CreateEjemplar")]
        // SysBiblioteca/API/Ejemplares/CreateEjemplar
        // Método que agrega un ejemplar del Libro al sistema
        public IActionResult CreateEjemplar([FromBody] Ejemplares ejemplar)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (ejemplar.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(ejemplar.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, ejemplar.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Libros libro = iLibrosService.getById(ejemplar.IdLibro);

                            if (libro != null)
                            {
                                Ejemplares newEjemplar = new Ejemplares
                                {
                                    IdLibro = libro.IdLibro,
                                    CodigoEjemplar = libro.ISBN + "|" + getNextEjemplarNumber(iEjemplaresService.getStock(libro.IdLibro)),
                                    Estado = true,

                                    UsuarioCreacion = user.Usuario,
                                    FechaCreacion = DateTime.Now,
                                };

                                iEjemplaresService.Create(newEjemplar);

                                if (newEjemplar.IdEjemplar > 0)
                                {
                                    var qrCodeBytes = GenerateQrCode(newEjemplar.CodigoEjemplar);
                                    string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Ejemplar creado exitosamente.";
                                    _rp.Datos = new { newEjemplar.CodigoEjemplar, qrCodeBase64 };
                                    return Ok(_rp);
                                }
                                else
                                {
                                    _rp.Mensaje = "ocurrio un error al registrar el ejemplar, intente nuevamente.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo identificar el libro, intente nuevamente.";
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
        [Route("GetEjemplares")]
        // SysBiblioteca/API/Ejemplares/GetEjemplares
        // Método que agrega un ejemplar del Libro al sistema
        public IActionResult GetEjemplares([FromBody] Ejemplares ejemplar)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (ejemplar.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(ejemplar.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, ejemplar.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            List<Ejemplares> ejemplares = iEjemplaresService.getStock(ejemplar.IdLibro);

                            if (ejemplares.Count > 0)
                            {
                                _rp.Resultado = 1;
                                _rp.Datos = ejemplares.OrderBy(e => e.CodigoEjemplar);
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Este libro aún no tiene ejemplares registrados.";
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
        [Route("GetScannedEjemplar")]
        // SysBiblioteca/API/Ejemplares/GetScannedEjemplar
        // Método que optiene la data del ejemplar escaneado y el status actual.
        public IActionResult GetScannedEjemplar([FromBody] Ejemplares ejemplar)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (ejemplar.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(ejemplar.Token);
                    if (user != null)
                    {
                        if (user.Rol?.Rol != "Administrador" && user.Rol?.Rol != "Empleado")
                        {
                            Ejemplares scannedEjemplar = iEjemplaresService.getByCodigo(ejemplar.CodigoEjemplar);
                            if (scannedEjemplar != null)
                            {
                                Prestamos lastPrestamo = iPrestamosService.getLastLoanByEjemplar(scannedEjemplar.IdEjemplar);

                                // Significa que NUNCA ha sido prestado y NO está reservado.
                                if (lastPrestamo == null)
                                {
                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Nuevo Prestamo";
                                    _rp.Datos = scannedEjemplar;
                                    return Ok(_rp);
                                }
                                // Significa que ya ha sido prestado anteriormente.
                                else
                                {
                                    // Significa que es un libro que está reservado y no ha sido entregado aún
                                    if (lastPrestamo.Entregado == false)
                                    {
                                        _rp.Resultado = 2;
                                        _rp.Mensaje = "Libro Reservado";
                                        _rp.Datos = lastPrestamo;
                                        return Ok(_rp);
                                    }
                                    // Significa que es un libro que vienen a devolver.
                                    else if (lastPrestamo.Entregado == true && lastPrestamo.Finalizado == false)
                                    {
                                        _rp.Resultado = 3;
                                        _rp.Mensaje = "Devolución de Libro";
                                        _rp.Datos = lastPrestamo;
                                        return Ok(_rp);
                                    }
                                    // Significa que este libro ya ha sido prestado anteriormente, pero que está disponible para un nuevo prestamo.
                                    else if (lastPrestamo.Entregado == true && lastPrestamo.Finalizado == true)
                                    {
                                        _rp.Resultado = 1;
                                        _rp.Mensaje = "Nuevo Prestamo 2";
                                        _rp.Datos = scannedEjemplar;
                                        return Ok(_rp);
                                    }
                                    else 
                                    {
                                        _rp.Mensaje = "Situación no controlada.";
                                        return Ok(_rp); 
                                    }
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo identificar el Ejemplar escaneado, intente nuevamente.";
                                return Ok(_rp);
                            }
                        }
                        else
                        {
                            _rp.Mensaje = "El Usuario no tiene permiso de usar esta pantalla.";
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
        [Route("GetQREjemplar")]
        // SysBiblioteca/API/Ejemplares/CreateEjemplar
        // Método que obtiene el QR del Ejemplar seleccionado
        public IActionResult GetQREjemplar([FromBody] Ejemplares ejemplar)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (ejemplar.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(ejemplar.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, ejemplar.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            Ejemplares _ejemplar = iEjemplaresService.getById(ejemplar.IdEjemplar);

                            if (_ejemplar != null)
                            {
                                var qrCodeBytes = GenerateQrCode(_ejemplar.CodigoEjemplar);
                                string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                                _rp.Resultado = 1;
                                _rp.Datos = new { _ejemplar.IdEjemplar, qrCodeBase64 };
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "Ocurrio un error al generar el QR, intente nuevamente.";
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
        [Route("DeactivateEjemplar")]
        // SysBiblioteca/API/Ejemplares/DeactivateEjemplar
        // Método que desactiva un ejemplar del Libro en el sistema
        public IActionResult DeactivateEjemplar([FromBody] Ejemplares ejemplar)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (ejemplar.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(ejemplar.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, ejemplar.ActualRute);
                        if (permisos != null && permisos.Delete)
                        {
                            Ejemplares _ejemplar = iEjemplaresService.getById(ejemplar.IdEjemplar);

                            if (_ejemplar != null) {
                                ejemplar.UsuarioModificacion = user.Usuario;

                                try
                                {
                                    iEjemplaresService.deactivateEjemplar(_ejemplar);
                                    iEjemplaresService.updateContext(_ejemplar);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Se eliminó correctamente el ejemplar.";
                                    return Ok(_rp);
                                }
                                catch (Exception)
                                {
                                    _rp.Mensaje = "Ocurrió un problema al borrar el ejemplar del sistema, intente nuevamente.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo identificar el ejemplar, intente nuevamente.";
                                return Ok(_rp);
                            }

                            Libros libro = iLibrosService.getById(ejemplar.IdLibro);

                            if (libro != null)
                            {
                                Ejemplares newEjemplar = new Ejemplares
                                {
                                    IdLibro = libro.IdLibro,
                                    CodigoEjemplar = libro.ISBN + "|" + getNextEjemplarNumber(iEjemplaresService.getStock(libro.IdLibro)),
                                    Estado = true,

                                    UsuarioCreacion = user.Usuario,
                                    FechaCreacion = DateTime.Now,
                                };

                                iEjemplaresService.Create(newEjemplar);

                                if (newEjemplar.IdEjemplar > 0)
                                {
                                    var qrCodeBytes = GenerateQrCode(newEjemplar.CodigoEjemplar);
                                    string qrCodeBase64 = Convert.ToBase64String(qrCodeBytes);

                                    _rp.Resultado = 1;
                                    _rp.Mensaje = "Ejemplar creado exitosamente.";
                                    _rp.Datos = new { newEjemplar.CodigoEjemplar, qrCodeBase64 };
                                    return Ok(_rp);
                                }
                                else
                                {
                                    _rp.Mensaje = "ocurrio un error al registrar el ejemplar, intente nuevamente.";
                                    return Ok(_rp);
                                }
                            }
                            else
                            {
                                
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

        private int getNextEjemplarNumber(List<Ejemplares> ejemplares)
        {
            int nextNumber = 1;
            List<int> ejemplarNumbers = new List<int>();
            
            foreach (var ejemplar in ejemplares)
            {
                var parts = ejemplar.CodigoEjemplar.Split('|');
                if (parts.Length == 2 && int.TryParse(parts[1], out int ejemplarNumber))
                {
                    ejemplarNumbers.Add(ejemplarNumber);
                }
            }
            
            ejemplarNumbers.Sort();
            for (int i = 1; i <= ejemplarNumbers.Count + 1; i++)
            {
                if (!ejemplarNumbers.Contains(i))
                {
                    nextNumber = i;
                    break;
                }
            }

            return nextNumber;
        }

        private byte[] GenerateQrCode(string text)
        {
            using (var qrGenerator = new QRCodeGenerator())
            using (var qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
            {
                var qrCode = new BitmapByteQRCode(qrCodeData);
                var qrCodeBytes = qrCode.GetGraphic(20);
                return qrCodeBytes;
            }
        }
    }
}