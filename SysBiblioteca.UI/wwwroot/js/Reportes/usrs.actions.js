$("#btnReportePrestamosActivos").click(function () {
    exportarExcel('tableActivos', 'Reporte_Usuarios_PrestamosActivos')
});

$("#btnReportePrestamosFinalizados").click(function () {
    exportarExcel('tableHistorial', 'Reporte_Usuarios_PrestamosFinalizados')
});