//多选通用方法
function BindCheckAll() {
    var checkall = $('.checkall');
    var checkone = $('.checkone');

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
}

function Operate(options) {
    options = $.extend({
        operateName: '',
        params: '',
        actionUrl: '',
        backUrl: ''
    }, options);

    if ($('.checkone:checked').length == 0) {
        Common.Tip("您尚未选择要" + options.operateName + "的记录！", null);
    } else {
        art.dialog.confirm("您确定要" + options.operateName + "所选记录吗？", function () {
            var idArray = new Array();
            for (var j = 0; j < $('.checkone:checked').length; j++) {
                idArray.push($('.checkone:checked').eq(j).val());
            }

            $.post(options.actionUrl, { id: idArray.join(',') }, function (msg) {
                if (msg == "1") {
                    Common.Success(options.operateName + "成功", function () { window.location = options.backUrl; });
                }
                else if (msg == "0") {
                    Common.Error(options.operateName + "失败", function () { });
                }
                else {
                    Common.Tip(msg, function () { });
                }
            }, "");
        }, null);
    }
}

function Redirect(options) {
    options = $.extend({
        operateName: '',
        actionUrl: ''
    }, options);

    if ($('.checkone:checked').length == 0) {
        Common.Tip("您尚未选择要" + options.operateName + "的记录！", null);
    }
    else if ($('.checkone:checked').length > 1) {
        Common.Tip("请选择一条记录进行" + options.operateName + "！", null);
    }
    else {
        window.location.href = options.actionUrl + "&id=" + $('.checkone:checked').val();
    }
}

function ShowProgress() {
    if ($('.checkone:checked').length == 0) {
        Common.Tip("您尚未选择记录！", null);
    }
    else if ($('.checkone:checked').length > 1) {
        Common.Tip("请选择一条记录查看审批进度！", null);
    }
    else {
        Common.ShowProgress($('.checkone:checked').val());
    }
}

function Edit(options) {
    options = $.extend({
        tableName: '',
        actionUrl: ''
    }, options);

    if ($('.checkone:checked').length == 0) {
        Common.Tip("您尚未选择要编辑的记录！", null);
    }
    else if ($('.checkone:checked').length > 1) {
        Common.Tip("请选择一条记录进行编辑！", null);
    }
    else {
        $.get("/Home/Validate/HasAccess?id=" + $('.checkone:checked').val() + "&tableName=" +
              options.tableName + "&operateType=1", function (msg) {
                  if ($.trim(msg) != "1") {
                      Common.Tip(msg, null);
                  }
                  else {
                      window.location.href = options.actionUrl + "&id=" + $('.checkone:checked').val();
                  }
              });
    }
}

function Delete(options) {
    options = $.extend({
        tableName: '',
        actionUrl: '',
        backUrl: ''
    }, options);

    if ($('.checkone:checked').length == 0) {
        Common.Tip("您尚未选择要删除的记录！", null);
    } else {
        art.dialog.confirm("您确定要删除所选记录吗？", function () {
            $.get("/Home/Validate/HasAccess?id=" + $('.checkone:checked').val() + "&tableName=" +
             options.tableName + "&operateType=2", function (msg) {
                 if ($.trim(msg) != "1") {
                     Common.Tip(msg, null);
                 }
                 else {
                     var idArray = new Array();
                     for (var j = 0; j < $('.checkone:checked').length; j++) {
                         idArray.push($('.checkone:checked').eq(j).val());
                     }

                     $.post(options.actionUrl, { id: idArray.join(',') }, function (msg) {
                         if (msg == "1") {
                             Common.Success("删除成功", function () { window.location = options.backUrl; });
                         }
                         else if (msg == "0") {
                             Common.Error("删除失败", function () { });
                         }
                         else {
                             Common.Tip(msg, function () { });
                         }
                     }, "");
                 }
             });
        }, null);
    }
}