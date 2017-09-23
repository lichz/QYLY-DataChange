using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using manage.Models;
using Show.Models;
using System.Data.Entity.SqlServer;
using System.Text;
using System.Data.Entity;

namespace Show.Controllers
{
    public class BuildingsController : Controller
    {
        private Context db = new Context();
        private int pageSize = 9;

        #region Action
        public ActionResult Index(int? pageIndex)
        {
            pageIndex = pageIndex ?? 1;
            //所属街道
            var parentID = db.Dictionarys.FirstOrDefault(p => p.Title == "所属街道").ID;
            return View(IndexInit(parentID,pageIndex));
        }
        /// <summary>
        /// 楼宇详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(Guid id)
        {
            return View(InitDetail(id));
        }
        #endregion

        #region Init
        public BuildingIndexViewModel IndexInit(Guid parentID,int? pageIndex)
        {
            BuildingIndexViewModel viewModel = new BuildingIndexViewModel();
            viewModel.SSJD = db.Dictionarys.Where(p => p.ParentID == parentID);
            //已建楼宇
            var buildedID = db.Dictionarys.FirstOrDefault(p => p.Title == "已建成").ID;
            viewModel.BuildedID = buildedID;
            //在建楼宇
            var buildingID = db.Dictionarys.FirstOrDefault(p => p.Title == "在建").ID;
            viewModel.BuildingID = buildingID;
            //重点楼宇
            var importantID = db.Dictionarys.FirstOrDefault(p => p.Title == "重点楼宇").ID;
            viewModel.ImportantID = importantID;
            //其他楼宇
            var otherID = db.Dictionarys.FirstOrDefault(p => p.Title == "其他楼宇").ID;
            viewModel.OtherID = otherID;
            //默认显示已建楼宇-重点楼宇
            IQueryable<BuildingModel> list1 = db.BuildingsAndBuildingMonthInfo.Where(p => p.JSJD == buildedID && p.IsImportant == importantID && p.Status == Status.Normal);
            viewModel.Pager = MyExtensions.GetPagingHtml(list1.Count(), pageSize, pageIndex.Value);
            viewModel.Buildings = db.BuildingsAndBuildingMonthInfo.Where(p => p.JSJD == buildedID && p.IsImportant == importantID && p.Status == Status.Normal).OrderByDescending(p => p.CreateTime).Skip(pageSize * (pageIndex.Value - 1)).Take(pageSize);
            viewModel.BuildingCount = viewModel.Buildings.Count();
            return viewModel;
        }
        public BuildingDetailViewModel InitDetail(Guid id)
        {
            BuildingDetailViewModel viewModel = new BuildingDetailViewModel();
            viewModel.Building = db.BuildingsAndBuildingMonthInfo.Include(p => p.SYZJ).Include(p => p.SSJDS).Include(p => p.HouseIDS).FirstOrDefault(p => p.ID == id);
            viewModel.HotBuilding = db.BuildingsAndBuildingMonthInfo.Where(p=>p.IsImportant!=null && p.Status == Status.Normal).Take(3).OrderByDescending(p => p.CreateTime).Include(p=>p.SYZJ);
            return viewModel;
        }
        #endregion

        #region 异步
        /// <summary>
        /// json 异步加载
        /// </summary>
        /// <param name="pageIndex">页下标</param>
        /// <param name="jsjd">建设阶段</param>
        /// <param name="typeid">是否重点楼宇</param>
        /// <param name="ssjd">所属街道</param>
        /// <param name="zmj">总面积</param>
        /// <param name="name">楼宇名称</param>
        /// <returns></returns>
        public JsonResult GetList(int pageIndex, Guid jsjd, Guid typeid, string ssjd, string zmj, string name)
        {
            StringBuilder html = new StringBuilder();
            IQueryable<BuildingModel> list = db.BuildingsAndBuildingMonthInfo.Where(p => p.IsImportant == typeid && p.JSJD == jsjd);
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
            int totalCount = list.Count();
            string pager = MyExtensions.GetPagingHtml(totalCount, pageSize, pageIndex);
            list = list.OrderByDescending(p => p.CreateTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize);

            int count = 0;
            foreach (var item in list)
            {
                count++;
                if (count % 3 == 0)
                {
                    html.Append(" <li class=\"fl\" style=\"margin-right:0;\"><div>");
                }
                else
                {
                    html.Append(" <li class=\"fl\"><div>");
                }
                html.AppendFormat(" <a href=\"{0}\"><img src=\"{1}\"></a>", Url.Action("Details", new { id = item.ID }), ControllersExtentstions.GetFullPath(string.IsNullOrWhiteSpace(item.XGT) ? "" : item.XGT.Split('|')[0]));
                html.AppendFormat(" <h5>{0}</h5>", item.Name);
                html.AppendFormat(" <p>面积：{0}<span>平方米</span></p>", item.ZJZMJ);
                html.AppendFormat("<p>开发商：{0}</p>  </div>", item.LYGLYYF);
                html.Append("</li>");
            }
            return Json(new { success = true, html = html.ToString(), pager = pager, totalCount = totalCount }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}