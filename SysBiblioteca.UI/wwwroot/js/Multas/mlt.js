$(document).ready(function () {
    verificarBase();

    $("#FechaValidacion").datepicker({
        maxDate: '0',
        dateFormat: 'dd/mm/yy',
        selectOtherMonths: true,
        language: 'es',
    });
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

                    loadMActivos();
                    loadMInactivos();
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
    $('#dataMulta').trigger('reset');
    $('#editionMode').hide();
    $('#btnEdit').hide();

    $('#btnSave').show();
    $('#btnCancel').show();

    $('#CodigodeMulta').attr("disabled", false);
    $('#CodigodePrestamo').attr("disabled", false);
    $('#Estado').attr("disabled", false);

    $('#FechaValidacion').attr("disabled", false);

    $("#PagoFisico").val('0').change();
    $('#PagoFisico').attr("disabled", false);
}

$('#btnEdition').click(function () {
    $('#editionMode').hide();
    $('#btnEdit').show();

    $('#CodigodeMulta').attr("disabled", false);
    $('#CodigodePrestamo').attr("disabled", false);
    $('#Estado').attr("disabled", false);

    $('#FechaValidacion').attr("disabled", false);

    $('#PagoFisico').attr("disabled", false);
});