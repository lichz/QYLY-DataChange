using RoadFlow.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers {
    public class SMSController : Controller {
        private RoadFlow.Platform.SMS sms = new RoadFlow.Platform.SMS();
        private readonly int pageSize = 13;

        public ActionResult Index() {
            string pager = string.Empty;
            //DataTable dt = sms.GetDataPage(out pager, "", pageSize, 1);
            string query = string.Format("&appid={0}&tabid={1}&flag={2}", Request.QueryString["appid"], Request.QueryString["tabid"], Request.QueryString["flag"]);
            DataTable dt = sms.GetDataPage(out pager, query, pageSize, RoadFlow.Utility.Tools.GetPageNumber());

            ViewBag.Pager = pager;
            List<RoadFlow.Data.Model.SMSModel> list = dt.ToList<RoadFlow.Data.Model.SMSModel>();
            List<RoadFlow.Data.Model.SMSModel> view = new List<RoadFlow.Data.Model.SMSModel>();
            foreach(var item in list){

                RoadFlow.Platform.UsersBLL user = new RoadFlow.Platform.UsersBLL();
                RoadFlow.Platform.Organize organize = new RoadFlow.Platform.Organize();

                //把sendTo里的id取出来，然后清空sendTo
                string[] sendTo = item.SendTo.Split(',');
                item.SendTo = string.Empty;

                foreach (var id in sendTo) {
                    if (id.Contains("u_")) {//个人
                        string newId = id.Remove(0, 2);
                        if (newId.IsGuid()) {
                            RoadFlow.Data.Model.UsersModel u = user.Get(Guid.Parse(newId));
                            if (u != null) {
                                item.SendTo += "," + u.Name;
                            } else {
                                item.SendTo = ",用户已删除";
                            }
                        }
                    } else {//选中的是组织
                        if (id.IsGuid()) {
                            RoadFlow.Data.Model.Organize o = organize.Get(Guid.Parse(id));
                            if (o != null) {
                                item.SendTo += "," + o.Name;
                            } else {
                                item.SendTo = ",组织机构已删除";
                            }
                        }
                    }
                }
                item.SendTo = item.SendTo.Remove(0,1);//去掉第一个多余的",".
                view.Add(item);
            }
            return View(view);
        }

        public ActionResult Send() {
            return View();
        }

        /// <summary>
        /// 发送短信信息
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Send(RoadFlow.Data.Model.SMSModel model) {
            if (ModelState.IsValid) {
                model.SendUser = RoadFlow.Platform.UsersBLL.CurrentUserID;
                model.SendUserName = RoadFlow.Platform.UsersBLL.CurrentUserName;
                if (sms.Add(model) > 0) {//保存成功,调用第三方发送短信
                    RoadFlow.Platform.UsersBLL user = new RoadFlow.Platform.UsersBLL();
                    RoadFlow.Platform.Organize organize = new RoadFlow.Platform.Organize();

                    List<string> tels = new List<string>();
                    string[] sendTo = model.SendTo.Split(',');

                    foreach (var item in sendTo) {
                        if (item.Contains("u_")) {//个人
                            string newId = item.Remove(0, 2);
                            if (newId.IsGuid()) {
                                RoadFlow.Data.Model.UsersModel u = user.Get(Guid.Parse(newId));
                                if (u != null&&!u.Tell.IsNullOrEmpty()) {
                                    tels.Add(u.Tell);
                                }
                            }
                        } else {//选中的是组织
                            if (item.IsGuid()) {
                                RoadFlow.Data.Model.Organize o = organize.Get(Guid.Parse(item));
                                if (o != null) {
                                    List<RoadFlow.Data.Model.UsersModel> list = organize.GetAllUsers(Guid.Parse(item));
                                    foreach (var u in list) {//遍历组织里所有User
                                        if(!u.Tell.IsNullOrEmpty()){
                                            tels.Add(u.Tell);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (var tel in tels) {//遍历所有手机号，发送短信
                        SMSMessage message = new SMSMessage {
                            Mobile = tel,
                            Content = "【青羊楼宇】" + model.Content
                        };
                        new SMS().SendSMS(message);
                    }
                    ViewBag.Success = true;
                }
            }

            return View(model);
        }

    }
}
