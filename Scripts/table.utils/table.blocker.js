ColapseLeftColumn = function (column) {
    $('.left-collapsable').toggleClass('collapsed');
    $table = $('.left-collapsable').parent().find('table');
    $column_header = $(column).parent().find('table');
    $target = $('#left-column ' + _left_column + '-left');
    $(column).toggleClass('collapsed');
    if ($(column).hasClass('collapsed')) {
        $('.left-collapsable.collapsed').css({ 'max-width': 0, 'width': 0, overflow: 'hidden', 'white-space': 'nowrap' });  
        $($table.find('td')).each(function (i, e) {
            if (!$(this).hasClass('left-collapsable')) {
                $(this).css({ 'max-width': '50%', 'width': '50%' });
            }

        });
        
        
        $target.find('tr').each(function (i, e) { $(this).find('td').each(function (i, e) { if ($target.find('th:eq(' + (i) + ')').hasClass('left-collapsable')) $(this).toggleClass('collapsed'); }); });
        $(column).html('+');
    } else {
        $('.left-collapsable').css({ 'max-width': '12%', 'width': '12%', overflow: 'auto', 'white-space': 'normal' });
        $($table.find('th')).each(function (i, e) {
            if (!$(column).hasClass('left-collapsable')) {
                $(column).css({ 'max-width': '40%', 'width': '40%' });
            }
        });
        $target.find('tr').each(function (i, e) {
            if ($(this).find('td').hasClass('collapsed')) $(this).find('td.collapsed').toggleClass('collapsed');
            if ($(this).find('th').hasClass('collapsed')) $(this).find('th.collapsed').toggleClass('collapsed');
        });
        $(column).html('-');
    }
    $target.find('th').each(function (i, e) {
        $(this).css({ 'max-width': $table.find('th:eq(' + i + ')').css('width'), 'width': $table.find('th:eq(' + i + ')').css('width') });
    });
    $column_header.find('th').each(function (i, e) {
        $(this).css({ 'max-width': $table.find('th:eq(' + i + ')').css('width'), 'width': $table.find('th:eq(' + i + ')').css('width') });
    });
    $target.css({ 'max-width': $(column).hasClass('collapsed') ? '250px' : '590px', 'width': $(column).hasClass('collapsed') ? '250px' : '590px' });
    $target.find('th').each(function(i,e){$(this).css({ 'display': $(this).hasClass('collapsed') ? 'none' : '' })});
    $target.find('td').each(function(i,e){$(this).css({ 'display': $(this).hasClass('collapsed') ? 'none' : '' })});
    $column_header.css({ 'max-width': $(column).hasClass('collapsed') ? '250px' : '590px', 'width': $(column).hasClass('collapsed') ? '250px' : '590px' });
    $column_header.find('th').each(function (i, e) { $(this).css({ 'display': $(this).hasClass('collapsed') ? 'none' : '' }) });
    $('#left-column').css({ 'width': (parseInt($target.css('max-width').replace('px', '')) + 1) + 'px', 'max-width': (parseInt($target.css('max-width').replace('px', '')) + 1) + 'px' });
    $('#wrapper_main').css({ left: $target.css('max-width') });
    $('#header-fix').css({ left: $target.css('max-width') });
    return null;
}
CloneLeftColumn = function (left_column) {
    _left_column = '#' + left_column;
    ///Funciones para la visualización de los laterales y cabecera:
    //Lateral 
    var _left = $(_left_column + ' thead').width();
    var _top = $(_left_column + ' thead').height();
    $('#wrapper_main').attr('style', 'right: 0px; bottom: 0px; overflow: scroll; position: absolute;').css({'top':_top,'left':_left}); //Estilo de la parte navegable para hacer sitio para el lateral
    header_fix = $('<div/>', { id: 'header-fix', style: 'position: absolute; top: 0px;overflow:hidden;right:0px;padding-top:1px;background-color:#FFF' }).css('left',_left);
    //COLUMNA IZQUIERDA
    $('<div/>', {
        id: 'left-column',
        style: 'left: 0px; top: 1px; right: 0px; bottom: 0px; overflow: hidden; position: absolute;',
        html: $(_left_column).clone().attr({ 'id': $(_left_column).attr('id') + '-left','style':'' }).css({'margin-bottom':'18px','width':_left}),
        class: 'wrapper mastertable',
    }).appendTo($('#wrapper_main').parent());
    $(header_fix).appendTo($('#wrapper_main').parent());

    //CABECERA
    $('<div/>', { style: 'margin:0;font-size:11px;', class: 'wrapper' }).appendTo(header_fix);
    var wrapper_width = 0;
    $('#wrapper_main .mastertable table').each(function () {
        if ($(this).attr('id') != left_column) {
            wrapper_width += 5 + $(this).width();
            $(this).css({ 'width': $(this).width() });
            $(this).clone()
                .appendTo($('#header-fix .wrapper'))
                .css({ float: 'left', 'margin-left': $(this).closest('td').css('padding-left'), 'width': $(this).width() })
                .attr({ 'id': $(this).attr('id') + '-fix' })
                .find('tbody').remove();
        }
    });
    $('#header-fix .wrapper').css({ 'width': wrapper_width + 22, 'overflow':'hidden', 'padding-right':'20px'});
    //$('<div>', { style: 'display:block;float:left;width:18px;height:100px;' }).appendTo('#header-fix .wrapper');

    //Bloqueo de la cabecera de la esquina superior izquierda
    $('<div/>', {
        id: 'header-block',
        style: 'position: absolute; top: 0px;overflow:hidden;left:0;padding-top:1px;background-color:#FFF;z-index:100;',
        html: $(_left_column).clone()
                .attr({'id': $(_left_column).attr('id') + '-block','style':'' }).css('width',_left)
    })
    .append($('<div/>', { attr: { 'class': 'collapser left_collapser absolute-right' }, html: '-', style: 'top:1px;' }))
    .appendTo($('#wrapper_main').parent())
    .find('tbody').remove();

    //Boton para minimizar las columnas
    $('.mastertable .collapsable').each(function (i, e) {
        var wrap_style = $('#header-fix table:eq(' + i + ')').attr('style')
        $table = $(this).find('table');
        var _tooltip = $table.attr('id').replace('_', ' ').replace('View', '');
        $('#header-fix table:eq(' + i + ')').wrap($('<div/>', { style: wrap_style, css: { 'position': 'relative', height: $('#header-fix table:eq(' + i + ')').height() } })).attr({ 'style': '' });
        $(this).find('.collapser').clone()
            .attr({ 'table-to-collapse': (i), title: _tooltip })
            .addClass('top_collapser')
            .addClass('absolute-right')
            .css({ 'z-index': ($(this).parent().css('z-index') + 1) })
            .appendTo($('#header-fix table:eq(' + i + ')').parent());
        $(this).find('.collapser').remove();
    });
    //Funcionalidad del botón para colapsar las columas data y source
    $('.collapser.left_collapser').on('click', function (event) {
        ColapseLeftColumn(this);
    });
    //Funcionalidad del botón
    $('.collapser.top_collapser').click(function () {
        var table_num = $(this).attr('table-to-collapse');
        var $parent = $(this).parent();
        var $table = $('.mastertable td.collapsable:eq(' + table_num + ') .grid-wrap');
        if ($(this).text() == '-') {
            $parent
                .attr({ 'ret-style': $parent.attr('style') })
                .css({ 'width': '8px', 'min-width': '8px' })
                .find('.table').addClass('grid-wrap').hide();
            $(this).text('+').removeClass('absolute-right').addClass('absolute-left');
            $table.attr('style', 'width:0px;overflow:hidden;padding-left:8px;').parent().css({ 'padding': '0', 'padding-left': '5px' });
        } else {
            $parent
                .attr({ 'style': $parent.attr('ret-style') }).css({ 'position': 'relative' })
                .find('.grid-wrap').show().removeClass('grid-wrap');
            $(this).text('-').removeClass('absolute-left').addClass('absolute-right');
            $table.attr('style', 'overflow:visible;width:auto;').parent().css({ 'padding-left': '5px' });
        }
    });

    //Últimos ajustes
    $('#wrapper_main .mastertable').css({ 'margin-top': _top * -1 });
    $(_left_column).css('display', 'none');
    //Vincular el scroll con la posición de cabecera y columna izquierda.
    $('#wrapper_main').on('scroll', function () { $('#left-column').scrollTop($(this).scrollTop()); $('#header-fix').scrollLeft($(this).scrollLeft()); });
    //Funcionalida para ajustar en caso de redimensionar la ventana
    $(window).on('resize', function (event) {
        $('.collapsable').find('table').each(function (i, e) {
            var _target = '#' + $(this).attr('id') + '-fix';
            $(_target).parent().css({ 'width': $(this).parent().width() });
            $(this).find('th').each(function (i, e) {
                $(_target + ' th:eq('+i+')').css({width:$(this).width()});
            });
        });
    });
    ColapseLeftColumn($('.collapser.left_collapser'));
}

