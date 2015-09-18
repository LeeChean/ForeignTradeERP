$(function () {
    //去除首尾的空格
    $(".trim").blur(function () {
        this.value = $.trim(this.value);
    });

    $(".number").keyup(function () {
        if (this.value.length == 1) {
            this.value = this.value.replace(/[^1-9]/g, '');
        }
        else {
            this.value = this.value.replace(/\D/g, '');
        }
    });

    $(".number").blur(function () {
        if (this.value.length == 1) {
            this.value = this.value.replace(/[^1-9]/g, '');
        }
        else {
            this.value = this.value.replace(/\D/g, '');
        }
    });
});