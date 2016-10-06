$.strwmObj = new Object();
$.strwmObj.dataGrid = 'PreviewDataGrid';
$.strwmObj.dataRow = 'PreviewDataRow';
$.strwmObj.dataRowEdit = 'PreviewDataRowEdit';
StrwmAjaxCall = function (_url, _data) {
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
}

$(document).ready(function () {
    $('.grid-mvc').addClass('tab-pane');
    $('.grid-mvc[data-gridname=market]').addClass('active');
});

$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _href = $.strwmObj.dataGrid + $(this).attr('href');
    _html = StrwmAjaxCall(_href).replace('\r', '').replace('\n', '').trim();
    $name = $(this).closest('.grid-mvc').attr('data-gridname');
    $obj = $('<div/>').append($.parseHTML(_html)).find('.grid-mvc[data-gridname=' + $name + ']');
    $(this).closest('.grid-mvc[data-gridname=' + $name + ']').find('.grid-wrap .table tbody').html($($obj).find('table tbody').html());
    $(this).closest('.grid-mvc[data-gridname=' + $name + ']').find('.grid-pager').html($($obj).find('.grid-pager').html());

});
$(document).on('click', '.grid-mvc table thead th a', function (event) {
    event.preventDefault();
    _href = $.strwmObj.dataGrid + $(this).attr('href');
    _html = StrwmAjaxCall(_href).replace('\r', '').replace('\n', '').trim();
    $name = $(this).closest('.grid-mvc').attr('data-gridname');
    $obj = $('<div/>').append($.parseHTML(_html)).find('.grid-mvc[data-gridname=' + $name + ']');
    $(this).closest('.grid-mvc[data-gridname=' + $name + ']').find('.grid-wrap .table tbody').html($($obj).find('.grid-mvc[data-gridname=' + $name + '] table tbody').html());
    $(this).closest('.grid-mvc[data-gridname=' + $name + ']').find('.grid-pager').html($($obj).find('.grid-mvc[data-gridname=' + $name + '] .grid-pager').html());
    $(this).closest('.grid-mvc[data-gridname=' + $name + ']').find('.grid-wrap .table thead').html($($obj).find('.grid-mvc[data-gridname=' + $name + '] table thead').html());
});
$(document).on('click', 'button[data-type=edit]', function (event) {
    event.preventDefault();
    _index = $(this).closest('tr')[0].rowIndex;
    $row_edit = $(this).closest('table').find('.grid-editing');
    $name = $(this).closest('.grid-mvc').attr('data-gridname');
    if ($($row_edit).length > 0) {
        $button = $($row_edit).find('button');
        _market = $($button).attr('data-target-market');
        _brand = $($button).attr('data-target-brand');
        _channel = $($button).attr('data-target-channel');
        _data = { 'market': _market, 'brand': _brand, 'channel': _channel };
        _url = $.strwmObj.dataRow;
        _html = StrwmAjaxCall(_url, _data);
        $obj = $('<div/>').append($.parseHTML(_html)).find('.grid-mvc[data-gridname=' + $name + ']');
        $($row_edit).html($($obj).find('tbody').find('tr').html()).removeClass('grid-editing');
    }
    _market = $(this).attr('data-target-market');
    _brand = $(this).attr('data-target-brand');
    _channel = $(this).attr('data-target-channel');
    _data = { 'market': _market, 'brand': _brand, 'channel': _channel, 'mode':'edit' };
    _url = $.strwmObj.dataRowEdit;
    _html = StrwmAjaxCall(_url, _data);
    $obj = $('<div/>').append($.parseHTML(_html)).find('.grid-mvc[data-gridname=' + $name + ']');
    $(this).closest('tr').html($($obj).find('tbody').find('tr').html()).addClass('grid-editing');
})