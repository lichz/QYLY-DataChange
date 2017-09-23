<%@ Page Language="C#" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>地图</title>
    <style>
    .map { height: 400px;}
    .mybutton{   background: #0695ce; border: 1px solid #0e66c8;color: #ffffff;line-height: 25px;vertical-align: middle; height: 25px;}
</style>
    <script src="../jquery-1.11.1.js"></script>
    <script src="../roadui.core.js"></script>
    <script src="../roadui.window.js"></script>
     <link href="../../Scripts/FormDesinger/ueditor/themes/default/dialogbase.css" rel="stylesheet" />
 <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=72f51002473d365dcc093849d5da0f44"></script>
</head>
<body>
    <div id="map" class="map"></div>
    <div class="buttondivs" style="text-align:center;margin-top:5px">
        <input id="btnOK" type="button" value="确定保存" class="mybutton" onclick="sure();" />
        <input type="button" class="mybutton" value="取消关闭" onclick="cancel();" style="margin-left: 5px;"  />
    </div>
</body>
</html>
<script type="text/javascript">
    var name = '<%=Request.QueryString["name"]%>';
    var tabid = '<%=Request.QueryString["tabid"]%>';
    var parentWindow = parent.frames[tabid + "_iframe"];
    var showBtn = '<%=Request.QueryString["showFlag"]%>';
    //百度地图API功能
    var map = new BMap.Map("map");
    $(function () {
        map.centerAndZoom(new BMap.Point(103.983973, 30.68976), 14);  // 初始化地图,设置中心点坐标和地图级别
        //map.addControl(new BMap.MapTypeControl());   //添加地图类型控件
        map.setCurrentCity("青羊");          // 设置地图显示的城市 此项是必须设置的
        map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放
        var dotpoint = $("[name='" + name + "']", parentWindow.document).val()
        if (dotpoint != "") {
            var grouppoint = dotpoint.split("|");
            for (i = 0; i < grouppoint.length; i++) {
                var point = new BMap.Point(grouppoint[i].split(",")[0], grouppoint[i].split(",")[1]);
                var marker = new BMap.Marker(point);
                map.addOverlay(marker);
                map.centerAndZoom(point, 14);
            }
        }
        //确定按钮是否显示
        if (showBtn == "0") {
            $("#btnOK").hide();
        } else {
            $("#btnOK").show();
        }
        map.addEventListener("click", showInfo);
    });

    function showInfo(e) {
        var point = new BMap.Point(e.point.lng, e.point.lat);
        var marker = new BMap.Marker(point);
        map.clearOverlays();   
        map.addOverlay(marker);
        dotpoint = e.point.lng + ", " + e.point.lat
    }

    function cancel() {
        new RoadUI.Window().close();
    }

    function sure() {
        $("[name='" + name + "']", parentWindow.document).val(dotpoint);
        new RoadUI.Window().close();
    }
</script>
