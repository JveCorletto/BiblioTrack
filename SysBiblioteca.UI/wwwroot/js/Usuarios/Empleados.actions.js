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