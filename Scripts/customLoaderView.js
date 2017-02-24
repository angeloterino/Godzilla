$(document).ready(function () {
    $.getScript('../../Scripts/bootstrap.inputs/bootstrap.file-inputs.js')
    .done(function (a, b) { $('input[type=file]').bootstrapFileInput(); })
    .fail(function (a, b, c) { alert(c); });
});
$.strawmanVars = new Object();
StrwmAjaxCall = function (_type) {
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
        .done(function (result) {
            $.strawmanVars.waitForResponse = false;
            _result = $.parseJSON(result);
            if (!_result['Success']) {
                alertModal(_result['Message'], _result['Alert'], _result['BtnText']);
            }
            else {
                $.strawmanVars.transactionID = $.strawmanVars.transactionID == null && _result['Transaction'] != null ? _result['Transaction'] : $.strawmanVars.transactionID;
                $.strawmanVars.resultData = _result;
                $.strawmanVars.result.call();
            }
        })
        .fail(function (xhr, status) {
            //alert(status);
        });
        $.strawmanVars.data = null;

}
    //Ejecución del Trigger
    var ExecuteTrigger = function () {
        $.strawmanVars.data = { 'transactionId': $.strawmanVars.transactionID[$.strawmanVars.transactionIndex] };
        $.strawmanVars.url = "ExecuteStoredProcedure";
        $.strawmanVars.result = RefreshTriggerStatus;
        StrwmAjaxCall('GET');
    }
    //Carga de datos en bbdd
    var LoadData = function (_url, _controller, _file_name, _file_type) {
        $.strawmanVars.data = { 'fileName': _file_name, 'fileType': _file_type };
        $.strawmanVars.url = _url;
        $.strawmanVars.controller = _controller;
        $.strawmanVars.result = ConfigData;
        StrwmAjaxCall('GET');
    }
    var ConfigData = function () {
        //$.strawmanVars.url = 'LoadConfig';
        //$.strawmanVars.controller = 'Config';
        $.get('/Config/LoadConfig', function (i, e, x) {
            $(document).find('.panel-body').html(i);
        });
        //$.strawmanVars.result = UpdateConfigData;
        //StrwmAjaxCall();
    };
    var UpdateConfigData = function () {
        var _result = $.strawmanVars.resultData;
    };
    //Registro del Trigger
    var RefreshTriggerStatus = function () {
        $.strawmanVars.url = "RefreshTriggerStatus";
        $.strawmanVars.data = { 'transactionId': $.strawmanVars.transactionID[$.strawmanVars.transactionIndex], 'lastId': $.strawmanVars.resultID };
        $.strawmanVars.result = ShowTriggerStatus;
        $.strawmanVars.resultStatus = setInterval("AjaxCallWaitFor()", 1000);
    }

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
    }

    var ShowTriggerStatus = function () {
        var result = $.strawmanVars.resultData;
        var wrapper = '.form-wrapper';
        HideForm(wrapper,true);
        //Obtener datos del log por transacción
        var transaction = $.strawmanVars.transactionID[$.strawmanVars.transactionIndex];
        var result_id = result['LastId'];
        //Mostrar datos del mensaje
        var message = result['Message'];
        $('.display_content').html(message[0]);
        if ($.strawmanVars.resultID != result_id) {
            if (result['Status'] != 'Running' && result['Status'] != 'Success') {
                var is_last_trans = ($.strawmanVars.transactionID.length <= $.strawmanVars.transactionIndex + 1);
                if (is_last_trans){
                    var panel_info = (result['Status'] == 'Error') ? 'error' : 'default';
                    if (result['Status'] == 'Done') panel_info = 'done';
                    var html_info = (result['Status'] == 'Done') ? result['Status'] + '!' : result['Status'];
                    $('#panel-info').trigger('changeStatus', [panel_info, html_info]);
                    $('.close-viewer button').trigger('changeStatus', 'close');
                    clearInterval($.strawmanVars.resultStatus);
                } else
                    $.strawmanVars.transactionIndex++;
            }
            $.strawmanVars.resultID = result_id;
        }
        $.strawmanVars.data = { 'transactionId': $.strawmanVars.transactionID[$.strawmanVars.transactionIndex], 'lastId': $.strawmanVars.resultID };
    }
    var HideForm = function (wrapper, appendViewer) {
        if ($(wrapper + ' form').css('display') != 'none') {
            //Fijamos el alto del contenedor
            $(wrapper).height($(wrapper).height());
            $(wrapper + ' form').hide();
            if (appendViewer) {
                //Añadimos el visor de eventos
                $(wrapper).append($.strawmanVars.viewer);
                $('.close-viewer button').trigger('changeStatus', 'default');
                //$(document).on('click', '.close-viewer button', function (e) {
                //    e.stopPropagation();
                //    e.preventDefault();
                //    $(this).closest('.panel-info').parent().find('form').show();
                //    $(this).closest('.panel-info').remove();
                //});
            }
        }
    }
    var AjaxCallWaitFor = function(){
        if (!$.strawmanVars.waitForResponse) {
            StrwmAjaxCall('GET');
        }
    };
    var RestoreSaveButton = function(){
      $(document).find('button li.fa').attr('class','fa fa-save');
    };
    $(document).on('click', 'button[data-type=save]', function (e) {
        e.preventDefault();
        $.strawmanVars.url = $(this).attr('data-action');
        $.strawmanVars.controller = $(this).attr('data-controller');
        var _this = this;
        $(_this).find('li').attr('class', 'fa fa-spinner fa-spin');
        $.strawmanVars.result = RestoreSaveButton;
        $.strawmanVars.url = '/' + $.strawmanVars.controller + '/' + $.strawmanVars.url;
        StrwmAjaxCall('GET');
    });
    $(document).on('change', 'select[data-type=change_origin]', function (e) {
        var _market = $(this).attr('data-market');
        var _brand = $(this).attr('data-brand');
        var _channel = $(this).attr('data-channel');
        var _source = $(this).attr('data-source');
        var _value = $(this).val();
        var data = { "market": _market, "brand": _brand, "channel": _channel, "source": _source, "value": _value };
        $.strawmanVars.url = 'SaveConfigItem';
        $.strawmanVars.controller = 'Config';
        $.strawmanVars.result = function () { return null; }; 
        $.strawmanVars.data = data;
        StrwmAjaxCall('GET');
    });

    /*$(document).on('click', 'button[data-type=save]', function(e){
       e.preventDefault();
       var _controller = $(this).attr('data-controller');
       if (_controller!= null) _controller = '/' + _controller;
       var _action = $(this).attr('data-action');
       if (_action!= null) _action = '/' + _action;
       var _url = _controller + _action;
       var _this = this;
       if($.strawmanVars.change_data.length > 0){
           $(_this).find('li').attr('class', 'fa fa-spinner fa-spin');
           $.get(_url,$.strawmanVars.change_data, function(i,e,x){
              var result = $.parseJSON(i);
              if(result.status == 'success'){
                $(_this).find('li').attr('class','fa fa-save');
              } 
           });
       }
    });*/

    $(document).on('click', '.grid-pager a', function (event) {
        event.preventDefault();
        _target = '/' + $(this).closest('.wrapper').attr('data-controller') + '/' + $(this).closest('.wrapper').attr('data-action');
        _href = _target + $(this).attr('href');
        var _this = this;
        $.get(_href, function (i, e, x) { 
            _html = i.replace('\r\n', '').trim();
            $obj = $.parseHTML(_html);
            $(_this).closest('.wrapper').find('.table tbody').html($($obj).find('table tbody').html());
            $(_this).closest('.wrapper').find('.grid-pager').html($($obj).find('.grid-pager').html());
        });

    });
    $(document).on('click', '.grid-mvc table thead th a', function (event) {
        event.preventDefault();
        var _target = '/' + $(this).closest('.wrapper').attr('data-controller') + '/' + $(this).closest('.wrapper').attr('data-action');
        _href = _target + $(this).attr('href');
        var _this = this;
        $.get(_href, function (i, e, x) { 
            _html = i.replace('\r\n', '').trim();
            $obj = $.parseHTML(_html);
            $(_this).closest('.wrapper').find('.table tbody').html($($obj).find('table tbody').html());
            $(_this).closest('.wrapper').find('.grid-pager').html($($obj).find('.grid-pager').html());
            $(_this).closest('.wrapper').find('.grid-mvc table thead').html($($obj).find('table thead').html());
        });
    });
    $(document).ready(function () {
        $.strawmanVars.waitForResponse = false;
        $.strawmanVars.resultID = 0;
        $.strawmanVars.transactionIndex = 0;
        $.strawmanVars.resultStatus = function () { return false };
        var header_message = "Loading process running...";
        //visor de eventos
        var _info_icon = $('<li>').addClass('fa fa-info').attr('style', 'margin-right: 12px;');
        var _info_content = $('<span>').addClass('info-content').html(header_message);
        var _header = $('<div>').addClass('panel-heading').append(_info_icon).append(_info_content);
        var _display_icon = $('<li>').addClass('fa fa-cloud-upload').attr('style', 'margin-right: 12px;');
        var _display_content = $('<span>').addClass('display_content');
        var _button = $('<button>').addClass('btn pull-right');
        $(_button).bind('changeStatus', function (e, b) {
            $(this).addClass(b + '-wrapper');
            switch(b){
                case "default":
                    $(this).removeClass('close-wrapper').html('Wait...');
                    break;
                case "close":
                    $(this).removeClass('default-wrapper').html('Close');
                    $(this).click(function (e) {
                        e.stopPropagation();
                        e.preventDefault();
                        if ($(this).hasClass('close-wrapper')) {
                            $(this).closest('.panel-info').parent().find('form').show();
                            $(this).closest('.panel-info').remove();
                        }
                    });
                break;
            }
        });
        var _clearfix = $('<div>').addClass('clearfix');
        var _button_wraper = $('<div>').addClass('bottom close-viewer').append(_button);
        var _body = $('<div>').addClass('panel-body').append(_display_icon).append(_display_content);
        var _cont = $('<div>').addClass('panel').addClass('panel-info').attr({'id': 'panel-info' }).append(_header).append(_body).append(_button_wraper)
                              .bind('changeStatus', function (e, b, html_info) {
                                  switch (b) {
                                      case 'default':
                                          $(this).attr('class','panel').addClass('panel-info');
                                          break;
                                      case 'error':
                                          $(this).attr('class', 'panel').addClass('panel-danger');
                                          $(this).find('.info-content').html(html_info);
                                          break;
                                      case 'done':
                                          $(this).attr('class', 'panel').addClass('panel-success');
                                          $(this).find('.info-content').html(html_info);
                                          break;
                                  }
                              });
        $(_clearfix).insertAfter($(_cont).find('.bottom.close-viewer'));
        $.strawmanVars.viewer = _cont;
//      <div class="panel panel-info" style="height: 100%; font-size: 1.5em;">
//          <div class="panel-heading"><span class="fa fa-info-circle" style="margin-right: 14px;"></span><span class="info-display">Loading process running...</span></div>
//          <div class="panel-body"><li class="fa fa-cloud-upload" style="margin-right: 12px;"></li><span>infoinoinonf finonfofin </span></div>
//      </div>
    });
    $(document).on('click','button[data-type=submit_data]',function(event){
        event.preventDefault();
        var _url  = 'ProcessTransaction';
        var _controller = 'Forms';
        var _file_name = $(this).attr('data-file-name');
        var _file_type = $(this).attr('data-file-type');
        var wrapper = '.form-wrapper';
        HideForm(wrapper, true);
        $.strawmanVars.data = { 'fileName': _file_name, 'fileType': _file_type };
        $.strawmanVars.url = _url;
        $.strawmanVars.controller = _controller;
        $.strawmanVars.result = ExecuteTrigger;
        StrwmAjaxCall('GET');
    });

    