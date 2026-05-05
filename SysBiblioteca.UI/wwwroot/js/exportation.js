function exportarExcel(IdTable, fileName) {
    // Obtiene el contenido de la tabla
    var tablaContenido = document.getElementById(IdTable);

    // Crea un nuevo libro de trabajo de Excel
    var workbook = new ExcelJS.Workbook();
    var sheet = workbook.addWorksheet('Cuerpo del Reporte');

    // Agrega las filas y celdas al libro de trabajo
    var rows = tablaContenido.querySelectorAll('tr');
    rows.forEach((row, rowIndex) => {
        var rowData = [];
        row.querySelectorAll('th, td').forEach((cell, cellIndex) => {
            rowData.push(cell.innerText);

            // Aplica negrita a los encabezados (en la primera fila)
            if (rowIndex === 0) {
                sheet.getCell(rowIndex, cellIndex + 1).font = { bold: true };
            }
        });
        sheet.addRow(rowData);
        rowInserted = true;
    });

    // Crea un blob con el contenido del libro de trabajo
    workbook.xlsx.writeBuffer()
        .then(buffer => {
            var blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });

            // Crea un objeto URL para el blob
            var url = URL.createObjectURL(blob);

            // Crea un enlace y haz clic en él para descargar el archivo
            var a = document.createElement('a');
            a.href = url;
            a.download = fileName + '.xlsx';
            document.body.appendChild(a);
            a.click();

            // Limpia el objeto URL y el enlace
            document.body.removeChild(a);
            URL.revokeObjectURL(url);
        })
        .catch(error => {
            Swal.fire({
                title: 'Error',
                icon: "error",
                html: 'Ocurri&oacute; un error y el reporte no pudo ser generado.',
                timer: 3000,
                timerProgressBar: true,
                didOpen: () => {
                    Swal.showLoading();
                },
            });;
        });
}