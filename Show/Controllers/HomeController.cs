using manage.Models;
using Show.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Show.Controllers
{
    public class HomeController : Controller
    {
        private Context db = new Context();

        #region Action
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            HomeIndexViewModel viewModel = new HomeIndexViewModel();
            //新闻资讯
            Guid type = db.Dictionarys.FirstOrDefault(p => p.Title == "新闻资讯").ID;
            ViewBag.NewTypeID = type;
            viewModel.News = db.Articles.Include(a => a.DictionaryType).Where(p => p.Type == type).Take(7).OrderByDescending(p => p.PublishTime);
            //通知公告
            Guid typeNotice = db.Dictionarys.FirstOrDefault(p => p.Title == "通知公告").ID;
            ViewBag.NoticeTypeID = typeNotice;
            viewModel.Notice = db.Articles.Include(a => a.DictionaryType).Where(p => p.Type == typeNotice).Take(7).OrderByDescending(p => p.PublishTime);
            //楼宇政策
            Guid typePolicy = db.Dictionarys.FirstOrDefault(p => p.Title == "楼宇政策").ID;
            ViewBag.PolicyTypeID = typePolicy;
            viewModel.Policy = db.Articles.Include(a => a.DictionaryType).Where(p => p.Type == typePolicy).Take(8).OrderByDescending(p => p.PublishTime);
            //楼宇展示
            var isImportant = db.Dictionarys.Where(p => p.Title == "重点楼宇").FirstOrDefault().ID;
            viewModel.BuildingShow = db.BuildingsAndBuildingMonthInfo.Where(p => p.IsImportant == isImportant && p.Status == Status.Normal).Take(8);
            //楼宇租 售
            viewModel.Buildings = db.BuildingsAndBuildingMonthInfo.Where(p=>p.IsImportant!=null&&p.Status == Status.Normal).Include(i => i.SYZJ).Include(t => t.SWZJ).Take(3).OrderByDescending(p => p.CreateTime);
            return View(viewModel);
        }
        /// <summary>
        /// 引导页
        /// </summary>
        /// <returns></returns>
        public ActionResult LeadPage()
        {
            return View();
        }
        #endregion
      
    }
}