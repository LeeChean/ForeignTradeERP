var comfirmList = Array();
function InitConfirm() {
    $.each(comfirmList, function (key, val) {
        $("#" + val.replace("confirm", "")).show();
        $("#" + val).remove();
    });
    comfirmList = Array();
    $(".msg").each(function () {
        $(this).hide();
        var id = $(this).attr("id");
        var msg = $(this).attr("msg");
        var classVal = $(this).attr("class");
        var link = $("<a href='javascript:void(0);'></a>");
        link.click(function () {
            art.dialog.confirm(msg, function () {
                $("#" + id).click();
            }, function () { });
        });
        link.attr("id", "confirm" + id);
        link.html($(this).html());
        link.attr("class", classVal);
        link.removeClass("msg");
        comfirmList.push(link.attr("id"));
        $(this).before(link);
    });
}
$(document).ready(function () {
    InitConfirm();
});