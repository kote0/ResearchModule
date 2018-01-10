var RM = function () { };


//поиск в input 
//для этого релизовать
//<div class="dropdown">
//    <input type="text" class="dropdown-toggle" id=".." data-toggle="dropdown" />
//</div>
//добавить setTimeout!
RM.SearchInput = function (id, url) {
    var elem = $('div.dropdown');
    $(elem).find("select").remove();
    if ($(id).val() === "") return;
    $.get(url + '?character=' + $(id).val(),
        function (data) {
            $(elem).append(data);
        });
};

//Добавление данных 
RM.AppendTo = function (url, setTo, isteadOff) {
    $.get(url, function (data) {
        if (!isteadOff) {
            $(setTo).append(data);
            return;
        }
        //Поставить данные вместо него из ссылки
        $(setTo).html(data);
    });
}

//Добавление выбранных елементов
RM.DropdownGroupItemClick = function (classDropdownGroupItem) {
    debugger;
    alert(5132);
};
RM.PreGetPostDataFuncs = new Array();

function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

RM.Submit = function (form) {
    $.ajax({
        url: $(form).attr('action'),
        type: "POST",
        data: {
            Data: JSON.stringify(getFormData($(form)))
        },
        success: function (data) {
        }
    });

}