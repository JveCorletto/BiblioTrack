$('#btnSave').click(function () {
    if ($('#Rol').val().trim() != "") {
        var Obj = {
                Rol: $('#Rol').val().trim(),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/CreateRol',
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
                            loadActivos();
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
    if ($('#Rol').val().trim() != "") {
            var Obj = {
                IdRol: parseInt($('#IdRol').val()),
                Rol: $('#Rol').val().trim(),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/UpdateRol',
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
                            loadActivos();
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
            html: 'Para continuar, debe de rellenar los campos requeridos.',
            timer: 5000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getRol(IdRol, action) {
    var Obj = {
        IdRol: IdRol,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Seguridad/GetRolById',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                if (action) {
                    resetForm();

                    $("#tituloModal").text("Datos del Rol");

                    $('#IdRol').val(data.datos.idRol);
                    $('#Rol').val(data.datos.rol).attr("disabled", true);

                    $('#btnSave').hide();
                    $('#editionMode').show();
                }
                else {
                    $('#NombreProducto_').val(data.datos.nombreProducto).attr("disabled", true);
                    getLogs(data.datos.idProducto);
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
                    if (action) {
                        $('#btnCancel').click();
                    }
                    else {
                        $('#btnCancelRol').click();
                    }
                });
            }
        }
    });
}

function validarFecha(fecha) {
    var regexFecha = /^(\d{2}\/\d{2}\/\d{4})$/;
    var fechaHoy = new Date();
    fechaHoy.setFullYear(fechaHoy.getFullYear() - 15);

    var partesFecha = fecha.split('/'); // Dividir la cadena en partes
    var fechaValidacion = new Date(partesFecha[2], partesFecha[1] - 1, partesFecha[0]);

    if (regexFecha.test(fecha) && (fechaValidacion < fechaHoy)) {
        return true;
    }
    else {
        return false;
    }
}

function deactivateRol(IdRol) {
    Swal.fire({
        title: 'Validación',
        text: "¿En verdad desea desactivar este Rol?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Desactivar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdRol: IdRol,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/DeactivateRol',
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
                            loadActivos();
                            loadInactivos();
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
    })
}

function activateRol(IdRol) {
    Swal.fire({
        title: 'Validación',
        text: "¿En verdad desea activar este Rol?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#08D1AD',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Activar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdRol: IdRol,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/ActivateRol',
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
                            loadActivos();
                            loadInactivos();
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
    })
}