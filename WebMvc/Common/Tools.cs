﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebMvc.Common
{
    public static class Tools
    {
        /// <summary>
        /// 包含文件
        /// </summary>
        public static string IncludeFiles
        {
            get 
            {
                return
                    string.Format(@"<link href=""{0}Content/Theme/Common.css"" rel=""stylesheet"" type=""text/css"" media=""screen""/>
    <link href=""{0}Content/Theme/{1}/Style/style.css"" id=""style_style"" rel=""stylesheet"" type=""text/css"" media=""screen""/>
    <link href=""{0}Content/Theme/{1}/Style/ui.css"" id=""style_ui"" rel=""stylesheet"" type=""text/css"" media=""screen""/> 
    <script type=""text/javascript"" src=""{0}Scripts/My97DatePicker/WdatePicker.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/jquery-1.11.1.min.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/jquery.cookie.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/json.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.core.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.button.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.calendar.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.file.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.member.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.dict.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.menu.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.select.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.combox.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.tab.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.text.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.textarea.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.editor.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.tree.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.validate.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.window.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.dragsort.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.selectico.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.accordion.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.grid.js""></script>
    <script type=""text/javascript"" src=""{0}Scripts/roadui.init.js""></script>"
    , BaseUrl, RoadFlow.Utility.Config.Theme);
            }
        }

        public static string BaseUrl
        {
            get
            { 
                var obj = System.Web.HttpContext.Current.Session[RoadFlow.Utility.Keys.SessionKeys.BaseUrl.ToString()];
                return obj == null ? "/" : obj.ToString();
            }
        }

        public static bool CheckLogin(out string msg)
        {
            msg = "";
            object session = System.Web.HttpContext.Current.Session[RoadFlow.Utility.Keys.SessionKeys.UserID.ToString()];
            Guid uid;
            if (session == null || !session.ToString().IsGuid(out uid) || uid == Guid.Empty)
            {
                return false;
            }

            //#if DEBUG
            return true; //正式使用时请注释掉这一行
            //#endif

            //string uniqueIDSessionKey = RoadFlow.Utility.Keys.SessionKeys.UserUniqueID.ToString();
            //var user = new RoadFlow.Platform.OnlineUsers().Get(uid);
            //if (user == null)
            //{
            //    return false;
            //}
            //else if (System.Web.HttpContext.Current.Session[uniqueIDSessionKey] == null)
            //{
            //    return false;
            //}
            //else if (string.Compare(System.Web.HttpContext.Current.Session[uniqueIDSessionKey].ToString(), user.UniqueID.ToString(), true) != 0)
            //{
            //    msg = string.Format("您的帐号在{0}登录,您被迫下线!", user.IP);
            //    return false;
            //}
            //return true;
        }

        public static bool CheckLogin(bool redirect = true)
        {
            string msg;
            if (!CheckLogin(out msg))
            {
                if (!redirect)
                {
                    System.Web.HttpContext.Current.Response.Write("登录验证失败!");
                    System.Web.HttpContext.Current.Response.End();
                    return false;
                }
                else
                {
                    System.Web.HttpContext.Current.Response.Write("<script>top.login();</script>");
                    System.Web.HttpContext.Current.Response.End();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 检查应用程序权限
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        public static bool CheckApp(out string msg, string appid = "")
        {
            msg = "";
            appid = appid.IsNullOrEmpty() ? System.Web.HttpContext.Current.Request.QueryString["appid"] : appid;
            Guid appGuid;
            if (!appid.IsGuid(out appGuid))
            {
                return false;
            }
            var app = new RoadFlow.Platform.RoleApp().GetFromCache(appid);
            if (app != null)
            {
                var roles = RoadFlow.Platform.UsersBLL.CurrentUserRoles;
                if (roles.Contains(app["RoleID"].ToString().Convert<Guid>()))
                {
                    return true;
                }
                else
                {
                    msg = "<script>top.login();</script>";
                }
            }
            else
            {
                var userID = RoadFlow.Platform.UsersBLL.CurrentUserID;
                if (userID.IsEmptyGuid())
                {
                    msg = "<script>top.login();</script>";
                    return false;
                }
                var userApp = new RoadFlow.Platform.UsersApp().GetUserDataRows(userID);
                foreach (System.Data.DataRow dr in userApp)
                {
                    if (dr["ID"].ToString().Convert<Guid>() == appGuid)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        /// 将当前url参数转换为RouteValueDictionary(以便于在mvc中重定向时的参数)
        /// </summary>
        /// <returns></returns>
        public static System.Web.Routing.RouteValueDictionary GetRouteValueDictionary()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            string query = System.Web.HttpContext.Current.Request.Url.Query;
            if (query.IsNullOrEmpty())
            {
                return new System.Web.Routing.RouteValueDictionary(dict);
            }
            string[] queryArray = query.TrimStart('?').Split('&');
            foreach (string q in queryArray)
            {
                string[] qArray = q.Split('=');
                if (qArray.Length < 2)
                {
                    continue;
                }
                dict.Add(qArray[0], qArray[1]);
            }
            return new System.Web.Routing.RouteValueDictionary(dict);
        }

        /// <summary>
        /// 关闭本窗口，刷新父窗口
        /// </summary>
        /// <returns></returns>
        public static string CloseReloadWin(string message) {
            return "alert('" + message + "'); new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
        }

        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="key">表字段</param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> GetAllTaskByUserId(Guid userID, string key, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            RoadFlow.Platform.WorkFlowTask taskDb = new RoadFlow.Platform.WorkFlowTask();
            string p = string.Empty;
            List<RoadFlow.Data.Model.WorkFlowTask> list = taskDb.GetTasks(userID,
               out p, "", "", "", "", "", "", 1);
            List<string> listStr = new List<string>();
            foreach (var item in list) {
                listStr.Add(item.InstanceID);
            }
            if (listStr.Count > 0 && RoadFlow.Platform.UsersBLL.CurrentUserName != "系统管理员") {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(key, RoadFlow.Data.Model.SQLFilterType.IN), listStr);
            } else if (RoadFlow.Platform.UsersBLL.CurrentUserName != "系统管理员") {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>(key, RoadFlow.Data.Model.SQLFilterType.EQUAL), "00000000-0000-0000-0000-000000000000");
            }
            return where;
        }


        /// <summary>
        /// 编码转换
        /// </summary>
        /// <returns></returns>
        public static DataTable DictionaryIdToName(DataTable dt) {
            Dictionary<string, string> piName = new Dictionary<string, string>();
            var list = new RoadFlow.Platform.DictionaryBLL().GetAll();

            //编码转换
            for (int i = 0; i < dt.Rows.Count; i++) {
                for (int j = 0; j < dt.Columns.Count; j++) {
                    if (dt.Columns[j].ColumnName == "SW_ZJ" || dt.Columns[j].ColumnName == "SY_ZJ") {

                    } else {
                        if (list.FindAll(x => x.Code == dt.Columns[j].ColumnName).Count > 0) {
                            string temp = Convert.ToString(dt.Rows[i][j]);
                            RoadFlow.Data.Model.DictionaryModel dictionary = list.Find(p => p.ID.ToString().ToUpper() == temp.ToUpper());
                            if (dictionary != null) {
                                dt.Rows[i][j] = dictionary.Title;
                            } else {
                                dt.Rows[i][j] = "已删除";
                            }

                        }
                    }
                }
            }
            return dt;
        }

    }

}