$('#btnSave').click(function () {
    if ($('#Nombres').val().trim() != "" && $('#Apellidos').val().trim() != "" && $('#Genero').val() > 0 && $('#CodigodeMulta').val().trim() != ""
        && $('#Correo').val().trim() != "" && $('#Direccion').val().trim() != "" && $('#Telefono').val().trim() != "" && $('#FechaNacimiento').val().trim() != ""
        && $('#Usuario').val().trim() != "" && $('#Rol').val() > 0 && $('#Cargo').val() > 0) {

        if (validarFecha($('#FechaValidacion').val().trim())) {
            var Obj = {
                IdMulta: parseInt($('#IdMulta').val()),

                IdRol: parseInt($('#Rol').val()),
                IdCargo: parseInt($('#Cargo').val()),
                Usuario: $('#Usuario').val().trim(),

                DatosPersonales: {
                    Nombres: $('#Nombres').val().trim(),
                    Apellidos: $('#Apellidos').val().trim(),
                    IdGenero: $('#Genero').val(),
                    DUI: $('#DUI').val().trim(),
                    Correo: $('#Correo').val().trim(),
                    Direccion: $('#Direccion').val().trim(),
                    Telefono: $('#Telefono').val().trim(),
                    FechaNacimiento: $('#FechaNacimiento').val().trim(),
                },

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/CreateEmpleado',
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
                html: 'La Fecha de Nacimiento debe de tener el formato de dd/mm/aaaa y la edad debe ser mayor a 18 años.',
                timer: 5000,
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
    if ($('#Nombres').val().trim() != "" && $('#Apellidos').val().trim() != "" && $('#Genero').val() > 0 && $('#DUI').val().trim() != ""
        && $('#Correo').val().trim() != "" && $('#Direccion').val().trim() != "" && $('#Telefono').val().trim() != "" && $('#FechaNacimiento').val().trim() != ""
        && $('#Usuario').val().trim() != "" && $('#Rol').val() > 0 && $('#Cargo').val() > 0) {

        if (validarFecha($('#FechaNacimiento').val().trim())) {
            var Obj = {
                IdUsuario: parseInt($('#IdUsuario').val()),

                IdRol: parseInt($('#Rol').val()),
                IdCargo: parseInt($('#Cargo').val()),
                Usuario: $('#Usuario').val().trim(),

                DatosPersonales: {
                    Nombres: $('#Nombres').val().trim(),
                    Apellidos: $('#Apellidos').val().trim(),
                    IdGenero: $('#Genero').val(),
                    DUI: $('#DUI').val().trim(),
                    Correo: $('#Correo').val().trim(),
                    Direccion: $('#Direccion').val().trim(),
                    Telefono: $('#Telefono').val().trim(),
                    FechaNacimiento: $('#FechaNacimiento').val().trim(),
                },

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Seguridad/UpdateEmpleado',
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
                html: 'La Fecha de Nacimiento debe de tener el formato de dd/mm/aaaa y la edad debe ser mayor a 18 años.',
                timer: 5000,
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
            html: 'Para continuar, debe de rellenar los campos requeridos.',
            timer: 5000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getEmpleado(IdUsuario, action) {
    var Obj = {
        IdUsuario: IdUsuario,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Seguridad/GetEmpleadoById',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                if (action) {
                    resetForm();

                    $("#tituloModal").text("Datos del Empleado");

                    $('#IdUsuario').val(data.datos.idUsuario);
                    $('#Nombres').val(data.datos.datosPersonales.nombres).attr("disabled", true);
                    $('#Apellidos').val(data.datos.datosPersonales.apellidos).attr("disabled", true);

                    $('#Genero').attr("disabled", true);
                    loadGeneros(data.datos.datosPersonales.idGenero, true);

                    $('#DUI').val(data.datos.datosPersonales.dui).attr("disabled", true);
                    $('#Correo').val(data.datos.datosPersonales.correo).attr("disabled", true);
                    $('#Direccion').val(data.datos.datosPersonales.direccion).attr("disabled", true);

                    $('#Telefono').val(data.datos.datosPersonales.telefono).attr("disabled", true);
                    $('#FechaNacimiento').val(data.datos.datosPersonales.fechaNacimiento).attr("disabled", true);

                    $('#Usuario').val(data.datos.usuario).attr("disabled", true);

                    $('#Rol').attr("disabled", true);
                    loadRoles(data.datos.idRol, true);

                    $('#Cargo').attr("disabled", true);
                    loadCargos(data.datos.idCargo, true);

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

function deactivateEmpleado(IdUsuario) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea desactivar este Usuario?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Desactivar",
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
                url: api + 'Seguridad/DeactivateUser',
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

function activateEmpleado(IdUsuario) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea activar este Usuario?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#08D1AD',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Activar",
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
                url: api + 'Seguridad/ActivateUser',
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