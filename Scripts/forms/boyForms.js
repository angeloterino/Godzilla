//Funcionalidad de los inputs del formulario BOY's
$.saveMenu = $('<div/>');
$.formId = null;
$(document).ready(function () {
    $(document).on("change paste", "input[type='text'][control-type='txt_boy_cfg']", function () {
        var _target = this.getAttribute("control-target-type");
        var _url = '../Forms/' +  this.getAttribute("control-target-url");
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
        var _this = this;
        //if (!($(this).closest('.wrapper').hasClass('selected'))) $(this).closest('.wrapper').toggleClass('selected').append(_loader);
        //$('.selected').append(_loader);
        //Loader para el formulario
        var _loader = $(_this).closest('div[data-type=partial_form]').find('div.close_form.wrapper');
        setLoader(_loader);
        //Reestablecemos el boton reset:
        $($.refreshButton).removeClass('disabled');
        $.ajax({
            url: _url,
            data: { '_id': _id, 'type': _control_type, 'brand': _brand, 'market': _market, 'channel': _channel, 'control': _column_id, 'column': _control_column_type, 'value': _value },
            async: true,
            contentType: 'application/html; charset=utf-8',
            type: 'GET',
            dataType: 'html'
        })
        .done(function (result) {
            var _result = $('<div/>').html(result);
            var _wrapper = $(_this).closest('div[data-wrapper=row]');
            $.each($(_result).find('div[data-editable=true]'), function () {
                $(_wrapper).find('div[data-type=' + $(this).attr('data-type') + ']').find('div[data-type=data_wrapper]').html($(this).find('div[data-type=data_wrapper]').html());
            });
            $.each($(_result).find('div[data-type=partial_form]'), function () {
                $(_wrapper).find('div[data-type=partial_form][data-source=' + $(this).attr('data-source') + ']').html($(this).html());
            });
            var _form = $(_result).find('form');
            $.formId = '#' + $(_form).attr('id');
            if ($($.formId).length <= 0) {
                $(document).find('div[id=body]').append(_form);
            } else {
                $.each($(_form).find('div[data-type=form_row]'), function () {
                    $(document).find($.formId).find('div[data-type=form_row][data-row=' + $(this).attr('data-row') + ']').html($(this).html());
                });
            }
            if ($('.boy.save_wrapper').length <= 0) {
                $($.saveMenu).css('display', '');
                if ($('#left-column').length <= 0) ('#wrapper_main').append($.saveMenu);
                else $('#wrapper_main').parent().append($.saveMenu)
            } else
                $($.saveMenu).show();
            //Eliminamos el gif de carga
            //$('.removeme').remove();

        })
        .fail(function (xhr, status) {
            alert(status);
            //$('.removeme').remove();
        })
        .complete(function () {
            setLoader(_loader);
        });
    });
    //Visualizar el botón que habilita los formularios para edición
    $(document).on('mouseenter mouseover', 'div[data-editable=true]', function () {
        //$('div[data-type=edit_enabled]').hide();
        if ($(document).find('button[data-type=edit_enabled] li.fa-spin').length > 0) return false;
        if (!$(this).find('div[data-type=edit_enabled]').length > 0 || $(this).find('div[data-type=edit_enabled]').css('display') == 'none') {
            var _row = $(this).closest('div[data-wrapper=row]').attr('data-row-id');
            var _channel = $('#channelID').val();
            var _this = this;
            $.each($('div[data-type=edit_enabled]'), function (i, e) {
               if(!$(this).find('li').hasClass('fa-spin'))
                    $(this).hide();
            });
            if ($(_this).find('div[data-type=edit_enabled]').length <= 0) {
                $.get('../Config/GetPermissionsByView', { '_view': 'BOY', '_row': _row, '_channel': _channel }, function (result) {
                    var _perms = result;
                    if (_perms.status) {
                        if ($(_this).find('div[data-type=edit_enabled]').length <= 0) {
                            var _div = $('div[data-type=edit_enabled][data-for=clone]').clone().removeAttr('data-for');
                            $(_div).find('button[data-type=edit_enabled]').attr('data-form', $(this).attr('data-type'));
                            $(_div).on('click', 'button[data-type=edit_enabled]', function () {
                                callEditForm(_this);
                            });
                            $(_this).css('position', 'relative').append($(_div).show());
                        }
                    }else
                        $(_this).attr('data-editable','');
                });
            } else {
                $(_this).find('div[data-type=edit_enabled]').show();
            }
        }
    });
    $(document).on('mouseout', 'div.edit_wrapper', function () {
        if(!$(this).find('li').hasClass('fa-spin'))
            $(this).hide();
    });

    //Funcionalidad del botón de ocultar formulario:
    $(document).on('click', 'button[data-type=close_form]', function (e) {
        e.preventDefault();
        var _form = $(this).closest('div[data-type=partial_form]');
        var _edit = $(this).closest('div[data-editable=false]');
        $(_edit).attr('data-editable', true);
        $(_form).remove();
    });
    //Menu salvar datos
    var _wrapper = $('<div/>').attr({'class':'boy save_wrapper','data-type':'submit_form'});
    var _button = $('<button/>').attr({ 'data-type': 'show_save', 'class': 'btn btn-default show_save' }).append($('<li/>').attr('class', 'fa fa-times'));
    $(_wrapper).append(_button);
    var _wrapperbtn = $('<div/>').attr('class','save_buttons wrapper');
    var _saveButton = $('<button/>').attr('class', 'btn btn-success submit').html($('<li/>').attr('class', 'fa fa-save')).append(' Save');
    $.saveButton = _saveButton;
    var _refreshButton = $('<button/>').attr('class', 'btn btn-danger reset').html($('<li/>').attr('class', 'fa fa-refresh')).append(' Reset');
    $.refreshButton = _refreshButton;
    $(_wrapperbtn).css('display', '').append($.saveButton).append($.refreshButton);
    $(_wrapper).append(_wrapperbtn);
    $(_wrapper).on('click', '.btn.show_save', function (e) {
        e.preventDefault();
        var _btnWrapper = $(this).parent().find('.save_buttons.wrapper');
        if ($(this).find('li').hasClass('fa-save')) {
            $(this).find('li').removeClass('fa-save').addClass('fa-times');
            $(_btnWrapper).show();
        }else{
            $(this).find('li').removeClass('fa-times').addClass('fa-save');
            $(_btnWrapper).hide();
        }
    });
    //Funcionalidad boton submit
    $(_wrapper).on('click', 'button.submit', function (e) {
        if ($.formId != null)
            $($.formId).submit();
        return false;
    });
    //Funcionalidad botón reset
    $(_wrapper).on('click', 'button.reset', function (e) {
        if ($.formId != null && !$(this).hasClass('disabled')) {
            RefreshData(_wrapper);
        }
        return false;
    });
    $.saveMenu = _wrapper;
});

