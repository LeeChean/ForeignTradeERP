$(function () {
    //去除首尾的空格
    $(".trim").blur(function () {
        this.value = $.trim(this.value);
    });
});