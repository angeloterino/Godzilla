$this = new Object();
$this.loader = $('<div>').addClass('loader table').css({ 'display': 'table-cell', 'position': 'absolute', 'top': '0', 'left': '0', 'right': '0', 'bottom': '0', 'text-align': 'center', 'vertical-align': 'middle', 'opacity': '0.7' }).append($('<li/>').css({ 'margin-top': '10%' }).addClass('fa fa-spinner fa-pulse fa-3x fa-fw'));
$this.dialog = 
$('<div/>').attr('title','Delete item').html(
   $('<p/>').attr('style','padding:2px;').append(
      $('<span/>').attr({'class':'ui-icon ui-icon-alert','style':'float:left;margin-top: -2px;margin-right: 5px;'})
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
}
$this.initTabs = function () {
    var hidWidth;
    var scrollBarWidths = 40;

    var widthOfList = function () {
        var itemsWidth = 0;
        $('.list li').each(function () {
            var itemWidth = $(this).outerWidth();
            itemsWidth += itemWidth;
        });
        return itemsWidth;
    };

    var widthOfHidden = function () {
        return (($('.tabs.wrapper').outerWidth()) - widthOfList() - getLeftPosi()) - scrollBarWidths;
    };

    var getLeftPosi = function () {
        return $('.list').position().left;
    };

    var reAdjust = function () {
        if (($('.tabs.wrapper').outerWidth()) < widthOfList()) {
            $('.scroller-right').show();
        }
        else {
            $('.scroller-right').hide();
        }

        if (getLeftPosi() < 0) {
            $('.scroller-left').show();
        }
        else {
            $('.item').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow');
            $('.scroller-left').hide();
        }
    }

    reAdjust();

    $(window).on('resize', function (e) {
        reAdjust();
    });

    $('.scroller-right').click(function () {

        $('.scroller-left').fadeIn('slow');
        $('.scroller-right').fadeOut('slow');

        $('.list').animate({ left: "+=" + widthOfHidden() + "px" }, 'slow', function () {

        });
    });

    $('.scroller-left').click(function () {

        $('.scroller-right').fadeIn('slow');
        $('.scroller-left').fadeOut('slow');

        $('.list').animate({ left: "-=" + getLeftPosi() + "px" }, 'slow', function () {

        });
    });
}
$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _target = $(this).closest('.wrapper').attr('data-controller');
    _href = _target + $(this).attr('href')+ "&_channel=" + $('.modal .nav .active a').html();
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $(this).closest('.wrapper').find('.table tbody').html($($obj).find('table tbody').html());
    $(this).closest('.wrapper').find('.grid-pager').html($($obj).find('.grid-pager').html());

});
$(document).on('click', '.grid-mvc table thead th a', function (event) {
    event.preventDefault();
    var _target = $(this).closest('.wrapper').attr('data-controller');
    _href = _target + $(this).attr('href')+ "&_channel=" + $('.modal .nav .active a').html();
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $(this).closest('.wrapper').find('.table tbody').html($($obj).find('table tbody').html());
    $(this).closest('.wrapper').find('.grid-pager').html($($obj).find('.grid-pager').html());
    $(this).closest('.wrapper').find('.grid-mvc table thead').html($($obj).find('table thead').html());
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
$(document).on('click','button[data-type=new]',function(e){
    e.preventDefault();
    var _modal = 'div[data-type=' + $(this).attr('data-modal')+ ']';
    var _controller = $(this).attr('data-controller');
    var _data = {"_group":$(this).attr('data-source')};
    SetModalItems(_modal,_controller,_data);
});
$(document).on('click', 'button[data-type=save]',function(e){
   e.preventDefault();
   SaveForm(this);
});
$(document).on('click', 'button[data-type=delete]',function(){
    var _id = $(this).attr('data-target-id');
    var _target = $(this).attr('data-controller');
    var _source = $(this).attr('data-source');
    SetDialog($this.dialog,_id, _target, _source);
});
$(document).on('click', 'button[data-type=duplicate]',function(e){
    e.preventDefault();
    $(this).closest('td').append($('<input/>').attr({'type':'hidden','name':'o.duplicate','value':true}));
    SaveForm(this);
});
$(document).on('change', 'select', function () {
    ChangeSelect(this);
});

$(document).on('click', 'input[type=checkbox][data-type=select]', function () {
    CheckedBox(this);
});
$(document).on('click', 'tr.grid-editor button[data-type=close]',function(event){
    event.preventDefault();
    $(this).closest('table').find('tr.grid-editing').show().removeClass('grid-editing');
    $(this).closest('table').find('thead th:eq('+($(this).closest('td').index())+')').css('min-width','');
    $(this).closest('tr.grid-editor').remove();
});
$(document).on('click', 'input[type=checkbox][data-type=append]', function () {
    var _typeId = $(this).attr('data-type-id');
    var _group = $(this).attr('data-group');
    var _market = $(this).attr('data-market');
    var _brand = $(this).attr('data-brand');
    var _channel = $(this).attr('data-channel');
    var _controller = $(this).attr('data-controller');
    var _selected = $(this).is(':checked');
    console.log(_typeId + ', ' + _group + ', ' + _controller + ', ' + _selected);
    var _html = $this.StrwmAjaxCall(_controller, { '_channel': _channel, '_market': _market, '_brand': _brand, '_group': _group, '_checked': _selected });
});
$(document).on('ready', function () {
    //$this.initTabs();
});
var ChangeSelect = function(_this){
    var _value = $(_this).find('option:selected').val();
    var _controller = $(_this).attr('data-controller');
    var _target = $(_this).attr('data-target');
    var target = $('div[data-type='+ _target + ']');
    $this.loader.appendTo($(target).find('div.panel'));

    console.log(_target + ', ' + _value + ', ' + _controller);

    if($(_this).attr('data-type') == 'group_types'){
        var _html = $this.StrwmAjaxCall(_controller, { 'type': _value });
        $(target).find('div.panel-body').html($.parseHTML(_html));
        $(target).find('div.panel-body table td input[type=checkbox]:eq(0)').click();
        $this.loader.remove();
    }
    if($(_this).attr('data-type')=='select_source'){
        var _group = $('button[data-type=new]').attr('data-source');
        var _modal = $(_this).closest('div.modal');
        var _data = {_group :_group, _source:_value};
        SetModalItems(_modal, _controller,_data);
    }
};
var CheckedBox = function(checkbox){
    var _this = checkbox;
    var _typeId = $(_this).attr('data-type-id');
    var _group = $(_this).attr('data-group');
    var _controller = $(_this).attr('data-controller');
    var _target = $(_this).closest('.wrapper').attr('data-target');
    var target = $('div[data-type='+_target+']');
    console.log(_typeId + ', ' + _group + ', ' + _controller + ', ' + _target);

    if ($(_this).is(':checked')) {
        $(_this).closest('.wrapper').find('input[type=checkbox]').prop('checked', false);
        $(_this).prop('checked', true);
    }

    var _html = $this.StrwmAjaxCall(_controller, { 'type': _typeId, 'group': _group });
    $(target).find('div.panel-body').html($.parseHTML(_html));
};
var SubmitForm = function(){
  $(this).find('button[data-type=save] li').attr('class','fa fa-spinner fa-spin');  
};
var UpdateSuccess = function(){
  CheckedBox($('input[type=checkbox][data-type=select]:checked'));
  $('.modal').modal('hide');
};
var UpdateSuccessMaster = function(){
    ChangeSelect($(document).find('select[data-type=group_types]'));
}
var SetModalItems = function(_modal, _url, _data){
    var _html = $this.StrwmAjaxCall(_url,_data);
    $obj=$.parseHTML(_html);
    if($($obj).find('.modal-header').length>0)
        $(_modal).find('.modal-header').html($($obj).find('.modal-header').html());
    if($($obj).find('.modal-body').length > 0)
        $(_modal).find('.modal-body').html($($obj).find('.modal-body').html());
    if($($obj).find('.modal-nav').length>0)
        $(_modal).find('.modal-nav').html($($obj).find('.modal-nav').html());
    if($($obj).find('.modal-footer').length>0)
        $(_modal).find('.modal-footer').html($($obj).find('.modal-footer').html());
    $(_modal).modal('show');
};
var SetDialog = function(_dialog,_id,_target,_source){
    $( _dialog ).dialog({
      resizable: false,
      height: "auto",
      width: 400,
      modal: true,
      buttons: {
        "Delete": function() {
          console.log($(this).attr('data-id'));
          var _id = $(this).attr('data-id');
          var _url = $(this).attr('data-target');
          var _source = $(this).attr('data-source');
          $this.StrwmAjaxCall(_url, {'_id':_id,'_source':_source});
          if(_source == 'GROUP_MASTER')
            ChangeSelect($(document).find('select[data-type=group_types]'));
          if(_source == 'GROUP_CONFIG')
            CheckedBox($('input[type=checkbox][data-type=select]:checked'));
          $( this ).dialog( "close" );
        },
        Cancel: function() {
          $( this ).dialog( "close" );
        }
      }
    }).attr({'data-id':_id, 'data-target': _target,'data-source':_source});
};
var SaveForm = function(_this){
   var _form = $(_this).closest('.wrapper').find('form');
   $(_form).hide().html("");//limpiamos primero el formulario
   $.each($(_this).closest('table').find('tr.grid-editor input'), function(i,e){           
                $(_form).append(this)
       });
   $.each($(_this).closest('table').find('tr.grid-editor select'), function(i,e){           
            $(_form).append(this)
   });
   $(_form).submit();  
};