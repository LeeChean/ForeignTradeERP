function PhotoImg(img, arry) {
    var imgPrevArray = new Array();
    var imgNextArray = new Array();
    var index = false;
    //遍历原始数组
    for (var i = 0; i < arry.length; i++) {
        if (arry[i].indexOf("" + img + "") >= 0) {
            imgPrevArray.splice(0, 0, arry[i]);
            index = true;
        } else {
            if (index == false) {
                imgNextArray.push(arry[i]);
            } else {
                imgPrevArray.push(arry[i]);
            }
        }
    }
    imgPrevArray = imgPrevArray.concat(imgNextArray);
    return imgPrevArray.join(",");
}