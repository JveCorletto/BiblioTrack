$('#FotoLibro').change(function () {
    var input = this;

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var img = $('<img>').attr('src', e.target.result);

            // Establecer estilos para la miniatura
            img.css('max-width', '100%');
            img.css('max-height', '100%');

            // Limpiar el contenedor antes de agregar una nueva miniatura
            $('#miniaturaContainer').html('');

            // Agregar la miniatura al contenedor
            $('#miniaturaContainer').append(img);

            var base64Image = e.target.result;
            localStorage.setItem("base64Image", base64Image);
        };

        // Leer la imagen como una URL de datos (data URL)
        reader.readAsDataURL(input.files[0]);
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Por favor, seleccione un archivo v&acute;lido para continuar',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnSave').click(function () {
    if ($('#Libro').val().trim() != "" && $('#Version').val().trim() != "" && $('#ISBN').val().trim() != "" && $('#Editorial').val() > 0
        && $('#AnioPublicacion').val() > 0 && $('#Descripcion').val().trim() != "" && $('#FotoLibro').val().trim() != "") {

        var Autores = JSON.parse(localStorage.getItem("Autores")) || [];
        var Generos = JSON.parse(localStorage.getItem("Generos")) || [];

        if (Autores != null && Generos != null) {
            var Obj = {
                FotoLibro: String(localStorage.getItem("base64Image")),
                Libro: $('#Libro').val(),
                Version: $('#Version').val(),
                ISBN: $('#ISBN').val(),
                IdEditorial: parseInt($('#Editorial').val()),
                AnioPublicacion: parseInt($('#AnioPublicacion').val()),
                Descripcion: $('#Descripcion').val(),

                Autores: Autores,
                GenerosLiterarios: Generos,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Libros/CreateLibro',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.message,
                            timer: 5000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            $('#btnCancel').click();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.message,
                            timer: 5000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                },
                error: function (data) {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: "Ocurrió un error, intente nuevamente",
                        timer: 5000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    });
                }
            });
        }
        else {
            Swal.fire({
                title: 'Validacion',
                icon: "warning",
                html: 'Debe de brindar al menos un g&eacute;nero literario y un autor para el libro',
                timer: 2500,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Para continuar, debe de rellenar todos los campos.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnEdit').click(function () {
    if ($('#Libro').val().trim() != "" && $('#Version').val().trim() != "" && $('#ISBN').val().trim() != "" && $('#Editorial').val() > 0
        && $('#AnioPublicacion').val() > 0 && $('#Descripcion').val().trim() != "") {

        var Autores = JSON.parse(localStorage.getItem("Autores")) || [];
        var Generos = JSON.parse(localStorage.getItem("Generos")) || [];

        if (Autores != null && Generos != null) {
            var Obj = {
                IdLibro: $('#IdLibro').val(),
                FotoLibro: String(localStorage.getItem("base64Image")),
                Libro: $('#Libro').val(),
                Version: $('#Version').val(),
                ISBN: $('#ISBN').val(),
                IdEditorial: parseInt($('#Editorial').val()),
                AnioPublicacion: parseInt($('#AnioPublicacion').val()),
                Descripcion: $('#Descripcion').val(),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Libros/UpdateLibro',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.message,
                            timer: 5000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            $('#btnCancel').click();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.message,
                            timer: 5000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                },
                error: function (data) {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: "Ocurrió un error, intente nuevamente",
                        timer: 5000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    });
                }
            });
        }
        else {
            Swal.fire({
                title: 'Validacion',
                icon: "warning",
                html: 'Debe de brindar al menos un g&eacute;nero literario y un autor para el libro',
                timer: 2500,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Para continuar, debe de rellenar todos los campos.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnActivar').click(function () {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea activar este libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#28a745',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Activar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdLibro: parseInt($("#IdLibro").val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Libros/ActivateBook',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            buscarLibro(false);
                            $('#btnCancel').click();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    });
});

