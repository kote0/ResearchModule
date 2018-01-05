var Site = function () { };

Site.select = function () {
    return {
        //входной параметр  $('.selectpicker > :selected');
        //return массив
        serialize: function (selectedList) {
            debugger;
            let selected = selectedList;
            let selectedCount = selected.length;
            if (selectedCount == 0) { return; }
            let result = [], SearchInResult;
            for (let i = 0; i < selectedCount; i++) {
                SearchInResult = result.findIndex(o => o === selected[i].value);
                if (SearchInResult == -1) {
                    result.push(selected[i].value);
                }
            }
            return result;
        }
    };
}();