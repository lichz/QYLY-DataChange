using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using RoadFlow.Web.Model;

namespace WebMvc.Controllers
{
    public class ArticleController : Controller
    {
        private RoadFlow.Platform.DictionaryBLL dicDB = new RoadFlow.Platform.DictionaryBLL();
        private RoadFlow.Platform.ArticleBLL articleDB = new RoadFlow.Platform.ArticleBLL();
        //
        // GET: /Article/

        public ActionResult Index()
        {
            return View(new ArticleIndexViewModel()
            {
                ArticleTypes = dicDB.GetArticleType()
            });
        }
        [HttpPost]
        public ActionResult Index(RoadFlow.Data.Model.ArticleModel model)
        {
            //文章类型
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
            return View(new ArticleIndexViewModel() { Model=model,ArticleTypes= dicDB.GetArticleType() });
        }
    }
}
