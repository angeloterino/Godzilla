var _loader = $('<div/>').attr('class','removeme');
$.strawmanVars = new Object();

var StrwmCall = function (_type) {
    $.strawmanVars.waitForResponse = true;
    if ($.strawmanVars.controller != null && $.strawmanVars.controller.length > 0)
        $.strawmanVars.url = '/' + $.strawmanVars.controller + '/' + $.strawmanVars.url;
    $.ajax({
        url: $.strawmanVars.url,
        data: $.strawmanVars.data,
        async: true,
        contentType: 'application/html; charset=utf-8',
        type: _type,
        dataType: 'html'
    })
        .done(function (result, i,e ,dataType) {
            $.strawmanVars.waitForResponse = false;
            _result = result;
            $.strawmanVars.resultData = _result;
            $.strawmanVars.result.call();
            
        })
        .complete(function(h,t){
            $('.removeme').remove();
        })
        .fail(function (xhr, status) {
            alert(status);
        });
        $.strawmanVars.data = null;
}
$(document).on('click', 'a[data-btn-type=new-group]', function (event) {
    event.preventDefault();
    var _target = $(this).attr('data-btn-target-id');
    var _modal = $(document).find('[data-name=' + _target + ']');
    $.strawmanVars.url = $(this).attr('data-btn-target-url');
    $.strawmanVars.result = ShowModalForm;
    StrwmCall('GET');
});
$(document).on("click", "a[data-btn-type=edit-group]", function () {    
    var _this = this;
    var _opt = $('select[data-type=select_source]').val();
    var _attr = $(_this).closest('.panel').find('form a.active').attr('data-btn-param');
    var _target = this.getAttribute("data-btn-target-id");
    var _url = this.getAttribute("data-btn-target-url");
    var _params = { "_option":_opt, "_id": _attr };
    if ($('select[data-type=edit-select]').length > 0) { _params = { "_selectedId": this.getAttribute("btn-param"), "_selectValue": $('select[data-type=edit-select]').val() }; };
    $(_this).closest('.panel-body').append(_loader);
    $.strawmanVars.url = _url;
    $.strawmanVars.data = _params;
    $.strawmanVars.result = ShowModalForm;
    StrwmCall('GET');
});
$(document).on('click','a[data-btn-type=show-group]',function(event){
    event.preventDefault(); 
    var _this = this;
    ShowItemsForm(_this);
});
$(document).on('click','a[data-btn-type=add-item-group]',function(event){
   event.preventDefault();
   var _this = this;
   var _wrapper = $(_this).closest('div.panel').find('div.panel-body');
   var _master = $(document).find('div[data-target=items_viewer] a.active');
   var _url = $(_this).attr('data-btn-target-url');
   var _opt = $('select[data-type=select_source]').val();
   var _data = {_option : _opt, _channel : $(_this).attr('data-btn-param'),_group: $(_master).attr('data-btn-param')};
   $(_wrapper).css('position','relative').append(_loader);
   $.get(_url, _data, function(i,e,h){
       var _html = $.parseHTML(i);
       $(_wrapper).html($('<div/>').append(_html).find('div.panel-body').html());
       $(_this).closest('.panel-footer').html($('<div/>').append(_html).find('.panel-footer').html());
       $(document).find('.removeme').remove();
   });
});
$(document).on('click','a[data-btn-type=close-item-group]',function(event){
    event.preventDefault();
    ShowItemsForm($('#list-groups').find('a.active'));
});
$(document).on('click','a[data-btn-type=save-item-group],a[data-btn-type=save-group]',function(event){
    event.preventDefault();
    $(this).closest('div.panel').find('form').submit();
});
$(document).on('click','button[data-btn-type=submit]',function(event){
    event.preventDefault();
    var _text = $(this).html();
    var _icon = $('<li/>').attr('class','fa fa-spinner fa-spin');
    $(this).html('').append(_icon).append(_text);
    $(this).closest('.modal').find('form').submit();
});
$(document).on('change','input[type=checkbox]',function(event){
    event.preventDefault();
    if($(this).parent().find('input[type=hidden][name=' + $(this).attr('name')+ ']').attr('data-changed') != 'true')
    {
        $(this).parent().find('input[type=hidden][name=' + $(this).attr('name')+ ']').attr('data-changed','true');
        $(this).parent().find('input[type=hidden][name=' + $(this).attr('name')+ ']').val(true);
    }
});
$(document).on('change','select[data-type=select_source]', function(event){
    $('div.panel .panel-body').append(_loader);
    SelectSourceChange(this);
});
var ShowMasterForm = function(_this){
    var _panel =  '#list-groups';
    $(_panel).append(_loader);
    var _param = $(_panel).find('a.active').attr('data-btn-param');
    var _html = $('<div/>').html($.strawmanVars.resultData);
    $(_panel).find('form').html($(_html).find('form').html());
    $(_panel).find('a[data-btn-param=' + _param + ']').addClass('active');
    $('#items_viewer .panel-body,#items_viewer .panel-footer').remove();
    $('.removeme').remove();
};
var ShowItemsForm = function(_this){
    var _wrapper = $(_this).closest('div');
    $(_wrapper).find('a.active').removeClass('active');
    $(_this).addClass('active');
    var _target = '#' + $(_wrapper).attr('data-target');
    $(_target).css('position','relative');
    var _url =$(_wrapper).attr('data-controller');
    var _param = {'_option':$('select[data-type=select_source]').val(),'_group':$(_this).attr('data-btn-param')};
    $(_wrapper).css('position','relative').append(_loader);
    $.get(_url,_param, function(i,e,h){
        var _html = $.parseHTML(i);
        if($(_target).find('.panel-body').length<=0)
            $(_target).find('.panel-heading').after(_html);
        else
        {
            $(_target).find('div.panel-body').html($('<div/>').append(_html).find('div.panel-body').html());
            $(_target).find('.panel-footer').html($('<div/>').append(_html).find('.panel-footer').html());
        }
        $(document).find('.removeme').remove();
    });
};
var ShowModalForm = function(){
    var _modal = $(document).find('div.modal');
    var _data = $.strawmanVars.resultData;
    var _html = _data;
    $(_modal).find('.modal-header').html($(_html).find('.modal-header').html());
    if($(_html).find('.modal-nav').length > 0)
        $(_modal).find('.modal-nav').html($(_html).find('.modal-nav').html());
    else
        $(_modal).find('.modal-nav').remove();
    $(_modal).find('.modal-body').html($(_html).find('.modal-body').html());
    $(_modal).find('.modal-footer').html($(_html).find('.modal-footer').html());
    $(_modal).find('div.modal-header h4').append(' '+$('select[data-type=select_source] option:selected').text());
    $(_modal).modal('show');
};

