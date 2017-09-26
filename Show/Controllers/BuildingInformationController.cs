using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using manage.Models;
using Show.Models;
using System.Text;

namespace Show.Controllers
{
    public class BuildingInformationController : Controller
    {
        private Context db = new Context();
        private int pageSize = 8;
        // GET: 楼宇资讯
        public ActionResult Index(Guid? typeID,int? pageIndex)
        {
            pageIndex = pageIndex ?? 1;
            string typeName = string.Empty;
            BuildingInformationViewModel viewModel = new BuildingInformationViewModel();
            var parentID = db.Dictionarys.FirstOrDefault(p => p.Title.Equals("楼宇资讯")).ID;
            viewModel.ArticleType = db.Dictionarys.Where(p => p.ParentID == parentID);
            if (typeID == null)
            {
                //默认第一个类型的列表
                var firstType = db.Dictionarys.Where(p => p.ParentID == parentID).Take(1);
                typeID = new Guid();
                foreach (var item in firstType)
                {
                    typeID = item.ID;
                    typeName = item.Title;
                }
            }
            else
            {
                typeName = db.Dictionarys.FirstOrDefault(p => p.ID == typeID).Title;
            }
            viewModel.TypeName = typeName;
            IQueryable<ArticleModel> list = db.Articles.Where(p => p.Type == typeID).OrderByDescending(p => p.PublishTime);
            viewModel.Pager = RoadFlow.Utility.New.Tools.GetPagingHtml(list.Count(), pageSize, pageIndex.Value);
            viewModel.FirstArticles = db.Articles.Where(p => p.Type == typeID).OrderByDescending(p => p.PublishTime).Skip(pageSize * (pageIndex.Value - 1)).Take(pageSize);
            return View(viewModel);
        }
      
        public ActionResult Details(Guid id,Guid typeID){
            return View(DetailInit(id,typeID));
        }

        #region Init
        public BuildingInformationDetailViewModel DetailInit(Guid id, Guid typeID)
        {
            BuildingInformationDetailViewModel viewModel = new BuildingInformationDetailViewModel();
            var parentID = db.Dictionarys.FirstOrDefault(p => p.Title.Equals("楼宇资讯")).ID;
            viewModel.ArticleType = db.Dictionarys.Where(p => p.ParentID == parentID);
            viewModel.Article = db.Articles.FirstOrDefault(p => p.Id == id);
            string name = db.Dictionarys.FirstOrDefault(p => p.ID == typeID).Title;
            ViewBag.Name = name;
            return viewModel;
        }
        #endregion

        #region 异步
        /// <summary>
        /// json异步处理
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="typeID"></param>
        /// <returns></returns>
        public JsonResult GetList(int pageIndex, Guid typeID)
        {
            BuildingInformationViewModel viewModel = new BuildingInformationViewModel();
            StringBuilder html = new StringBuilder();
            IQueryable<ArticleModel> list = db.Articles.Where(p => p.Type == typeID).OrderByDescending(p => p.PublishTime);
            //分页
            viewModel.Pager = RoadFlow.Utility.New.Tools.GetPagingHtml(list.Count(), pageSize, pageIndex);
            //列表
            viewModel.Articles = db.Articles.Where(p => p.Type == typeID).OrderByDescending(p => p.PublishTime).Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            if (list.Count() == 0)
            {
                viewModel.IsNull = true;
                html.Append("<span>暂无数据</span>");
                return Json(new { success = true, isNull = viewModel.IsNull, html = html.ToString() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var item in viewModel.Articles)
                {
                    html.Append("<li>");
                    html.AppendFormat("<a class=\"clearfix\" href=\"{0}\">", Url.Action("Details", new { id = item.Id, typeID = item.Type }));
                    html.Append("<div class=\"fl date_box\">");
                    html.AppendFormat("<span class=\"date\">{0}</span>", ((DateTime)item.PublishTime).ToString("MM-dd"));
                    html.AppendFormat("<span class=\"year\">{0}</span></div>", ((DateTime)item.PublishTime).ToString("yyyy"));
                    html.Append("<div class=\"fl\">");
                    html.AppendFormat("<h3 class=\"tit\">{0}</h3>", item.Title);
                    html.AppendFormat("<p class=\"cnt\">{0}</p></div> </a></li>", item.BriefIntroduction);
                }
                return Json(new { success = true, html = html.ToString(), pager = viewModel.Pager }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}