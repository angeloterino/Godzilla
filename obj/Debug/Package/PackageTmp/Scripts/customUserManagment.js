$(document).ready(function () {
    $(document).on('click', 'button[type=submit]', function (e) {
        e.preventDefault();
        var form = $(this).closest('form');
        $(this).find('li').attr('class','fa fa-spinner fa-spin');
        $.each($(form).find('input[type=checkbox]'), function (i, e) {
            if (!$(this).attr('data-renamed')) {
                if (!$(this).attr('data-value'))
                    $(this).attr('data-value', 'BRAND:' + $(this).attr('data-brand') + '_MARKET:' + $(this).attr('data-market') + '_CHANNEL:' + $(this).attr('data-channel'));

                $(this).attr('name', $(this).attr('name') + "_" + $(this).attr('data-value'));
                var select = $(this).closest('tr').find('select');
                $(select).attr('name', $(select).attr('name') + "_" + $(this).attr('data-value'));
                $(this).attr('data-renamed', true);
            }
        });
        $('input[type=hidden][id=user]').val($('select[id=user]').find('option:selected').text());
        $(form).submit();
    });
    $(document).on('change', 'select[id=user]', function (e) {
       $('input[type=hidden][id=user]').val($(this).find('option:selected').text());
       var _url = 'UsersManagement' 
       var _data = {"_user":$(this).find('option:selected').text(),'_channel':1};
       $.get(_url,_data,function(d,t,j){
           var _content = $.parseHTML(d);
           $.each($(_content).find('.grid-mvc'),function(i,e){
              $('.grid-mvc:eq('+i+')').html($(this).html());
           });
           $('ul.nav-pills a:eq(0)').click();
       });
    });
});

var UpdateSuccess = function () {
    $('button[type=submit]').find('li').attr('class','fa fa-save');
};
var UpdateFailure = function () {
};