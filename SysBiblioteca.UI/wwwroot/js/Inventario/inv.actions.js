//--------------- SECCIONES --------------------------
$('#btnSaveSeccion').click(function () {
    if ($('#Seccion').val().trim() != "") {
        var Obj = {
            Seccion: $('#Seccion').val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Inventario/CreateSeccion',
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
                        $('#btnCancelSeccion').click();
                        loadSecciones();
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
            html: 'Para continuar, debe de rellenar todos los campos.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnEditSeccion').click(function () {
    if ($('#Seccion').val().trim() != "") {
        var Obj = {
            IdSeccion: parseInt($('#IdSeccion').val()),
            Seccion: $('#Seccion').val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Inventario/UpdateSeccion',
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
                        $('#btnCancelSeccion').click();
                        loadSecciones();
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
            html: 'Para continuar, debe de rellenar los campos requeridos.',
            timer: 5000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getSeccion(IdSeccion, action) {
    var Obj = {
        IdSeccion: IdSeccion,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Inventario/GetSeccion',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                if (action) {
                    resetFormSeccion();

                    $("#tituloModalSeccion").text("Datos de la Seccion");

                    $('#IdSeccion').val(data.datos.idSeccion);
                    $('#Seccion').val(data.datos.seccion).attr("disabled", true);

                    $('#btnSaveSeccion').hide();
                    $('#editionModeSeccion').show();
                }
                else {
                    $('#IdSeccionEstanteria').val(data.datos.idSeccion).attr("disabled", true);
                    $("#nombreSeccion").html("Estanter&iacute;as de la Secci&oacute;n: " + data.datos.seccion);
                    loadEstanterias(data.datos.idSeccion);
                }
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
                    if (action) {
                        $('#closeSeccionModal').click();
                    }
                    else {
                        $('#btnCancelEstanterias').click();
                    }
                });
            }
        }
    });
}

function deleteSeccion(IdSeccion) {
    Swal.fire({
        title: 'Validacion',
        html: '&iquest;En verdad desea eliminar esta Secci&oacute;n?',
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Eliminar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdSeccion: IdSeccion,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Inventario/DeleteSeccion',
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
                            $('#btnCancelSeccion').click();
                            loadSecciones();
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
    })
}


//--------------- ESTANTERIAS ------------------------
$('#btnSaveEstanteria').click(function () {
    if ($('#Estanteria').val().trim() != "") {
        var Obj = {
            IdSeccion: parseInt($('#IdSeccionEstanteria').val().trim()),
            Estanteria: $('#Estanteria').val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Inventario/CreateEstanteria',
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
                        loadEstanterias(parseInt($('#IdSeccionEstanteria').val().trim()));
                        $('#btnCancelEstanteria').click();
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
            html: 'Para continuar, debe de rellenar todos los campos.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnEditEstanteria').click(function () {
    if ($('#Estanteria').val().trim() != "") {
        var Obj = {
            IdSeccion: parseInt($('#IdSeccionEstanteria').val().trim()),
            IdEstanteria: parseInt($('#IdEstanteria').val().trim()),
            Estanteria: $('#Estanteria').val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Inventario/UpdateEstanteria',
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
                        loadEstanterias(parseInt($('#IdSeccionEstanteria').val().trim()));
                        $('#btnCancelEstanteria').click();
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
            html: 'Para continuar, debe de rellenar los campos requeridos.',
            timer: 5000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getEstanteria(IdEstanteria, action) {
    var Obj = {
        IdEstanteria: IdEstanteria,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Inventario/GetEstanteria',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                if (action) {
                    resetFormEstanterias();

                    $("#tituloModalEstanteria").text("Datos de la Estanter&iacute;a");

                    $('#IdEstanteria').val(data.datos.idEstanteria);
                    $('#Estanteria').val(data.datos.estanteria).attr("disabled", true);

                    $('#btnSaveEstanteria').hide();
                    $('#editionModeEstanteria').show();
                }
                else {
                    $('#IdNivelEstanteria').val(data.datos.idEstanteria).attr("disabled", true);
                    $("#nombreEstanteria").html("Niveles de la Estanter&iacute;a #: " + data.datos.estanteria);
                    loadNiveles(data.datos.idEstanteria);
                }
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
                    if (action) {
                        $('#closeModalEstanteria').click();
                    }
                    else {
                        $('#btnCancelEstanteria').click();
                    }
                });
            }
        }
    });
}

function deleteEstanteria(IdEstanteria) {
    Swal.fire({
        title: 'Validacion',
        html: '&iquest;En verdad desea eliminar esta Estanter&iacute;a?',
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Eliminar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdEstanteria: IdEstanteria,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Inventario/DeleteEstanteria',
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
                            loadEstanterias(parseInt($('#IdSeccionEstanteria').val().trim()));
                            $('#btnCancelEstanteria').click();
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
    })
}


//--------------- NIVELES -----------------------------
$('#btnSaveNivel').click(function () {
    if ($('#Nivel').val().trim() != "") {
        var Obj = {
            IdEstanteria: parseInt($('#IdNivelEstanteria').val().trim()),
            Nivel: $('#Nivel').val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Inventario/CreateNivel',
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
                        loadNiveles(parseInt($('#IdNivelEstanteria').val().trim()));
                        $('#btnCancelNivel').click();
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
            html: 'Para continuar, debe de rellenar todos los campos.',
            timer: 1000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

$('#btnEditNivel').click(function () {
    if ($('#Nivel').val().trim() != "") {
        var Obj = {
            IdNivel: parseInt($('#IdNivel').val().trim()),
            IdEstanteria: parseInt($('#IdNivelEstanteria').val().trim()),
            Nivel: $('#Nivel').val().trim(),

            Token: localStorage.getItem("UserToken"),
            ActualRute: window.location.hash.replace('#', '')
        };
        var api = localStorage.getItem('apiURL');

        $.ajax({
            type: 'POST',
            url: api + 'Inventario/UpdateNivel',
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
                        loadNiveles(parseInt($('#IdNivelEstanteria').val().trim()));
                        $('#btnCancelNivel').click();
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
            html: 'Para continuar, debe de rellenar los campos requeridos.',
            timer: 5000,
            timerProgressBar: true,
            didOpen: () => {
                Swal.showLoading();
            },
        });
    }
});

function getNivel(IdNivel) {
    var Obj = {
        IdNivel: IdNivel,

        Token: localStorage.getItem("UserToken"),
        ActualRute: window.location.hash.replace('#', '')
    };
    var api = localStorage.getItem('apiURL');

    $.ajax({
        type: 'POST',
        url: api + 'Inventario/GetNivel',
        contentType: "Application/json",
        data: JSON.stringify(Obj),
        success: function (data) {
            if (data.resultado == 1) {
                resetFormNiveles();

                $("#tituloModalNivel").text("Datos del Nivel");

                $('#IdNivel').val(data.datos.idNivel);
                $('#Nivel').val(data.datos.nivel).attr("disabled", true);

                $('#btnSaveNivel').hide();
                $('#editionModeNivel').show();
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
                    if (action) {
                        $('#closeModalNivel').click();
                    }
                    else {
                        $('#btnCancelNivel').click();
                    }
                });
            }
        }
    });
}

function deleteNivel(IdNivel) {
    Swal.fire({
        title: 'Validacion',
        html: '&iquest;En verdad desea eliminar este Nivel?',
        icon: 'warning',
        showCancelButton: true,

        cancelButtonColor: '#181C32',
        confirmButtonColor: '#FF3D60',

        cancelButtonText: "Cancelar",
        confirmButtonText: "Eliminar",
    }).then((result) => {
        if (result.isConfirmed) {
            var Obj = {
                IdNivel: IdNivel,

                Token: localStorage.getItem("UserToken"),
                ActualRute: window.location.hash.replace('#', '')
            };
            var api = localStorage.getItem('apiURL');

            $.ajax({
                type: 'POST',
                url: api + 'Inventario/DeleteNivel',
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
                            loadNiveles(parseInt($('#IdNivelEstanteria').val().trim()));
                            $('#btnCancelNivel').click();
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
    })
}