using SysBiblioteca.API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using SysBiblioteca.API.Management;
using SysBiblioteca.API.Models.ADM;
using SysBiblioteca.API.Models.INV;
using SysBiblioteca.API.Services.INV.LibrosService;
using SysBiblioteca.API.Services.INV.AutoresService;
using SysBiblioteca.API.Services.ADM.UsuariosService;
using SysBiblioteca.API.Services.ADM.LinkRolMenuService;
using SysBiblioteca.API.Services.INV.AutoresLibrosService;
using SysBiblioteca.API.Services.INV.GenerosLibrosService;
using SysBiblioteca.API.Services.INV.GenerosLiterariosService;
using SysBiblioteca.API.Services.INV.EditorialesService;

namespace SysBiblioteca.API.Controllers
{
    [ApiController]
    [EnableCors("SysBiblioteca_Policy")]
    [Route("SysBiblioteca/API/[controller]")]
    public class LibrosController : Controller
    {
        private readonly iUsuariosService iUsuarios;
        private readonly iLibrosService iLibrosService;
        private readonly iAutoresService iAutoresService;
        private readonly iEditorialesService iEditorialesService;
        private readonly iLinkRolMenuService iLinkRolMenuService;
        private readonly iGenerosLibrosService iGenerosLibrosService;
        private readonly iAutoresLibrosService iAutoresLibrosService;
        private readonly iGenerosLiterariosService iGenerosLiterariosService;

        public LibrosController(iUsuariosService usuariosService, iLinkRolMenuService linkRolMenuService, iAutoresService autoresService, iGenerosLiterariosService generosLiterariosService, iLibrosService librosService,
            iAutoresLibrosService autoresLibrosService, iGenerosLibrosService generosLibrosService, iEditorialesService editorialesService)
        {
            iUsuarios = usuariosService;
            iLibrosService = librosService;
            iAutoresService = autoresService;
            iEditorialesService = editorialesService;
            iLinkRolMenuService = linkRolMenuService;
            iGenerosLibrosService = generosLibrosService;
            iAutoresLibrosService = autoresLibrosService;
            iGenerosLiterariosService = generosLiterariosService;
        }

        [HttpPost]
        [Route("GetGeneros")]
        // SysBiblioteca/API/Libros/GetGeneros
        // Método que obtiene el listado de Generos Literarios
        public IActionResult GetGeneros([FromBody] GenerosLiterarios _generos)
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
                            IEnumerable<GenerosLiterarios> generos = iGenerosLiterariosService.Read();
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
        [Route("GetAutores")]
        // SysBiblioteca/API/Libros/GetAutores
        // Método que obtiene el listado de Autores
        public IActionResult GetAutores([FromBody] Autores _autores)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_autores.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_autores.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _autores.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Autores> autores = iAutoresService.Read();
                            if (autores.Count() > 0)
                            {
                                _rp.Datos = autores;
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
        [Route("GetEditoriales")]
        // SysBiblioteca/API/Libros/GetEditoriales
        // Método que obtiene el listado de Autores
        public IActionResult GetEditoriales([FromBody] Editoriales _editoriales)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_editoriales.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_editoriales.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _editoriales.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Editoriales> editoriales = iEditorialesService.Read();
                            if (editoriales.Count() > 0)
                            {
                                _rp.Datos = editoriales;
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
        [Route("CreateLibro")]
        // SysBiblioteca/API/Libros/CreateLibro
        // Método que agrega un nuevo Libro al sistema
        public IActionResult CreateLibro([FromBody] LibrosDTO_ADD _libro)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (_libro.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(_libro.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, _libro.ActualRute);
                        if (permisos != null && permisos.Create)
                        {
                            Libros newLibro = new Libros {
                                FotoLibro = _libro.FotoLibro,
                                Libro = _libro.Libro,
                                Version = (_libro.Version != null ? _libro.Version : "N/A"),
                                ISBN = _libro.ISBN,
                                AnioPublicacion = _libro.AnioPublicacion,
                                Descripcion = _libro.Descripcion,
                                Cantidad = _libro.Cantidad,

                                IdEditorial = _libro.IdEditorial,
                                IdEstado = 1,

                                UsuarioCreacion = user.Usuario,
                                FechaCreacion = DateTime.Now
                            };

                            iLibrosService.Create(newLibro);

                            if (newLibro.IdLibro > 0)
                            {
                                foreach (var generoLiterario in _libro.GenerosLiterarios)
                                {
                                    GenerosLibros newGenero = new GenerosLibros {
                                        IdLibro = newLibro.IdLibro,
                                        IdGenero = generoLiterario.IdGenero
                                    };

                                    iGenerosLibrosService.Create(newGenero);
                                }

                                foreach (var autor in _libro.Autores)
                                {
                                    AutoresLibros newAutor = new AutoresLibros
                                    {
                                        IdLibro = newLibro.IdLibro,
                                        IdAutor = autor.IdAutor
                                    };

                                    iAutoresLibrosService.Create(newAutor);
                                }

                                _rp.Resultado = 1;
                                _rp.Mensaje = "Libro ingresado correctamente.";
                                return Ok(_rp);
                            }
                            else
                            {
                                _rp.Mensaje = "No se pudo crear el libro, intente nuevamente.";
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
        [Route("SearchLibros")]
        // SysBiblioteca/API/Libros/SearchLibros
        // Método que busca un libro que encaje en la descripción específicada
        public IActionResult SearchLibros([FromBody] searchQuery searchQuery)
        {
            Reply _rp = new Reply { Resultado = 0 };

            try
            {
                if (searchQuery.Token != null)
                {
                    Usuarios user = iUsuarios.getTokenActual(searchQuery.Token);
                    if (user != null)
                    {
                        Link_Rol_Menu permisos = iLinkRolMenuService.validateVista(user.IdRol, searchQuery.ActualRute);
                        if (permisos != null && permisos.Read)
                        {
                            IEnumerable<Libros> libros = iLibrosService.searchByName(searchQuery.Titulo);
                            if (libros.Count() > 0)
                            {
                                List<LibrosDTO> resultsLibros = new List<LibrosDTO>();

                                foreach (var libro in libros)
                                {
                                    resultsLibros.Add(new LibrosDTO
                                    {
                                        IdLibro = libro.IdLibro,
                                        FotoLibro = libro.FotoLibro,
                                        Libro = libro.Libro,
                                        Autores = getAutoresName(iAutoresLibrosService.getAutoresLibro(libro.IdLibro)),
                                        Generos = getGenerosName(iGenerosLibrosService.getGenerosLibro(libro.IdLibro)),
                                        AnioPublicacion = libro.AnioPublicacion,
                                        Cantidad = libro.Cantidad
                                    });
                                }
                                _rp.Datos = resultsLibros;
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

        private string getGenerosName(List<GenerosLiterarios> generosLiterarios)
        {
            String generos = String.Empty;
            foreach (var genero in generosLiterarios)
            {
                generos += genero.Genero + ", ";
            }
            return generos;
        }

        private string getAutoresName(List<Autores> autoresLibros)
        {
            String autores = String.Empty;
            foreach (var autor in autoresLibros)
            {
                autores += autor.Autor + ", ";
            }
            return autores;
        }
    }
}