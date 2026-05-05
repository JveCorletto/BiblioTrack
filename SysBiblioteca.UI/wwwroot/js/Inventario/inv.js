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
                    loadSecciones();
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

function resetFormSeccion() {
    $('#dataSeccion').trigger('reset');
    $('#editionModeSeccion').hide();
    $('#btnEditSeccion').hide();

    $('#btnSaveSeccion').show();
    $('#btnCancelSeccion').show();

    $('#Seccion').attr("disabled", false);

    $("#tituloModalSeccion").text("Nueva Secci&oacute;n");
}

$('#btnEditionSeccion').click(function () {
    $('#editionModeSeccion').hide();
    $('#btnEditSeccion').show();

    $('#Seccion').attr("disabled", false);

    $("#tituloModalSeccion").text("Edicion de la Secci&oacute;n");
});

function resetFormEstanterias() {
    $('#dataEstanteria').trigger('reset');
    $('#editionModeEstanteria').hide();
    $('#btnEditEstanteria').hide();

    $('#btnSaveEstanteria').show();
    $('#btnCancelEstanteria').show();

    $('#Estanteria').attr("disabled", false);

    $("#tituloModalEstanteria").text("Nueva Estanter&iacute;a");
}

$('#btnEditionEstanteria').click(function () {
    $('#editionModeEstanteria').hide();
    $('#btnEditEstanteria').show();

    $('#Estanteria').attr("disabled", false);

    $("#tituloModalEstanteria").text("Edicion de la Estanter&iacute;a");
});

function resetFormNiveles() {
    $('#dataNivel').trigger('reset');
    $('#editionModeNivel').hide();
    $('#btnEditNivel').hide();

    $('#btnSaveNivel').show();
    $('#btnCancelNivel').show();

    $('#Nivel').attr("disabled", false);

    $("#tituloModalNivel").text("Nuevo Nivel");
}

$('#btnEditionNivel').click(function () {
    $('#editionModeNivel').hide();
    $('#btnEditNivel').show();

    $('#Nivel').attr("disabled", false);

    $("#tituloModalNivel").text("Edicion del Nivel");
});