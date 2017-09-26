using System;
using System.Data;
using System.Web.Mvc;
using RoadFlow.Platform;
using RoadFlow.Data.Model;

namespace WebMvc.Controllers
{
    public class PostController : Controller
    {
        private readonly Post post = new Post();

        public ActionResult Index()
        {
            return Index(null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection collection)
        {
            string name = "";

            string wher = "";
            if (collection != null)
            {
                name = Request.Form["Name"];
            }
            string query = string.Format("&appid={0}&tabid={1}&Name={2}",
                Request.QueryString["appid"], Request.QueryString["tabid"], name);
            string pager;
            var dt = post.GetPostDataPage(out pager, query, 30, RoadFlow.Utility.Tools.GetPageNumber(), name, wher);
            ViewBag.Pager = pager;
            ViewBag.Query = query;
            ViewBag.Name = name;
            return View(dt);
        }


        public ActionResult Edit()
        {
                string id = Request.QueryString["id"];
                PostModel postmodel = new PostModel();
                if(id != "0")
                {
                    postmodel = new RoadFlow.Platform.Post().GetPostModel(id.Convert<int>());
                }
                return View(postmodel);
        }

        [HttpPost]
        public ActionResult Edit(FormCollection collection)
        {
            PostModel model = new PostModel();
            string id = Request.QueryString["id"];
            if (id == "0")
            {
                if (collection != null)
                {
                    string title = collection["Title"];
                    string type = collection["Type"];
                    string adresse = collection["Adresse"];
                    string acreage = collection["Acreage"];
                    //decimal price = collection["Price"].Convert<decimal>(0);
                    string mobile = collection["Mobile"];
                    string valid = collection["IsValid"];
                    string content = collection["Content"];

                    model.Type = type;
                    model.Acreage = acreage;
                    //model.Price = price;
                    model.Adresse = adresse;
                    model.Mobile = mobile;
                    model.Title = title;
                    model.Contents = content;
                    model.AddTime = DateTime.Now;
                    model.AddUserId = RoadFlow.Platform.Users.CurrentUserID;
                    //model.AddUserName = RoadFlow.Platform.Users.CurrentUserName;

                    post.AddPost(model);
                    ViewBag.Script = "new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
                }
            }
            else
            {
                if (collection != null)
                {
                    string title = collection["Title"];
                    string type = collection["Type"];
                    string adresse = collection["Adresse"];
                    string acreage = collection["Acreage"];
                    //decimal price = collection["Price"].Convert<decimal>(0);
                    string mobile = collection["Mobile"];
                    string valid = collection["IsValid"];
                    string content = collection["Content"];
                    model = post.GetPostModel(id.Convert<int>());
                    model.Type = type;
                    model.Acreage = acreage;
                    //model.Price = price;
                    model.Adresse = adresse;
                    model.Mobile = mobile;
                    model.Title = title;
                    model.Contents = content;
                    model.IsValid = valid.Convert<int>();

                    Guid userId = RoadFlow.Platform.Users.CurrentUserID;
                    post.UpdatePost(model);
                    ViewBag.Script = "new RoadUI.Window().reloadOpener();new RoadUI.Window().close();";
                }
            }
            return View(model);
        }

        public ActionResult Details()
        {
            int id = int.Parse(Request.QueryString["id"]);
            string query = string.Format("&id={0}&tabid={1}&Name={2}",
                Request.QueryString["id"], Request.QueryString["tabid"], id);
            ViewBag.Query = query;
            return View();
        }

        public int Del(int id)
        {
           return  post.DelPost(id);
        }
    }
}