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
    _index = $(this).closest('tr')[0].rowIndex;
    _url = $(this).attr('data-controller');
    _id = $(this).attr('data-id');
    _data = { 'id': _id};
    _html = $this.StrwmAjaxCall(_url, _data);
    $obj = $.parseHTML(_html);
    $(this).closest('tr').html($($obj).find('tbody').find('tr').html()).addClass('grid-editing');
})

$(document).on('click', 'a[data-type=group_types]', function () {

    var _value = $(this).attr('data-value');
    var _controller = $(this).attr('data-controller');
    var _wrapper = $(this).closest('.wrapper');
    var _target = $(_wrapper).attr('data-target');

    $(_wrapper).find('li').removeClass('active');
    $(this).parent().addClass('active');

    $this.loader.appendTo($(_wrapper).find('div.panel'));

    console.log(_target + ', ' + _value + ', ' + _controller);

    var _html = $this.StrwmAjaxCall(_controller, { 'type': _value });
    $(_target).find('div.panel-body').html($.parseHTML(_html));
    $this.loader.remove();
});

$(document).on('click', 'input[type=checkbox][data-type=select]', function () {
    var _typeId = $(this).attr('data-type-id');
    var _group = $(this).attr('data-group');
    var _controller = $(this).attr('data-controller');
    var _target = $(this).closest('.wrapper').attr('data-target');
    console.log(_typeId + ', ' + _group + ', ' + _controller + ', ' + _target);

    if ($(this).is(':checked')) {
        $(this).closest('.wrapper').find('input[type=checkbox]').prop('checked', false);
        $(this).prop('checked', true);
    }

    var _html = $this.StrwmAjaxCall(_controller, { 'type': _typeId, 'group': _group });
    $(_target).find('div.panel-body').html($.parseHTML(_html));
});

$(document).on('ready', function () {
    //$this.initTabs();
});