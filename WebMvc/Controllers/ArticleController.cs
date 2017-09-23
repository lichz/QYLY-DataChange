using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace WebMvc.Controllers
{
    public class ArticleController : Controller
    {
        private RoadFlow.Platform.Dictionary dicDB = new RoadFlow.Platform.Dictionary();
        private RoadFlow.Platform.ArticleBLL articleDB = new RoadFlow.Platform.ArticleBLL();
        //
        // GET: /Article/

        public ActionResult Index()
        {
            //文章类型
            DataTable articleType = dicDB.GetArticleType();
            ViewBag.Type = articleType;
            return View();
        }
        [HttpPost]
        public ActionResult Index(RoadFlow.Data.Model.ArticleModel model)
        {
            //文章类型
            DataTable articleType = dicDB.GetArticleType();
            ViewBag.Type = articleType;
            if (model.Title.IsNullOrEmpty() || model.BriefIntroduction.IsNullOrEmpty() || model.PublishTime==null || model.Content.IsNullOrEmpty())
            {
                ViewBag.Script = "alert('不能为空！')";
                return View();
            }
            else
            {
                articleDB.Add(model);
                ViewBag.Script = "alert('发布成功！')";
            }
            return View(model);
        }
    }
}
