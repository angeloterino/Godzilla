$this = new Object();
$this.loader = $('<div>').addClass('loader table').css({ 'display': 'table-cell', 'position': 'absolute', 'top': '0', 'left': '0', 'right': '0', 'bottom': '0', 'text-align': 'center', 'vertical-align': 'middle', 'opacity': '0.7' }).append($('<li/>').css({ 'margin-top': '10%' }).addClass('fa fa-spinner fa-pulse fa-3x fa-fw'));
$this.dialog =
$('<div/>').attr('title', 'Delete item').html(
   $('<p/>').attr('style', 'padding:2px;').append(
      $('<span/>').attr({ 'class': 'ui-icon ui-icon-alert', 'style': 'float:left;margin-top: -2px;margin-right: 5px;' })
   ).append('Are you sure to delete this item permanently?')
);
$this.StrwmAjaxCall = function (_url, _data) {
    var _return = "";
    $.ajax({
        url: _url,
        data: _data,
        async: false,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .done(function (result) {
        _return = result;
    })
    .fail(function (xhr, status) {
        alert(status);
    });
    return _return;
};
$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _target = $(this).closest('.wrapper').attr('data-controller');
    _href = _target + $(this).attr('href') + "&_channel=" + $('.modal .nav .active a').html();
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    //_pane = $('div.tools-bar ul li.active').find('a').attr('href');
    $obj = $.parseHTML(_html);
    $(this).closest('.wrapper').find('.table tbody').html($($obj).find('table tbody').html());
    $(this).closest('.wrapper').find('.grid-pager').html($($obj).find('.grid-pager').html());

});
$(document).on('click', '.grid-mvc table thead th a', function (event) {
    event.preventDefault();
    var _target = $(this).closest('.wrapper').attr('data-controller');
    _href = _target + $(this).attr('href') + "&_channel=" + $('.modal .nav .active a').html();
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    _pane = $('div.tools-bar ul li.active').find('a').attr('href');
    $obj = $.parseHTML(_html);
    $(this).closest('.wrapper').find('div.active  .table tbody').html($($obj).find('div'+_pane+' table tbody').html());
    $(this).closest('.wrapper').find('div.active  .grid-pager').html($($obj).find('div'+_pane+' .grid-pager').html());
    $(this).closest('.wrapper').find('div.active  .grid-mvc table thead').html($($obj).find('div'+_pane+' table thead').html());
});
$(document).on('click', 'input[type=checkbox][data-type=select]', function () {
    CheckedBox(this);
});
$(document).on('click', 'button[data-type=edit]', function (event) {
    event.preventDefault();
    //Destruimos formularios previos y mostramos las filas ocultas:
    $('tr.grid-editor').remove();
    $('tr.grid-editing').removeClass('grid-editing').show();
    _index = $(this).closest('tr')[0].rowIndex;
    _url = $(this).attr('data-controller');
    _id = $(this).attr('data-target-id');
    _data = { '_id': _id};
    _html = $this.StrwmAjaxCall(_url, _data);
    $obj = $.parseHTML(_html);
    $(this).closest('tr').before($('<div/>').append($obj).find('tbody tr').addClass('grid-editor').addClass('grid-row-selected'));
    $(this).closest('tr').addClass('grid-editing').hide();
    if($(this).closest('.wrapper').find('form').length <= 0){
        $(this).closest('.wrapper').append($('<div/>').append($obj).find('form'));
    }else
        $(this).closest('.wrapper').find('form').html("");
    var _minWidth = $($obj).find('table thead th:eq('+($(this).closest('td').index())+')').width();
    $(this).closest('table').find('thead th:eq('+($(this).closest('td').index())+')').css('min-width',_minWidth);
});
$(document).on('click', 'tr.grid-editor button[data-type=close]',function(event){
    event.preventDefault();
    $(this).closest('table').find('tr.grid-editing').show().removeClass('grid-editing');
    $(this).closest('table').find('thead th:eq('+($(this).closest('td').index())+')').css('min-width','');
    $(this).closest('tr.grid-editor').remove();
});

var CheckedBox = function(checkbox){
    var _this = checkbox;
    var _value = $(_this).attr('data-value');
    var _controller = $(_this).attr('data-controller');
    var _target = $(_this).closest('.wrapper').attr('data-target');
    var target = $('div[data-type='+_target+']');
    if ($(_this).is(':checked')) {
        $(_this).closest('.wrapper').find('input[type=checkbox]').prop('checked', false);
        $(_this).prop('checked', true);
    }

    var _html = $this.StrwmAjaxCall(_controller, { 'order': _value });
    $(target).html($.parseHTML(_html));
};