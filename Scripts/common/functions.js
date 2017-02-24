var check_loading = true;
var cookie = {
    setCookie:function (key, value) {
            var expires = new Date();
            expires.setTime(expires.getTime() + (1 * 24 * 60 * 60 * 1000));
            document.cookie = key + '=' + value + ';expires=' + expires.toUTCString();
        },

    getCookie: function(key) {
            var keyValue = document.cookie.match('(^|;) ?' + key + '=([^;]*)(;|$)');
            return keyValue ? keyValue[2] : null;
        }
    };

var number_format = function (number, decimals, dec_point, thousands_sep) {
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
};
var setHmtlContent = function (_target,_value) {
    $('#' + _target).html('x' + number_format(parseInt(_value),0,',','.'));
    $('#' + _target).attr('data-amount', _value);
};

var setDataAmount = function (val, dec) {
    var _currency = $('#currency').val().replace(',','.');
    $.each($('td[data-type=amount]'), function (i, e) {
        var _value = parseInt($(this).attr('data-value')) * parseFloat( _currency);
        var _valueWC = null
        if (checkWCCell(this))
        {
            if($('#wc').is(':checked'))
                _valueWC = parseInt($(this).attr('data-wc')) * parseFloat(_currency);
            setDataPCWC($('#wc').is(':checked'),this);
        }
        $(this).attr('title', number_format(parseInt(_valueWC||_value), 0, ',', '.'));
        $(this).html(number_format(parseInt(_valueWC ||_value) / val, dec, ',', '.'));
    });
    return true;
};

var setDataPC = function (dec){
    $.each($('td[data-type=pc]'), function (i, e) {
        var _value = $(this).attr('data-value').replace('%','').replace(',','.');
        var _valueWC = null
        if (checkWCCell(this)){
            if($('#wc').is(':checked'))
                _valueWC = $(this).attr('data-wc').replace('%','').replace(',','.');
        }
        $(this).attr('title', number_format(_valueWC||_value, 2, ',', '.') + '%');
        $(this).html(number_format(_valueWC ||_value, dec, ',', '.')+ '%');
    });
}

var checkCookies = function(){
    var _currency_adjust = cookie.getCookie('currency_adjust');
    var _pc_adjust = cookie.getCookie('pc_adjust');
    var _currency = cookie.getCookie('currency');
    var _wc = cookie.getCookie('wc');
    this.check_loading = false;
    if(_currency!=null)
        $('#currency').trigger('change');
    if(_currency_adjust != null)
        $('#currency_adjust').trigger('change');
    if(_wc!= null && _wc != 'false')
        $('#wc').trigger('change');
    if(_pc_adjust!= null)
        $('#pc_adjust').trigger('change');
    this.check_loading = true;
};

var setBanner = function(){
    var _currency_adjust = cookie.getCookie('currency_adjust');
    var _pc_adjust = cookie.getCookie('pc_adjust');
    var _currency = cookie.getCookie('currency');
    var _wc = cookie.getCookie('wc');
    if(_currency!=null)
        $('#currency').val(_currency);
    if(_currency_adjust != null)
        $('#currency_adjust').val(_currency_adjust);
    if(_wc!= null  && _wc != 'false')
        $('#wc').attr('checked','checked');
    if(_pc_adjust!= null)
        $('#pc_adjust').val(_pc_adjust);
};

var changeAmountRatio = function (val) {
    $('#currency_adjust').trigger('change');
    //val = val.replace(',','.');
    //var _adjust = $('#currency_adjust').val();
    //$.each($('td[data-type=amount]'), function (i, e) {
    //    var _value = parseInt($(this).attr('data-value')) * val / _adjust;
    //    var _valueWC = null;
    //    //Comprobamos si está activa WC y si es casilla WC cambiamos el valor.
    //    if ($('#wc').is(':checked') && checkWCCell(this))
    //        _valueWC = parseInt($(this).attr('data-wc')) * val  / _adjust;
    //    $(this).html(number_format(_valueWC||_value, 0, ',', '.'))
    //    if ($(this).attr('data-value-change') == null) {
    //        $(this).attr('data-value-change',_value);
    //    }
    //    if ($(this).attr('data-wc-change') == null) {
    //        $(this).attr('data-wc-change', _valueWC);
    //    }
    //});
    
    return true;
}

var setDataPCWC = function (_checked, _cell) {
    var _target = $(_cell).parent().find('td[data-type=pc]');
    $(_target).html(_checked ? $(_target).attr('data-wc') : $(_target).attr('data-value'));
}

var checkWCCell = function (_cell) {
    return $(_cell).attr('data-wc') != null;
};


$(document).ready(function () {
    // Cambio €/$
    $(document).on('change', '#currency', function (event) {
        if ($('.load-mask').css('display') == 'none' || !check_loading) {
            var _li = $('<li>').attr('class','fa fa-cog fa-spin');
            var _span = $('<span>').attr('class','loading').append(_li).append('Wait..');
            $(this).prepend(_span);
            if (changeAmountRatio($(this).val())) {
                //cookie.setCookie('currency',$(this).val());
                setHmtlContent('adjust', 1);
                $('.loading').remove();
            }
        }
    });
    //Reducir-aumentar multiplicador
    $(document).on('change', '#currency_adjust', function (event) {
        event.preventDefault();
        if ($('.load-mask').css('display') == 'none' || !check_loading) {
            var _target = $(this).parent().find('#' + $(this).attr('data-target'));
            var _current = $(this).val();
            var max_val = $(this).attr('data-max');
            var _decimal = (_current >= max_val)? 1:0;
            if (setDataAmount(_current, _decimal)) {
                //cookie.setCookie('currency_adjust',$(this).val());
                setHmtlContent($(this).attr('data-target'), _current);
            }
        }
    });
    //Reducir-aumentar pc
    $(document).on('change', '#pc_adjust', function (event) {
        event.preventDefault();
        if ($('.load-mask').css('display') == 'none' || !check_loading) {
            var _adjust = $(this).val();
            var _decimal =_adjust;
            setDataPC(_decimal);
            //cookie.setCookie("pc_adjust",$(this).val());
        }
    });
    //Activar vista WC
    $(document).on('change', '#wc', function (event) {
        event.preventDefault();
        if ($('.load-mask').css('display') == 'none' || !check_loading) {
            $.get('../Menu/SetBannerCookie',{'key':$(this).attr('id'), 'value':$(this).is(':checked')});
            var _this = '#currency_adjust';
                var _current = $(_this).val();
                var _decimal = (_current >= $(_this).attr('data-max')) ? 1 : 0;
                var ret = setDataAmount(_current, _decimal);
                //cookie.setCookie('wc',$(this).is(':checked'));
        } else {
            $(this).prop('checked', false);
        }
    });

    $(document).on('change','.form-control',function(){
        $.get('../Menu/SetBannerCookie',{'key':$(this).attr('id'), 'value':$(this).val()});
    });
    setBanner();
});