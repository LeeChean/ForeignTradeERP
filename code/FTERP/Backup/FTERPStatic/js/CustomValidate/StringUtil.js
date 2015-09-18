//*****************************************************************
//
// File Name:   StringUtil.js
//
// Description: JS字符串工具类
//
// Coder:       zishisheng
//
// Date:        2013-09-04 15:51
//
// History:     1、2013-09-04 14:16  qk zishisheng
//*****************************************************************

var StringUtil = new Object();

//====================================
//将字符数组的值拼接成一个以逗号间隔的串
//====================================
StringUtil.StringJoin = function (tringArray) {

    var ids = new StringUtil.StringBuilder();
    for (var i = 0; i < tringArray.length; i++) {
        var id = tringArray[i];
        ids.Append(id)
        if (i < tringArray.length - 1) {
            ids.Append(",");
        }
    }
    var strIds = ids.ToString();
    return strIds;
}

//====================================
// 用来连接字符串，提高字符串的拼接速度
//====================================
StringUtil.StringBuilder = function () {
    this.buffer = new Array();
}

StringUtil.StringBuilder.prototype.Append = function Append(string) {
    if ((string == null) || (typeof (string) == 'undefined'))
        return;
    if ((typeof (string) == 'string') && (string.length == 0))
        return;
    this.buffer.push(string);
};
StringUtil.StringBuilder.prototype.AppendLine = function AppendLine(string) {
    this.Append(string);
    this.buffer.push("\r\n");
};
StringUtil.StringBuilder.prototype.Clear = function Clear() {
    if (this.buffer.length > 0) {
        this.buffer.splice(0, this.buffer.length);
    }
};
StringUtil.StringBuilder.prototype.IsEmpty = function IsEmpty() {
    //    return (this.buffer.length == 0);
};
StringUtil.StringBuilder.prototype.ToString = function ToString() {
    return this.buffer.join("");
};


//====================================
// 字符串去空格
//====================================
//去左空格
StringUtil.ltrim = function (str) {
    return str.replace(/^\s*/, "");
}
//去右空格
StringUtil.rtrim = function (str) {
    return str.replace(/\s*$/, "");
}

//去两侧空格
StringUtil.trim = function (str) {
    return this.rtrim(this.ltrim(str));
}



//去两侧自定义字符
StringUtil.trimChar = function (str, char) {
    var reg = /(^\,*)|(\,$)/gi;
    str = str.replace(reg, "");
    if (str.charAt(0) == char || str.charAt(str.length - 1) == char) {
        str = StringUtil.trimChar(str, char)
    }
    return str
}


//====================================
// 去除文件扩展名
//====================================

StringUtil.delExtension = function (filename) {
    var sufix = /\.[^\.]+/.exec(filename);
    return filename.replace(sufix, '');
}

//====================================
// 获取字节长度
//====================================
String.prototype.getBytesLength = function () {
    var totalLength = 0;
    var charCode;
    for (var i = 0; i < this.length; i++) {
        charCode = this.charCodeAt(i);
        if (charCode < 0x007f) {
            totalLength++;
        } else if ((0x0080 <= charCode) && (charCode <= 0x07ff)) {
            totalLength += 2;
        } else if ((0x0800 <= charCode) && (charCode <= 0xffff)) {
            totalLength += 3;
        } else {
            totalLength += 4;
        }
    }
    return totalLength;
}



String.prototype.getByteLength = function () {
    var l = 0;
    var a = this.split("");
    for (var i = 0; i < a.length; i++) {
        if (a[i].charCodeAt(0) < 299) {
            l++;
        } else {
            l += 2;
        }
    }
    return l;
}