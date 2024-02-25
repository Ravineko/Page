function sendData() {
    var rows = parseInt(document.getElementById('rows').value);
    var columns = parseInt(document.getElementById('columns').value);
    var gridContainer = document.getElementById('gridContainer');
    var cells = gridContainer.querySelectorAll('.cell');
    var selectedCells = [];

    cells.forEach(function(cell) {
        if (cell.classList.contains('cut')) {
            var cellIdParts = cell.id.split('_');
            selectedCells.push({
                row: parseInt(cellIdParts[1]),
                column: parseInt(cellIdParts[2])
            });
        }
    });

    var data = {
        rows: rows,
        columns: columns,
        cutCells: selectedCells
    };

    console.log("Data being sent to server:", data);

    $.ajax({
        url: 'https://localhost:44307/CalculateParts',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        success: function(response) {
            $('#result').text('The grid will be partitioned into ' + response.partsCount + ' parts.');
        },
        error: function(xhr, textStatus, errorThrown) {
            $('#result').text('Error: ' + textStatus);
        }
    });
}
