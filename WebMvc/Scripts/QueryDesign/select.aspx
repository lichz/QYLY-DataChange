<%@ Page Language="C#" %>
<%@ Import Namespace="RoadFlow.Utility" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="common.js"></script>
    <link href="../../Scripts/FormDesinger/ueditor/themes/default/dialogbase.css" rel="stylesheet" />
    <style>
    .tabbody{height:240px!important;}
    </style>
    <%=WebMvc.Common.Tools.IncludeFiles %>
</head>
<body>
<% 
    WebMvc.Common.Tools.CheckLogin();
    RoadFlow.Platform.WorkFlowForm workFlowFrom = new RoadFlow.Platform.WorkFlowForm(); 
%>
<div class="wrapper">
    <div id="tabhead" class="tabhead">
        <span class="tab focus" data-content-id="text_attr">&nbsp;&nbsp;属性&nbsp;&nbsp;</span>
        <span class="tab" data-content-id="text_default" onclick="loadOptions();">&nbsp;&nbsp;默认值&nbsp;&nbsp;</span>
  
    </div>
    <div id="tabbody" class="tabbody" style="height:300px;">
        <div id="text_attr" class="panel focus">
    <table cellpadding="0" cellspacing="1" border="0" width="100%" class="formtable">  
        <tr>
            <th>数据源：</th>
            <td><%=workFlowFrom.GetDataSourceRadios("datasource","0","onclick='dsChange(this.value);'") %></td>
        </tr>
        <tr id="ds_dict">
            <th>字典项：</th>
            <td><input type="text" class="mydict" id="ds_dict_value" more="0" value="" /></td>
        </tr>   
    </table>
    <table cellpadding="0" cellspacing="1" border="0" width="100%" id="ds_custom" style="display:none; margin-top:5px;" align="center">
        <tr>
            <td colspan="2">
                <fieldset style="padding:5px;">
                <legend style=""> 自定义选项 </legend>
                <div style="margin:0 auto; padding:0 5px;">
                    <div style="height:25px; padding:5px 0;">格式：选项文本1,选项值1;选项文本2,选项值2</div>
                    <textarea class="mytextarea" id="custom_string" style="height:122px; width:100%;"></textarea>
                </div>
                </fieldset>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="1" border="0" width="100%" id="ds_sql" style="display:none; margin-top:5px;" align="center">
        <tr>
            <td colspan="2">
                <fieldset style="padding:3px;">
                <legend style=""> SQL语句 </legend>
                <table border="0" style="width:100%;">
                    <tr>
                        <td style="width:80%">
                          <div>1.SQL应返回两个字段的数据源</div>
                          <div>2.第一个字段为值，第二个字段为标题</div>
                          <div>3.如果只返回一个字段则值和标题一样</div>
                        </td>
                        <td>
                            <input type="button" value="测试SQL" onclick="testSql($('#ds_sql_value').val());" class="mybutton" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-top:4px;">
                            <textarea cols="1" rows="1" id="ds_sql_value" style="width:99%; height:80px; font-family:Verdana;" class="mytextarea"></textarea>
                        </td>
                    </tr>
                </table>
                </fieldset>
            </td>
        </tr>
    </table>
    </div>
        <div id="text_default" class="panel">
            <div id="text_default_list" style="height:288px; overflow:auto;">

            </div>
        </div>
        <div id="text_event" class="panel">
           <%Server.Execute("/Scripts/FormDesinger/ueditor/plugins/dialogs/events.aspx"); %>
        </div>
    </div>
</div>
        <div class="buttondivs" style="text-align:center;margin-top:5px">
        <input type="button" value="确定保存" class="mybutton" onclick="ok()" />
        <input type="button" class="mybutton" value="取消关闭" onclick="winclose()" style="margin-left: 5px;"  />
    
    </div>
    <%
        var json = new RoadFlow.Data.Model.Element();
        var datastr = Request.QueryString["data"];
        if (!datastr.IsNullOrEmpty()) 
        {
             json = Request.QueryString["data"].ToString().JsonConvertModel<RoadFlow.Data.Model.Element>();
        }
       
     %>
    <div id="data">
         <input type="hidden"  id="datasourse" name="<%=Request.QueryString["name"] %>" title="<%=Request.QueryString["title"] %>" datasource="<%=json.dictid!=""?"0":json.sql!=""?"2":"1" %>"  dictid="<%=json.dictid!=null?json.dictid:"" %>" sql="<%=json.sql!=null?json.sql:"" %>"   value="" defaultvalue="" hasempty="0"   customopts=<%=json.customopts!=null?json.customopts.ToJson():"" %> /></div>
         <input type="hidden" id="tabid" value="<%="window_" + Request.QueryString["appid"].Replace("-","") + "_iframe"%>" />
    <script type="text/javascript">

    </script>
