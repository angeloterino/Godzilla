$(document).ready(function () {
    $.getScript('../../Scripts/bootstrap.inputs/bootstrap.file-inputs.js')
    .done(function (a, b) { $('input[type=file]').bootstrapFileInput(); })
    .fail(function (a, b, c) { alert(c); });
});
$.strawmanVars = new Object();
StrwmAjaxCall = function () {
    $.strawmanVars.waitForResponse = true;
    $.ajax({
        url: $.strawmanVars.url,
        data: $.strawmanVars.data,
        async: true,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
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

}
    //Ejecución del Trigger
    var ExecuteTrigger = function () {
        $.strawmanVars.data = { 'transactionId': $.strawmanVars.transactionID[$.strawmanVars.transactionIndex] };
        $.strawmanVars.url = "ExecuteStoredProcedure";
        $.strawmanVars.result = RefreshTriggerStatus;
        StrwmAjaxCall();
    }
    //Carga de datos en bbdd
    var LoadData = function (_url, _controller, _file_name, _file_type) {
        var wrapper = '.form-wrapper';
        HideForm(wrapper, true);
        $.strawmanVars.data = { 'fileName': _file_name, 'fileType': _file_type };
        $.strawmanVars.url = _url;
        $.strawmanVars.controller = _controller;
        $.strawmanVars.result = ExecuteTrigger;
        StrwmAjaxCall();
    }
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
            StrwmAjaxCall();
        }
    }
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
        var _button_wraper = $('<div>').addClass('bottom close-viewer').attr('style', 'width: 100%; bottom: 8px; position: absolute;').append(_button).append(_clearfix);
        var _body = $('<div>').addClass('panel-body').append(_display_icon).append(_display_content);
        var _cont = $('<div>').addClass('panel').addClass('panel-info').attr({ 'style': 'height: 100%; font-size: 1.5em; position: relative;', 'id': 'panel-info' }).append(_header).append(_body).append(_button_wraper)
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
        $.strawmanVars.viewer = _cont;
//      <div class="panel panel-info" style="height: 100%; font-size: 1.5em;">
//          <div class="panel-heading"><span class="fa fa-info-circle" style="margin-right: 14px;"></span><span class="info-display">Loading process running...</span></div>
//          <div class="panel-body"><li class="fa fa-cloud-upload" style="margin-right: 12px;"></li><span>infoinoinonf finonfofin </span></div>
//      </div>
    });

    