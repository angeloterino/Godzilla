$this = new Object();
$this.loader = $('<div>').addClass('loader table').css({ 'display': 'table-cell', 'position': 'absolute', 'top': '0', 'left': '0', 'right': '0', 'bottom': '0', 'text-align': 'center', 'vertical-align': 'middle', 'opacity': '0.7' }).append($('<li/>').css({ 'margin-top': '10%' }).addClass('fa fa-spinner fa-pulse fa-3x fa-fw'));
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
    _href = _target + $(this).attr('href');
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());

});
$(document).on('click', '.grid-mvc table thead th a', function (event) {
    event.preventDefault();
    var _target = $(this).closest('.wrapper').attr('data-controller');
    _href = _target + $(this).attr('href');
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());
    $('.grid-mvc table thead').html($($obj).find('table thead').html());
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
    $(this).closest('tr').before($($obj).find('tbody tr').addClass('grid-editor').addClass('grid-row-selected'));
    $(this).closest('tr').addClass('grid-editing').hide();
    if($(this).closest('.wrapper').find('form').length <= 0){
        $(this).closest('.wrapper').append($($obj).find('form'));
    }else
        $(this).closest('.wrapper').find('form').html("");
})
$(document).on('click', 'button[data-type=save]',function(e){
   e.preventDefault();
   var _form = $(this).closest('.wrapper').find('form');
   $(_form).html("");//limpiamos primero el formulario
   $.each($(this).closest('table').find('tr.grid-editor input'), function(i,e){           
                $(_form).append(this)
       });
   $.each($(this).closest('table').find('tr.grid-editor select'), function(i,e){           
            $(_form).append(this)
   });
   $(_form).submit();
});$(document).on('change', 'select[data-type=group_types]', function () {

    var _value = $(this).find('option:selected').val();
    var _controller = $(this).attr('data-controller');
    var _target = $(this).attr('data-target');
    var target = $('div[data-type='+ _target + ']');
    $this.loader.appendTo($(target).find('div.panel'));

    console.log(_target + ', ' + _value + ', ' + _controller);

    var _html = $this.StrwmAjaxCall(_controller, { 'type': _value });
    $(target).find('div.panel-body').html($.parseHTML(_html));
    $(target).find('div.panel-body table td input[type=checkbox]:eq(0)').click();
    $this.loader.remove();
});

$(document).on('click', 'input[type=checkbox][data-type=select]', function () {
    var _typeId = $(this).attr('data-type-id');
    var _group = $(this).attr('data-group');
    var _controller = $(this).attr('data-controller');
    var _target = $(this).closest('.wrapper').attr('data-target');
    var target = $('div[data-type='+_target+']');
    console.log(_typeId + ', ' + _group + ', ' + _controller + ', ' + _target);

    if ($(this).is(':checked')) {
        $(this).closest('.wrapper').find('input[type=checkbox]').prop('checked', false);
        $(this).prop('checked', true);
    }

    var _html = $this.StrwmAjaxCall(_controller, { 'type': _typeId, 'group': _group });
    $(target).find('div.panel-body').html($.parseHTML(_html));
});

$(document).on('ready', function () {
    //$this.initTabs();
});