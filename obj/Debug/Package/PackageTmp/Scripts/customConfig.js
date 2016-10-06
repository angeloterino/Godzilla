//Actualización del comentario con éxito
$(document).ready(function () {
    $('#wrapper_main div.wrapper').css({ 'margin': '0' });
    $(document).on("change", "select", function () {
        ClearCommentMessage();
        EnableSubmitButton();
    });
});
$(document).on('change', 'select[id=selected_year_period]', function (m) {
    var $val = $(this).val();
    $.getJSON('Config/GetMonthList', { '_year': $val }).done(function (result) {
        var ddl = $('select[id=selected_month_period]');
        ddl.empty();
        $(result).each(function () {
            $(document.createElement('option'))
            .attr('value', this.Value)
            .text(this.Text)
            .appendTo(ddl);
        });
    });
});
UpdateSuccess = function (_message) {
    $('#group_selected').val(0);
    $('textarea').val('');
    if ($('.message')) {
        $('.message').html(_message).addClass('success');
        if ($('#message-gryphicon').attr('class') == null) {
            $('<div/>', {
                class: 'float-left success glyphicon glyphicon-ok',
                id: 'message-gryphicon'
            }).insertBefore('.message');
        } else {
            $('#message-gryphicon')
                    .removeClass('glyphicon-warning-sign')
                    .removeClass('glyphicon-ok')
                    .addClass('glyphicon-ok')
        }
    }
    EnableSubmitButton();

}
UpdateFailure = function (_message) {
    if ($('.message')) $('.message').html(_message).addClass('fail');
    $('<div/>', {
        class: 'message float-left success glyphicon glyphicon-warning-sign'
    }).insertBefore('.message');
    EnableSubmitButton();
}
ClearCommentMessage = function () {
    $('.message').html('');
    $('#message-gryphicon').remove();
}
OnSubmiting = function (_message) {
    $('input[type=submit]').prop('disabled',true);
}
EnableSubmitButton = function () {
    $('input[type=submit]').prop('disabled', false);
}
