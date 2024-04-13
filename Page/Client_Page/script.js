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

    var queryParams = [];
    queryParams.push('Rows=' + rows);
    queryParams.push('Columns=' + columns);

    selectedCells.forEach(function(cell, index) {
        queryParams.push('CutCells[' + index + '][row]=' + cell.row);
        queryParams.push('CutCells[' + index + '][column]=' + cell.column);
    });

    var queryString = queryParams.join('&');

    console.log("Data being sent to server:", queryString);

    $.ajax({
        url: 'https://localhost:44307/CalculateParts?' + queryString,
        type: 'GET',
        contentType: 'application/json',
        success: function(response) {
            $('#result').text('The grid will be partitioned into ' + response.partsCount + ' parts.');
        },
        error: function(xhr, textStatus, errorThrown) {
            $('#result').text('Error: ' + textStatus);
        }
    });
}
