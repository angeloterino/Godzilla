StrwmAjaxCall = function (_url,_data) {
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
$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _href = 'MasterDataGrid' + $(this).attr('href');
    _html = StrwmAjaxCall(_href).replace('\r\n','').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());

});
$(document).on('click', '.grid-mvc table thead th a', function (event) {
    event.preventDefault();
    _href = 'MasterDataGrid' + $(this).attr('href');
    _html = StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());
    $('.grid-mvc table thead').html($($obj).find('table thead').html());
});
$(document).on('click', 'button[data-type=edit]', function (event) {
    event.preventDefault();
    _index = $(this).closest('tr')[0].rowIndex;
    $row_edit = $('table .grid-editing');
    if ($($row_edit).length > 0) {
        $button = $($row_edit).find('button');
        _market = $($button).attr('data-target-market');
        _brand = $($button).attr('data-target-brand');
        _channel = $($button).attr('data-target-channel');
        _data = { 'market': _market, 'brand': _brand, 'channel': _channel };
        _url = 'MasterDataRow';
        _html = StrwmAjaxCall(_url, _data);
        $obj = $.parseHTML(_html);
        $($row_edit).html($($obj).find('tbody').find('tr').html()).removeClass('grid-editing');
    }
    _market = $(this).attr('data-target-market');
    _brand = $(this).attr('data-target-brand');
    _channel = $(this).attr('data-target-channel');
    _data = { 'market': _market, 'brand': _brand, 'channel': _channel };
    _url = 'MasterDataRowEdit';
    _html = StrwmAjaxCall(_url, _data);
    $obj = $.parseHTML(_html);
    $(this).closest('tr').html($($obj).find('tbody').find('tr').html()).addClass('grid-editing');
})