$('#btnDesactivar').click(function () {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea desactivar este libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Desactivar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdLibro: parseInt($("#IdLibro").val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Libros/DeactivateBook',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            buscarLibro(true);
                            $('#btnCancel').click();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    });
});

function buscarLibro(action) {
    var searchQuery = {
        Titulo: (action ? $("#LibroSearch").val().trim() : $("#LibroSearch2").val().trim()),
        IdAutor: (action ? parseInt($("#AutorSearch").val()) : parseInt($("#AutorSearch2").val())),
        IdGenero: (action ? parseInt($("#GeneroSearch").val()) : parseInt($("#GeneroSearch2").val())),
        Prestamo: false,
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };

    var api = localStorage.getItem('apiURL');
    var renderTarget = action ? "#renderLibrosActivos" : "#renderLibrosInactivos";
    var apiEndpoint = action ? "SearchLibros" : "SearchLibrosInactivos";

    $.ajax({
        type: 'POST',
        url: `${api}Libros/${apiEndpoint}`,
        contentType: "application/json",
        data: JSON.stringify(searchQuery),
        success: function (data) {
            if (data.resultado == 1) {
                $(renderTarget).html(null);
                var html = data.datos.map(generateBookCard).join('');
                $(renderTarget).html(html);
            } else {
                Swal.fire({
                    title: 'Información',
                    icon: "info",
                    html: "No apareci&oacute; ningún libro en la búsqueda",
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $(renderTarget).html(null);
                });
            }
        }
    });
}

function generateBookCard(book) {
    return `
        <div class="col-md-4">
            <div class="card mb-3 hover shadow-lg" onclick="getBook(${book.idLibro})">
                <div class="row no-gutters align-items-center">
                    <div class="col-md-4">
                        <img class="card-img" src="${book.fotoLibro}" alt="${book.libro}">
                    </div>
                    <div class="col-md-8">
                        <div class="card-body">
                            <p class="card-text"><b>${book.libro}</b></p>
                            <p class="card-text"><b>Autor(es): </b>${book.autores}</p>
                            <p class="card-text"><b>A&ntilde;o Publicaci&oacute;n:</b> ${book.anioPublicacion}</p>
                            <p class="card-text"><b>Disponibles para Pr&eacute;stamo:</b> ${book.cantidad}</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    `;
}

function getBook(IdLibro) {
    var Obj = {
        IdLibro: IdLibro,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/GetLibro',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $('#openModal').click();

                $("#tituloModal").text("Datos del Libro");

                $('#IdLibro').val(data.datos.idLibro);
                $('#Libro').val(data.datos.libro).attr("disabled", true);
                $('#Version').val(data.datos.version).attr("disabled", true);
                $('#ISBN').val(data.datos.isbn).attr("disabled", true);

                $('#Editorial').attr("disabled", true);
                loadEditoriales(data.datos.idEditorial, true);
                $('#AnioPublicacion').val(data.datos.anioPublicacion).attr("disabled", true);
                $('#Descripcion').val(data.datos.descripcion).attr("disabled", true);

                var img = $('<img>').attr('src', data.datos.fotoLibro);
                img.css('max-width', '100%');
                img.css('max-height', '100%');
                $('#miniaturaContainer').html('');
                $('#miniaturaContainer').append(img);
                localStorage.setItem("base64Image", data.datos.fotoLibro);
                $('#FotoLibro').attr("disabled", true);

                $.ajax({
                    type: 'POST',
                    url: api + 'Libros/GetAutoresLibro',
                    contentType: "Application/json",
                    data: JSON.stringify(Obj),
                    success: function (data) {
                        if (data.resultado == 1) {
                            localStorage.setItem("Autores", JSON.stringify(data.datos));
                            renderAutores(data.datos);
                            $('#Autor').attr("disabled", true);
                            $('#btnAutor').attr("disabled", true);
                        }
                        else {
                            Swal.fire({
                                title: 'Error',
                                icon: "warning",
                                html: data.mensaje,
                                timer: 1000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading();
                                },
                            }).then(function () {
                                $('#btnCancel').click();
                            });
                        }
                    }
                });

                $.ajax({
                    type: 'POST',
                    url: api + 'Libros/GetGenerosLibro',
                    contentType: "Application/json",
                    data: JSON.stringify(Obj),
                    success: function (data) {
                        if (data.resultado == 1) {
                            localStorage.setItem("Generos", JSON.stringify(data.datos));
                            renderGeneros(data.datos);
                            $('#GeneroLiterario').attr("disabled", true);
                            $('#btnGeneroLiterario').attr("disabled", true);
                        }
                        else {
                            Swal.fire({
                                title: 'Error',
                                icon: "warning",
                                html: data.mensaje,
                                timer: 1000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading();
                                },
                            }).then(function () {
                                $('#btnCancel').click();
                            });
                        }
                    }
                });

                if (data.datos.idEstado == 1) {
                    $('#btnActivar').hide();
                    $('#btnDesactivar').show();

                    $('#btnSave').hide();
                    $('#btnEdition').show();
                    $('#editionMode').show();
                }
                else {
                    $('#btnActivar').show();
                    $('#btnDesactivar').hide();

                    $('#btnSave').hide();
                    $('#btnEdition').hide();
                    $('#editionMode').show();
                }
            }
            else {
                Swal.fire({
                    title: 'Error',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 1000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $('#btnCancel').click();
                });
            }
        }
    });
}