CloneLeftBOYColumn = function (left_column) {
    //Versión específica para los BOY's
    _left_column = 'div[data-type=' + left_column + ']';
    ///Funciones para la visualización de los laterales y cabecera:
    //Lateral 
    var _left = $(_left_column).width();
    var _top = $(_left_column).height();
    $('#wrapper_main').attr('style', 'right: 0px; bottom: 0px; overflow: scroll; position: absolute;').css({'left': _left, 'top':0 }); //Estilo de la parte navegable para hacer sitio para el lateral
    header_fix = $('<div/>', { id: 'header-fix', style: 'position: absolute; top: 0px;overflow:hidden;right:0px;padding-top:1px;background-color:#FFF' }).css('left', _left);
    //COLUMNA IZQUIERDA
    $('<div/>', {
        id: 'left-column',
        style: 'left: 0px; top: 0px; right: 0px; bottom: 0px; overflow: hidden; position: absolute;border-right:1px solid rgba(100,100,100,.5)',
        class: 'wrapper mastertable',
    })
            .append($(_left_column).clone().attr({ 'id': $(_left_column).attr('data-type') + '-left' }).css('display', 'table-row'))
            .css({ 'margin-bottom': '18px', 'width': _left })
            .appendTo($('#wrapper_main').parent());
    $(header_fix).appendTo($('#wrapper_main').parent());

    ////CABECERA
    //$('<div/>', { style: 'margin:0;', class: 'wrapper' }).appendTo(header_fix);
    //var wrapper_width = 0;
    //$('#wrapper_main .mastertable div').eq(0).children('div').each(function () {
    //    if ($(this).attr('id') != left_column && $(this).html().length > 0) {
    //        wrapper_width += 5 + $(this).width();
    //        $(this).css({ 'width': $(this).width() });
    //        $(this).clone()
    //            .appendTo($('#header-fix .wrapper'))
    //            .css({ 'width': $(this).width(), 'height': $(this).find('table:eq(0)').height() })
    //            .attr({ 'id': $(this).attr('id') + '-fix' })
    //            .find('table').not(':eq(0)').remove();
    //    }
    //});
    //$('#header-fix .wrapper').css({ 'width': wrapper_width + 22, 'overflow': 'hidden' });

    //Bloqueo de la cabecera de la esquina superior izquierda
    //var header_fix = $(_left_column).eq(0).clone()
    //            .attr({ 'id': $(_left_column).attr('id') + '-block'})
    //            .css('width', _left);
    //$(header_fix).find('div.comments-button').remove()
    //$(header_fix).find('table').not(':eq(0)').remove();
        
    //$('<div/>', {
    //    id: 'header-block',
    //    style: 'position: absolute; top: 0px;overflow:hidden;left:0;padding-top:1px;background-color:#FFF;z-index:100;',
    //    html: $(header_fix).html()
    //})
    //.appendTo($('#wrapper_main').parent());

    //Últimos ajustes
    $('#wrapper_main .mastertable').css({ 'margin-left': _left * -1 });
    //Vincular el scroll con la posición de cabecera y columna izquierda.
    $('#wrapper_main').on('scroll', function () { $('#left-column').scrollTop($(this).scrollTop()); });
    //Funcionalida para ajustar en caso de redimensionar la ventana
    //$(window).on('resize', function (event) {
    //    $('.collapsable').find('table').each(function (i, e) {
    //        var _target = '#' + $(this).attr('id') + '-fix';
    //        $(_target).parent().css({ 'width': $(this).parent().width() });
    //        $(this).find('th').each(function (i, e) {
    //            $(_target + ' th:eq(' + i + ')').css({ width: $(this).width() });
    //        });
    //    });
    //});
}

