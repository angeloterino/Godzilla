$.saveMenu = $('<div/>');
$.formId = null;

$(document).ready(function () {
    //Funcionalidad del botón editar formulario
    $(document).on('mouseover mouseenter', 'thead[data-editable=true]', function (i, e, d) {
        if ($(document).find('.edit-wrapper button li.fa-spinner').length > 0) return false;
        if (!$(this).find('table[data-type=edit_enabled]').length > 0) {
            $(this).closest('table').attr('data-type', 'edit_enabled')
            if (!$(this).closest('div').hasClass('editor'))
                $(this).closest('div').css('position', 'relative').addClass('editor');
            $($.strawmanVars.editButton).css('height', $(this).height());
            $($.strawmanVars.editButton).css({ 'top': '-1px' });
            $($.strawmanVars.editButton).on('mouseleave', function () {
                HideEditor(this);
            });
            $($.strawmanVars.editButton).appendTo($(this).closest('div')).show();
        }
    });
    $(document).on('mouseout', '.edit_wrapper', function (i, e, d) {
        HideEditor(this);
    });
    $(document).on("change paste", "input[type='text'][data-control-type='txt_kpi_cfg']", function () {
        if ($('.boy.save_wrapper').length <= 0) {
            $($.saveMenu).css('display', '');
            if ($('#left-column').length <= 0) $('#wrapper_main').append($.saveMenu);
            else $('#wrapper_main').parent().append($.saveMenu)
        } else
            $($.saveMenu).show();
        $($.refreshButton).removeClass('disabled');
    });
    //Botón de edición de los comentarios
    $.strawmanVars.editButton = $('<div/>').addClass('edit-wrapper');
    var _ebutton = $('<button/>').addClass('float-left').attr('data-target', 'kpi_editor');
    var _li = $('<li/>').addClass('fa fa-edit');
    $(_ebutton).on('click', function (e) {
        e.preventDefault();
        if ($(this).find('li').hasClass('fa-edit')) {
            $(this).find('li').attr('class', 'fa fa-spinner fa-spin');
            ShowForms($(this).attr('data-target'), $(this).closest('div.editor'));
        } else {
            $(this).parent().closest('.editor').find('form').remove();
            $($.strawmanVars.editButton).find('li').removeClass('fa-times').addClass('fa-edit');
        }
    });
    $(_ebutton).append(_li).appendTo($.strawmanVars.editButton);

    //Menu salvar datos
    var _wrapper = $('<div/>').attr({ 'class': 'boy save_wrapper', 'data-type': 'submit_form' });
    var _button = $('<button/>').attr({ 'data-type': 'show_save', 'class': 'btn btn-default show_save' }).append($('<li/>').attr('class', 'fa fa-times'));
    $(_wrapper).append(_button);
    var _wrapperbtn = $('<div/>').attr('class', 'save_buttons wrapper');
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
        } else {
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
            $('.editor').find('form').remove();
            $('.save_wrapper').hide();
            $($.strawmanVars.editButton).find('li').removeClass('fa-times').addClass('fa-edit');
        }
        return false;
    });
    $.saveMenu = _wrapper;
});
var HideEditor = function (wrapper) {
    $(wrapper).closest('table').attr('data-type', '');
    //if(!$(wrapper).find('button li').hasClass('fa-spinner')){
    //    $(wrapper).find('button li').attr('class','fa fa-edit');
    $(wrapper).hide();
    //}
}
var ShowForms = function (_source, wrapper) {
    var _id = $(wrapper).attr('data-letter-id');
    var _name = $(wrapper).attr('data-value-name');
    //cargamos el modal con el formulario que muestra los mensages.
    $.get('../Config/KPIEditor', { source: _source }, function (d, t, j) {
        $(wrapper).append(d);
        var _wrapper = $(wrapper).find('.edit-wrapper');
        $.formId = $(wrapper).find('form');
        $('.edit-wrapper button li').attr('class', 'fa fa-times');
        HideEditor($(wrapper).find('.edit-wrapper'));
    });
}
var SubmitForm = function () {
    $.formId = null;
    $($.saveButton).find('li').attr('class','fa fa-spinner fa-spin');
}
var UpdateSuccess = function () {
    $('.editor').find('form').remove();
    $(document).find('thead[data-editable=false]').attr('data-editable', true);
    $($.saveButton).find('li').attr('class', 'fa fa-save');
    $('.edit-wrapper button li').attr('class', 'fa fa-edit');
    $($.refreshButton).addClass('disabled');
    $('.save_wrapper').hide();
    RefreshData();
}
var RefreshData = function(){
     loadTables(0, 2);
}
