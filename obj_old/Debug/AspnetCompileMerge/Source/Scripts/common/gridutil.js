$(document).on('click', '.grid-pager a', function (event) {
    event.preventDefault();
    _href = window.location.pathname + $(this).attr('href');
    _html = $this.StrwmAjaxCall(_href).replace('\r\n', '').trim();
    $obj = $.parseHTML(_html);
    $('.table tbody').html($($obj).find('table tbody').html());
    $('.grid-pager').html($($obj).find('.grid-pager').html());

});