var btn_file_wrapper = $('<button/>').addClass('btn btn-default').html($('<li/>').addClass('fa fa-caret-up up'));
var file_wrapper = $('<div/>').addClass('file-wrapper').append(btn_file_wrapper);
$(document).ready(function(){
   $(document).on('mouseover', 'td[data-type=grouper]', function(){
      if($(document).find('.file-wrapper').length > 0)
         $(this).find('.file-wrapper').hide();
      if($(this).find('.file-wrapper').length == 0){
          $(this).css('position','relative');
          $(this).append($(file_wrapper).clone ());
          var _this = this;
          $(this).on('click','.file-wrapper button', function(){
              var _top = $(this).closest('tr').attr('data-target');
              var _index = $(this).closest('tr').index();
              var _table = $(this).closest('table');
              var _up = $(this).find('li').hasClass('up');
              for(var i = _index - 1; i > 0; i--){
                  var _tr = $(_table).find('tbody tr:eq(' + i + ')');
                  if($(_tr).attr('data-type') == _top){
                      if($(_tr).find('li').length > 0)
                        $(_tr).find('li').attr('class','fa fa-caret-up up');
                      if(_up)
                        $('.table').find('tbody tr:eq(' + i + ')').hide();
                      else
                        $('.table').find('tbody tr:eq(' + i + ')').show();
                  }
              }
              if(_up){
                $(this).find('li').removeClass('up');
                $(this).find('li').removeClass('fa-caret-up').addClass('fa-caret-down');
              }else{
                $(this).find('li').addClass('up');
                $(this).find('li').removeClass('fa-caret-down').addClass('fa-caret-up');
              }
            });
          }else{
              $(this).find('.file-wrapper').show();
          } 
       }); 
   $(document).on('mouseout','td[data-type=grouper]', function(){
       $(this).find('.file-wrapper').hide();
   });
});
