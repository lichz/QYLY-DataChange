using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            string msg;
            if (!WebMvc.Common.Tools.CheckLogin(out msg)) {
                return RedirectToAction("Index", "Login");
                //return Redirect(Url.Action("Index", "Login"));
            }

            RoadFlow.Platform.UsersRole buserRole = new RoadFlow.Platform.UsersRole();
            RoadFlow.Platform.Role brole = new RoadFlow.Platform.Role();
            var roles = buserRole.GetByUserID(RoadFlow.Platform.UsersBLL.CurrentUserID);
            ViewBag.RoleLength = roles.Count;
            ViewBag.DefaultRoleID = string.Empty;
            ViewBag.RolesOptions = string.Empty;
            if (roles.Count > 0)
            {
                var mainRole = roles.Find(p => p.IsDefault);
                ViewBag.defaultRoleID = mainRole != null ? mainRole.RoleID.ToString() : roles.First().RoleID.ToString();
                List<RoadFlow.Data.Model.Role> roleList = new List<RoadFlow.Data.Model.Role>();
                foreach (var role in roles)
                {
                    var role1 = brole.Get(role.RoleID);
                    if (role1 == null)
                    {
                        continue;
                    }
                    roleList.Add(role1);
                }

                ViewBag.RolesOptions = brole.GetRoleOptions("", "", roleList);
            }

            var user = RoadFlow.Platform.UsersBLL.CurrentUser;
            ViewBag.UserName = user == null ? "" : user.Name;
            ViewBag.DateTime = DateTime.Now.ToDateWeekString();

            return View();
        }

        public ActionResult Home()
        {
            return View();
        }

        //
        // GET: /Home/
        public ActionResult LeadPage() {
            return View();
        }


        public string Menu()
        { 
            string roleID = Request.QueryString["roleid"];
            string userID = Request.QueryString["userid"];
            Guid gid,uid;
            if(!roleID.IsGuid(out gid) || !userID.IsGuid(out uid))
            {
                return "[]";
            }
            else
            {
                return new RoadFlow.Platform.RoleApp().GetRoleAppJsonString(gid, uid, Url.Content("~/").TrimEnd('/'));
            }
        }

        public string MenuRefresh()
        { 
            string roleID=Request.QueryString["roleid"];
            string userID = Request.QueryString["userid"];
            string refreshID = Request.QueryString["refreshid"];
            Guid gid,refreshid,uid;
            if(!roleID.IsGuid(out gid) || !refreshID.IsGuid(out refreshid) || !userID.IsGuid(out uid))
            {
                return "[]";
            }
            else
            {
                return new RoadFlow.Platform.RoleApp().GetRoleAppRefreshJsonString(gid, uid, refreshid, Url.Content("~/").TrimEnd('/'));
            }
        }


        ///// <summary>
        ///// Action执行前判断
        ///// </summary>
        ///// <param name="filterContext"></param>
        //private  void xx() {
        //    string msg;
        //    if (!this.CheckLogin(out msg)) {
        //        if (filterContext.HttpContext.Request.IsAjaxRequest()) {
        //            filterContext.Result = Content("{\"loginstatus\":-1}");
        //        } else {
        //            filterContext.Result = Content(string.Concat("<script>",
        //                msg.IsNullOrEmpty() ? "" : string.Format("alert('{0}');", msg),
        //                string.Compare(filterContext.Controller.ToString(), "WebMvc.Controllers.HomeController", true) == 0 ? "top.location='" + Url.Content("~/Login") + "'" : "top.login();", "</script>"), "text/html");
        //        }
        //    }

        //    base.OnActionExecuting(filterContext);
        //}

        ///// <summary>
        ///// 验证登录
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //protected virtual bool CheckLogin(out string msg) {
        //    return WebMvc.Common.Tools.CheckLogin(out msg);
        //}

        //public ActionResult Test() {
        //    return PartialView("~/Views/WorkFlowFormDesigner/Forms/e58f8cdb-0b45-4319-a2a1-8015b6fc8cf5.cshtml");
        //}


        //public ActionResult Test2() {
        //    return PartialView("~/Views/WorkFlowFormDesigner/Forms/44a7c61b-736c-4ca2-9a27-0305fe57bdc9.cshtml");
        //}
    }
}
