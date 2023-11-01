$(document).ready(function () {
    verificarBase();
});

function validateVista() {
    var api = localStorage.getItem('apiURL');
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        URL: window.location.hash.replace('#', '')
    };

    $.ajax({
        type: 'POST',
        url: api + 'Authentication/GetMenu',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado != null) {
                if (data.resultado == 1) {

                    loadActivos();
                    loadGeneros();
                    loadRoles();
                    loadCargos();
                }
                else {
                    window.location = "../../../";
                }
            }
            else {
                window.location = "../../../";
            }
        }
    });
}

function resetForm() {
    $('#dataEmpleado').trigger('reset');
    $('#editionMode').hide();
    $('#btnEdit').hide();

    $('#btnSave').show();
    $('#btnCancel').show();

    $('#Nombres').attr("disabled", false);
    $('#Apellidos').attr("disabled", false);
    $("#Genero").val('0').change();
    $('#Genero').attr("disabled", false);

    $('#DUI').attr("disabled", false);
    $('#Correo').attr("disabled", false);
    $('#Direccion').attr("disabled", false);

    $('#Telefono').attr("disabled", false);
    $('#FechaNacimiento').attr("disabled", false);

    $('#Usuario').attr("disabled", false);
    $("#Rol").val('0').change();
    $('#Rol').attr("disabled", false);
    $("#Cargo").val('0').change();
    $('#Cargo').attr("disabled", false);

    $("#tituloModal").text("Nuevo Empleado");
}