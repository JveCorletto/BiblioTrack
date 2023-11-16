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
        && $('#AnioPublicacion').val() > 0 && $('#Cantidad').val() > 0 && $('#Descripcion').val().trim() != "" && $('#FotoLibro').val().trim() != "") {

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
                Cantidad: parseInt($('#Cantidad').val()),
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

function buscarLibro(action) {
    var searchQuery = {
        Titulo: (action ? $("#LibroSearch").val().trim() : $("#LibroSearch2").val().trim()),
        IdAutor: (action ? parseInt($("#AutorSearch").val()) : parseInt($("#AutorSearch2").val())),
        IdGenero: (action ? parseInt($("#GeneroSearch").val()) : parseInt($("#GeneroSearch2").val())),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Libros/SearchLibros',
        contentType: "Application/json",
        data: JSON.stringify(searchQuery),
        success: function (data) {
            if (data.resultado == 1) {
                if (action) {
                    $("#renderLibrosActivos").html(null);
                    var html = "";

                    $.each(data.datos, function () {
                        html += '<div class="col-md-4">';
                        html += '   <div class="card mb-3 hover shadow-lg" onclick="getBook(' + this.idLibro + ')">';
                        html += '       <div class="row no-gutters align-items-center">';
                        html += '           <div class="col-md-4">';
                        html += '               <img class="card-img" src="' + this.fotoLibro + '">';
                        html += '           </div>';
                        html += '           <div class="col-md-8">';
                        html += '               <div class="card-body">';
                        html += '                   <p class="card-text"><b>' + this.libro + '</b></p>';
                        html += '                   <p class="card-text"><b>Autor(es): </b>' + this.autores + '</p>';
                        html += '                   <p class="card-text"><b>A&ntilde;o Publicaci&oacute;n:</b> ' + this.anioPublicacion + '</p>';
                        html += '                   <p class="card-text"><b>Cantidad en Stock:</b> ' + this.cantidad + '</p>';
                        html += '               </div>';
                        html += '           </div>';
                        html += '       </div>';
                        html += '   </div>';
                        html += '</div>';
                    });

                    $("#renderLibrosActivos").html(html);
                }
                else {
                    $("#renderLibrosInactivos").html(null);
                }
            }
            else {
                Swal.fire({
                    title: 'Información',
                    icon: "Information",
                    html: "No apareci&oacute; ningún libro en la busqueda",
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        }
    });
}

function agregarAutor() {
    var nuevoIdAutor = $("#Autor").val();

    if (nuevoIdAutor > 0) {
        var Autores = JSON.parse(localStorage.getItem("Autores")) || [];

        var existeAutor = Autores.some(function (autor) {
            return autor.IdAutor === nuevoIdAutor;
        });

        if (!existeAutor) {
            var nuevoAutor = {
                IdAutor: nuevoIdAutor,
                Autor: $("#Autor option:selected").text()
            };

            Autores.push(nuevoAutor);
            localStorage.setItem("Autores", JSON.stringify(Autores));

            renderAutores(Autores);
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

function renderAutores(Autores) {
    $("#renderAutores").html(null);
    var html = "";
    $.each(Autores, function () {
        html += '<li class="list-group-item d-flex justify-content-between align-items-center">';
        html += '   ' + this.Autor + '';
        html += '   <span class="badge bg-danger rounded-pill"><i class="fas fa-times"></i></span>';
        html += '</li>';
    });

    $("#renderAutores").html(html);
}

function agregarGenero() {
    var nuevoIdGenero = $("#GeneroLiterario").val();

    if (nuevoIdGenero > 0) {
        var Generos = JSON.parse(localStorage.getItem("Generos")) || [];

        var existeGenero = Generos.some(function (genero) {
            return genero.IdGenero === nuevoIdGenero;
        });

        if (!existeGenero) {
            var nuevoGenero = {
                IdGenero: nuevoIdGenero,
                Genero: $("#GeneroLiterario option:selected").text()
            };

            Generos.push(nuevoGenero);
            localStorage.setItem("Generos", JSON.stringify(Generos));

            renderGeneros(Generos);
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

function renderGeneros(Generos) {
    $("#renderGeneros").html(null);
    var html = "";
    $.each(Generos, function () {
        html += '<li class="list-group-item d-flex justify-content-between align-items-center">';
        html += '   ' + this.Genero + '';
        html += '   <span class="badge bg-danger rounded-pill"><i class="fas fa-times"></i></span>';
        html += '</li>';
    });

    $("#renderGeneros").html(html);
}