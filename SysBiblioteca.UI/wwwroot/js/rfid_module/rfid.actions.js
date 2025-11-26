function selectUser(IdUsuario) {
    Swal.fire({
        title: 'Validacion',
        html: "&iquest;Es este el usuario a realizar el prestamo?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#ffc107',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Prestar"
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdUsuario: IdUsuario,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.pathname
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

function prestar() {
    if ($('#IdUsuario').val() > 0 && $('#IdEjemplar').val() > 0 && $('#DiasPrestamo').val() > 0) {
        var Obj = {
            IdPrestamo: parseInt($('#IdPrestamo').val()),
            IdLibro: parseInt($('#IdLibro').val()),
            IdEjemplar: parseInt($('#IdEjemplar').val()),
            IdUsuario: parseInt($('#IdUsuario').val()),
            DiasPrestamo: parseInt($('#DiasPrestamo').val()),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.pathname
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Prestamos/PrestamoRFID',
            contentType: "application/json",
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
                        $("#btnCancelPrestamo").click();
                        window.close();
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
                    }).then(function () {
                        $("#IdLibro").val(null);
                        $("#Libro").val(null);
                        $("#miniaturaContainer").html(null);
                    });
                }
            },
            error: function (data) {
                Swal.fire({
                    title: 'Error',
                    icon: "warning",
                    html: 'Ocurrió un error al procesar la solicitud. Intente nuevamente.',
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
}

function recibir() {
    var pkg = {
        IdPrestamo: parseInt($("#IdDevolucion").val()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.pathname
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
                    $("#btnCancelDevolucion").click();
                    windows.close();
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
}

function cobrar() {
    var pkg = {
        IdPrestamo: parseInt($("#IdDevolucion").val()),
        DiasRetraso: parseInt($("#DiasRetrazo").val()),
        Monto: parseFloat($("#dollars").val() + $("#cents").text()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.pathname
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
                    $("#btnCancelDevolucion").click();
                    windows.close();
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
}

function generar() {
    var pkg = {
        IdPrestamo: parseInt($("#IdDevolucion").val()),
        DiasRetraso: parseInt($("#DiasRetrazo").val()),
        Monto: parseFloat($("#dollars").val() + $("#cents").text()),

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.pathname
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
                    $("#btnCancelDevolucion").click();
                    windows.close();
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
}