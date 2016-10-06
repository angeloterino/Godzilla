$.strawmanVars = new Object();

$(document).ready(function () {
    $.strawmanVars.cancelLoad = false

    $(document).on("click", "a[role=menuitem][role=tabitem]", function () {
        $.strawmanVars.cancelLoad = true;
    });

    $.strawmanVars.ajaxCall = function (_path, _data, _type, _async) {
        if (!$.strawmanVars.cancelLoad) {
            $.ajax({
                url: _path,
                data: _data,
                async: _async != null ? true : _async,
                contentType: 'application/html; charset=utf-8',
                type: _type != null ? _type : 'GET',
                dataType: 'html'
            })
            .done(function (result) {
                return result;
            })
            .fail(function (xhr, status) {
                alert(status);
                return status;
            });
        }
    }
});

