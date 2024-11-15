function loadEjemplares() {
    var IdLibro = $("#IdLibro").val();

    var Obj = {
        IdLibro: IdLibro,
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Ejemplares/GetEjemplares',
        contentType: "application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                $("#ejemplaresList").html('');
                $("#qrCodeContainer").html(null);

                var html = '';
                $.each(data.datos, function (index, ejemplar) {
                    html += `
                        <a class="list-group-item list-group-item-action d-flex justify-content-between align-items-center" aria-current="true" onclick="generateQR(${ejemplar.idEjemplar});">
                            ${ejemplar.codigoEjemplar}
                            <span class="badge bg-primary rounded-pill"><i class="fas fa-chevron-right text-white"></i></span>
                        </a>
                    `;
                });

                $("#ejemplaresList").html(html);
            } else {
                Swal.fire({
                    title: 'Información',
                    icon: "Info",
                    html: data.mensaje,
                    timer: 3000,
                    timerProgressBar: true,
                    didOpen: () => {
                        Swal.showLoading();
                    },
                }).then(function () {
                    $("#ejemplaresList").html('');
                    $("#qrCodeContainer").html(null);
                });
            }
        }
    });
}

function generateQR(IdEjemplar) {
    var Obj = {
        IdEjemplar: IdEjemplar,
        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: `${api}Ejemplares/GetQREjemplar`,
        contentType: "application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                const qrCodeBase64 = data.datos.qrCodeBase64;

                // Generar el HTML con el código QR y el botón de borrar, ambos dentro de un contenedor común
                var html = `
                    <div class="text-center">
                        <img src="data:image/png;base64,${qrCodeBase64}" alt="Código QR" class="img-fluid mb-3" width="50%" /><br/>
                        <button onclick="borrarEjemplar(${data.datos.idEjemplar});" type="button" class="btn btn-danger font-weight-bold">
                            <i class="fas fa-times"></i> Borrar
                        </button>
                    </div>
                `;
                $("#qrCodeContainer").html(html);
            } else {
                mostrarError(data.mensaje);
            }
        },
        error: function () {
            mostrarError("Ocurrió un error al comunicarse con el servidor.");
        }
    });
}

function mostrarError(mensaje) {
    Swal.fire({
        title: 'Error',
        icon: "warning",
        html: mensaje,
        timer: 1000,
        timerProgressBar: true,
        didOpen: () => {
            Swal.showLoading();
        },
    });
}

function addEjemplar() {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea registrar un nuevo ejemplar de este libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#0bb7af',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Agregar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdLibro: $("#IdLibro").val(),

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Ejemplares/CreateEjemplar',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            loadEjemplares();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    });
}

function borrarEjemplar(IdEjemplar) {
    Swal.fire({
        title: 'Validacion',
        text: "¿En verdad desea elimnar este ejemplar del libro?",
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Eliminar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdEjemplar: IdEjemplar,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Ejemplares/DeactivateEjemplar',
                contentType: "Application/json",
                data: JSON.stringify(Obj),
                success: function (data) {
                    if (data.resultado == 1) {
                        Swal.fire({
                            title: 'Exito',
                            icon: "success",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        }).then(function () {
                            loadEjemplares();
                        });
                    }
                    else {
                        Swal.fire({
                            title: 'Error',
                            icon: "warning",
                            html: data.mensaje,
                            timer: 2000,
                            timerProgressBar: true,
                            didOpen: () => {
                                Swal.showLoading();
                            },
                        });
                    }
                }
            });
        }
    });
}