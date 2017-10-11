using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers
{
    //文章管理
    public class ArticleManageController : Controller
    {
        private RoadFlow.Platform.DictionaryBLL dicDB = new RoadFlow.Platform.DictionaryBLL();
        private RoadFlow.Platform.ArticleBLL articleDB = new RoadFlow.Platform.ArticleBLL();

        private int pageSize = 13;
        //
        // GET: /ArticleManage/

        public ActionResult Index()
        {
            //文章类型
            //DataTable articleType = ;
            //ViewBag.Type = articleType;
            string query = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>();
            string pager = string.Empty;
            string title = Request["Name"];
            string typeID = Request["typeID"];
            if(!string.IsNullOrWhiteSpace(title)){
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Title", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), title);
            }
            if (!string.IsNullOrWhiteSpace(typeID))
            {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Type", RoadFlow.Data.Model.SQLFilterType.EQUAL), typeID);
            }
            //DataTable dt = 
           
            //ViewBag.Pager = pager;
            //ViewBag.Name = title;
            return View(new ArticleManageIndexViewModel {
                ArticleTypes = dicDB.GetArticleType(),
                 Articles = articleDB.GetPagerData(out pager, pageSize, RoadFlow.Utility.Tools.GetPageNumber(), where),
                  Pager = pager,
                   Title = title
        });
        }
        public ActionResult Edit(string id)
        {
            //文章类型
            //DataTable articleType = dicDB.GetArticleType();
            //ViewBag.Type = articleType;

            //RoadFlow.Data.Model.ArticleModel model = articleDB.Get(id);
            return View(new ArticleManageEditViewModel
            {
                 ArticleTypes = dicDB.GetArticleType(),
                 ArticleModel = articleDB.Get(id)
            });
        }

        [HttpPost]
        public ActionResult Edit([Bind(Exclude = "Id")]RoadFlow.Data.Model.ArticleModel model, string id)
        {
            //文章类型
            //DataTable articleType = dicDB.GetArticleType();
            //ViewBag.Type = articleType;

            string query = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
            int rows = articleDB.Update(model,id.Convert<Guid>());
            if (rows>0)
            {
                return RedirectToAction("Index", new { appid = Request["appid"] });
            }
            //ViewBag.Script = "alert('修改成功！;new RoadUI.Window().reloadOpener();)";
            return View(new ArticleManageEditViewModel
            {
                ArticleTypes = dicDB.GetArticleType(),
                ArticleModel = model,
                Script = "alert(\"修改失败！\");"
            });
        }
       public ActionResult Delete(string id)
       {
           string query = string.Format("&appid={0}&tabid={1}", Request.QueryString["appid"], Request.QueryString["tabid"]);
           RoadFlow.Data.Model.ArticleModel model =new RoadFlow.Data.Model.ArticleModel();
           model.Status = 255;  //状态为无效
           articleDB.Update(model,id.Convert<Guid>());
           //ViewBag.Script = "alert('删除成功！;new RoadUI.Window().reloadOpener();)";
           return RedirectToAction("Index", new { appid = Request["appid"] });
       }
    }
}
