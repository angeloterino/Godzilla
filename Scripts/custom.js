$.getScript('../../Scripts/table.utils/table.blocker.js');
strawmanVars = new Object();
strawmanVars.cancelLoad = false;
strawmanVars.headerLock = false;
var total_size, partial_size = 0;
var _collapser = '<div class = "collapser">-</div>';
var _lastrow, _lastcolor, _lastfontcolor;
var _paths = 'Market';
var _path, _marketpaths = ['/MarketView/', '/MarketViewChannel/', '/MarketViewFranchise/', '/MarketViewKeybrands/'], _brandpath = ['/BrandView/', '/BrandViewChannel/', '/BrandViewFranchise/', '/BrandViewKeybrands/'],
_sharepaths = ['/ValueShare/', '/ValueShareChannel/', '/ValueShareFranchise/', '/ValueShareKeybrands/'];
_ntspaths = ['/NTSView/', '/NTSViewChannel/', '/NTSViewFranchise/', '/NTSViewKeybrands/'];
var _loader = '<tr class="removeme"><td colspan="3" rowspan="5" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px;"><img id ="rotate" src="../images/loading_anim.gif"/></td></tr>';
var _dvloader = '<tr class="removeme"><td colspan="4" rowspan="5" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px;"><img id="rotate" src="../images/loading_anim.gif"/></td></tr>';
var _pvloader = '<tr class="removeme"><td colspan="5" rowspan="5" style="border: 0px; border-image: none; text-align: center; vertical-align: middle; min-height:100px;"><img id ="rotate" src="../images/loading_anim.gif"/></td></tr>';
var _stdControls = ['GetDataView', 'GetMonth', 'GetYTD', 'GetMAT', 'GetBTG', 'GetTotalCustom', 'GetBOY', 'GetPCVSPY'];
var _stdViewsids = ['_DataView', '_MonthView', '_YTDView', '_MATView', '_BTGView', '_TotalCustomView', '_BOYView', '_PCVSPYView'];
var _controls, _viewsids;
var _shareControls = ['GetLM', 'GetYTD', 'GetMAT', 'GetBTG', 'GetTotal', 'GetBOY'];
var _shareViewsids = ['_LMView', '_YTDView', '_MATView', '_BTGView', '_TOTALView', '_BOYView'];
var _NTScontrols = ['GetNTSView'];
var _NTSviewid = ['View']
$(document).ready(function () {
    $(document).on("click", ["a[role=menuitem]", "a[role=tabitem]"], function () {
        strawmanVars.cancelLoad = true;
    });
    //$('.grid-wrap').css({ 'height': '100%' });
    //$('.loader').clone().css({ 'display': 'block' }).appendTo('.grid-wrap');
    //Carga de datos por Channel
    

    //Funciones para poder colapasar columnas.
    $('.collapsable').each(function () {        
        $(this).css({ 'position': 'relative' });
        $(this).append(_collapser);
    });

    $('.collapser').click(function () {
        if ($(this).text() == '-') {
            $(this).parent().children('.grid-wrap').css({ 'overflow': 'hidden', 'width': '0px', 'margin-left': '28px' });
            $(this).text('+').css({ 'border': '1px solid rgb(0,0,0)' });
            $(this).parent().children('.grid-wrap');
        } else {
            $(this).parent().children('.grid-wrap').css({ 'overflow': 'visible', 'width': 'auto', 'margin-left': '0px' });
            $(this).text('-').css({ 'border-width': '0 1px' });
        }
    });
    //Primera llamada a la carga ajax de las tablas
    startLoadTables();
});
startLoadTables = function () {
    _viewsids = _stdViewsids;
    _controls = _stdControls;
    total_size = _marketpaths.length * _controls.length + _brandpath.length * _controls.length + _sharepaths.length * _shareControls.length;
    partial_size = 0;
    _paths = 'Market';
    if (!$('.load-mask').is(':visible')) {
        $('.load-mask').appendTo($('div#wrapper_main').parent());
        $('.loading-bar').width('1%');
        $('.load-mask').show();
    }//Si la máscara de carga está oculta la mostramos.
    strawmanVars.cancelLoad = false;
    strawmanVars.cacheSet = $('#cache_set').val();//Recogemos el estado de la caché.
    initializeTables();
    loadTables(0, 0);
}
initializeTables = function () {
    for (i = 0; i < _viewsids.length; i++)
    {
        $('#Market' + _viewsids[i] + ' tbody').html('');
        $('#Brand' + _viewsids[i] + ' tbody').html('');
    }
    for (i = 0; i < _shareViewsids.length; i++)
    {
        $('#ValueShare' + _shareViewsids[i] + ' tbody').html('');
    }
    $('#NTSView tbody').html('');
}
loadTables = function (pathindex, controlindex) {
    $('.loading-bar').css({ 'border-color': 'rgb(0,0,220)' });
    //Comprobamos la variable que indica si son Market o Brand a cargar
    //Se asigna el array con la carpeta de las vistas a la variable auxiliar.
    var _auxpath = (_paths == 'Market') ? _marketpaths : (_paths == 'Brand') ? _brandpath : (_paths == 'ValueShare')? _sharepaths: _ntspaths;
    if (pathindex < _auxpath.length) {
        //Si el indice es menor que el número de carpetas, entro.
        //Asigno a la variable de la carpeta el registro actual.
        _path = _auxpath[pathindex];
        //Agrego el gif de carga
        $('#' + _paths + _viewsids[controlindex] + ' tbody').append(_loader);
        $('.loading-info').text(_path + _controls[controlindex]);
        //Se realiza la llamada Ajax por carpeta + funcion del controlador en el arrray _controls
        if (!strawmanVars.cancelLoad) {
            $.ajax({
                url: _path + _controls[controlindex],
                async: true,
                data:{'cache':strawmanVars.cacheSet},
                contentType: 'application/html; charset=utf-8',
                type: 'GET',
                dataType: 'html'
            })
            .done(function (result) {
                strawmanVars.cancelLoad = false;
                //Agregamos el resultado a la tabla por id
                $('#' + _paths + _viewsids[controlindex] + ' tbody').append(result);
                //Eliminamos el gif de carga
                $('.removeme').remove();
                //Seguimos con la siguiente función del controlador
                controlindex++;
                if (controlindex < _controls.length) {
                    partial_size++;
                    strawmanVars.cancelLoad = false;
                    loadTables(pathindex, controlindex);
                }
                else {
                    //Hemos finalizado el array de controladores. Seguimos con la siguiente carpeta de vista. 
                    $('.loading-bar').animate({ 'border-color': 'rgb(100,100,255)' }, 'fast', function () {
                        $('.loading-bar').animate({ width: ((partial_size * 100) / total_size) + '%' }, 'fast');
                    });                    
                    loadTables(pathindex + 1, 0);
                }
            })
            .fail(function (xhr, status) {
                $('.loading-bar').css({ 'border-color': 'rgb(220,0,0)' });
                alert(status);
            });
        }

    } else {
        //Hemos finalizado el array de carpetas. Comprobamos si se ha renderizado la parte de Brands.
        if (_paths != 'NTS') {
            _paths = (_paths == 'Market') ? 'Brand' : (_paths == 'Brand') ? 'ValueShare' : 'NTS';
            if (_paths == 'ValueShare') {
                _viewsids = _shareViewsids;
                _controls = _shareControls;
            }
            if (_paths == 'NTS') {
                _viewsids = _NTSviewid;
                _controls = _NTScontrols;
            }
            strawmanVars.cancelLoad = false;
            loadTables(0, 0);
        }else{
            //Se ha finalizado la carga. De momento no hacemos nada, aquí habría que mostrar los datos, ocultando el div de carga.
            //Clonamos la columna de la izquierda para permitir la navegabilidad
            $('.mastertable').show();
            $('.loading-bar').animate({ 'border-color': 'rgb(0,220,0)' }, 'slow');
            $('.loading-bar').animate({ width: '100%', 'border-color': 'rgb(0,220,0)' }, 'fast', function () {
                $('.load-mask').animate({ opacity: 'hide' }, 'slow', function () {
                    if (!strawmanVars.headerLock) CloneLeftColumn('Market_DataView');
                    $periodCombo();
                    $(this).hide();
                    partial_size = 0;
                    strawmanVars.headerLock = true;
                    $('#cache_set').val(true);//Activamos la caché
                    checkCookies();
                });
            });

            //$('.load-mask').hide();                
        }
    }
}
