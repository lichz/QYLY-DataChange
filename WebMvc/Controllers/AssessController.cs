using RoadFlow.Utility;
using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers {
    /// <summary>
    /// 报送考核
    /// </summary>
    public class AssessController : Controller {
        public RoadFlow.Platform.BuildingMonthModifyCountBLL BLL { get; private set; }
        public int PageSize { get; private set; }
        public AssessController() 
        {
            BLL = new RoadFlow.Platform.BuildingMonthModifyCountBLL();
            PageSize = 13;
        }

        public ActionResult Index(int? pageIndex) {
            AssessIndexViewModel viewModel = new AssessIndexViewModel();
            pageIndex = pageIndex ?? 1;
            string pager = string.Empty;
            
            #region where
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string,RoadFlow.Data.Model.SQLFilterType>,object>();
            if (!string.IsNullOrWhiteSpace(Request["name"])) {
                viewModel.Name = Request["name"];
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("BuildingName", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), viewModel.Name);
            }
            if(!string.IsNullOrWhiteSpace(Request["timeArea"])){
                viewModel.TimeArea = Request["timeArea"];
                string timeArea = Request["timeArea"].Replace("-","");
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.EQUAL), timeArea);
            }else
            {
                viewModel.TimeArea = DateTime.Now.ToString("yyyy-MM");
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.EQUAL), viewModel.TimeArea.Replace("-", ""));
            }
	        #endregion
            //var list = BLL.GetPagerData(out pager, PageSize, pageIndex.Value, where).ToList<RoadFlow.Data.Model.BuildingMonthModifyCountModel>();
            var list = BLL.GetPagerDataHouse(out pager, PageSize, pageIndex.Value, where);
            viewModel.List = list;
            viewModel.Pager = pager;
            return View(viewModel);
        }

        public ActionResult Chart()
        {
            return View();
        }


        #region 操作（导出，删除等没有视图的操作）
        public void Export() {
            //获取导出列表
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string,RoadFlow.Data.Model.SQLFilterType>,object>();//筛选条件
            if (!string.IsNullOrWhiteSpace(Request["name"]))
            {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("BuildingName", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), Request["Name"]);
            }
            if (!string.IsNullOrWhiteSpace(Request["timeArea"]))
            {
                string timeArea = Request["timeArea"].Replace("-", "");
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.EQUAL), timeArea);
            }

            string pager=string.Empty;
            var list = BLL.GetALLOnHouse(where);
            ExportExcel.Export(list, "报送考核");
        }
        #endregion
    }
}
