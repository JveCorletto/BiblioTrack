$('#btnSave').click(function () {
    if ($('#IdUsuario').val() > 0 && $('#IdLibro').val() > 0 && $('#DiasPrestamo').val() > 0) {
        var Obj = {
            IdLibro: parseInt($('#IdLibro').val()),
            IdUsuario: parseInt($('#IdUsuario').val()),
            DiasPrestamo: parseInt($('#DiasPrestamo').val()),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Prestamos/CreatePrestamo',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    Swal.fire({
                        title: 'Exito',
                        icon: "success",
                        html: data.mensaje,
                        timer: 5000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    }).then(function () {
                        $('#btnCancel').click();
                        loadPrestamosActivos();
                    });
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: data.mensaje,
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
                    html: data.mensaje,
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
            html: 'Para continuar, debe de rellenar todos los campos.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnEntregar').click(function () {
    Swal.fire({
        title: 'Validacion',
        html: "&iquest;Desea marcar como entregado este libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#ffc107',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Entregar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdPrestamo: parseInt($("#IdPrestamo").val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Prestamos/LoanBook',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 1000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            $('#btnCancel').click();
                            loadPrestamosActivos();
                            loadPrestamosPendientes();
                        });
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
    });
});

function getPrestamo(IdPrestamo) {
    var pkg = {
        IdPrestamo: IdPrestamo,

        Token: localStorage.getItem("UserToken"),
        URL: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetLoan',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                $('#IdPrestamo').val(data.datos.idPrestamo);
                $('#IdUsuario').val(data.datos.idUsuario);
                $('#IdLibro').val(data.datos.idLibro);
                $('#Usuario').val(data.datos.usuario.usuario).attr("disabled", true);
                $('#Libro').val(data.datos.libro.libro).attr("disabled", true);
                $('#DiasPrestamo').val(data.datos.diasPrestamo).attr("disabled", true);
                $('#btnSearchBook').attr("disabled", true);
                $('#btnSearchUser').attr("disabled", true);

                var img = $('<img>').attr('src', data.datos.libro.fotoLibro);
                img.css('max-width', '100%');
                img.css('max-height', '100%');
                $('#miniaturaContainer').html(null);
                $('#miniaturaContainer').append(img);


                $('#btnSave').hide();
                $('#btnEntregar').show();
            }
            else {
                Swal.fire({
                    title: 'Validacion',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 1000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        }
    });
}

function buscarLibro() {
    var searchQuery = {
        Titulo: $("#LibroSearch").val().trim(),
        IdAutor: parseInt($("#AutorSearch").val()),
        IdGenero: parseInt($("#GeneroSearch").val()),
        Prestamo: true,

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
                $("#renderLibrosActivos").html(null);
                var html = "";

                $.each(data.datos, function () {
                    html += '<div class="col-md-4">';
                    html += '   <div class="card mb-3 hover shadow-lg" onclick="selectBook(' + this.idLibro + ')">';
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
                Swal.fire({
                    title: 'Información',
                    icon: "info",
                    html: "No apareci&oacute; ningún libro en la busqueda",
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $("#renderLibrosActivos").html(null);
                });
            }
        }
    });
}

function selectBook(IdLibro) {
    Swal.fire({
        title: 'Validacion',
        html: "&iquest;Es este el libro que desea para el prestamo?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#ffc107',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Prestar",
    }).then((result) => {
        if (result.isConfirmed) {
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
                        $('#IdLibro').val(data.datos.idLibro);
                        $('#Libro').val(data.datos.libro).attr("disabled", true);

                        var img = $('<img>').attr('src', data.datos.fotoLibro);
                        img.css('max-width', '100%');
                        img.css('max-height', '100%');
                        $('#miniaturaContainer').html('');
                        $('#miniaturaContainer').append(img);

                        $('#btnCancelBook').click();
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
    });
}

function selectUser(IdUsuario) {
    Swal.fire({
        title: 'Validacion',
        html: "&iquest;Es este el usuario a realizar el prestamo?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#ffc107',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Prestar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdUsuario: IdUsuario,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/GetUserById',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        $('#IdUsuario').val(data.datos.idUsuario);
                        $('#Usuario').val(data.datos.usuario).attr("disabled", true);

                        $('#btnCancelUser').click();
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
                            $('#btnCancelUser').click();
                        });
                    }
                }
            });
        }
    });
}