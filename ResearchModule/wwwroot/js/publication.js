Publication = {
    volumeShow: true // показывается ли объем
};

//конвертирование листа на корректные номера
function convertName(list) {
    let oldName = '';
    let countItem = -1;
    let id;
    for (var item of list) {

        let name = item.name.match(/\d/g)[0];
        if (oldName != item.name && id !== name) {
            id = name;
            countItem++;
        }
        $("[name='" + item.name + "']").attr("name", item.name.replace(/\d/g, countItem));
        oldName = item.name;
    }
    return countItem;
}


// обработка перед submit
function formSubmit(formId) {
    debugger;
    if (!isValid(formId)) {
        setInfo("Заполните обязательные поля");
        return;
    }
    var searchResult = $("#searchResult").find("input, [type=hidden]");
    var createResult = $("#createResult").find("input");
    var getResult = $("#getResult").find("input, [type=hidden]");
    let c = convertName(searchResult) + convertName(createResult) + convertName(getResult);
    if (c === -3) {
        setInfo("Отстутствуют авторы");
        return;
    }
    $("#" + formId).submit();
}

function setInfo(text) {
    //$(".info").html(`<h3 style='margin-top:0;'><span class='label label-danger'>${text}</span></h3>`);
    $(".info").html(`<div class="alert alert-danger" role="alert">${text}</div>`);
}


function isValid(formId) {
    let res = true;
    var reqInput = $(`#${formId} [data-val='True']`);
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
                    let addResId = "#Additional_SearchResult_SearchAuthorsModal";
                    $.ajax({
                        type: 'POST',
                        url: '/Author/Search?character=' + elem.value,
                        success: function (data) {
                            $(addResId).html(data);
                        }
                    });
                }
            }
        },
        searchResult: function () {
            let searchResId = "#Selected_SearchResult_SearchAuthorsModal";
            return {
                append: function (elem, id) {
                    debugger;
                    let selector = "#AuthorItem_" + id;
                    if (elem.checked) {
                        let tr = $(selector).clone();
                        $(searchResId).append(tr);
                    }
                    else {
                        $(`[name='${elem.name}']`).attr("checked", false);
                        $(`${searchResId} ${selector}`).detach();
                        // данные полученные при поиске
                        $(`#searchResult ${selector}`).detach();
                        // исходные данные при наличии модели
                        $(`#getResult ${selector}`).detach();
                    }
                },
                serialize: function () {
                    debugger;
                    var searchResult = $("#searchResult").html($(`${searchResId} tr`).clone());
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
    showVolume(id === electronicForm || id === audioForm ? false : true);
}

Requied = function (elem, required) {
    $(elem).attr("data-val", required ? "True" : "False");
}
HasRequered = function (elem) {
    return $(elem).attr("data-val");
}



$(function () {

    $("select#Publication_PublicationForm").on('change', { elem: this }, changePublicationForm);

    let selectListLanguage = new language("Publication.text");
    $("#languages").html(selectListLanguage.items);
});