<script type="text/javascript">

    var oNode = true, thePlugins = 'formselect';
    var tabid =$("#tabid").val();
    var parentWindow = parent[tabid].frames["Iframe"];
    var attJSON = parentWindow.formattributeJSON;
    var parentEvents = parent.formEvents;
    var events = [];
    var eventsid = RoadUI.Core.newid(false);
    $(function () {
        oNode = $("#datasourse");
        if (oNode) {
            $text = $(oNode);
            $("#hasempty").prop("checked", "1" == $text.attr("hasempty"));
            $("input[name='datasource'][value='" + $text.attr('datasource') + "']").prop('checked', true);
            if ("1" == $text.attr("datasource")) {
                $("#ds_dict").hide();
                $("#ds_sql").hide();
                $("#ds_custom").show();
                var custionJSONString = $text.attr("customopts");
                if ($.trim(custionJSONString).length > 0) {
                    var customJSON = JSON.parse(custionJSONString);
                    var customString = [];
                    for (var i = 0; i < customJSON.length; i++) {
                        customString.push(customJSON[i].title + "," + customJSON[i].value);
                    }
                    $("#custom_string").val(customString.join(';'));
                }
                new RoadUI.DragSort($("#custom_sort"));
            }
            else if ("0" == $text.attr("datasource")) {
                $("#ds_dict").show();
                $("#ds_sql").hide();
                $("#ds_custom").hide();
                $("#ds_dict_value").val($text.attr('dictid'));
                new RoadUI.Dict().setValue($("#ds_dict_value"));
            }
            else if ("2" == $text.attr("datasource")) {
                $("#ds_dict").hide();
                $("#ds_sql").show();
                $("#ds_custom").hide();
                $("#ds_sql_value").val($text.attr('sql'));
            }

            if ($text.attr('eventsid')) {
                eventsid = $text.attr('eventsid');
                events = getEvents(eventsid);
            }
        }

        new RoadUI.DragSort($("#custom_sort"));
        initTabs();
    });

    function loadOptions(e) {
        var $listDiv = $("#text_default_list");
        var datasource = $(":checked[name='datasource']").val();
        var dvalue = $(":checked", $listDiv).val() || ($(oNode).attr("defaultvalue") || "");
        $listDiv.html('');

        if ("1" == datasource) {
            var custom_string = ($("#custom_string").val() || "").split(';');
            for (var i = 0; i < custom_string.length; i++) {
                var titlevalue = custom_string[i].split(',');
                if (titlevalue.length != 2) {
                    continue;
                }
                var title = titlevalue[0];
                var value = titlevalue[1];
                var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                $listDiv.append(option);
            }
        }
        else if ("0" == datasource) {
            var dictid = $("#ds_dict_value").val();
            $.ajax({
                url: "/Scripts/FormDesinger/ueditor/plugins/dialogs/getdictchilds.aspx?dictid=" + dictid, cache: false, async: false, dataType: "json", success: function (json) {
                    for (var i = 0; i < json.length; i++) {
                        var title = json[i].title;
                        var value = json[i].id;
                        var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                        $listDiv.append(option);
                    }
                }
            });
        }
        else if ("2" == datasource) {
            var sql = $("#ds_sql_value").val();
            $.ajax({
                url: "/Scripts/FormDesinger/ueditor/plugins/dialogs/getsqljson.aspx", type: "post", data: { sql: sql, conn: attJSON.dbconn }, cache: false, async: false, dataType: "json", success: function (json) {
                    for (var i = 0; i < json.length; i++) {
                        var title = json[i].title;
                        var value = json[i].id;
                        var option = '<div><input type="radio" ' + (value == dvalue ? 'checked="checked"' : '') + ' value="' + value + '" id="defaultvalue_' + value + '" style="vertical-align:middle;" name="defaultvalue"/><label style="vertical-align:middle;" for="defaultvalue_' + value + '">' + title + '(' + value + ')</label></div>';
                        $listDiv.append(option);
                    }
                }
            });
        }
    }

    function dsChange(value) {
        if (value == 0) {
            $("#ds_dict").show();
            $("#ds_custom").hide();
            $("#ds_sql").hide();
        }
        else if (value == 1) {
            $("#ds_dict").hide();
            $("#ds_sql").hide();
            $("#ds_custom").show();
        }
        else if (value == 2) {
            $("#ds_dict").hide();
            $("#ds_custom").hide();
            $("#ds_sql").show();
        }
    }

    function winclose() {
        $(".gridpager", parentWindow.document).show();;
        new RoadUI.Window().close();
    }

    function ok() {

        var id = $("#datasourse").prop("name");
        var titleName = $("#datasourse").prop("title");
        var datasource = $(":checked[name='datasource']").val();
        var width = $("#width").val();
        var hasempty = $("#hasempty").prop("checked") ? "1" : "0";
        var radios = [];
        var dictid = "";
        var sql = "";
        var dvalue = $(":checked[name='defaultvalue']", $("#text_default_list")).val() || "";
        if ("1" == datasource) {
            var custom_string = ($("#custom_string").val() || "").split(';');
            for (var i = 0; i < custom_string.length; i++) {
                var titlevalue = custom_string[i].split(',');
                if (titlevalue.length != 2) {
                    continue;
                }
                var title = titlevalue[0];
                var value = titlevalue[1];
                radios.push({ title: title, value: value });
            }
        }
        else if ("0" == datasource) {
            dictid = $("#ds_dict_value").val();
        }
        else if ("2" == datasource) {
            sql = $("#ds_sql_value").val();
        }

        var html = '<input type="hidden"  id="datasourse" title="' + title + '" name="' + id + '" datasource="' + datasource + '" dictid="' + dictid + '"  defaultvalue="' + dvalue + '" hasempty="' + hasempty + '"';
        if (width) {
            html += 'style="width:' + width + '" ';
            html += 'width1="' + width + '" ';
        }
        if ("1" == datasource) {
            html += "customopts='" + JSON.stringify(radios) + "' ";
        }
        if ("2" == datasource) {
            html += 'sql="' + sql.replaceAll('"', '&quot;') + '" ';
        }
        html += ' />';

        $("#data").html(html);
        var result = "{'name':'" + id + "','type':'select','title':'" + titleName + "','dictid':'" + dictid + "','customopts':" + JSON.stringify(radios) + ",'sql':'" + sql + "'}";
        result == escape(result);
        $("#" + id, parentWindow.document).val(result);
        $(".gridpager", parentWindow.document).show();
        new RoadUI.Window().close();
    }
</script>

</body>
</html>