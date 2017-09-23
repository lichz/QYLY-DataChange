var baiduMap = {};
//创建标注
baiduMap.createMarker = function(point, index) {
    //var imageOffsetX = 21 * index;
    //var myIcon = new BMap.Icon("/Content/Resources/map-baidu/markers_new2_4ab0bc5.png", new BMap.Size(21, 33), {
    //    // 指定定位位置。   
    //    // 当标注显示在地图上时，其所指向的地理位置距离图标左上    
    //    // 角各偏移10像素和25像素。您可以看到在本例中该位置即是   
    //    // 图标中央下端的尖角位置。    
    //    offset: new BMap.Size(10, 33),
    //    // 设置图片偏移。   
    //    // 当您需要从一幅较大的图片中截取某部分作为标注图标时，您   
    //    // 需要指定大图的偏移位置，此做法与css sprites技术类似。    
    //    imageOffset: new BMap.Size(-imageOffsetX, 0)   // 设置图片偏移    
    //});
    //var marker = new BMap.Marker(new BMap.Point(point[0], point[1]), { icon: myIcon });
    //return marker;

    var myIcon = new BMap.Icon("/Content/Resources/map-baidu/markers_new2_4ab0bc5.png", new BMap.Size(26, 36), {
        // 指定定位位置。   
        // 当标注显示在地图上时，其所指向的地理位置距离图标左上    
        // 角各偏移10像素和25像素。您可以看到在本例中该位置即是   
        // 图标中央下端的尖角位置。    
        offset: new BMap.Size(10, 33),
        // 设置图片偏移。   
        // 当您需要从一幅较大的图片中截取某部分作为标注图标时，您   
        // 需要指定大图的偏移位置，此做法与css sprites技术类似。    
        imageOffset: new BMap.Size(-225, -187)   // 设置图片偏移    
    });
    var marker = new BMap.Marker(new BMap.Point(point[0], point[1]), { icon: myIcon });
    return marker;
}
//标注选中
baiduMap.isMarkerFocus = false;
baiduMap.markerFocus = function (marker) {
    baiduMap.markerIconBlue(marker);
    baiduMap.isMarkerFocus = true;
    return marker;
}
//标注取消
baiduMap.markerBlur = function (marker) {
    baiduMap.markerIconRed(marker);
    baiduMap.isMarkerFocus = false;
    return marker;
}
//鼠标进入标注
baiduMap.markerMouseover = function (marker) {
    if (!baiduMap.isMarkerFocus) {
        baiduMap.markerIconBlue(marker);
    }
    return marker;
}
//鼠标离开标注
baiduMap.markerMouseout = function (marker) {
    if (!baiduMap.isMarkerFocus) {
        baiduMap.markerIconRed(marker);
    }
    return marker;
}
//背景变蓝
baiduMap.markerIconBlue = function (marker) {
    var row = 2;//第row+1行
    var icon = marker.getIcon();
    icon.setImageOffset(new BMap.Size(-225, -223));
    marker.setIcon(icon);
    return marker;
}
//背景还原成红色
baiduMap.markerIconRed = function (marker) {
    var row = 0;//第row+1行
    var icon = marker.getIcon();
    icon.setImageOffset(new BMap.Size(-225, -187));
    marker.setIcon(icon);
    return marker;
}
//分页html
baiduMap.getPaging = function (allPages, curPage, onclick) {
    if (onclick==null) {
        onclick = "toPage";
    }
    var div = "<div class=\"poi-page\" id=\"poi_page\"><p class=\"page\">";
    if (curPage>1) {
        div += "<span class=\"curPage\"><a onclick=\""+onclick+"(" + (curPage-1) + ");return false;\" tid=\"secPageNum\" href=\"javascript:void(0)\">上一页</a></span>";
    }
    var pageBegin = curPage - 1;
    if (pageBegin<1) {
        pageBegin = 1;
    }
    if (pageBegin > allPages - 1) {
        pageBegin = allPages - 1;
    }
    var pageEnd = pageBegin + 3;
    if(pageEnd>allPages){
        pageEnd = allPages;
    }
    for (var i = pageBegin; i <= pageEnd; i++) {
        if (curPage == i) {
            div += "<span class=\"curPage\">" + i + "</span>";
        } else {
            div += "<span><a onclick=\"" + onclick + "(" + i + ");return false;\" tid=\"secPageNum\" href=\"javascript:void(0)\">" + i + "</a></span>";
        }
    }
    if (curPage<allPages) {
        div += "<span class=\"curPage\"><a onclick=\"" + onclick + "(" + (curPage + 1) + ");return false;\" tid=\"secPageNum\" href=\"javascript:void(0)\">下一页</a></span>";
    }
    div += "</p></div>";
    return div;
}