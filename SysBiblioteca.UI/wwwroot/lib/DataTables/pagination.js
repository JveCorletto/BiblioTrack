function paginate(id) {
    return $('#' + id).DataTable({
        destroy: true,
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "Sin registros",
            "info": "Mostrando _PAGE_ de _PAGES_",
            "infoEmpty": "Sin registros que mostrar",
            "infoFiltered": "(Buscando en _MAX_ registros)",
            "thousands": ",",
            "decimal": ".",
            "search": "Buscar",
            "loadingRecords": "Cargando...",
            "processing": "Procesando...",
            "paginate": {
                "first": "Primero",
                "last": "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        }
    });
}

function recolor() {
    var elements = document.getElementsByClassName('thead-dark');
    for (var i = 0; i < elements.length; i++) {
        elements[i].style.cssText = 'color: white !important';
    }
}