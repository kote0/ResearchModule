Publication = {
    volumeShow: true // показывается ли объем
};

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
    return countItem;
}


// обработка перед submit
function formSubmit(formId) {
    debugger;
    if (!isValid()) {
        get_Info("Заполните обязательные поля");
        return;
    }
    var searchResult = $("#searchResult").find("[type=hidden], input:checked");
    var createResult = $("#createResult").find("input");
    let c = convertName(searchResult) + convertName(createResult);
    if (c === -2) {
        get_Info("Отстутствуют авторы");
        return;
    }
    $("#" + formId).submit();
}

function get_Info(text) {
    //$(".info").html(`<h3 style='margin-top:0;'><span class='label label-danger'>${text}</span></h3>`);
    $(".info").html(`<div class="alert alert-danger" role="alert">${text}</div>`);
}


function isValid() {
    let res = true;
    var reqInput = $("[data-val='True']");
    for (var input of reqInput) {
        let parent = $(input).parent();
        if (input.value === "") {
            parent.addClass("has-error")
            res = false;
        }
        else {
            parent.removeClass("has-error")
        }
    }
    return res;
}




function clickIsTarslate(e) {
    Requied("#Publication_TranslateText", e.checked);
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
                            <input class="form-control " data-val="True" id="PublicationType_Name" name="PublicationType.Name" placeholder="Название вида" type="text" value="">
                            <span class="input-group-btn">
                                <button class="btn btn-default" id="Delete" onclick="PublicationTypes.remove()" type="button"><span class="glyphicon glyphicon-remove"></span></button>
                            </span>
                        </div>
                </div>`;
            $(".CreateTypePublication").html(val);
            disable(`[name='Publication.${name}']`, true);

        },
        remove: function () {
            $(".CreateTypePublication").empty();
            disable(`[name='Publication.${name}']`, false);
        }
    };
}();


var timers = timers || [];
timers[0] = 0;
Author = function () {
    var resultauthorJson = [];
    var countAuthor = 0;
    let c = 0;
    let searchRes = "_SearchResult_SearchAuthorsModal";
    let select = "Selected";
    let add = "Additional";
    return {
        append: function (url) {
            $.ajax({
                type: 'POST',
                url: url + '?id=' + countAuthor,
                success: function (data) {
                    countAuthor++;
                    $("#createResult").append(data);
                    showWeight(Publication.volumeShow);
                }
            });
        },
        remove: function (cardName) {
            let cardId = "[id$='" + cardName + "']";
            $(cardId).detach();
        },
        search: function (elem) {
            return {
                onKeyUp: function () {
                    if (elem.value === "") return;
                    clearTimeout(timers[0]);
                    Author.search(elem).start();
                },
                start: function () {
                    timers[0] = setTimeout(function () { Author.search(elem).onChange() }, 500);
                },
                onChange: function () {
                    $.ajax({
                        type: 'POST',
                        url: '/Author/Search?character=' + elem.value,
                        success: function (data) {
                            $("#" + add + searchRes).html(data);
                        }
                    });
                }
            }
        },
        searchResult: function () {
            return {
                append: function (elem, id) {
                    let selector = "#AuthorItem_" + id;
                    if (elem.checked) {
                        let tr = $(selector).clone();
                        $("#" + select + searchRes).append(tr);
                    }
                    else {
                        $(`[name='${elem.name}']`).attr("checked", false);
                        $(`#${select+searchRes} ${selector}`).detach();
                        // данные полученные при поиске
                        $(`#searchResult ${selector}`).detach();
                        // исходные данные при наличии модели
                        $(`#getResult ${selector}`).detach();
                    }
                },
                serialize: function () {
                    var searchResult = $("#searchResult").html($(`#${select + searchRes} tr`).clone());
                    let weight = "<input class='form-control' id='Author_00__Weight' data-val='True' name='Author[00].Weight' placeHolder='Вес' type='text' value='' />";
                    searchResult.find("tr:has(td)").append(`<td>${weight}</td>`);
                    showWeight(Publication.volumeShow);
                }
            }
        }
    };
}();



function showWeight(show) {
    let allWeigth = "[id$='Weight']";
    let title = "#WeightTitle";
    Requied(allWeigth, show);
    if (show) {
        $(allWeigth).show();
        $(title).show();
        return;
    }
    $(allWeigth).hide();
    $(title).hide();
}

function showVolume(show) {
    let div = "#VolumeCreate";
    let name = "#Publication_Volume";
    Publication.volumeShow = show;
    if (show) {
        // показать
        $(div).show();
        Requied(name, true);
        showWeight(true);
        return;
    }
    // скрыть
    $(div).hide()
    Requied(name, false);
    showWeight(false);
}

changePublicationForm = function (elem) {
    // формы публикации
    const electronicForm = "2";
    const audioForm = "3";

    var select = $(elem.target).find(":selected");
    if (select.length === 0) {
        return; // ничего не выбрано
    }
    var id = select.attr("value");
    showVolume(id === electronicForm || id === audioForm ? false: true );
}

Requied = function (elem, required) {
    $(elem).attr("data-val", required ? "True" : "False");
}
HasRequered = function (elem){
    return $(elem).attr("data-val");
}

