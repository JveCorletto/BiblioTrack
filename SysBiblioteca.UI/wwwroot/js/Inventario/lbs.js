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
                    loadGeneros();
                    loadEditoriales();
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
    $('#dataLibro').trigger('reset');
    $('#editionMode').hide();
    $('#btnEdit').hide();
    $('#btnActivar').hide();
    $('#btnDesactivar').hide();

    $('#btnSave').show();
    $('#btnCancel').show();
    $('#btnEdition').show();

    $('#Libro').attr("disabled", false);
    $('#Version').attr("disabled", false);
    $('#ISBN').attr("disabled", false);

    $("#Editorial").val('0').change();
    $('#Editorial').attr("disabled", false);
    $('#AnioPublicacion').attr("disabled", false);
    $('#Descripcion').attr("disabled", false);

    $("#Autor").val('0').change();
    $('#Autor').attr("disabled", false);
    $("#renderAutores").html(null);
    $('#btnAutor').attr("disabled", false);

    $("#GeneroLiterario").val('0').change();
    $('#GeneroLiterario').attr("disabled", false);
    $("#renderGeneros").html(null);
    $('#btnGeneroLiterario').attr("disabled", false);

    $("#miniaturaContainer").html(null);
    $("#FotoLibro").attr("disabled", false);

    localStorage.removeItem("Autores");
    localStorage.removeItem("Generos");
    localStorage.removeItem("base64Image");

    $("#tituloModal").text("Nuevo Libro");
}

$('#btnEdition').click(function () {
    $('#editionMode').hide();
    $('#btnEdit').show();

    $('#Libro').attr("disabled", false);
    $('#Version').attr("disabled", false);
    $('#ISBN').attr("disabled", false);

    $('#Editorial').attr("disabled", false);
    $('#AnioPublicacion').attr("disabled", false);

    $('#Descripcion').attr("disabled", false);

    $('#Autor').attr("disabled", false);
    $('#btnAutor').attr("disabled", false);

    $('#GeneroLiterario').attr("disabled", false);
    $('#btnGeneroLiterario').attr("disabled", false);

    $('#FotoLibro').attr("disabled", false);

    $("#tituloModal").html("Edici&oacute;n del Libro");
});