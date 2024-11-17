function validateState() {
    var urlParams = new URLSearchParams(window.location.search);
    var CodigoEjemplar = urlParams.get('CodigoEjemplar');

    getInfo(CodigoEjemplar);

    var diasInput = $('#DiasPrestamo');
    diasInput.on('input', function () {
        var valor = parseInt(diasInput.val());

        $('#validation').empty();
        if (isNaN(valor) || valor <= 0 || valor >= 31) {
            var html = "";
            html += '<div class="alert alert-danger" role="alert">';
            html += '   Ingrese un número válido (mayor a 0 y menor a 31)';
            html += '</div>';
            $('#validation').html(html);
            diasInput.val('');
        }
    });
}

function getInfo(CodigoEjemplar) {
    var Obj = {
        CodigoEjemplar: CodigoEjemplar,
        
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.pathname
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Ejemplares/GetScannedEjemplar',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            // Prestamos
            if (data.resultado == 1) {
                Swal.fire({
                    title: '¡Información!',
                    icon: "info",
                    html: "Ejemplar libre para préstamo.",
                    timer: 5000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $('#dataLoan').trigger('reset');

                    $("#IdEjemplar").val(data.datos.idEjemplar);
                    $("#IdLibro").val(data.datos.idLibro);
                    $("#Libro").val(data.datos.libro.libro);
                    var img = $('<img>').attr('src', data.datos.libro.fotoLibro);
                    img.css('max-width', '100%');
                    img.css('max-height', '100%');
                    $('#miniaturaContainer').html(null);
                    $('#miniaturaContainer').append(img);

                    $("#tituloModalPrestamo").text("Nuevo Préstamo");
                    $('#staticPrestamo').modal('show');
                });
            }
            // Entrega
            else if (data.resultado == 2) {
                Swal.fire({
                    title: '¡Atención!',
                    icon: "warning",
                    html: "Este libro ya está reservado, mostrando datos de la reserva:",
                    timer: 5000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $('#dataLoan').trigger('reset');

                    $("#IdUsuario").val(data.datos.idUsuario);
                    $("#IdPrestamo").val(data.datos.idPrestamo);
                    $("#IdEjemplar").val(data.datos.idEjemplar);
                    $("#Usuario").val(data.datos.usuario.datosPersonales.nombres + " " + data.datos.usuario.datosPersonales.apellidos + " - " + data.datos.usuario.datosPersonales.dui);
                    $("#DiasPrestamo").val(data.datos.diasPrestamo).attr("disabled", true);

                    $("#IdLibro").val(data.datos.ejemplar.idLibro);
                    $("#Libro").val(data.datos.ejemplar.libro.libro);
                    var img = $('<img>').attr('src', data.datos.ejemplar.libro.fotoLibro);
                    img.css('max-width', '100%');
                    img.css('max-height', '100%');
                    $('#miniaturaContainer').html(null);
                    $('#miniaturaContainer').append(img);
                    $('#btnSearchUser').attr("disabled", true);

                    $("#tituloModalPrestamo").text("Libro reservado");
                    $('#staticPrestamo').modal('show');
                });
            }
            // Devolución
            else if (data.resultado == 3) {
                Swal.fire({
                    title: '¡Información!',
                    icon: "info",
                    html: "Este libro está en estatus de préstamo. Mostrando los datos:",
                    timer: 5000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $('#dataDevolucion').trigger('reset');

                    $('#IdDevolucion').val(data.datos.idPrestamo);

                    $('#UsuarioDevolucion').val(data.datos.usuario.usuario).attr("disabled", true);
                    $('#DUIDevolucion').val(data.datos.usuario.datosPersonales.dui).attr("disabled", true);
                    $('#TelefonoDevolucion').val(data.datos.usuario.datosPersonales.telefono).attr("disabled", true);
                    $('#CorreoDevolucion').val(data.datos.usuario.datosPersonales.correo).attr("disabled", true);

                    $('#UsuarioEntrego').val(data.datos.usuarioEntrego.usuario).attr("disabled", true);
                    $('#DiasPrestamoDevolucion').val(data.datos.diasPrestamo).attr("disabled", true);
                    $('#FechaPrestamo').val(data.datos.fechaPrestamo).attr("disabled", true);

                    $('#LibroDevolucion').val(data.datos.ejemplar.libro.libro).attr("disabled", true);
                    var img = $('<img>').attr('src', data.datos.ejemplar.libro.fotoLibro);
                    img.css('max-width', '100%');
                    img.css('max-height', '100%');
                    $('#miniaturaContainerDevolucion').html(null);
                    $('#miniaturaContainerDevolucion').append(img);

                    let estado = getStatus(data.datos.diasPrestamo, data.datos.fechaPrestamo, data.datos.entregado)

                    $('#StatusPrestamo').html(null);
                    $('#StatusPrestamo').html('<div class="alert alert-' + getclass(estado) + ' text-center font-weight-bolder" role="alert">' + estado.toUpperCase() + '</div>');

                    $('#staticDevolucion').modal('show');
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
                    windows.close();
                });
            }
        },
        error: function(data) {
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
                windows.close();
            });
        }
    });
}

function resetFormUser() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.pathname
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Prestamos/GetUserForLoans',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                if ($.fn.dataTable.isDataTable('#tableUsuarios')) {
                    $('#tableUsuarios').DataTable().clear().destroy();
                }
                $("#tUsuarios").html(null);

                $.each(data.datos, function () {
                    var html = "";
                    html += "<tr onclick='selectUser(" + this.idUsuario + ")'>";
                    html += "   <td>" + this.datosPersonales.dui + "</td>";
                    html += "   <td>" + this.usuario + "</td>";
                    html += "   <td>" + this.datosPersonales.apellidos + ", " + this.datosPersonales.nombres + "</td>";
                    html += "   <td>" + this.datosPersonales.telefono + "</td>";
                    html += "   <td>" + this.datosPersonales.fechaNacimiento + "</td>";
                    html += "   <td>" + this.datosPersonales.genero.genero + "</td>";
                    html += "</tr>";
                    $("#tUsuarios").append(html);
                });
                paginate('tableUsuarios');
            }
            else {
                if ($.fn.dataTable.isDataTable('#tableUsuarios')) {
                    $('#tableUsuarios').DataTable().clear().destroy();
                }
                $("#tUsuarios").html(null);
                var html = "";
                html += "<tr>";
                html += "   <td colspan='6'><center class='text-danger font-weight-bolder'>" + data.mensaje + "</center></td>";
                html += "</tr>";
                $("#tUsuarios").append(html);
            }
        }
    });
}

function getStatus(diasPrestamo, fechaPrestamo, entregado) {
    let fechaPrestamoDate = new Date(fechaPrestamo);

    let fechaActual = new Date();
    fechaActual.setHours(0, 0, 0, 0);

    let fechaDevolucion = new Date(fechaPrestamoDate);
    fechaDevolucion.setDate(fechaDevolucion.getDate() + diasPrestamo);

    if (entregado) {
        return fechaActual <= fechaDevolucion ? "A tiempo" : "Demorado";
    } else {
        return "Pendiente";
    }
}

function getclass(estado) {
    var cssClass = "";

    switch (estado) {
        case "A tiempo":
            cssClass = 'success'
            break;
        case "Demorado":
            cssClass = 'danger'
            break;
        case "Pendiente":
            cssClass = 'warning'
            break;
    }

    return cssClass;
}