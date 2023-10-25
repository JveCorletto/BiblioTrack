$(document).ready(function () {
    verificarBase();
});

function validateVista() {
    var api = localStorage.getItem('apiURL');
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        URL: window.location.hash.replace('#', ''),
        AppToken: localStorage.getItem("AppToken")
    };

    $.ajax({
        type: 'POST',
        url: api + 'Auth/ValidateView',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado != null) {
                if (data.resultado == 1) {
                    loadActivas();
                    loadInactivas();
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

function resetForm(action) {
    $('#dataAplicacion').trigger('reset');

    //Creación
    if (action) {
        $('#editionMode').hide();

        $('#btnSave').show();
        $('#btnEdit').hide();
        $('#btnCancel').show();

        $('#NombreAplicacion').attr("disabled", false);
        $('#URLAplicacion').attr("disabled", false);

        $("#modalTittle").text("Registro de Nueva Aplicación");
    }
    //Edición
    else {
        $('#editionMode').show();

        $('#btnSave').hide();
        $('#btnEdit').hide();
        $('#btnCancel').hide();

        $('#NombreAplicacion').attr("disabled", true);
        $('#URLAplicacion').attr("disabled", true);

        $("#modalTittle").text("Datos de la Aplicación");
    }
}

$('#btnEdition').click(function () {
    $('#editionMode').hide();

    $('#btnSave').hide();
    $('#btnEdit').show();
    $('#btnCancel').show();

    $('#NombreAplicacion').attr("disabled", false);
    $('#URLAplicacion').attr("disabled", false);

    $("#modalTittle").text("Editar Aplicación");
});