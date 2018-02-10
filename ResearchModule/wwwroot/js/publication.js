//конвертирование листа на корректные номера
function convertName(list) {
    let countItem = -1;
    let id;
    for (var item of list) {
        let name = item.name.match(/\d/g)[0];
        if (id !== name) {
            id = name;
            countItem++;
        }
        $("[name='" + item.name + "']").attr("name", item.name.replace(/\d/g, countItem));

    }
}

// обработка перед submit
function formSubmit(formId) {
    debugger;
    var searchResult = $("#searchResult").find("[type=hidden], input:checked");
    var createResult = $("#createResult").find("input");
    convertName(searchResult);
    convertName(createResult);
    $("#" + formId).submit();
}

function clickIsTarslate(e) {
    $("#translate_Publication").css("display", e.checked ? "" : "none");
}

PublicationTypes = function () {
    let name = "PublicationType";
    return {
        create: function () {
            let val =
                `<div class="col-md-4">Название вида</div>
                    <div class="col-md-8">
                        <div class="input-group">
                            <input class="form-control " id="PublicationType_Name" name="PublicationType.Name" placeholder="Название вида" type="text" value="">
                            <span class="input-group-btn">
                                <button class="btn btn-default" id="Delete" onclick="PublicationTypes.remove()" type="button"><span class="glyphicon glyphicon-remove"></span></button>
                            </span>
                        </div>
                </div>`;
            $(".CreateTypePublication").html(val);
            disable("[name='Publication." + name + "']", true);

        },
        remove: function () {
            $(".CreateTypePublication").empty();
            disable("[name='Publication." + name + "']", false);
        }
    };
}();