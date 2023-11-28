$('#btnModify').click(function () {
    if ($('#OldPassword').val().trim() != "" && $('#NewPassword').val().trim() != "" && $('#RepNewPassword').val().trim() != "") {

        if ($('#NewPassword').val().trim() == $('#RepNewPassword').val().trim()) {
            var Obj = {
                OldPassword: $('#OldPassword').val().trim(),
                NewPassword: $('#NewPassword').val().trim(),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'UsuariosExternos/ChangeMyPassword',
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
                            location.reload();
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
                html: 'Las contrase&ntilde;as no coinciden',
                timer: 2000,
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

$('#btnSaveChanges').click(function () {
    if ($('#Nombres').val().trim() != "" && $('#Apellidos').val().trim() != "" && $('#Genero').val() > 0 && $('#DUI').val().trim() != ""
        && $('#Correo').val().trim() != "" && $('#Direccion').val().trim() != "" && $('#Telefono').val().trim() != "" && $('#FechaNacimiento').val().trim() != "") {

        if (validarFecha($('#FechaNacimiento').val().trim())) {
            var Obj = {
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
                url: api + 'UsuariosExternos/UpdateMyData',
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
                            location.reload();
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