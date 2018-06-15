var site = function () { };


site.tableToJson = function (table) {
    var data = [];
    var headers = [];

    for (let i = 0; i < table.rows[0].cells.length; i++) {
        headers[i] = table.rows[0].cells[i].getAttribute("data_id"); // TODO: id изменить на data_id
    }
    for (let i = 1; i < table.rows.length; i++) {
        var tableRow = table.rows[i]; var rowData = {}; let d;
        for (var j = 0; j < tableRow.cells.length; j++) {
            d = tableRow.cells[j].childNodes[0];
            if (d === undefined) continue;
            if (d.nodeValue !== null) {
                rowData[headers[j]] = d.nodeValue;
            }
            else {
                rowData[headers[j]] = d.value;
            }
        } data.push(rowData);
    }
    return data;
};

function disable(selctor, disabled) {
    $(selctor).attr("disabled", disabled);
}
function empty(selector) {
    $(selector).empty();
}

$(function () {

    // We can attach the `fileselect` event to all file inputs on the page
    $(document).on('change', ':file', function () {
        //debugger;
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        input.trigger('fileselect', [numFiles, label]);
    });

    // We can watch for our custom `fileselect` event like this
    $(document).ready(function () {
        //debugger;
        $(':file').on('fileselect', function (event, numFiles, label) {

            var input = $(this).parents('.input-group').find(':text'),
                log = numFiles > 1 ? numFiles + ' files selected' : label;

            if (input.length) {
                input.val(log);
            } else {
                if (log) alert(log);
            }

        });
    });

});


class Search {
    constructor(url, resultFunc) {
        this.timers = this.timers || [];
        this.timers[0] = 0;
        this.url = url;
        if (typeof(resultFunc) === 'function')
            this.setResult = resultFunc;
        else {
            throw `Неверный входной параметр. ${resultFunc} is not a function`;
        }
    }
    onKeyUp(elem) {
        if (this.input == null) 
            this.input = elem;
        if (this.input.value === "") return;
        clearTimeout(this.timers[0]);
        this.start();
    }

    onChange() {
        let res = this.setResult;
        $.ajax({
            type: 'POST',
            url: this.url + '?character=' + this.input.value,
            success: function (data) {
                res(data);
            }
        });
    }
    start() {
        this.timers[0] = setTimeout(this.onChange(), 500);
    }
}