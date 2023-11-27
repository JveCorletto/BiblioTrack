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