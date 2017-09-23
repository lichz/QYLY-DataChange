using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using manage.Models;
using Show.Models;

namespace Show.Controllers
{
    public class MapController : Controller
    {
        private Context db = new Context();
        #region Action
         // GET: 地图
        public ActionResult Map()
        {
            HomeMapViewModel viewModel = new HomeMapViewModel();
            var parent = db.Dictionarys.Where(p => p.Code == "SSJD").FirstOrDefault();
            viewModel.Street = db.Dictionarys.Where(p => p.ParentID == parent.ID);
            return View(viewModel);
        }
        #endregion

        #region 异步
        //地图获取楼宇列表
        public JsonResult GetList(string ssjd, string zmj, string name)
        {
            IQueryable<BuildingModel> list = db.BuildingsAndBuildingMonthInfo.Where(p=> p.IsImportant != null&&p.Status == Status.Normal);
            if (!string.IsNullOrWhiteSpace(ssjd))
            {
                Guid ssjdGuid = Guid.Parse(ssjd);
                list = list.Where(p => p.SSJD == ssjdGuid);
            }
            if (!string.IsNullOrWhiteSpace(zmj))
            {
                int begin = zmj.Split(',')[0].Convert<int>(0);
                if (begin != 0)
                {
                    list = list.Where(p => p.ZJZMJ > begin);
                }
                int end = zmj.Split(',')[1].Convert<int>(0);
                if (end != 0)
                {
                    list = list.Where(p => p.ZJZMJ < end);
                }
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                list = list.Where(p => p.Name.Contains(name));
            }
            return Json(new { success = true, list = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}