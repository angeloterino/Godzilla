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
};

$this.GetAjaxForm = function(_this,_modal,_url,_data){
    $.get(_url,_data,function (d, t, j) {
        var _content = $.parseHTML(d);
        $('div[data-name=' + _modal + ']').find('.form-horizontal').html($(_content).html());
    }).success(function(){ 
        $(_this).find('li').remove();
        $(_this).closest('div').find('button').removeClass('btn-success').addClass('btn-default');
        $(_this).removeClass('btn-default').addClass('btn-success');
    });
};

$this.SetGroupSelected = function(_obj){
    var _grp = 0;
    var _market = $(_obj).val();
    if(_market > 0){
        $.get('SetGroupByMarket',{'_var1':_market}, function(_result){
            _grp = _result.group;
        }).success(function(){
            $('select[for=group]').val(_grp);
        });
    }
}

$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _href =  'MasterDataGrid' + $(this).attr('href');
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
    _market = $(this).attr('data-target-market');
    _brand = $(this).attr('data-target-brand');
    _channel = $(this).attr('data-target-channel');
    _data = { 'market': _market, 'brand': _brand, 'channel': _channel };
    var _modal = ($(this).attr('data-btn-variable')!=null)?$(this).attr('data-btn-variable'):$('div[data-type=modal]').attr('data-name');
    _url = 'MasterDataRowEdit';
    $(this).find('li').attr('data-class',$(this).find('li').attr('class')).attr('class','fa fa-spinner fa-spin');
    var _this = this;
    $.get(_url,_data,function (d, t, j) {
        var _content = $.parseHTML(d);
        $('div[data-name=' + _modal + ']').find('.modal-body').html($(_content).find('.modal-body').html());
        $('div[data-name=' + _modal + ']').find('.modal-nav').html($(_content).find('.modal-nav').html());
        $('div[data-name=' + _modal + ']').find('.modal-footer').html($(_content).find('.modal-footer').html());
        $('div[data-name=' + _modal + ']').find('.modal-header').html($(_content).find('.modal-header').html());
    }).success(function(){ $('div[data-name=' + _modal + ']').modal('show'); $(_this).find('li').attr('class',$(_this).find('li').attr('data-class'))});
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
$(document).on('click','button[data-btn-type=new_item]',function(e){
    e.preventDefault();
    var _modal = $(this).attr('data-btn-variable');
    var _url = $(this).attr('data-btn-target');
    var _channel = $('.config-wrapper').find('button.btn-success').attr('data-btn-variable');
    var _type = $(this).attr('data-btn-default');
    var _data = {'type':'','channel':$('.config-wrapper button.btn-success').attr('data-btn-variable')};
    $(this).html('<li class="fa fa-spinner fa-spin"></li>' + $(this).html());
    var _this = this;
    $.get(_url,_data,function (d, t, j) {
        var _content = $.parseHTML(d);
        $('div[data-name=' + _modal + ']').find('.modal-body').html($(_content).find('.modal-body').html());
        $('div[data-name=' + _modal + ']').find('.modal-nav').html($(_content).find('.modal-nav').html());
        $('div[data-name=' + _modal + ']').find('.modal-footer').html($(_content).find('.modal-footer').html());
        $('div[data-name=' + _modal + ']').find('.modal-header').html($(_content).find('.modal-header').html());
    }).success(function(){ $('div[data-name=' + _modal + ']').modal('show'); $(_this).find('li').remove()});
});
$(document).on('click','.modal-nav button',function(e){
    e.preventDefault();
    var _url = $(this).attr('data-btn-target');
    var _channel = $('.config-wrapper').find('button.btn-success').attr('data-btn-variable');
    var _type = $(this).attr('data-btn-variable');
    var _data = {'type':_type,'channel':_channel};
    var _modal = $(this).closest('div.modal').attr('data-name');
    $this.GetAjaxForm(this,_modal,_url,_data);
})
$(document).on('click','button[data-btn-type=submit]',function(e){
    e.preventDefault();
    $(this).html('<li class="fa fa-spinner fa-spin"></li> '+ $(this).html());
    $(this).closest('.modal').find('form').submit();
});
$(document).on('click','.select-input input[type=text]', function(e){
    $(this).closest('.select-input').find('select').val(0);
});
$(document).on('change','.select-input select', function(e){
    $('input[data-type='+$(this).attr('for')+']').val($(this).find('option:selected').text());
    $('input[type=hidden][for='+$(this).attr('for')+']').val($(this).val());
    $this.SetGroupSelected(this);
    $(this).val(0);
});
$(document).on('change', 'form select[data-type=update]', function () {
    var _for = $(this).attr('for');
    var _array = _for.split(',');
    var _url = $(this).attr('data-target');
    var _thisData = $(this).val();
    var _this = this;
    $.each(_array, function(i,e){
        var _type = $(e).attr('for');
        var _data = { "_data": _thisData,"_type":_type };
        var _result = $.parseHTML($this.StrwmAjaxCall(_url,_data));
        if($(e).closest('.select-input').length > 0){
            $(_result).prepend($('<option/>').attr('value','0')).val(0);
            $(e).closest('.select-input').find('input[data-type=' + _type + ']').val('');
        }
        $(e).html($(_result).html());
    });
});