var alertModal = function (_message, _alert, _btn_text) {
    var wrapper = $('<div id="alert_modal" class="panel panel-danger form-wrapper modal-content"></div>');
    var wrapper_alert = $('<div class="modal-body"></div>');    
    var alert_label = $('<label class="input-validation-error error" style="font-size:12px;">Se ha producido un error al cargar el archivo.</label>');
    var glyphicon = $('<span class="btn glyphicon glyphicon-alert" style="font-size:20px;"></span>');
    var wrapper_message = $('<div class="modal-body" style="font-size:12px;"></div>');
    var message_label = $('<label class="caption"></label>');
    var wrapper_button = $('<div class="modal-body"></div>');
    var button_modal = $('<button class="btn btn-toolbar btn-success center-block" text="Aceptar"> Aceptar</button>');

    if (_message != null) $(message_label).html(_message);
    if (_alert == null) _alert = $(alert_label).html();
    $(alert_label).html('').append(glyphicon).append(_alert);
    if (_btn_text != null) $(button_modal).html(_btn_text);

    $(button_modal).click(function () {
        var wrapper = $(this).closest('.form-wrapper');
        $(wrapper).hide().toggleClass('form-wrapper');
        $('.form-wrapper').show();
        $(wrapper).remove();
    });

    $(wrapper_alert).html(alert_label);
    $(wrapper_message).html(message_label);
    $(wrapper_button).html(button_modal);

    $(wrapper).append(wrapper_alert).append(wrapper_message).append(wrapper_button);
    $('.form-wrapper').hide();
    $(wrapper).insertBefore('.form-wrapper');
};
var FormSucces = function(){
    $(document).find('.modal btn li.fa').remove();
    $('.modal').modal('hide');
    SelectSourceChange($(document).find('select[data-type=select_source]'));
};
var SelectSourceChange = function(_select){
    $('a.btn.disabled').removeClass('disabled');
    $(document).find('a.btn[data-disabled=' + $(_select).val()+']').addClass('disabled');
    $.strawmanVars.url = $(_select).attr('data-controller');
    $.strawmanVars.data = {'_option': $(_select).val()};
    $.strawmanVars.result = ShowMasterForm;
    StrwmCall('GET');  
};
var UpdateSuccess = function(type){
    if(type == "ADD_ITEMS"){
        ShowItemsForm($('#list-groups').find('a.active'));
    }
}