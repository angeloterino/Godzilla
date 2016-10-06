$initCombos = function () {
    $('.menu_wrapper select').each(function(i,e){
        var _parent = $(this).parent();
        if(!$(_parent).hasClass('dropdown_wrapper')){
            var _id = $(this).attr('id');
            var $button = $('<a>').attr({ 'id': _id + '_menu', 'data-toggle': 'dropdown', 'aria-expanded': 'false', 'class': 'btn btn-default dropdown-toggle','data-control':_id });
            var $chevron = $('<div>').append($('<li>').attr({'class':'fa fa-chevron-down fa-adjust'}));
            $($button).append($chevron);
            var $options = $('<ul>').attr({ 'class': 'dropdown-menu dropdown-menu-right', 'role': 'menu', 'aria-labeledfor': _id + '_menu', 'style': 'z-index: 10001;' });
            $(this).children('option').each(function (i, e) {
                var $opt = $('<li>').attr({ 'role': 'presentation' }).append($('<a>').attr({ 'role': 'menuitem', 'data-value': $(this).attr('value'), 'data-type':'menu-option' })).html($(this).html());
                $($options).append($opt);
            });
            var _cont = $('<div>').attr({ 'class': 'btn-group menu-select' }).append($options);
            //$(this).hide();
            $(_parent).css({ 'position': 'relative' }).addClass('dropdown_wrapper');
            $($button).on('click', function (i) { $('#' + _id).click(); });
            $(_parent).append($button);
            $(_parent).append(_cont);
        }
    });
    $('.dropdown_wrapper a.btn').each(function (i, e) { $(this).on('click', function () { $('#' + $(this).attr('data-control')).click(); }) });
        //<a class="btn btn-default dropdown-toggle" aria-expanded="true" data-toggle="dropdown">
        //<li class="fa fa-bars fa-adjust"></li>
        //<div><li class="fa fa-chevron-down fa-adjust"></li></div>
        //</a>
    //$('.dropdown-toggle').dropdown();
}