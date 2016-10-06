function number_format(number, decimals, dec_point, thousands_sep) {
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        toFixedFix = function (n, prec) {
            // Fix for IE parseFloat(0.55).toFixed(0) = 0;
            var k = Math.pow(10, prec);
            return Math.round(n * k) / k;
        },
        s = (prec ? toFixedFix(n, prec) : Math.round(n)).toString().split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}
function setHmtlContent(_target,_value) {
    $('#' + _target).html('x' + number_format(parseInt(_value),0,',','.'));
    $('#' + _target).attr('data-amount', _value);
}

function setDataAmount(val, dec) {
    $.each($('td[data-type=amount]'), function (i, e) {
        var _currency = $('#currency option:selected').text();
        var _value = (_currency == '€') ? $(this).attr('data-value') : $(this).attr('data-value-change');
        var _valueWC = null
        if (checkWCCell(this))
        {
            if($('#wc').is(':checked'))
                _valueWC = (_currency == '€') ? $(this).attr('data-wc') : $(this).attr('data-wc-change');
            setDataPC($('#wc').is(':checked'),this);
        }
        $(this).attr('title', number_format(parseInt(_valueWC||_value), 0, ',', '.'));
        $(this).html(number_format(parseInt(_valueWC ||_value) / val, dec, ',', '.'));
    });
    return true;
}

function changeAmountRatio(val) {
    val = val.replace(',','.');
    $.each($('td[data-type=amount]'), function (i, e) {
        var _value = parseInt($(this).attr('data-value')) * val;
        var _valueWC = null;
        //Comprobamos si está activa WC y si es casilla WC cambiamos el valor.
        if ($('#wc').is(':checked') && checkWCCell(this))
            _valueWC = parseInt($(this).attr('data-wc')) * val;
        $(this).html(number_format(_valueWC||_value, 0, ',', '.'))
        if ($(this).attr('data-value-change') == null) {
            $(this).attr('data-value-change',_value);
        }
        if ($(this).attr('data-wc-change') == null) {
            $(this).attr('data-wc-change', _valueWC);
        }
    });
    
    return true;
}

var setDataPC = function (_checked, _cell) {
    var _target = $(_cell).parent().find('td[data-type=pc]');
    $(_target).html(_checked ? $(_target).attr('data-wc') : $(_target).attr('data-value'));
}

var checkWCCell = function (_cell) {
    return $(_cell).attr('data-wc') != null;
};


$(document).ready(function () {
    // Cambio €/$
    $(document).on('change', '#currency', function (event) {
        if ($('.load-mask').css('display') == 'none') {
            var _li = $('<li>').attr('class','fa fa-cog fa-spin');
            var _span = $('<span>').attr('class','loading').append(_li).append('Wait..');
            $(this).prepend(_span);
            if (changeAmountRatio($(this).val())) {
                setHmtlContent('adjust', 1);
                $('.loading').remove();
            }
        }
    });
    //Reducir-aumentar multiplicador
    $(document).on('change', '#currency_adjust', function (event) {
        event.preventDefault();
        if ($('.load-mask').css('display') == 'none') {
            var _target = $(this).parent().find('#' + $(this).attr('data-target'));
            var _current = $(this).val();
            var max_val = $(this).attr('data-max');
            var _decimal = (_current >= max_val)? 1:0;
            if (setDataAmount(_current, _decimal)) {
                setHmtlContent($(this).attr('data-target'), _current);
            }
        }
    });
    //Activar vista WC
    $(document).on('change', '#wc', function (event) {
        event.preventDefault();
        if ($('.load-mask').css('display') == 'none') {
            var _this = '#currency_adjust';
                var _current = $(_this).val();
                var _decimal = (_current >= $(_this).attr('data-max')) ? 1 : 0;
                var ret = setDataAmount(_current, _decimal);
        } else {
            $(this).prop('checked', false);
        }
    });
});