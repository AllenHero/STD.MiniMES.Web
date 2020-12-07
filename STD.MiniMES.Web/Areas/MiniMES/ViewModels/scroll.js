

/**********
    功能：实现水平或垂直无缝滚动
    参数：direction方向，总共4个值：left,right,top,bottom
          speed移动距离
          iTime多少时间后开始移动，若不写则页面加载完开始移动
**********/
function scroll(direction, speed, iTime) {
    var timer = null;
    var iSpeed = 0;
    var flag = true;    //判断水平移动还是垂直移动
    switch (direction) {
        case 'left':
            iSpeed = -speed;
            $("#productlist").width($("#productlist").find('li')[0].offsetWidth * $("#productlist").find('li').length);
            flag = true;
            break;
        case 'right':
            iSpeed = speed;
            $("#productlist").width($("#productlist['li']")[0].offsetWidth * $("#productlist").find('li').length);
            flag = true;
            break;
        case 'top':
            iSpeed = -speed;
            flag = false;
            break;
        case 'bottom':
            iSpeed = speed;
            flag = false;
            break;
    };

    setTimeout(move, iTime);

    $("#productlist").mouseover(function () {
        clearInterval(timer);
    });

    $("#productlist").mouseout(function () {
        move();
    });
    function move() {
        timer = setInterval(function () {
            if (flag) {
                $("#productlist").css('left', $("#productlist").get(0).offsetLeft + iSpeed);
                if ($("#productlist").offsetLeft < -$("#productlist").get(0).offsetWidth / 2) {
                    $("#productlist").css('left', 0);
                } else if ($("#productlist").get(0).offsetLeft > 0) {
                    $("#productlist").css('left', -$("#productlist").get(0).offsetWidth / 2);
                }
            } else {
                $("#productlist").css('top', $("#productlist").get(0).offsetTop + iSpeed);
                if ($("#productlist").get(0).offsetTop <= -$("#productlist").get(0).offsetHeight / 2) {
                    $("#productlist").css('top', 0);
                } else if ($("#productlist").get(0).offsetTop >= 0) {
                    $("#productlist").css('top', -$("#productlist").get(0).offsetHeight / 2);
                };
            };
        }, 30);
    };
};

function scrolls(obj, left, direction, speed, iTime) {
    if (left < 1)
        left = 1366;
    var timer = null;
    var iSpeed = 0;
    var flag = true;    //判断水平移动还是垂直移动
    switch (direction) {
        case 'left':
            iSpeed = -speed;
            $(obj).width(left);
            flag = true;
            break;
        case 'right':
            iSpeed = speed;
            $(obj).width(left);
            flag = true;
            break;
        case 'top':
            iSpeed = -speed;
            flag = false;
            break;
        case 'bottom':
            iSpeed = speed;
            flag = false;
            break;
    };

    setTimeout(move, iTime);

    $(obj).mouseover(function () {
        clearInterval(timer);
    });

    $(obj).mouseout(function () {
        move();
    });
    //$(obj).css('left', left * 0.38 + 15);

    function move() {
        timer = setInterval(function () {
            if (flag) {
                $(obj).css('left', $(obj).get(0).offsetLeft + iSpeed);
                if ($(obj).get(0).offsetLeft < -left / 2) {
                    $(obj).css('left', 0);
                } else if ($(obj).get(0).offsetLeft > left) {
                    $(obj).css('left', -$(obj).get(0).offsetWidth / 2);
                }
            } else {
                $(obj).css('top', $(obj).get(0).offsetTop + iSpeed);
                if ($(obj).get(0).offsetTop <= -$(obj).get(0).offsetHeight / 2) {
                    $(obj).css('top', 0);
                } else if ($(obj).get(0).offsetTop >= 0) {
                    $(obj).css('top', -$(obj).get(0).offsetHeight / 2);
                };
            };
        }, 30);
    };
};