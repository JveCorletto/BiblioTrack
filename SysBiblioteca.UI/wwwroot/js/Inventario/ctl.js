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
                    loadAutores();
                    loadEditoriales();
                    loadGenerosLiterarios();
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

function resetFormAutores() {
    $('#dataAutores').trigger('reset');
    $('#editionModeAutores').hide();
    $('#btnEditAutores').hide();

    $('#btnSaveAutores').show();
    $('#btnCancelAutores').show();

    $('#Autor').attr("disabled", false);

    $("#tituloModalAutores").text("Nuevo Autor");
}

$('#btnEditionAutores').click(function () {
    $('#btnEditionAutores').hide();
    $('#btnEditAutores').show();

    $('#Autor').attr("disabled", false);

    $("#tituloModalAutores").html("Edici&oacute;n del Autor");
});

function resetFormEditoriales() {
    $('#dataEditoriales').trigger('reset');
    $('#editionModeEditoriales').hide();
    $('#btnEditEditoriales').hide();

    $('#btnSaveEditoriales').show();
    $('#btnCancelEditoriales').show();

    $('#Editorial').attr("disabled", false);

    $("#tituloModalEditoriales").text("Nueva Editorial");
}

$('#btnEditionEditoriales').click(function () {
    $('#btnEditionEditoriales').hide();
    $('#btnEditEditoriales').show();

    $('#Editorial').attr("disabled", false);

    $("#tituloModalEditoriales").html("Edición de la Editorial");
});

function resetFormGenerosLiterarios() {
    $('#dataGenerosLiterarios').trigger('reset');
    $('#editionModeGenerosLiterarios').hide();
    $('#btnEditGenerosLiterarios').hide();

    $('#btnSaveGenerosLiterarios').show();
    $('#btnCancelGenerosLiterarios').show();

    $('#Genero').attr("disabled", false);

    $("#tituloModalGenerosLiterarios").text("Nuevo Género Literario");
}

$('#btnEditionGenerosLiterarios').click(function () {
    $('#btnEditionGenerosLiterarios').hide();
    $('#btnEditGenerosLiterarios').show();

    $('#Genero').attr("disabled", false);

    $("#tituloModalGenerosLiterarios").html("Edición del Género Literario");
});