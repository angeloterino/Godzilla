$.strawmanVars = new Object();
$(document).ready(function () {
    $(document).on("click", ["a[role=menuitem]", "a[role=tabitem]"], function () {
        $.strawmanVars.cancelLoad = true;
    });
    //$('.grid-wrap').css({ 'height': '100%' });
    //$('.loader').clone().css({ 'display': 'block' }).appendTo('.grid-wrap');
    //Carga de datos por Channel
    var _lastrow, _lastcolor, _lastfontcolor;
    var _paths = 'BoyViews';
    var _path, _pathboys = ['/ChartJJVInternal/'];
    var _loader = '<tr class="removeme"><td colspan="3" rowspan="5" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px;"><img id ="rotate" src="../images/loading_anim.gif"/></td></tr>';
    var _dvloader = '<tr class="removeme"><td colspan="4" rowspan="5" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px;"><img id="rotate" src="../images/loading_anim.gif"/></td></tr>';
    var _pvloader = '<tr class="removeme"><td colspan="5" rowspan="5" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px;"><img id ="rotate" src="../images/loading_anim.gif"/></td></tr>';
    var _controls = ['ByChannel','ByFranchise'];
    var _genericControl = 'GetChart';
    var _viewsids = ['_GenericChart'];
    var _channel = ($('#channelID')) ? $('#channelID').attr('value') : 'MASS';
    //Primera llamada a la carga ajax de las tablas
    $('.mastertable').hide();
    $('.loading-bar').css({ 'border-color': 'rgb(0,0,220)' });
    loadTables(0, 0);

    var total_size = _pathboys.length * _controls.length;
    var partial_size = 0;
    $.strawmanVars.cancelLoad = false;
    function loadTables(pathindex, controlindex) {
        //Comprobamos la variable que indica si son Market o Brand a cargar
        //Se asigna el array con la carpeta de las vistas a la variable auxiliar.
        var _auxpath = _pathboys;
        if (pathindex < _auxpath.length) {
            //Si el indice es menor que el número de carpetas, entro.
            //Asigno a la variable de la carpeta el registro actual.
            _path = _auxpath[pathindex];
            //Agrego el gif de carga
            $('#' + _paths + _viewsids[controlindex]).append(_loader);
            $('.loading-info').text(_path + _controls[controlindex]);
            //Se realiza la llamada Ajax por carpeta + funcion del controlador en el arrray _controls
            if (!$.strawmanVars.cancelLoad) {
                $.ajax({
                    url: _path + _genericControl,
                    data: { 'type': _controls[controlindex] },
                    async: true,
                    contentType: 'application/html; charset=utf-8',
                    type: 'GET',
                    dataType: 'html'
                })
                .done(function (result) {
                    //Agregamos el resultado a la tabla por id
                    //$('#' + _paths + _viewsids[controlindex]).append(result);
                    $('.mastertable').append(result); // Test
                    //Eliminamos el gif de carga
                    $('.removeme').remove();
                    //Seguimos con la siguiente función del controlador
                    controlindex++;
                    if (controlindex < _controls.length) {
                        partial_size++;
                        $('.loading-bar').animate({ 'border-color': 'rgb(100,100,255)' }, 'slow', function () {
                            $('.loading-bar').animate({ width: ((partial_size * 100) / total_size) + '%' }, 'slow');
                        });
                        loadTables(pathindex, controlindex);
                    } else {
                        //Hemos finalizado el array de controladores. Seguimos con la siguiente carpeta de vista. 
                        loadTables(pathindex + 1, 0);
                    }
                })
                .fail(function (xhr, status) {
                    $('.loading-bar').css({ 'border-color': 'rgb(220,0,0)' });
                    //alert(status);
                    controlindex++;
                    if(controlindex < _controls.leght) loadTables(pathindex, controlindex);
                });
            }
        } else {
            //Se ha finalizado la carga. De momento no hacemos nada, aquí habría que mostrar los datos, ocultando el div de carga.
            $('.mastertable').show();
            $('.loading-bar').animate({ 'border-color': 'rgb(0,220,0)' }, 'slow');
            $('.loading-bar').animate({ width: '100%', 'border-color': 'rgb(0,220,0)' }, 'slow', function () {
                $('.load-mask').animate({ opacity: 'hide' }, 'slow', function () { $(this).hide() });
            });
        }
    }
});