function agregarAutor() {
    var IdLibro = parseInt($("#IdLibro").val().trim());
    var nuevoIdAutor = parseInt($("#Autor").val());

    if (nuevoIdAutor > 0) {
        var Autores = JSON.parse(localStorage.getItem("Autores")) || [];

        var existeAutor = Autores.some(function (autor) {
            return autor.idAutor === nuevoIdAutor;
        });

        if (!existeAutor) {
            var nuevoAutor = {
                idAutor: nuevoIdAutor,
                autor: $("#Autor option:selected").text()
            };

            Autores.push(nuevoAutor);
            localStorage.setItem("Autores", JSON.stringify(Autores));

            renderAutores(Autores);

            if (IdLibro > 0) {
                var Obj = {
                    IdLibro: IdLibro,
                    IdAutor: nuevoIdAutor,

                    Token: localStorage.getItem("UserToken"),
                    ActualRute: window.location.hash.replace('#', '')
                };
                var api = localStorage.getItem('apiURL');

                $.ajax({
                    type: 'POST',
                    url: api + 'Libros/AddAutor',
                    contentType: "Application/json",
                    data: JSON.stringify(Obj),
                    success: function (data) {
                        if (data.resultado == 1) {
                            Swal.fire({
                                title: 'Exito',
                                icon: "success",
                                html: data.message,
                                timer: 5000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading();
                                },
                            });
                        }
                        else {
                            Swal.fire({
                                title: 'Error',
                                icon: "warning",
                                html: data.message,
                                timer: 5000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading();
                                },
                            });
                        }
                    },
                    error: function (data) {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: "Ocurrió un error, intente nuevamente",
                            timer: 5000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                });
            }
        }
        else {
            Swal.fire({
                title: 'Error',
                icon: "warning",
                html: "Ya se ha a&ntilde;adido este autor",
                timer: 2500,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    }
    else {
        Swal.fire({
            title: 'Error',
            icon: "warning",
            html: "Selecciona un autor valido",
            timer: 2500,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
}

function removeAuthor(IdAutor) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad eliminar este Autor al libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Quitar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdAutor: IdAutor,
                IdLibro: parseInt($("#IdLibro").val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Libros/RemoveAutor',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            var autores = JSON.parse(localStorage.getItem("Autores")) || [];

                            var indiceAutor = -1;
                            for (var i = 0; i < autores.length; i++) {
                                if (autores[i].idAutor === IdAutor) {
                                    indiceAutor = i;
                                    break;
                                }
                            }

                            if (indiceAutor !== -1) {
                                autores.splice(indiceAutor, 1);

                                localStorage.setItem("Autores", JSON.stringify(autores));
                                renderAutores(autores);
                            }
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    });
}

function renderAutores(Autores) {
    $("#renderAutores").html(null);
    var html = "";
    $.each(Autores, function () {
        html += '<li class="list-group-item d-flex justify-content-between align-items-center">';
        html += '   ' + this.autor + '';
        html += '   <span class="badge bg-danger rounded-pill" onclick="removeAuthor(' + this.idAutor + ')"><i class="fas fa-times" style="color:white !important;"></i></span>';
        html += '</li>';
    });

    $("#renderAutores").html(html);
    $("#Autor").val('0').change();
}

function agregarGenero() {
    var IdLibro = parseInt($("#IdLibro").val().trim());
    var nuevoIdGenero = parseInt($("#GeneroLiterario").val());

    if (nuevoIdGenero > 0) {
        var Generos = JSON.parse(localStorage.getItem("Generos")) || [];

        var existeGenero = Generos.some(function (genero) {
            return genero.idGenero === nuevoIdGenero;
        });

        if (!existeGenero) {
            var nuevoGenero = {
                idGenero: nuevoIdGenero,
                genero: $("#GeneroLiterario option:selected").text()
            };

            Generos.push(nuevoGenero);
            localStorage.setItem("Generos", JSON.stringify(Generos));

            renderGeneros(Generos);

            if (IdLibro > 0) {
                var Obj = {
                    IdLibro: IdLibro,
                    IdGenero: nuevoIdGenero,

                    Token: localStorage.getItem("UserToken"),
                    ActualRute: window.location.hash.replace('#', '')
                };
                var api = localStorage.getItem('apiURL');

                $.ajax({
                    type: 'POST',
                    url: api + 'Libros/AddGender',
                    contentType: "Application/json",
                    data: JSON.stringify(Obj),
                    success: function (data) {
                        if (data.resultado == 1) {
                            Swal.fire({
                                title: 'Exito',
                                icon: "success",
                                html: data.message,
                                timer: 5000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading();
                                },
                            });
                        }
                        else {
                            Swal.fire({
                                title: 'Error',
                                icon: "warning",
                                html: data.message,
                                timer: 5000,
                                timerProgressBar: true,
                                didOpen: () => {
                                    Swal.showLoading();
                                },
                            });
                        }
                    },
                    error: function (data) {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: "Ocurrió un error, intente nuevamente",
                            timer: 5000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                });
            }
        }
        else {
            Swal.fire({
                title: 'Error',
                icon: "warning",
                html: "Ya se ha a&ntilde;adido este G&eacute;nero Literario",
                timer: 2500,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });
        }
    }
    else {
        Swal.fire({
            title: 'Error',
            icon: "warning",
            html: "Selecciona un G&eacute;nero Literario valido",
            timer: 2500,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
}

function removeGender(IdGenero) {
    Swal.fire({
        title: 'Validacion',
        html: "¿En verdad eliminar este G&eacute;nero literario del libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Quitar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdGenero: IdGenero,
                IdLibro: parseInt($("#IdLibro").val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Libros/RemoveGender',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            var generos = JSON.parse(localStorage.getItem("Generos")) || [];

                            var indiceGenero = -1;
                            for (var i = 0; i < generos.length; i++) {
                                if (generos[i].idGenero === IdGenero) {
                                    indiceGenero = i;
                                    break;
                                }
                            }

                            if (indiceGenero !== -1) {
                                generos.splice(indiceGenero, 1);

                                localStorage.setItem("Generos", JSON.stringify(generos));
                                renderGeneros(generos);
                            }
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    });
}

function renderGeneros(Generos) {
    $("#renderGeneros").html(null);
    var html = "";
    $.each(Generos, function () {
        html += '<li class="list-group-item d-flex justify-content-between align-items-center">';
        html += '   ' + this.genero + '';
        html += '   <span class="badge bg-danger rounded-pill" onclick="removeGender(' + this.idGenero + ')"><i class="fas fa-times" style="color:white !important;"></i></span>';
        html += '</li>';
    });

    $("#renderGeneros").html(html);
    $("#GeneroLiterario").val('0').change();
}