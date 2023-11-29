function getLoan(IdPrestamo) {
    var pkg = {
        IdPrestamo: IdPrestamo,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Devoluciones/GetLoan',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                $('#IdPrestamo').val(data.datos.idPrestamo);

                $('#Usuario').val(data.datos.usuario).attr("disabled", true);
                $('#DUI').val(data.datos.dui).attr("disabled", true);
                $('#Telefono').val(data.datos.telefono).attr("disabled", true);
                $('#Correo').val(data.datos.correo).attr("disabled", true);

                $('#UsuarioEntrego').val(data.datos.usuarioEntrego).attr("disabled", true);
                $('#DiasPrestamo').val(data.datos.diasPrestamo).attr("disabled", true);
                $('#FechaPrestamo').val(data.datos.fechaPrestamo).attr("disabled", true);

                $('#Libro').val(data.datos.libro.libro).attr("disabled", true);
                var img = $('<img>').attr('src', data.datos.libro.fotoLibro);
                img.css('max-width', '100%');
                img.css('max-height', '100%');
                $('#miniaturaContainer').html(null);
                $('#miniaturaContainer').append(img);

                $('#StatusPrestamo').html(null);
                $('#StatusPrestamo').html('<div class="alert alert-' + getclass(data.datos.estado) + ' text-center font-weight-bolder" role="alert">' + String(data.datos.estado).toUpperCase() + '</div>');
            }
            else {
                Swal.fire({
                    title: 'Información',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $("#btnCancel").click();
                });
            }
        }
    });
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

$("#btnRecibir").click(function () {
    var pkg = {
        IdPrestamo: parseInt($("#IdPrestamo").val()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Devoluciones/FinishLoan',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                Swal.fire({
                    title: 'Exito',
                    icon: "success",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $("#btnCancel").click();
                    loadPrestamosActivos();
                    loadPrestamosFinalizados();
                });
            }
            else if (data.resultado == 2) {
                $('#retrasoAlert').html(data.mensaje);

                $('#UsuarioMulta').val(data.datos.usuario);
                $('#FechaPrestamoMulta').val(String(data.datos.fechaPrestamo).split('T')[0] + ' ' + String(data.datos.fechaPrestamo).split('T')[1]);
                $('#DiasPrestamoMulta').val(data.datos.diasPrestamo);
                $('#DiasRetrazo').val(data.datos.diasExcedidos);

                $('#dollars').val(String(data.datos.penalizacion).split('.')[0]);
                $('#cents').html('.' + String(data.datos.penalizacion).split('.')[1]);

                $('#staticMulta').modal('show');
            }
            else {
                Swal.fire({
                    title: 'Información',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        }
    });
});

$("#btnCobrar").click(function () {
    var pkg = {
        IdPrestamo: parseInt($("#IdPrestamo").val()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Devoluciones/PayFine',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                Swal.fire({
                    title: 'Exito',
                    icon: "success",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $("#btnCancelMulta").click();
                    $("#btnCancel").click();
                    loadPrestamosActivos();
                    loadPrestamosFinalizados();
                });
            }
            else {
                Swal.fire({
                    title: 'Información',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        }
    });
});

$("#btnGenerar").click(function () {
    var pkg = {
        IdPrestamo: parseInt($("#IdPrestamo").val()),
        DiasRetraso: parseInt($("#DiasRetrazo").val()),
        Monto: parseFloat($("#dollars").val() + $("#cents").text()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GenerateInvoce',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado == 1) {
                Swal.fire({
                    title: 'Exito',
                    icon: "success",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $("#btnCancelMulta").click();
                    $("#btnCancel").click();
                });
            }
            else {
                Swal.fire({
                    title: 'Información',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        }
    });
});