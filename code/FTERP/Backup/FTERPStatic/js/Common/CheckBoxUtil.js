//多选通用方法
function BindCheckAll(options) {
    options = $.extend({
        btnClass: '.btnDelete',
        checkall: '.checkall',
        checkone: '.checkone',
        noSelectTip: '您尚未选择要删除的选项！',
        confirmTip: '您确定要删除所选项吗？',
        actionUrl: location.pathname + '/Delete',
        backUrl: location.pathname + '/Index'
    }, options);

    var checkall = $(options.checkall);
    var checkone = $(options.checkone);
    var btnDelete = $(options.btnClass);

    checkall.bind('click', function () {
        var allState = (($(this).attr('checked') || '') != '');
        if (allState == '') {
            checkone.removeAttr('checked');
        } else {
            checkone.attr('checked', 'checked');
        }
    });

    checkone.each(function () {
        $(this).bind('click', function () {
            if ($('.checkone:checked').length == checkone.length) {
                checkall.attr('checked', 'checked');
            } else {
                checkall.removeAttr('checked');
            }
        });
    });

    btnDelete.bind('click', function () {
        if ($('.checkone:checked').length == 0) {
            Common.Tip(options.noSelectTip, null);
        } else {
            art.dialog.confirm(options.confirmTip, function () {
                var idArray = new Array();
                for (var j = 0; j < $('.checkone:checked').length; j++) {
                    idArray.push($('.checkone:checked').eq(j).val());
                }

                $.post(options.actionUrl, { id: idArray.join(',') }, function (msg) {
                    if (msg == "1") {
                        Common.Success("删除成功", function () { window.location = options.backUrl; });
                    }
                    else {
                        Common.Error("删除失败", function () { });
                    }

                }, "");
            }, null);
        }
    });
}