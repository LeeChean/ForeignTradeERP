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
                else {
                    Common.Error("保存失败", function () { });
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

Common.Xiangce = function (imgs, w, h) {
    art.dialog.data("xiangce_imgs", imgs);
    art.dialog.data("width", 990);
    art.dialog.data("height", 560);
    art.dialog.open('/Home/Public/Xiangce', {
        width: 990,
        height: 560,
        title: false,
        background: '#000', // 背景色
        opacity: 0.87, // 透明度
        padding: 0,
        id: 'EF893L',
        fixed: true,
        resize: false,
        drag: false,
        lock: true
    });
}

Common.MessageT = function () {
    $.post("/Home/IzoneMessage/MessageReadCount", function (data) {
        var c = parseInt(data);
        if (c > 0 && c < 100) {
            $(".ico-mail").html("<em>" + data + "</em>");
        } else if (c == 0) {
            $(".ico-mail").html('');
        } else {
            $(".ico-mail").html("<em>...</em>");
        }
    });
}

/*根据播放地址获取对应的图片地址及标题,
callback第一个参数为图片地址
第二个参数为标题
第三个参数为视频地址
*/
Common.YouKuGetInfoByUrl = function (playUrl, theWebUrl, callback) {

    var id = "";
    var reg = new RegExp(".*id_([A-Za-z0-9_=]+)\\.html*");
    var r = playUrl.match(reg);

    if (r != null) {
        id = (unescape(r[1]));
    }
    else {
        alert("您输入的优酷地址不正确，请检查地址是否正确");
        return;
    }

    $.getJSON(theWebUrl + "/Web/Services/YouKuByUrl?id=" + id, function (obj) {

        var img = obj.data[0].logo;
        var title = "";
        if (obj.data[0].list == undefined) {
            title = (obj.data[0].title);
        }
        else {
            title = (obj.data[0].list[0].title);
        }
        var playurl = "http://player.youku.com/embed/" + id;

        callback(img, title, playurl);
    });

};

Common.CheckSession = function () {
    var retResult = true;
    $.ajax({
        url: '/Home/Public/CheckSession?' + Math.random(),
        type: "POST",
        async: false,
        dataType: "json",
        success: function (data) {
            if (data == "0") {
                retResult = false;
            }
        }
    });
    return retResult;
}

Common.CheckSchoolRight = function () {
    var code = true;
    var info = "";
    var url = "";
    if (TCCommon.CheckSession() == false) {
        code = false;
        info = "页面超时，请重新登录！";
        return { status: code, info: info, url: '/web/login/index' };
    }

    $.ajax({
        url: '/Home/PublicResourceManage/CheckSchoolRight?' + Math.random(),
        type: "POST",
        async: false,
        dataType: "json",
        success: function (data) {
            if (data.code == "0") {
                code = false;
                info = data.info;
                url = data.url;
            }
        }
    });

    return { status: code, info: info, url: url };
}

$(document).ready(function () {
    Common.Menu();
    //Common.MessageT();
    //window.setInterval(Common.MessageT, 20 * 1000);
});