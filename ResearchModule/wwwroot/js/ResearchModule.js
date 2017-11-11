var RM = function () { };


//поиск в input 
//для этого релизовать
//<div class="dropdown">
//    <input type="text" class="dropdown-toggle" id=".." data-toggle="dropdown" />
//</div>
//добавить setTimeout!
RM.SearchInput = function (id, url) {
    var elem = $('div.dropdown:has(#' + id + ')');
        $(elem).find("ul").remove();
        if ($('#' + id).val() === "") return;
        $.get(url + '?character=' + $('#' + id).val(),
            function (data) {
                var list = data;
                if (list === null)
                    list = "<ul class='dropdown-menu'><li class='dropdown-group-item'>Нет данных для отоборажения</li></ul>";
                $(elem).append(list);
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
RM.GetPostData = function (selector) {
    var postData = null;
        if (!postData) {
            postData = {};
        }
        for (var j = 0; j < RM.PreGetPostDataFuncs.length; j++) {
            if (!RM.PreGetPostDataFuncs[j].selector || RM.PreGetPostDataFuncs[j].selector === selector)
                RM.PreGetPostDataFuncs[j].func.call();
        }
        var dataInputs = $(selector).find("input:not(:disabled),textarea:not(:disabled),select:not(:disabled)");
        var checkboxes = {};
        for (var j = 0; j < dataInputs.length; j++) {
            if ($(dataInputs[j]).is(':checkbox')) {
                var chName = dataInputs[j].name;
                checkboxes[chName] = dataInputs[j];
            }
        }
        for (var i = 0; i < dataInputs.length; i++) {
            if ($(dataInputs[i]).attr('notposted') || $(dataInputs[i]).hasClass('autocompliteValidateField'))
                continue;
            var item = {};
            var name = dataInputs[i].name;
            if (!$(dataInputs[i]).is(':checkbox') && checkboxes[name])
                continue;

            item[name] = RM.GetInputValue($(dataInputs[i]));
            postData = jQuery.extend(true, postData, item);
        }
        return postData;
}

RM.GetInputValue = function (input) {
    if (input.is(':checkbox'))
        return input.is(':checked');
    if (input.is('[type=radio]'))
        return $('input[name="' + input.attr('name') + '"]:checked').val();
    if (input.is('textarea') && typeof tinymce == "object") {
        var id = input.attr('id');
        for (var i = 0; i < tinymce.editors.length; i++) {
            if (tinymce.editors[i].id == id) {
                tinymce.editors[i].editorManager.triggerSave();
                break;
            }
        }
    }
    return input.val();
};

RM.Submit = function (form) {
    debugger;
    $.ajax({
        url: $(form).attr('action'),
        type: "POST",
        data: {
            section: RM.GetPostData("#accordion3"),
            publication: RM.GetPostData("#accordion"),
            typePublication: RM.GetPostData("#accordion0"),
            author: RM.GetPostData("#accordion1"),
            formWork: RM.GetPostData("#accordion2")
        },
        success: function (data) {
            alert(11111111111111111);
        }
    });
    
}