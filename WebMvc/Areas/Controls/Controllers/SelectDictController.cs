using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Areas.Controls.Controllers
{
    public class SelectDictController : MyController
    {
        //
        // GET: /Controls/SelectDict/

        public ActionResult Index()
        {
            RoadFlow.Platform.DictionaryBLL Dict = new RoadFlow.Platform.DictionaryBLL();

            string values = Request.QueryString["values"];
            string rootid = Request.QueryString["rootid"];


            string defaultValuesString = "";
            System.Text.StringBuilder defautlSB = new System.Text.StringBuilder();
            foreach (string value in values.Split(','))
            {
                Guid id;
                if (!value.IsGuid(out id))
                {
                    continue;
                }
                defautlSB.AppendFormat("<div onclick=\"currentDel=this;showinfo('{0}');\" class=\"selectorDiv\" ondblclick=\"currentDel=this;del();\" value=\"{0}\">", value);
                defautlSB.Append(Dict.GetTitle(id));
                defautlSB.Append("</div>");
            }
            defaultValuesString = defautlSB.ToString();
            ViewBag.defaultValuesString = defaultValuesString;
            return View();
        }

        public string GetNames()
        {
            string values = Request.QueryString["values"];
            RoadFlow.Platform.DictionaryBLL Dict = new RoadFlow.Platform.DictionaryBLL();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string value in values.Split(','))
            {
                var dict = Dict.Get(value.Convert<Guid>());
                if (dict != null)
                {
                    sb.Append(dict.Title);
                    sb.Append(',');
                }
            }
            return sb.ToString().TrimEnd(',');
        }

        public string GetNote()
        {
            string id = Request.QueryString["id"];
            Guid gid;
            string note = "";
            if (id.IsGuid(out gid))
            {
                var dict = new RoadFlow.Platform.DictionaryBLL().Get(gid);
                if (dict != null)
                {
                    note = dict.Note;
                }
            }
            return note;
        }

    }
}
