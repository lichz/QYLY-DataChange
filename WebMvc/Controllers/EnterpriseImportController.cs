using RoadFlow.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers {
    /// 楼宇导入
    public class EnterpriseImportController : Controller {
        private readonly int pageSize = 13;
        #region 企业信息
        // GET: /EnterpriseImport/
        public ActionResult Index() {
            return Index(null);
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection) {
            DataTable dt = new DataTable();
            string keyword = string.Empty;
            string pager = string.Empty;
            string wher = string.Empty;
            string query = string.Empty;
            string TYSHXYDM = string.Empty;
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>();
            if (collection != null) {
                if (!TYSHXYDM.IsNullOrEmpty()) {
                    keyword = Request["Keyword"];
                    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TYSHXYDM", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), keyword);
                }
            }
            query = string.Format("&appid={0}&tabid={1}&Keyword={2}&flag={3}", Request.QueryString["appid"], Request.QueryString["tabid"], keyword, Request.QueryString["flag"]);
            RoadFlow.Platform.EnterpriseInfo infoDb = new RoadFlow.Platform.EnterpriseInfo();
            dt = infoDb.GetPagerData(out pager, query, pageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            ViewBag.DisplayName = MyExtensions.GetModelDispalyName<RoadFlow.Data.Model.EnterpriseInfo>(new List<string>() { "Name", "TYSHXYDM", "Type", "ArtificialPerson" });
           
            ViewBag.Pager = pager;
            ViewBag.Keyword = keyword;
            return View(dt);
        }


        /// <summary>
        /// 详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id) {
            string query = string.Format("&appid={0}&tabid={1}&flag={2}", Request.QueryString["appid"], Request.QueryString["tabid"], Request.QueryString["flag"]);
            RoadFlow.Data.Model.EnterpriseInfo model = new RoadFlow.Platform.EnterpriseInfo().Get<RoadFlow.Data.Model.EnterpriseInfo>(new KeyValuePair<string, object>("ID", id));
            return View(model);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id) {
            string query = string.Format("&appid={0}&tabid={1}&flag={2}", Request.QueryString["appid"], Request.QueryString["tabid"], Request.QueryString["flag"]);
            RoadFlow.Data.Model.EnterpriseInfo model = new RoadFlow.Platform.EnterpriseInfo().Get<RoadFlow.Data.Model.EnterpriseInfo>(new KeyValuePair<string, object>("ID", id));
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(RoadFlow.Data.Model.EnterpriseInfo model, int keyId) {
            //RoadFlow.Data.Model.EnterpriseInfo model = new RoadFlow.Platform.EnterpriseInfo().Get<RoadFlow.Data.Model.EnterpriseInfo>(new KeyValuePair<string, object>("ID", keyId));
            int result = new RoadFlow.Platform.EnterpriseInfo().Update(model, keyId);
            if (result > 0) {
                return RedirectToAction("Index", new { appid = Request["appid"] });
            }
            return View();
        }
        #endregion

        #region 地税税收
        public ActionResult LandTax() {
            return LandTax(null);
        }

        [HttpPost]
        public ActionResult LandTax(FormCollection collection) {
            DataTable dt = new DataTable();
            string keyword = string.Empty;
            string pager = string.Empty;
            string wher = string.Empty;
            string query = string.Empty;
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>();
            if (collection != null) {
                if (!keyword.IsNullOrEmpty()) {
                    keyword = Request["Keyword"];
                    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("EnterpriseName", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), keyword);
                }
            }
            query = string.Format("&appid={0}&tabid={1}&Keyword={2}&flag={3}", Request.QueryString["appid"], Request.QueryString["tabid"], keyword, Request.QueryString["flag"]);
            RoadFlow.Platform.LandTax db = new RoadFlow.Platform.LandTax();
            dt = db.GetPagerData(out pager, query, pageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            ViewBag.DisplayName = MyExtensions.GetModelDispalyName<RoadFlow.Data.Model.LandTax>(new List<string>() { "EnterpriseName", "YYS", "QYSDS", "GRSDS", "Area" });

            //导入税收条数
            DataTable all = db.GetAll(where);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("EnterpriseID", RoadFlow.Data.Model.SQLFilterType.MINNotEqual), 0);
            DataTable matching = db.GetAll(where);
            ViewBag.All = all.Rows.Count;
            ViewBag.Matching = matching.Rows.Count;
            ViewBag.NotMatching = all.Rows.Count - matching.Rows.Count;

            ViewBag.Pager = pager;
            ViewBag.Keyword = keyword;
            return View(dt);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LandTaxEdit(int id) {
            string query = string.Format("&appid={0}&tabid={1}&flag={2}", Request.QueryString["appid"], Request.QueryString["tabid"], Request.QueryString["flag"]);
            RoadFlow.Data.Model.LandTax model = new RoadFlow.Platform.LandTax().Get<RoadFlow.Data.Model.LandTax>(new KeyValuePair<string, object>("ID", id));
            return View(model);
        }
        [HttpPost]
        public ActionResult LandTaxEdit(RoadFlow.Data.Model.LandTax model, int id) {
            //RoadFlow.Data.Model.LandTax model = new RoadFlow.Platform.LandTax().Get<RoadFlow.Data.Model.LandTax>(new KeyValuePair<string, object>("ID", id));
            int result = new RoadFlow.Platform.LandTax().Update(model, id);
            if (result > 0) {
                return RedirectToAction("LandTax", new { appid = Request["appid"] });
            }
            return View();
        }

        #endregion

        #region 国税税收
        public ActionResult CentralTax() {
            return CentralTax(null);
        }

        [HttpPost]
        public ActionResult CentralTax(FormCollection collection) {
            DataTable dt = new DataTable();
            string keyword = string.Empty;
            string pager = string.Empty;
            string wher = string.Empty;
            string query = string.Empty;
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>();
            if (collection != null) {
                if (!keyword.IsNullOrEmpty()) {
                    keyword = Request["Keyword"];
                    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("EnterpriseName", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), keyword);
                }
            }
            query = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            RoadFlow.Platform.CentralTax db = new RoadFlow.Platform.CentralTax();
            dt = db.GetPagerData(out pager, query, pageSize, RoadFlow.Utility.Tools.GetPageNumber(), where);
            ViewBag.DisplayName = MyExtensions.GetModelDispalyName<RoadFlow.Data.Model.CentralTax>(new List<string>() { "EnterpriseName", "ZZS", "QYSDS", "Area" });

            //导入税收条数
            DataTable all = db.GetAll(where);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("EnterpriseID", RoadFlow.Data.Model.SQLFilterType.MINNotEqual), 0);
            DataTable matching = db.GetAll(where);
            ViewBag.All = all.Rows.Count;
            ViewBag.Matching = matching.Rows.Count;
            ViewBag.NotMatching = all.Rows.Count - matching.Rows.Count;

            ViewBag.Pager = pager;
            ViewBag.Keyword = keyword;
            return View(dt);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult CentralTaxEdit(int id) {
            string query = string.Format("&appid={0}&tabid={1}&flag={2}", Request.QueryString["appid"], Request.QueryString["tabid"], Request.QueryString["flag"]);
            RoadFlow.Data.Model.CentralTax model = new RoadFlow.Platform.CentralTax().Get<RoadFlow.Data.Model.CentralTax>(new KeyValuePair<string, object>("ID", id));
            return View(model);
        }
        [HttpPost]
        public ActionResult CentralTaxEdit(RoadFlow.Data.Model.CentralTax model, int id) {
            //RoadFlow.Data.Model.LandTax model = new RoadFlow.Platform.LandTax().Get<RoadFlow.Data.Model.LandTax>(new KeyValuePair<string, object>("ID", id));
            int result = new RoadFlow.Platform.CentralTax().Update(model, id);
            if (result > 0) {
                return RedirectToAction("CentralTax", new { appid = Request["appid"] });
            }
            return View();
        }

        #endregion

        #region 公共
        //导入
        [HttpPost]
        public string Import(string path, string type,string area) {
            try {
                if (path.IsNullOrEmpty()) {
                    return string.Empty;
                }
                int allCount = 0;
                if (path.Contains("|")) {//多文件
                    string[] arr = path.Split('|');
                    for (var i = 0; i < arr.Length; i++) {
                        allCount += Case(arr[i], type,area);
                    }
                } else {
                    allCount += Case(path, type, area);
                }
                return RoadFlow.Utility.ObjectExpand.ToJson(new { success = true, allCount });
            } catch (Exception e) {
                return RoadFlow.Utility.ObjectExpand.ToJson(new { success = false,message = e.Message });
            }
        }

        //类型分支（返回导入行数）
        private int Case(string path, string type, string area) {
            if (type == "info") {//企业信息导入
                var list = ExportExcel.ImportToTable<RoadFlow.Data.Model.EnterpriseInfo>(Server.MapPath(path));
                return new RoadFlow.Platform.EnterpriseInfo().ImportToDatabase(list);
            } else if (type == "landTax") {//地税
                var list = ExportExcel.ImportToTable<RoadFlow.Data.Model.LandTax>(Server.MapPath(path));
                return new RoadFlow.Platform.LandTax().ImportToDatabase(list, area);
            } else if (type == "centralTax") {//国税
                var list = ExportExcel.ImportToTable<RoadFlow.Data.Model.CentralTax>(Server.MapPath(path));
                return new RoadFlow.Platform.CentralTax().ImportToDatabase(list, area);
            }
            return 0;
        }


        #endregion

    }
}
