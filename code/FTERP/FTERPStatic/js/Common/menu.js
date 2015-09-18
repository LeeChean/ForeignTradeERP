
$(document).ready(function () {
    /* 一级导航 */
    var topMenu = $(".m-menu ul li");
    topMenu.each(function () {
        $(this).click(function () {
            $('.scd').slideUp();
            var aSel = $(this).attr('selected');
            if (aSel != 'selected') {
                topMenu.removeAttr('selected').removeClass('z-crt');
                $(this).addClass("z-crt").attr("selected", "selected");
            } else {
                topMenu.removeAttr('selected').removeClass('z-crt');
            }
            var pid = $(this).children('a').first().attr('menuid');
            $('.scdli').addClass('z-hide');
            $('.scdli[pid="' + pid + '"]').removeClass('z-hide');
        });
    });
    /* /一级导航 */

    /* 二级导航 */
    var firstMenu = $(".fst");
    var secondMenu = $('.scd');
    var secondMenuli = $('.scd li a');
    firstMenu.each(function () {
        $(this).click(function () {
            var aSel = $(this).attr('selected');
            if (aSel != 'selected') {
                secondMenu.slideUp();
                $(this).siblings(".scd").slideToggle();
                firstMenu.removeAttr('selected').removeClass('z-crt');
                $(this).addClass("z-crt").attr("selected", "selected");
            } else {
                $(this).siblings(".scd").slideUp(function () {
                    firstMenu.removeAttr('selected').removeClass('z-crt');
                });
            }
        });
    });
    secondMenuli.each(function () {
        $(this).click(function () {
            var aSel = $(this).attr('selected');
            if (aSel != 'selected') {
                secondMenuli.removeAttr('selected').removeClass('z-crt');
                $(this).addClass("z-crt").attr("selected", "selected");
            } else {
                $(this).siblings(".scd").slideUp(function () {
                    secondMenuli.removeAttr('selected').removeClass('z-crt');
                });
            }
        });
    });
    /* /二级导航 */
});

