$('#ComprobantePago').change(function () {
    var input = this;

    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            var ComprobantePago = e.target.result;
            sessionStorage.setItem("ComprobantePago", ComprobantePago);
        };

        reader.readAsDataURL(input.files[0]);
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Por favor, seleccione un archivo v&acute;lido para continuar',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$("#btnCargar").click(function () {
    if ($('#ComprobantePago').val().trim() != "") {
        var Obj = {
            IdMulta: parseInt($("#IdMulta").val()),
            ComprobantePago: String(sessionStorage.getItem("ComprobantePago")),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Multas/UploadInvoce',
            contentType: "Application/json",
            data: JSON.stringify(Obj),
            success: function (data) {
                if (data.resultado == 1) {
                    Swal.fire({
                        title: 'Exito',
                        icon: "success",
                        html: data.mensaje,
                        timer: 5000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    }).then(function () {
                        $('#btnCancel').click();
                        loadMultasPendientes();
                    });
                }
                else {
                    Swal.fire({
                        title: 'Error',
                        icon: "warning",
                        html: data.mensaje,
                        timer: 5000,
                        timerProgressBar: true,
                        didOpen: () => {
                            Swal.showLoading();
                        },
                    });
                }
            },
            error: function (data) {
                Swal.fire({
                    title: 'Error',
                    icon: "warning",
                    html: "Ocurrió un error, intente nuevamente",
                    timer: 5000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                });
            }
        });
    }
    else {
        Swal.fire({
            title: 'Validacion',
            icon: "warning",
            html: 'Para continuar, debe cargar el comprobante de la transferencia',
            timer: 2500,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getMulta(IdMulta) {
    var Obj = {
        IdMulta: IdMulta,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetInvoce',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $('#staticFine').modal('show');

                $('#IdMulta').val(data.datos.idMulta);
                $('#DiasRetraso').val(data.datos.idMulta);
                $('#dollars').val(String(data.datos.monto).split('.')[0]);
                $('#cents').html('.' + String(data.datos.monto).split('.')[1]);

                $('#Libro').val(data.datos.libro).attr("disabled", true);
                var img = $('<img>').attr('src', data.datos.fotoLibro);
                img.css('max-width', '100%');
                img.css('max-height', '100%');
                $('#miniaturaContainer').html('');
                $('#miniaturaContainer').append(img);
            }
            else {
                Swal.fire({
                    title: 'Error',
                    icon: "warning",
                    html: data.mensaje,
                    timer: 1000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $('#btnCancel').click();
                });
            }
        }
    });
}