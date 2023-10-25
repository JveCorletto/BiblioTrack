function paginate(id, settings, cols) {
    if (settings == undefined) settings = new Object();
    if (settings.ordering == undefined) settings.ordering = true;
    if (settings.columnDefs == undefined) settings.columnDefs = [];
    return $('#' + id).DataTable({
        destroy: true,
        ordering: settings.ordering,
        columnDefs: settings.columnDefs,
        dom: 'Bfrtip',
        buttons: [
            'copy', 'csv', 'excel', 'pdf', 'print'
        ],
        "lengthMenu": [15, 25, 50],
        "pageLength": 15,
        "language": {
            "lengthMenu": "Mostrar _MENU_ registros por página",
            "zeroRecords": "Sin registros",
            "info": "Mostrando _PAGE_ de _PAGES_",
            "infoEmpty": "Sin registros que mostrar",
            "infoFiltered": "(Buscando en _MAX_ registros)",
            "search": "Buscar",
            "paginate": {
                "previous": "Anterior",
                "next": "Siguiente"
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