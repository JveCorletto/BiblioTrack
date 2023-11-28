function loadMyData() {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'UsuariosExternos/GetMyData',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $('#Nombres').val(data.datos.datosPersonales.nombres);
                $('#Apellidos').val(data.datos.datosPersonales.apellidos);

                $('#Telefono').val(data.datos.datosPersonales.telefono);
                $('#FechaNacimiento').val(data.datos.datosPersonales.fechaNacimiento);
                loadGeneros(data.datos.datosPersonales.idGenero);

                $('#DUI').val(data.datos.datosPersonales.dui);
                $('#Correo').val(data.datos.datosPersonales.correo);
                $('#Direccion').val(data.datos.datosPersonales.direccion);

                $('#Usuario').val(data.datos.usuario);
                $('#lbUserName').html(data.datos.usuario);
                $('#lbEmail').html(data.datos.datosPersonales.correo);
            }
            else {
                Swal.fire({
                    title: 'Error',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    window.location = "../../../";
                });
            }
        }
    });
}

//Carga los Generos
function loadGeneros(IdGenero) {
    var Obj = {
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Catalogos/GetGeneros',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            $("#Genero").html(null);

            if (data.resultado == 1) {
                $("#Genero").html(null);
                var html = "";
                html += "<option value>Elija un Género</option>";
                $.each(data.datos, function () {
                    html += "<option value='" + this.idGenero + "'>" + this.genero + "</option>";
                });
                $("#Genero").html(html);

                if (IdGenero > 0) {
                    $("#Genero").val('' + IdGenero + '').change();
                }
                else {
                    $("#Genero").val('0').change();
                }
            }
            else {
                var html = "";
                html += "<option value='0'>" + data.mensaje + "</option>";
                $("#Genero").html(html);
            }
        }
    });
}