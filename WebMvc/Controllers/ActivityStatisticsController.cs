using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RoadFlow.Data.Model;
using System.Data;

namespace WebMvc.Controllers
{
    public class ActivityStatisticsController : Controller {
        public ActionResult Index() {
            //RoadFlow.Utility.Tools.GetPageNumber()配合框架使用。
            DataTable dt = GetAll(RoadFlow.Utility.Tools.GetPageNumber(), Request["pagesize"].Convert<int>(14));
            return View(dt);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection) {
            string query = "{}";
            if (!Request["dataBegin"].IsNullOrEmpty() && !Request["dataEnd"].IsNullOrEmpty()) {
                query = RoadFlow.Utility.ObjectExpand.ToJson(new { TimeBegin = Request["dataBegin"], TimeEnd = Request["dataEnd"] });
                ViewBag.dataBegin = Request["dataBegin"];
                ViewBag.dataEnd = Request["dataEnd"];
            } else {
                if (!Request["dataBegin"].IsNullOrEmpty()) {
                    query = RoadFlow.Utility.ObjectExpand.ToJson(new { TimeBegin = Request["dataBegin"] });
                    ViewBag.dataBegin = Request["dataBegin"];
                }
                if (!Request["dataEnd"].IsNullOrEmpty()) {
                    query = RoadFlow.Utility.ObjectExpand.ToJson(new { TimeEnd = Request["dataEnd"] });
                    ViewBag.dataEnd = Request["dataEnd"];
                }
            }
            DataTable dt = GetAll(query, RoadFlow.Utility.Tools.GetPageNumber(), 14);
            return View(dt);
        }

        /// <summary>
        /// 获取所有岗位
        /// </summary>
        public DataTable GetAll(int pageIndex, int pageSize = 14) {
            return GetAll("{}",pageIndex,pageSize);
        }
        /// <summary>
        /// 获取所有岗位
        /// </summary>
        public DataTable GetAll(string query,int pageIndex, int pageSize = 14) {
            RoadFlow.Platform.ActivityStatistics a = new RoadFlow.Platform.ActivityStatistics();
            int allPage = 0, count = 0;
            DataTable dt = a.GetPagerData(out allPage, out count, query, pageIndex, pageSize);
            //ViewBag.Pager = RoadFlow.Utility.Tools.GetPagerHtml(count, pageSize, pageIndex, null);
            ViewBag.Pager = MyExtensions.GetPagerHtml(count, pageSize, pageIndex);
            return dt;
        }
        
        /// <summary>
        /// 获取所有岗位(页面Ajax访问)
        /// </summary>
        /// <param name="query">查询参数</param>
        /// <param name="pageIndex">页标</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        public string GetAllToJson(string query, int pageIndex, int pageSize = 30) {
            RoadFlow.Platform.ActivityStatistics a = new RoadFlow.Platform.ActivityStatistics();
            int allPage = 0, count = 0;
            DataTable dt = a.GetPagerData(out allPage, out count, query, pageIndex, pageSize);
            //ViewBag.Pager = RoadFlow.Utility.Tools.GetPagerHtml(count, pageSize, pageIndex, null);
            ViewBag.Pager = MyExtensions.GetPagerHtml(count, pageSize, pageIndex);
            return RoadFlow.Utility.ObjectExpand.ToJson(dt);
        }

    }
}
