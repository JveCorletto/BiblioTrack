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
                    localStorage.removeItem("IdRol");

                    loadActivos();
                    loadInactivos();
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
    $('#dataRol').trigger('reset');
    $('#editionMode').hide();
    $('#btnEdit').hide();

    $('#btnSave').show();
    $('#btnCancel').show();

    $('#Rol').attr("disabled", false);
    $("#tituloModal").text("Nuevo Rol");
}

$('#btnEdition').click(function () {
    $('#editionMode').hide();
    $('#btnEdit').show();
    $("#Rol").attr("disabled", false);

    $("#tituloModal").text("Edicion de Rol");
});