function getMulta(IdMulta) {
    var Obj = {
        IdMulta: IdMulta,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Multas/GetUnpaidInvoce',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $('#staticFine').modal('show');

                $('#IdMulta').val(data.datos.idMulta);
                $('#Usuario').val(data.datos.usuario);
                $('#DiasRetraso').val(data.datos.idMulta);
                $('#dollars').val(String(data.datos.monto).split('.')[0]);
                $('#cents').html('.' + String(data.datos.monto).split('.')[1]);

                $('#Libro').val(data.datos.libro).attr("disabled", true);
                var img = $('<img>').attr('src', data.datos.fotoLibro);
                img.css('max-width', '100%');
                img.css('max-height', '100%');
                $('#miniaturaContainer').html(null);
                $('#miniaturaContainer').append(img);

                var comprobante = $('<img>').attr('src', data.datos.comprobantePago);
                comprobante.css('max-width', '35%');
                comprobante.css('max-height', '35%');
                $('#miniaturaComprobanteContainer').html(null);
                $('#miniaturaComprobanteContainer').append(comprobante);
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

$("#btnAprobar").click(function () {
    Swal.fire({
        title: 'Validacion',
        html: "&iquest;En verdad desea aprobar este pago de multa?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#ffc107',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Aprobar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdMulta: parseInt($('#IdMulta').val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Multas/ApproveInvoce',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 3000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            $('#btnCancel').click();
                            loadMultasPendientes();
                            loadMultasPagadas();
                        });
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
    });
});

$("#btnCobrar").click(function () {
    Swal.fire({
        title: 'Validacion',
        html: "&iquest;En verdad realizar el cobro f&iacute;ico de esta multa?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#ffc107',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Cobrar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdMulta: parseInt($('#IdMulta').val()),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Multas/PhysicalPayment',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 3000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            $('#btnCancel').click();
                            loadMultasPendientes();
                            loadMultasPagadas();
                        });
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
    });
});