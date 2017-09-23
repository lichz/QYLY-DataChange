using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RoadFlow.Utility;

namespace WebMvc.Controllers
{
    /// <summary>
    /// 查询配置
    /// </summary>
    public class QueryDesignController : Controller
    {
        //
        // GET: /QueryDesign/
        private readonly RoadFlow.Platform.DBConnection conn = new RoadFlow.Platform.DBConnection();
        private readonly RoadFlow.Platform.QueryDesign queryDesign = new RoadFlow.Platform.QueryDesign();
        public ActionResult Index()
        {
            return Index(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            string name = string.Empty;
            ViewBag.Query1 = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            RoadFlow.Platform.QueryDesign queryDesign = new RoadFlow.Platform.QueryDesign();
            IEnumerable<RoadFlow.Data.Model.QueryDesign> queryDesignList;

            if (collection != null)
            {
                if (!Request.Form["DeleteBut"].IsNullOrEmpty())
                {
                    string ids = Request.Form["checkbox_app"];
                    foreach (string id in ids.Split(','))
                    {
                        Guid bid;
                        if (!id.IsGuid(out bid))
                        {
                            continue;
                        }
                        queryDesign.Delete(bid);
                    }
                }
                queryDesignList = queryDesign.GetAll();

                if (!Request.Form["Search"].IsNullOrEmpty())
                {
                    name = Request.Form["Name"];
                    if (!name.IsNullOrEmpty())
                    {
                        queryDesignList = queryDesignList.Where(p => p.Name.IndexOf(name) >= 0);
                    }
                }
            }
            else
            {
                queryDesignList = queryDesign.GetAll();
            }
            ViewBag.Name = name;
            return View(queryDesignList);
        }

        public ActionResult Edit()
        {
            return Edit(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection collection)
        {
            string name = string.Empty;
            ViewBag.Query1 = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            string id = Request.QueryString["id"];
            string type = Request.QueryString["type"];
            var model = new RoadFlow.Data.Model.QueryDesign();
            Guid tid;
            if (id.IsGuid(out tid))
            {
                model = queryDesign.Get(tid);
            }

            ViewBag.Connopts = conn.GetAllOptions(model.ConnectionID.ToString()); 
            ViewBag.Tableopts =getTableOps(model.ConnectionID, model.TableName);
            ViewBag.type = type;

            return View(model);
        }

        public ActionResult DisplayItem()
        {
            return DisplayItem(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisplayItem(FormCollection collection)
        {
            RoadFlow.Data.Model.QueryDesign model = new QueryDesign();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            ViewBag.Query1 = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            string id = Request.QueryString["id"];
            string tableName = Request.QueryString["table"];
            string connid = Request.QueryString["connid"];
            string name = Request["SJCXNAMEFVHJ"];
            dic = conn.GetFields(connid.ToGuid(), tableName);
            if (id.IsNullOrEmpty())
            {
                 if (!Request["save"].IsNullOrEmpty())
                 {
                     string eleme = Request["reldata"];
                     eleme = "[" + eleme + "]";
                     model.ID = Guid.NewGuid();
                     model.Name = name;
                     model.TableName = tableName;
                     model.CreateUserID = RoadFlow.Platform.Users.CurrentUserID;
                     model.CreateUserName = RoadFlow.Platform.Users.CurrentUserName;
                     model.ConnectionID = connid.ToGuid();
                     model.DisplayItem = eleme;
                     model.Status = 0;
                     queryDesign.Add(model);
                     ViewBag.Script = "alert('保存成功!');new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
                 }
                 else
                 {
                     model.Name = name;
                     model.TableName = tableName;
                     model.ConnectionID = connid.ToGuid();
                 }
            }
            else
            {
                model = queryDesign.Get(id.ToGuid());
                if (connid.IsNullOrEmpty() || tableName.IsNullOrEmpty())
                {
                    tableName = model.TableName;
                    connid = model.ConnectionID.ToString();
                    dic = conn.GetFields(connid.ToGuid(), tableName);
                }
                if (!Request["save"].IsNullOrEmpty())
                {
                    string eleme = Request["reldata"];
                    eleme = "[" + eleme + "]";
                    model.Name = name;
                    model.TableName = tableName;
                    model.ConnectionID = connid.ToGuid();
                    model.DisplayItem = eleme;
                    queryDesign.Update(model);
                    ViewBag.Script = "alert('保存成功!');new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
                }
            }
            ViewBag.DisplayItem = model.DisplayItem;
            ViewBag.Connid = model.ConnectionID;
            ViewBag.TableName = model.TableName;
            ViewBag.Name = model.Name;
            return View(dic);
        }

        public ActionResult Content()
        {
            return Content(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Content(FormCollection collection)
        {
            string name = string.Empty;
            ViewBag.Query1 = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            string id = Request.QueryString["id"];
            string tableName = Request.QueryString["table"];
            string connid = Request.QueryString["connid"];
            RoadFlow.Data.Model.QueryDesign model= new QueryDesign();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (!id.IsNullOrEmpty()) {  model = queryDesign.Get(id.ToGuid()); }
            if (tableName.IsNullOrEmpty() && connid.IsNullOrEmpty()&&model!=null)
            {
                tableName = model.TableName;
                connid = model.ConnectionID.ToString();
                ViewBag.SearchJson = model.SearchJson;
                ViewBag.Name = model.Name;
            }

            //save
            if (!Request["save"].IsNullOrEmpty())
            {
                string eleme = Request["result"];
                eleme = "[" + eleme + "]";
                model.SearchJson = eleme;
                model.Name = Request["SJCXNAMEFVHJ"];
                if (!id.IsNullOrEmpty())
                {
                    queryDesign.Update(model);
                }
                else
                {
                    model.ConnectionID = connid.ToGuid();
                    model.CreateUserID = RoadFlow.Platform.Users.CurrentUserID;
                    model.CreateUserName = RoadFlow.Platform.Users.CurrentUserName;
                    model.ID = Guid.NewGuid();
                    model.TableName = tableName;
                    model.Status = 0;
                    queryDesign.Add(model);
                }
                ViewBag.SearchJson = model.SearchJson;
                ViewBag.Name = model.Name;
                ViewBag.Script = "alert('保存成功!');new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
            }

            if (!tableName.IsNullOrEmpty() && !connid.IsNullOrEmpty())
            {
                dic = conn.GetFields(connid.ToGuid(), tableName);
            }
            ViewBag.Connid = connid;
            ViewBag.TableName = tableName;
            return View(dic);
        }

        public ActionResult Search(string id)
        {
            var model = queryDesign.Get(id,RoadFlow.Platform.Users.CurrentUserID);
            return View(model);
        }

        public ActionResult Item(string id)
        {
            var model = queryDesign.Get(id, RoadFlow.Platform.Users.CurrentUserID);
            ViewBag.id = model.ID;
            return View(model);
        }

        /// <summary>
        /// 更新个人显示配置
        /// </summary>
        /// <param name="id"></param>
        /// <param name="retdata"></param>
        /// <returns></returns>
        public int UpdateItem(string id,string retdata)
        {
            try
            {
                if (retdata.IsNullOrEmpty()) return 1;
                List<ColItem> ls = retdata.JsonConvertModel<List<ColItem>>();
                var model = queryDesign.Get(id.ToGuid());
                string showItem = string.Empty;
                List<ColItem> ll = model.DisplayItem.JsonConvertModel<List<ColItem>>();
                //读取最大排序编号
                int cnt = 0;
                foreach (ColItem item in ll)
                {
                    if (item.chk == true)
                    {
                        if (item.sortid > cnt)
                        {
                            cnt = item.sortid;
                        }
                    }
                }
                foreach (ColItem it in ls)
                {
                    ll.Find(x => x.id == it.id).chk = it.chk;
                    it.sortid = it.sortid == 0 ? ++cnt : it.sortid;
                }
                showItem = ll.ToJson();
                if (model.CreateUserID == RoadFlow.Platform.Users.CurrentUserID)
                {
                    model.DisplayItem = showItem;
                    new RoadFlow.Platform.QueryDesign().Update(model);
                }
                else
                {
                    model.ID = Guid.NewGuid();
                    model.CreateUserID = RoadFlow.Platform.Users.CurrentUserID;
                    model.CreateUserName = RoadFlow.Platform.Users.CurrentUserName;
                    model.DisplayItem = showItem;

                    model.Status = 1;
                    new RoadFlow.Platform.QueryDesign().Add(model);
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return 1;
         }

        /// <summary>
        /// TABLE显示顺序排序用
        /// </summary>
        private class myComparer : IComparer<ColItem>
        {
            //实现按升序排列
            public int Compare(ColItem x, ColItem y)
            {
                return (x.sortid.CompareTo(y.sortid));
            }
        }

        /// <summary>
        /// 连接下所有表下拉框
        /// </summary>
        /// <param name="connid"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public string getTableOps(Guid connid,string table)
        {
            var options = "";
            var tableds =conn.GetTables(connid);
            foreach (var value in tableds)
            {
                options += "<option value='" + value + "' " + (value == table ? "selected='selected'" : "") + ">" + value + "</option>";
            }
            return options;
        }
    }
}
