//====================================
//长度验证
//====================================
jQuery.validator.addMethod("lengthcheck",
        function (value, element, params) {

            var length = params.length;

            var result = value.getByteLength() <= length;
            //alert(value.getByteLength());
            //alert(maxAge);
            //alert(result);
            return result;
        });

jQuery.validator.unobtrusive.adapters.add("lengthcheck", ["length"], function (options) {
    options.rules["lengthcheck"] = {
        length: options.params.length
    };
    options.messages["lengthcheck"] = options.message;
});

//====================================
//范围验证
//====================================
jQuery.validator.addMethod("rangecheck",
        function (value, element, params) {

            var minAge = params.minage;
            var maxAge = params.maxage;

            var length = value.getByteLength();
            var result = length >= minAge && length <= maxAge;
            //alert(value.getByteLength());
            //alert(maxAge);
            //alert(result);
            return result;
        });

jQuery.validator.unobtrusive.adapters.add("rangecheck", ["minage", "maxage"], function (options) {
    options.rules["rangecheck"] = {
        currentdate: options.params.currentdate,
        minage: options.params.minage,
        maxage: options.params.maxage
    };
    options.messages["rangecheck"] = options.message;
});

//====================================
//年龄验证
//====================================
jQuery.validator.addMethod("agerangecheck",
    function (value, element, params) {

        var minAge = params.minage;
        var maxAge = params.maxage;

        //alert(minAge);
        //alert(maxAge);
        //alert(params.currentdate);

        var literalCurrentDate = params.currentdate;
        var literalBirthDate = value;
        var literalCurrentDates = literalCurrentDate.split('-');
        var literalBirthDates = literalBirthDate.split('-');

        var birthDate = new Date(literalBirthDates[0], literalBirthDates[1], literalBirthDates[2]);
        var currentDate = new Date(literalCurrentDates[0], literalCurrentDates[1], literalCurrentDates[2]);
        //alert("birthDate:" + birthDate);
        //alert("currentDate:" + currentDate);
        var age = currentDate.getFullYear() - birthDate.getFullYear();
        //alert(age);
        return age >= minAge && age <= maxAge
    });

jQuery.validator.unobtrusive.adapters.add("agerangecheck", ["currentdate", "minage", "maxage"], function (options) {
    options.rules["agerangecheck"] = {
        currentdate: options.params.currentdate,
        minage: options.params.minage,
        maxage: options.params.maxage
    };
    options.messages["agerangecheck"] = options.message;
});

//====================================
//非空验证
//====================================
jQuery.validator.addMethod("nullcheck",
        function (value, element, params) {

            var nullstrings = params.nullstrings;

            var str = value;
            var strArray = new Array(); //定义一数组
            strArray = nullstrings.split(","); //字符分割  

            var bResult = true;
            for (var i in strArray) {
                //alert(str.replace(/(^\s+)|(\s+$)/g, ""));
                //alert(strArray[i].replace(/(^\s+)|(\s+$)/g, ""));
                if (str.replace(/(^\s+)|(\s+$)/g, "") == strArray[i].replace(/(^\s+)|(\s+$)/g, "")) { //去两侧空格后比较
                    bResult = false;
                    break;
                }
            }

            return bResult;
        });

        jQuery.validator.unobtrusive.adapters.add("nullcheck", ["nullstrings"], function (options) {
            options.rules["nullcheck"] = {
                nullstrings: options.params.nullstrings
    };
    options.messages["nullcheck"] = options.message;
});