$(document).ready(function () {
    $(document).on('click', 'button[btn-type=btn-change-chanel]', function (object) {
        var _target = $('#list-groups');
        var _url = 'ItemsConfig/' + $(this).attr("btn-target");
        var _params = $(this).attr("btn-variable");
        $('.btn-success').addClass('btn-default').removeClass('btn-success');
        $(this).addClass('btn-success').removeClass('btn-default');
        $.ajax({
            url: _url,
            data: { "_channel": _params },
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {

            $(_target).html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();

        })
        .fail(function (xhr, status) {
            alert(status);
        });
    });
});