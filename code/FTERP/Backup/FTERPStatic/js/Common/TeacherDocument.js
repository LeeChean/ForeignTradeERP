window.UEDITOR_CONFIG
var ucontent = UE.getEditor('Content', {
    initialFrameWidth: 600,
    initialFrameHeight: 300,
    toolbars: [
            ["source", "bold", "italic", "underline", "forecolor", "fontfamily", "fontsize", "undo", "redo", "backcolor", "insertunorderedlist", "insertorderedlist", "justifyleft", "justifycenter", "justifyright", "justifyjustify", "indent", "paragraph", "rowspacingbottom", "rowspacingtop", "lineheight", "inserttable", "link", "horizontal", "insertimage", "wordimage", "cleardoc", "formatmatch", "fullscreen"]]
});

function icoImg(type) {
    var str = "";
    switch (type) {
        case "文档":
            str = "ico-word";
            break;
        case "图片":
            str = "ico-img";
            break;
        case "视频":
            str = "ico-video";
            break;
        case "音频":
            str = "ico-voice";
            break;
        case "FLASH":
            str = "ico-flash";
            break;
    }
    return str;
}

$(function () {
    //页面加载时判断显示隐藏
    dldisplay();

    $("#ul_DocumentId li").click(function () {
        dldisplay();
    });

    var artheight = 560;
    if ($.browser.msie) {
        if ($.browser.version == "6.0") {
            artheight = 573;
        }
    }
    function dldisplay() {
        var $documentValue = $("#DocumentId").val();
        var $value = $documentValue.split(',')[1];
        if ($value == 1) {
            $("dl").children(":gt(3):lt(4)").hide().end().children().eq(9).hide();
            $("dl").children(":gt(9):lt(3)").hide();
        } else {
            $("dl").children(":gt(8):lt(1)").show();
            var $ulresdoclength = $("#dd_resdoc").find("li").length;
            if ($ulresdoclength > 0) {
                $("dl").children(":gt(9):lt(2)").show();
            } else {
                $("dl").children(":gt(9):lt(2)").hide();
            }
            $("dl").children(":gt(3):lt(4)").show().end().children().eq(12).show();
        }
    }
    /*选择资源*/
    $("#xzbutton").click(function () {
        var resdocIds = "";
        var resodcval = "";
        $("#ul_resdoc li").each(function () {
            resdocIds += $(this).attr("data-resdocId") + ",";
            resodcval += $(this).attr("data-resdoc-val") + ",";
        });
        art.dialog.open('/Home/TeacherDocumentManage/ResourceSelect?resdocIds=' + resdocIds + "&resdocval=" + resodcval, {
            title: '选择资源',
            width: 828,
            height: artheight,
            fixed: true,
            lock: true,
            close: function (e) {
                var $resource = art.dialog.data('resource');
                if (typeof ($resource) != "undefined") {
                    if ($resource.length != 0) {
                        $("#dd_resdoc,#dt_resodc").show();
                        $("#ul_resdoc").empty();
                        for (var i = 0; i < $resource.length; i++) {
                            var $resdoc = $resource[i].split('||')
                            var $icoimg = icoImg($resdoc[2]);
                            $("#ul_resdoc").append("<li data-resdoc-val=\"" + $resource[i] + "\" data-resdocId=\"" + $resdoc[1] + "\"><span class=\"busnconl\"><i class=\"" + $icoimg + "\"></i>" + $resdoc[0] + "</span><span class=\"busnconr\"><a class=\"up\">上移</a><a class=\"dw\">下移</a><a class=\"del\">删除</a></span></li>");
                        }
                    } else {
                        $("#dd_resdoc,#dt_resodc").hide();
                        $("#ul_resdoc").empty();
                    }
                }
            }
        });
    });
    /**/
    $("#form").submit(function () {
        var resdocIds = "";
        var $documentId_text = $("#DocumentId_text").val();
        if ($("#DocumentId_text").val() == "请选择") {
            $("#spandocumentText").show();
            return false;
        }
        $("#ul_resdoc li").each(function () {
            resdocIds += $(this).attr("data-resdocId") + ",";
        });
        $("#resdocList").val(resdocIds);
        return validate();
    });


    /* 富文本框验证*/
    ucontent.addListener('ready', function (editor) {
        ucontent.addListener('blur', function (t, args) {
            validate();
        }),
    ucontent.addListener('focus', function (t, args) {
        validate();
    })
    });

    function validate() {
        var content = ucontent.getContent();
        if (content.length == 0) {
            $("#spanContent").show();
            return false;
        }
        else {
            $("#spanContent").hide();
        }
    }
    //上移下移删除
    $(".up").live('click', function () {
        var onthis = $(this).parent("span").parent("li");
        var getup = $(this).parent("span").parent("li").prev();
        if (getup.html() == null) {
            var getup1 = $(this).parent("span").parents("li").siblings().last();
            $(getup1).after(onthis);
        } else {
            $(getup).before(onthis);
        }
    });

    $(".dw").live('click', function () {
        var onthis = $(this).parent("span").parent("li");
        var getdown = $(this).parent("span").parent("li").next();
        if (getdown.html() == null) {
            var getup1 = $(this).parent("span").parents("li").siblings().first();
            $(getup1).before(onthis);
        } else {
            $(getdown).after(onthis);
        }
    });

    $(".del").live('click', function () {
        $(this).parent("span").parent("li").remove();
        var $lilength = $("#ul_resdoc").children("li").length;
        if ($lilength == 0) {
            $("#dd_resdoc,#dt_resodc").hide();
        }
    });
});