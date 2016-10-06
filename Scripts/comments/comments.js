var showComments = function (button) {
    var _this = button;
    var _market = $(_this).attr('data-value-market');
    var _brand = $(_this).attr('data-value-brand');
    var _channel = $(_this).attr('data-value-channel');
    var _modal = $(_this).attr('data-link');
    var _name = $(_this).attr('data-value-name');
    $.get("../Comments/Comments", { "market": _market, "brand": _brand, "channel": _channel }, function (d, t, j) {
        $('div[data-name=' + _modal + ']').find('.modal-dialog').html(d);
        $('div[data-name=' + _modal + ']').find('.modal-header h4').html($('div[data-name=' + _modal + ']').find('.modal-header h4').html() + ' ' + _name);
        $('div[data-name=' + _modal + ']').modal('show');
        $('button[data-type=new_comment]').attr({ 'data-market': _market, 'data-brand': _brand, 'data-channel': _channel, 'data-name': _name });
    });
};


$(document).ready(function () {
    $(document).on("click", "button[data-type=btn-comment]", function () {
        showComments(this);
    });
});