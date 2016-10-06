$.getScript('../../Scripts/bootstrap.inputs/bootstrap.file-inputs.js')
.success(function (a, b) { $('input[type=file]').bootstrapFileInput(); })
.fail(function (a, b, c) { alert(c); });

StrwmAjaxCall = function (object, _url, _type,_params) {
    $(object).append(_loader);
    //$(object).hide();
    _type = _type || 'POST';
    _params = _params || null;
    $.ajax({
        url: _url,
        data: _params,
        async: true,
        contentType: 'application/html; charset=utf-8',
        type: _type,
        dataType: 'html'
    })
        .done(function (result) {
            $(object).html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();
            //$(object).show();
            //$('.loading-bar').animate({ 'border-color': 'rgb(0,220,0)' }, 'slow');
            //$('.loading-bar').animate({ width: '100%', 'border-color': 'rgb(0,220,0)' }, 'slow', function () {
            //    $('.load-mask').animate({ opacity: 'hide' }, 'slow', function () { $(this).hide() });
            //});
        })
        .fail(function (xhr, status) {
            alert(status);
        });

}
$.strawmanVars = new Object();
var _loader = '<div class="removeme" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px; width:100%;height:100; positon:absolute;top:0;left:0;bottom:0;"><img id ="rotate" src="../images/loading_anim.gif" style="margin:0 auto;"/></div>';
$(document).ready(function () {
    $('#wrapper_main').css({ 'width': '100%', 'height': '100%' });
    $('#wrapper_main div.wrapper').css({ 'margin': '0' });
    $(document).on("click", "a[btn-type=new-group]", function () {
        var _target = this.getAttribute("btn-target-id");
        var _url = this.getAttribute("btn-target-url");
        var _params = { "_selectedId": this.getAttribute("btn-edit-id") };
        if ($('select[data-type=edit-select]').length > 0) { _params = { "_selectedId": this.getAttribute("btn-edit-id"), "_kpiColumn": $('select[data-type=edit-select]').val() }; };
        $(this).closest('.panel-body').append(_loader);
        $.ajax({
            url: _url,
            data: _params,
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            $('#' + _target).html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();

        })
        .fail(function (xhr, status) {
            alert(status);
        });
    });

    $(document).on("change paste", "input[type='text'][control-type='txt_boy_cfg']", function () {
        var _target = this.getAttribute("control-target-type");
        var _url = this.getAttribute("control-target-url");
        var _brand = this.getAttribute("param-brand");
        var _market = this.getAttribute("param-market");
        var _channel = this.getAttribute("param-channel");
        var _id = this.getAttribute("param-id");
        var _control_type = this.getAttribute("control-type");
        var _control_column_type = this.getAttribute("control-column-type");
        var _value = $(this).val();
        var _column_id = this.getAttribute("id");
        var _defaultvalue = $(this).prop('defaultValue');
        //_value = _defaultvalue;

        if (!($(this).closest('.wrapper').hasClass('selected'))) $(this).closest('.wrapper').toggleClass('selected').append(_loader);
        $('.selected').append(_loader);
        $.ajax({
            url: _url,
            data: { '_id': _id, 'type': _control_type, 'brand': _brand, 'market': _market, 'channel': _channel, 'control': _column_id, 'column': _control_column_type, 'value': _value },
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            $('.selected').html(result).toggleClass('selected');
            //Eliminamos el gif de carga
            $('.removeme').remove();

        })
        .fail(function (xhr, status) {
            alert(status);
            $('.removeme').remove();
        });
    });
    $(document).on("click", "a[btn-type=edit-group]", function () {
        $('form a[btn-type=edit-group]').removeClass('active');
        var _this = this;
        var _target = this.getAttribute("btn-target-id");
        var _url = this.getAttribute("btn-target-url");
        var _params = { "_selectedId": this.getAttribute("btn-param") };
        if ($('select[data-type=edit-select]').length > 0) { _params = { "_selectedId": this.getAttribute("btn-param"), "_selectValue": $('select[data-type=edit-select]').val() }; };
        $(_this).addClass("active");
        $(_this).closest('.panel-body').append(_loader);
        $.ajax({
            url: _url,
            data: _params,
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            if (_target == 'self') {
                var $cont = AppendForm(result);
                $($cont).insertAfter(_this);
            } else {
                $('#' + _target).html(result);
            }
            //Eliminamos el gif de carga
            $('.removeme').remove();

        })
        .fail(function (xhr, status) {
            alert(status);
        });
    });
    $(document).on("click", "a[btn-type=reset-form]", function () {
        StrwmAjaxCall('#BoyConfigure', 'BOYForm','POST');
    });
    //Carga texto ManagementLetters 
    $(document).on('change', 'select[select-type=groups-update]', function (object, _url) {
        UpdateSelectList(this);
    });
    //Carga texto MonthlyComments

    $(document).on('click', 'button[btn-type=btn-change-chanel]', function (object) {
        var _target = $('form[form-id=ActiveForm]').closest('div');
        var _url = $(this).attr("btn-target");
        var _params = $(this).attr("btn-variable");
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
    //Borrar el texto del cuadro de texto
    $(document).on('click', 'input[btn-type=btn-delete]', function (object) {
        object.preventDefault();
        $('textarea[select-id=comment-text]')
                .val('');
    });

});
//Funciones personalizadas

var AjaxCall = function (_url, _target, _id) {
        var _params = { "_selectedId": _id };
        var _this = '#' + _target;  
        if ($('select[data-type=edit-select]').length > 0) { _params = { "_selectedId": _id, "_selectValue": $('select[data-type=edit-select]').val() }; };
        $(_this).addClass("active");
        $(_this).closest('.panel-body').append(_loader);
        $.ajax({
            url: _url,
            data: _params,
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            if (_target == 'self') {
                var $cont = AppendForm(result);
                $($cont).insertAfter(_this);
            } else {
                $('#' + _target).html(result);
            }
            //Eliminamos el gif de carga
            $('.removeme').remove();

        })
        .fail(function (xhr, status) {
            alert(status);
        });
    };

var UpdateSelectList = function (_this) {
    var target_id = $(_this).attr("select-target");
    var main_group = $(_this).attr("select-main-group");
    var val = $(_this).attr("select-id") == 'main-group' ? $('select[select-id=' + main_group + ']').val() : $(_this).val();
    var val_mg = $(_this).attr("select-id") == 'main-group' ? $(_this).val() : $('select[select-id=' + main_group + ']').val();
    var _url = $(_this).attr('select-url');
    var _select_id = $(_this).attr("select-id");
    var _type = $(_this).attr("select-controller-type");
    $.getJSON(_url, { '_selectedId': val, '_main': val_mg, 'type': _type })
        .done(function (result) {
            var ddl = $('select[select-id=' + target_id + ']');
            ddl.empty();
            $(result).each(function () {
                $(document.createElement('option'))
                .attr('value', this.Id)
                .text(this.Value)
                .appendTo(ddl);
            });
        });
    $('textarea[select-id=comment-text]')
                .val('');
    ClearCommentMessage();
    if (ChechCheckbox(_select_id)) {
        UpdateCommentText(_type);
    }
}
var UpdateCommentText = function (_type) {
    var val = $('#group_selected').val();
    var _url = 'GetCommentText';
    var target_id = 'comment-text';
    
    $.getJSON(_url, { '_selectedId': val, '_type' : _type })
            .done(function (result) {
                $('textarea[select-id=' + target_id + ']')
                    .val(result.text);
                $('input[id=id]').val(result.id);
            });
}

var ChechCheckbox = function (_select_id) {
    //var _enable = true;
    //$('select[select-type=groups-update]').each(function () {
    //    if ($(this).attr('select-id') != 'select-groups') _enable = !($(this).val() == 0);
    //});
    return _select_id == 'select-groups';
    //if(_enable) $('input[type=submit]').toggleClass('disabled');
    //else $('input[type=submit]').addClass('disabled');
}

function InsertGroupSuccess(args) {

    var _target = args[0];
    var _origin = args[1];
    var _url = args[2];
    $('#' + _target).html('');
    $('#' + _origin).append(_loader);
    $.ajax({
        url: _url,
        async: true,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
        .done(function (result) {
            $('#' + _origin).html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();
            $('a[btn-type=edit-group]').removeClass("active");
        })
        .fail(function (xhr, status) {
            alert(status);
        });
    }

    function UpdateBOYSuccess(data) {
        
        var _url = data['Controller'];
        var params = {"market":data[1],"brand":data[1],"channel":data[2]};
        $('form[form-id=ActiveForm]').closest('div').append(_loader);
        $.ajax({
            url: _url,
            data: params,
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            $('form[form-id=ActiveForm]').closest('div').html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();
        })
        .fail(function (xhr, status) {
            alert(status);
        });
    }

    function SubmitForm() {
        $('form[form-id=ActiveForm]').closest('div').append(_loader);
    }

    function ReadFile(data, status, xhr) {
        var _target = args[0];
        var _origin = args[1];
        var _url = args[2];
        $('#' + _target).html('');
        $.ajax({
            url: _url,
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            $('#' + _origin).html(result);
            //Eliminamos el gif de carga
            $('.removeme').remove();
            $('a[btn-type=edit-group]').removeClass("active");
        })
        .fail(function (xhr, status) {
            alert(status);
        });
    }

    //Actualización del comentario con éxito
    UpdateCommentSuccess = function (_message) {
        UpdateSelectList('#channel_selected');
        $('textarea').val('');
        if ($('.message')) {
            $('.message').html(_message);
            if ($('#message-gryphicon').attr('class') == null) {
                $('<div/>', {
                    class: 'float-left success glyphicon glyphicon-ok',
                    id: 'message-gryphicon'
                }).insertBefore('.message');
            } else {
                $('#message-gryphicon')
                    .removeClass('glyphicon-warning-sign')
                    .removeClass('glyphicon-ok')
                    .addClass('glyphicon-ok')
            }
        }

    }
    UpdateCommentFailure = function (_message) {
        if ($('.message')) $('.message').html(_message).addClass('fail');
        $('<div/>', {
            class: 'message float-left success glyphicon glyphicon-warning-sign'
        }).insertBefore('.message');
    }
    ClearCommentMessage = function () {
        $('.message').html('');
        $('#message-gryphicon').remove();
    }
    AppendForm = function (result) {
        //$('form a').show();
        $('#new-group').remove();
        var $cont = $('<div/>');
        $($cont).attr({ 'id': 'new-group', 'class': 'list-group-item' });
        $($cont).append(result);
        return $cont;
    }
    setCheckBoxFunction = function () {
        $('input[data-type=total]').on('click', function (event) {
            $id = $(this).attr('data-id');
            $('input[data-parent-id=' + $id + ']').prop('checked',$(this).prop('checked'));

        });
        $('input[data-type=partial]').on('click', function (event) {
            $id = $(this).attr('data-parent-id');
            $select = true;
            $.each($('input[data-type=partial][data-parent-id=' + $id + ']'), function (e, i) {
                if (!i.checked) { $select = false; }
            });
            $('input[data-type=total][data-id=' + $id + ']').prop('checked',$select);
        });
    }