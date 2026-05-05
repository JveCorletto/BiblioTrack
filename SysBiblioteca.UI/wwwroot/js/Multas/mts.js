$(document).ready(function () {
    verificarBase();
});

function validateVista() {
    var pkg = {
        Token: localStorage.getItem("UserToken"),
        URL: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Authentication/GetMenu',
        contentType: "Application/json",
        data: JSON.stringify(pkg),
        success: function (data) {
            if (data.resultado != null) {
                if (data.resultado == 1) {
                    loadMultasPendientes();
                    loadMultasPagadas();
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
    $("#miniaturaContainer").html(null);
    $("#miniaturaComprobanteContainer").html(null);
    sessionStorage.removeItem("ComprobantePago");
}