var callEditForm = function (_editable) {
    $(_editable).find('div[data-type=edit_enabled]').find('li').attr('class', 'fa fa-spinner fa-spin');
    $(_editable).attr('data-editable', false);
    var _url = '../Config/GetBoyEdit';
    var _row = $(_editable).closest('div[data-wrapper=row]').attr('data-row-id');
    var _channel = $('#channelID').val();
    var _type = $(_editable).attr('data-type');
    if (_url != null) {
        $.get(_url, { '_type': _type, '_row': _row, '_channel': _channel }, function (result) {
            $(_editable).append(result);
            $(_editable).find('div[data-type=edit_enabled]').find('li').attr('class', 'fa fa-edit');
            $(_editable).find('div[data-type=edit_enabled]').css('display', 'none');
        });
    }
};

var SubmitForm = function () {
    $.formId = null;
    $($.saveButton).find('li').attr('class','fa fa-spinner fa-spin');
}

var UpdateBOYSuccess = function () {
    $.formId = $(document).find('form').attr('id');
    $(document).find('div[data-type=partial_form]').remove();
    $(document).find('div[data-editable=false]').attr('data-editable', true);
    $($.saveButton).find('li').attr('class', 'fa fa-save');
    $($.refreshButton).addClass('disabled');
    RefreshData();
}

var setLoader = function (_this) {
    $(_this).toggleClass('loading');
    if ($(_this).hasClass('loading')) {
        $(_this).find('li').attr('class', 'fa fa-spinner fa-spin');
    } else {
        $(_this).find('li').attr('class', 'fa fa-times');
    }
}

var RefreshData = function () {
    var _wrapper = $.saveMenu;
    $('div[data-editable=false]').attr('data-editable', true);
    if(_wrapper.length > 0) $(_wrapper).hide();
    $.resetForms = false;
    startLoadTables();
    $($.formId).remove();
}