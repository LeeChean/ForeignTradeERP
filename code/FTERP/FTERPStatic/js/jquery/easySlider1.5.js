/*
* 	Easy Slider 1.5 - jQuery plugin
*	written by Alen Grakalic	
*	http://cssglobe.com/post/4004/easy-slider-15-the-easiest-jquery-plugin-for-sliding
*
*	Copyright (c) 2009 Alen Grakalic (http://cssglobe.com)
*	Dual licensed under the MIT (MIT-LICENSE.txt)
*	and GPL (GPL-LICENSE.txt) licenses.
*
*	Built for jQuery library
*	http://jquery.com
*
*/

/*
*	markup example for $("#slider").easySlider();
*	
* 	<div id="slider">
*		<ul>
*			<li><img src="images/01.jpg" alt="" /></li>
*			<li><img src="images/02.jpg" alt="" /></li>
*			<li><img src="images/03.jpg" alt="" /></li>
*			<li><img src="images/04.jpg" alt="" /></li>
*			<li><img src="images/05.jpg" alt="" /></li>
*		</ul>
*	</div>
*
*/

(function ($) {
    $.fn.easySlider = function (options) {
        var defaults = {
            prevId: 'prevBtn',
            nextId: 'nextBtn',
            vertical: false,
            speed: 800,
            auto: false,
            pause: 2000,
            continuous: false
        };
        var options = $.extend(defaults, options);
        this.each(function () {
            var obj = $(this);
            var _w = 0;
            var $ie6 =false;
            var s = $("li", obj).length;
            var w = $("li", obj).width();
            //ÅÐ¶ÏIE6
            if ($.browser.msie) {
                if ($.browser.version == "6.0") {
                    $ie6 =true;
                }
            }
            if ($ie6 == 1) {
                _w = w + 20;
            } else {
                _w = w + 10;
            }
            var h = $("li", obj).height();
            obj.width(_w);
            obj.height(h);
            obj.css("overflow", "hidden");
            var ts = s - 1;
            var t = 0;
            $("ul", obj).css('width', s * _w);
            if (!options.vertical) $("li", obj).css('float', 'left');

            var marginwidth = (_w * (s - 2));
            t = marginwidth / _w;
            p = (t * _w * -1);
            //³õÊ¼»¯
            $("ul", obj).animate({ marginLeft: p });

            $("#" + options.nextId).click(function () {
                animate("next", true);
            });
            $("#" + options.prevId).click(function () {
                var $marginleft = $("#" + options.nextId).prev().find("ul").css("margin-left");
                var $width = $marginleft.replace(/px/, "").replace(/-/, "");
                if ($width != marginwidth) {
                    animate("prev", true);
                }
            });
            function animate(dir, clicked) {
                var ot = t;
                switch (dir) {
                    case "next":
                        //ÅÐ¶ÏIE6
                        if ($ie6) {
                            $("#" + options.nextId).prev().find("li").eq(t).attr("class", "f-fl marl10");
                        }
                        t = (t <= 0) ? (options.continuous ? ts : 0) : t - 1;
                        break;
                    case "prev":
                        //ÅÐ¶ÏIE6
                        if ($ie6) {
                            $("#" + options.nextId).prev().find("li").eq(t + 1).attr("class", "f-fl marl20");
                        }
                        t = (ot >= ts) ? (options.continuous ? 0 : ts) : t + 1;
                        break;
                    default:
                        break;
                };
                //                var diff = Math.abs(ot - t);
                if (!options.vertical) {
                    p = (t * _w * -1);
                    $("ul", obj).animate(
						{ marginLeft: p },
						100
					);
                } else {
                    p = (t * h * -1);
                    $("ul", obj).animate(
						{ marginTop: p },
						100
					);
                };
            };
        });
    };
})(jQuery);


