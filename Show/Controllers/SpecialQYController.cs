using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Show.Models;
using manage.Models;
using System.Text;

namespace Show.Controllers
{
    public class SpecialQYController : Controller
    {
        private Context db = new Context();

        // GET: 魅力青羊
        public ActionResult Index()
        {
            SpecialQYIndexViewModel viewModel = new SpecialQYIndexViewModel();
            var parentID = db.Dictionarys.FirstOrDefault(p => p.Title.Equals("魅力青羊")).ID;
            var articleType = db.Dictionarys.Where(p => p.ParentID == parentID);
            //默认第一个类型的文章
            var firstType = db.Dictionarys.Where(p => p.ParentID == parentID).Take(1);
            Guid typeID = new Guid();
            string typeName = string.Empty;
            foreach (var item in firstType)
            {
                typeID = item.ID;
                typeName = item.Title;
            }
            viewModel.TypeName = typeName;
            ArticleModel model = db.Articles.FirstOrDefault(p => p.Type == typeID);
            if (model==null)
            {
                viewModel.Content = "暂无数据";
            }
            else
            {
                viewModel.Content = model.Content;
            }
            viewModel.ArticleType = articleType;
            return View(viewModel);
          
        }

        #region Init
        public SpecialQYIndexViewModel IndexInit()
        {
            SpecialQYIndexViewModel viewModel = new SpecialQYIndexViewModel();

            return viewModel;
        }
        #endregion

        #region 异步
        public JsonResult getArticle(Guid typeID)
        {
            SpecialQYIndexViewModel viewModel = new SpecialQYIndexViewModel();
            ArticleModel model = db.Articles.FirstOrDefault(p => p.Type == typeID);
            if (model == null)
            {
                viewModel.Content = "暂无数据";
            }
            else
            {
                viewModel.Content = model.Content;
            }
            return Json(new { success = true, content = viewModel.Content }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}