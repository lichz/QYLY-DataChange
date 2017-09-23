using System;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using Show.Models;
using manage.Models;

namespace Show.Controllers
{
    public class ArticleModelController : Controller
    {
        private Context db = new Context();

        // GET: ArticleModel
        public ActionResult Index()
        {
            ArticleIndexViewModel viewModel = new ArticleIndexViewModel();
            var articles = db.Articles.Include(a => a.DictionaryType);

            viewModel.List = articles;
            return View(viewModel);
        }

        // GET: ArticleModel/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleModel articleModel = db.Articles.Find(id);
            if (articleModel == null)
            {
                return HttpNotFound();
            }
            return View(articleModel);
        }

        // GET: ArticleModel/Create
        public ActionResult Create()
        {
            ViewBag.Type = new SelectList(db.Dictionarys, "ID", "Title");
            return View();
        }

        // POST: ArticleModel/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Type,BriefIntroduction,Content,PublishTime,CreateTime,Status")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                articleModel.Id = Guid.NewGuid();
                db.Articles.Add(articleModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Type = new SelectList(db.Dictionarys, "ID", "Title", articleModel.Type);
            return View(articleModel);
        }

        // GET: ArticleModel/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleModel articleModel = db.Articles.Find(id);
            if (articleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.Type = new SelectList(db.Dictionarys, "ID", "Title", articleModel.Type);
            return View(articleModel);
        }

        // POST: ArticleModel/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Type,BriefIntroduction,Content,PublishTime,CreateTime,Status")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(articleModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Type = new SelectList(db.Dictionarys, "ID", "Title", articleModel.Type);
            return View(articleModel);
        }

        // GET: ArticleModel/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArticleModel articleModel = db.Articles.Find(id);
            if (articleModel == null)
            {
                return HttpNotFound();
            }
            return View(articleModel);
        }

        // POST: ArticleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ArticleModel articleModel = db.Articles.Find(id);
            db.Articles.Remove(articleModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
