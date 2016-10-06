$.getScript('../Scripts/combo/init.js');
$periodCombo = function () {
    
    $date_container = $('.date.wrapper');
    if (!$($date_container).find('.date').length) {
        $($date_container).find('.banner').remove();
        $($date_container).append(StrwmAjaxReturn('../Config/PeriodicCombo', ''));
    } else {
        $($date_container).find('.date').show();
    }

}
$(document).on('change', '.date select', function (event) {
    event.preventDefault();
    var _cont = $('.date.wrapper');
    var _val = $(this).val();
    $year_selector = $('#select-year').val();
    $month_selector = $('#select-month').val();

    if ($(this).attr('id') == 'select-year') {
        $month_selector = '12';//Si se ha cambiado el año, por defecto mostramos 12
        $year_selector = _val;
    } else {
        $month_selector = _val;
    }
    StrwmAjaxReturn('../Config/ChangePeriodAjax', { _year: $year_selector, _month: $month_selector });
    $(_cont).find('.date').remove();
    $(_cont).append(StrwmAjaxReturn('../Config/GetPeriodBanner', ''));
    //Actualizamos los años de las cabeceras
    var _current = $('#year_control').html();
    var _last = _current - 1;
    var _two = _current - 2;
    var _three = _current - 3;
    $head = $('#header-fix table thead');
    $head.find('.current_year_control').html(_current);
    $head.find('.last_year_control').html(_last);
    $head.find('.two_year_control').html(_two);
    $head.find('.three_year_control').html(_three);
    $('#cache_set').val(false);//Desactivamos la caché.
    startLoadTables();//volvemos a cargar las tablas
    
});
StrwmAjaxCall = function (object, _url) {
    //$(object).append(_loader);
    $(object).hide();
    $.ajax({
        url: _url,
        async: true,
        contentType: 'application/html; charset=utf-8',
        type: 'POST',
        dataType: 'html'
    })
        .done(function (result) {
            $(object).html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();
            $(object).show();
            $('.loading-bar').animate({ 'border-color': 'rgb(0,220,0)' }, 'slow');
            $('.loading-bar').animate({ width: '100%', 'border-color': 'rgb(0,220,0)' }, 'slow', function () {
                $('.load-mask').animate({ opacity: 'hide' }, 'slow', function () { $(this).hide() });
            });
        })
        .fail(function (xhr, status) {
            alert(status);
        });

}

StrwmAjaxReturn = function (_url, _data) {
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