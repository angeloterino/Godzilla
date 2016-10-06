$(document).ready(function () {
    $(document).on('click', 'input[type=submit]', function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        $.each($(form).find('input[type=checkbox]'), function (i, e) {
            if (!$(this).attr('data-renamed')) {
                if (!$(this).attr('data-value'))
                    $(this).attr('data-value', 'BRAND:' + $(this).attr('data-brand') + '_MARKET:' + $(this).attr('data-market') + '_CHANNEL:' + $(this).attr('data-channel'));

                $(this).attr('name', $(this).attr('name') + "_" + $(this).attr('data-value'));
                var select = $(this).closest('tr').find('select');
                $(select).attr('name', $(select).attr('name') + "_" + $(this).attr('data-value'));
                $(this).attr('data-renamed', true);
            }
        });
        $(form).submit();
    });
    $('input[type=hidden][id=user]').val($('select[id=user]').find('option:selected').text());
    $(document).on('change', 'select[id=user]', function (e) {
        $('input[type=hidden][id=user]').val($(this).find('option:selected').text());
    });
});

var UpdateSuccess = function () {
};
var UpdateFailure = function () {
};