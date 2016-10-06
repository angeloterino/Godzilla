$this = new Object();
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
}
$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _href = 'MasterDataGrid' + $(this).attr('href');
    _html = $this.StrwmAjaxCall(_href).replace('\r\n','').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());

});
$(document).on('click', '.grid-mvc table thead th a', function (event) {
    event.preventDefault();
    _href = 'MasterDataGrid' + $(this).attr('href');
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());
    $('.grid-mvc table thead').html($($obj).find('table thead').html());
});
$(document).on('click', 'button[data-type=edit]', function (event) {
    event.preventDefault();
    $(this).closest('table').find('tr.grid-row-selected').removeClass('grid-row-selected');
    _index = $(this).closest('tr:eq(0)').rowIndex;
    $row_edit = $('table .grid-editing');
    if ($($row_edit).length > 0) {
        $button = $($row_edit).find('button');
        _market = $($button).attr('data-target-market');
        _brand = $($button).attr('data-target-brand');
        _channel = $($button).attr('data-target-channel');
        _data = { 'market': _market, 'brand': _brand, 'channel': _channel };
        _url = 'MasterDataRow';
        _html = $this.StrwmAjaxCall(_url, _data);
        $obj = $.parseHTML(_html);
        $($row_edit).html($($obj).find('tbody').find('tr').html()).removeClass('grid-editing');
    }
    _market = $(this).attr('data-target-market');
    _brand = $(this).attr('data-target-brand');
    _channel = $(this).attr('data-target-channel');
    _data = { 'market': _market, 'brand': _brand, 'channel': _channel };
    _url = 'MasterDataRowEdit';
    _html = $this.StrwmAjaxCall(_url, _data);
    $obj = $.parseHTML(_html);
    $(this).closest('tr').html($($obj).find('tbody').find('tr').html()).addClass('grid-editing').addClass('grid-row-selected');
});
$(document).on('click', 'button[data-btn-type=btn-change-chanel]', function (object) {
    var _target = $('.config.wrapper');
    var _url = $(this).attr("data-btn-target");
    var _params = $(this).attr("data-btn-variable");
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

$(document).on('change', 'select[id=o_channel]', function () {
    var _target = $('select[id=o_group]').parent();
    var _url = 'GetGroupsSelectList';
    var _channel = $(this).val();
    var _data = { "_channel": _channel };
    $(_target).html($this.StrwmAjaxCall(_url,_data));

});