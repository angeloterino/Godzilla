var newComment = function (market, brand, channel, button) {
    var _market = market;
    var _brand = brand;
    var _channel = channel;
    var _body = $('.modal-body');
    var _wrapper = $(button).parent();
    var _save = $('<button>').attr({ "class": "btn btn-success", "data-type": "save_comment" }).html("Save Comment");
    var _cancel = $('<button>').attr({ "class": "btn btn-danger", "data-type": "cancel_comment" }).html("Cancel");
    var _name = $(button).attr('data-name');
    $(_wrapper).append(_save);
    $(_wrapper).append(_cancel);
    $(button).hide();
    $(_wrapper).on('click', 'button[data-type=cancel_comment]', function () {
        $(button).show();
        $('form').remove();
        $(this).parent().find('button[data-type=save_comment]').remove();
        $(this).remove();
    });
    $(_wrapper).on('click', 'button[data-type=save_comment]', function () {
        $('form').submit();
    });
    $.get("../Comments/NewComment", { "market": _market, "brand": _brand, "channel": _channel, "name": _name }, function (d, t, j) {
        $(_body).append(d);
    });
};

var editComment = function (id) {
    var _body = $('.modal-body');
    var _footer = $('.modal-footer');
    $(_body).find('form').remove();
    var _save = $('<button>').attr({ "class": "btn btn-success", "data-type": "save_comment" }).html("Save Comment");
    var _cancel = $('<button>').attr({ "class": "btn btn-danger", "data-type": "cancel_comment" }).html("Cancel");
    var _new = $(_footer).find('button[data-type=new_comment]');
    if ($(_footer).find('button[data-type=save_comment]').length <= 0)
        $(_footer).append(_save);
    if ($(_footer).find('button[data-type=cancel_comment]').length <= 0)
        $(_footer).append(_cancel);
    $(_new).hide();
    $(_footer).on('click', 'button[data-type=cancel_comment]', function () {
        $(_new).show();
        $('form').remove();
        $(this).parent().find('button[data-type=save_comment]').remove();
        $(this).remove();
    });
    $(_footer).on('click', 'button[data-type=save_comment]', function () {
        $('form').submit();
    });
    $.get("../Comments/EditComment", { "id": id }, function (d, t, j) {
        $(_body).append(d);
    });

};
var UpdateCommentSuccess = function () {
    var _modal = 'modal_comments';
    var _market = $('form').find('input[id=market]').val();
    var _brand = $('form').find('input[id=brand]').val();
    var _channel = $('form').find('input[id=channel]').val();
    var _name = $('form').find('input[id=name]').val();
    $.get("../Comments/Comments", { "market": _market, "brand": _brand, "channel": _channel }, function (d, t, j) {
        $('div[data-name=' + _modal + ']').find('.modal-dialog').html(d);
        $('div[data-name=' + _modal + ']').find('.modal-header h4').html($('div[data-name=' + _modal + ']').find('.modal-header h4').html() + ' ' + _name);
        $('div[data-name=' + _modal + ']').modal('show');
        $('button[data-type=new_comment]').attr({ 'data-market': _market, 'data-brand': _brand, 'data-channel': _channel, 'data-name': _name });
    });
};

$(document).ready(function () {
    $(document).on('click', 'button[data-type=new_comment]', function () {
        var _market = $(this).attr('data-market');
        var _brand = $(this).attr('data-brand');
        var _channel = $(this).attr('data-channel');
        newComment(_market, _brand, _channel, this);
    });
    $(document).on('click', 'a[data-type=edit_button]', function (e) {
        e.preventDefault();
        var _id = $(this).attr('data-id');
        editComment(_id);
    });
});