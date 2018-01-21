var site = function () { };

site.tableToJson = function(table) { 
    var data = [];
    var headers = [];

    for (var i = 0; i < table.rows[0].cells.length; i++) {
        headers[i] = table.rows[0].cells[i].getAttribute("data_id"); // TODO: id изменить на data_id
    }
    for (var i = 1; i < table.rows.length; i++) {
        var tableRow = table.rows[i]; var rowData = {}; let d;
        for (var j = 0; j < tableRow.cells.length; j++) {
            d = tableRow.cells[j].childNodes[0];
            if (d == undefined) continue;
            if (d.nodeValue != null) {
                rowData[headers[j]] = d.nodeValue;
            }
            else {
                rowData[headers[j]] = d.value;
            }
        } data.push(rowData);
    }
    return data;
}

function disable(selctor, disabled) {
    $(selctor).attr("disabled", disabled);
}
function empty(selector) {
    $(selector).empty();
}