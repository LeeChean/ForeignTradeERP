var Common = {}

//异步保存
Common.Save = function (options) {
    options = $.extend({
        actionUrl: "",
        backUrl: ""
    }, options);

    var flag = $("form").valid();
    if (flag) {
        //异步提交
        $("form").ajaxSubmit({
            url: options.actionUrl,
            type: "Post",
            dataType: "text",
            data: $("form").serialize(),
            success: function (msg) {
                if (msg == "1") {
                    Common.Success("保存成功", function () {
                        if (options.backUrl != "") {
                            window.location = options.backUrl;
                        }
                    });
                }
                else if (msg == "0") {
                    Common.Error("保存失败", function () { });
                }
                else {
                    Common.Tip(msg, function () { });
                }
            }
        });
    }
}

//写入cookie
Common.SetCookie = function (name, value) {
    var expireDate = new Date();
    expireDate.setMinutes(expireDate.getMinutes + 5);
    document.cookie = name + "=" + escape(value) + ";expires=" + expireDate.toGMTString();
}

//读取cookie
Common.GetCookie = function (name) {
    if (document.cookie.length > 0) {
        start = document.cookie.indexOf(name + "=")
        if (start != -1) {
            start = start + name.length + 1
            end = document.cookie.indexOf(";", start)
            if (end == -1) end = document.cookie.length
            return unescape(document.cookie.substring(start, end))
        }
    }
    return ""
}

Common.Success = function (content, callback) {

    artDialog({
        id: 'Common_Success',
        icon: 'succeed',
        title: false,
        cancel: false,
        fixed: true,
        lock: false,
        content: content,
        //ok: true,
        close: callback,
        clickToClose: true
    }).time(2);
}

Common.Error = function (content, callback) {
    artDialog({
        id: 'Common_Error',
        icon: 'error',
        title: false,
        cancel: false,
        fixed: true,
        lock: false,
        content: content,
        //ok: true,
        close: callback,
        clickToClose: true
    }).time(2);
}

Common.Tip = function (content, callback) {
    artDialog({
        id: 'Common_Tip',
        icon: 'warning',
        title: false,
        cancel: false,
        fixed: true,
        lock: false,
        content: content,
        //ok: true,
        close: callback,
        clickToClose: true
    }).time(2);
}

Common.Menu = function () {
    $(".m-menulef ul li .leftmenuscd").each(function () {
        $(this).prev().click(function () {
            $(".leftmenuscd").each(function () {
                $(this).hide();
                $(this).prev().find("i").attr("class", "z-arrowd");
            });
            $(this).next().removeClass("z-hide").show();
            $(this).find("i").attr("class", "z-arrowu");
        });
    });
}

//查看审批进度
Common.ShowProgress = function (docId) {
    art.dialog.open('/Home/ApprovalRule/ShowProgress?docId=' + docId, {
        title: '审批进度',
        fixed: true,
        width: "500px"
    });
}

$(document).ready(function () {
    Common.Menu();
});