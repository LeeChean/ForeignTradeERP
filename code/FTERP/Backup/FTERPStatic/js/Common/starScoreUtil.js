var starScoreUtil = {
    defaults: {
        scoreSpanSelector: 'span[name=score]',
        scoreLiSelector: '.m-score li',
        baseScore: 10
    },
    ChangeLight: function () {
        if ($(this).attr('class') == 'ico-starl01') {
            $(this).attr('class', 'ico-starl');
        }
        if ($(this).attr('class') == 'ico-starr01') {
            $(this).attr('class', 'ico-starr');
        }
    },
    ChangeDuck: function () {
        if ($(this).attr('class') == 'ico-starl') {
            $(this).attr('class', 'ico-starl01');
        }
        if ($(this).attr('class') == 'ico-starr') {
            $(this).attr('class', 'ico-starr01');
        }
    },
    Init: function (options) {
        if (typeof options != 'undefined') {
            if (typeof options.scoreSpanSelector != 'undefined') {
                starScoreUtil.defaults.scoreSpanSelector = options.scoreSpanSelector;
            }
            if (typeof options.scoreLiSelector != 'undefined') {
                starScoreUtil.defaults.scoreLiSelector = options.scoreLiSelector;
            }
            if (typeof options.baseScore != 'undefined') {
                starScoreUtil.defaults.baseScore = options.baseScore;
            }
        }
        $(starScoreUtil.defaults.scoreLiSelector).each(function () {
            $(this).hover(function () {
                if ($(this).attr('class') == 'ico-starl01') {
                    $(this).attr('class', 'ico-starl');
                }
                if ($(this).attr('class') == 'ico-starr01') {
                    $(this).attr('class', 'ico-starr');
                }
                $(this).prevAll('li').each(starScoreUtil.ChangeLight);
                $(this).nextAll('li').each(starScoreUtil.ChangeDuck);
                var score = $(starScoreUtil.defaults.scoreLiSelector + '[class$=duck]').length * starScoreUtil.defaults.baseScore;
                $(starScoreUtil.defaults.scoreSpanSelector).text(score);
            });
        });
    }
};