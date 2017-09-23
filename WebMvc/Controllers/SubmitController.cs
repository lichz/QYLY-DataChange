using RoadFlow.Data.Model;
using RoadFlow.Platform;
using RoadFlow.Utility;
using RoadFlow.Web.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace WebMvc.Controllers {
    /// <summary>
    /// 信息填报更新（楼栋信息、税收信息等）
    /// </summary>
    public class SubmitController : Controller {
        public BuildingsDataBLL BuildingsDataBLL { get; private set; }
        public ElementOrganizeBLL ElementOrganizeBLL { get; private set; }

        public SubmitController() { 
            BuildingsDataBLL = new BuildingsDataBLL();
            ElementOrganizeBLL = new ElementOrganizeBLL();
        }

        #region action

        //楼栋报送
        public ActionResult Index() {
            return View(IndexInit());
        }

        //楼栋更新
        public ActionResult BuildingModify(int? pageIndex) {
            return View(BuildingModifyInit(pageIndex));
        }

        //企业报送
        public ActionResult Enterprise(int? pageIndex) {
            return View(EnterpriseInit(pageIndex));
        }

        //企业变更
        public ActionResult EnterpriseModify(int? pageIndex) {
            return View(EnterpriseModifyInit(pageIndex));
        }

        //每月报送
        public ActionResult MonthInfo(int? pageIndex) {
            return View(MonthInfoInit(pageIndex));
        }

        //每月更新
        public ActionResult MonthInfoModify(int? pageIndex) {
            return View(MonthInfoModifyInit(pageIndex));
        }
       
        #endregion
        
        #region action初始化方法
        private SubmitIndexViewModel IndexInit() {
            SubmitIndexViewModel viewModel = new SubmitIndexViewModel();
            //获取当前用户所属组织有的楼盘权限
            DataTable dt = ElementOrganizeBLL.GetCurrentBuildingModify();
            viewModel.List = dt;
            DictionaryBLL dictionaryBLL = new DictionaryBLL();
            viewModel.HouseName = dictionaryBLL.GetListByCode("LPMC");
            viewModel.HouseName.PrimaryKey = new DataColumn[] { viewModel.HouseName.Columns["ID"] };
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            viewModel.CreateFlowID = workFlowBLL.GetByName("楼栋报送").ID;

            viewModel.ToStreetID = GetToStreetID(dt);
            return viewModel;
        }

        private SubmitBuildingModifyViewModel BuildingModifyInit(int? pageIndex) {
            pageIndex = pageIndex ?? 1;
            string pager = string.Empty;
            int pageSize = 14;
            SubmitBuildingModifyViewModel viewModel = new SubmitBuildingModifyViewModel();
            DataTable houses = ElementOrganizeBLL.GetCurrentBuildingModify();
            if (houses.Rows.Count == 0) {
                viewModel.Message = "该用户还没有分配权限，可以联系商务局相关人员询问具体情况。";
                return viewModel;
            }
            #region where
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, SQLFilterType>, object>();
            if (!string.IsNullOrWhiteSpace(Request["name"])) {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), Request["name"]);
                viewModel.ParaName = Request["name"];
            }
            #endregion
            DataTable dt = BuildingsDataBLL.GetCurrentUsersOrganizeList(pageIndex.Value, pageSize, where, out pager);
            viewModel.List = dt;
            viewModel.Pager = pager;
            DictionaryBLL dictionaryBLL = new DictionaryBLL();
            viewModel.HouseName = dictionaryBLL.GetListByCode("LPMC");
            viewModel.HouseName.PrimaryKey = new DataColumn[] { viewModel.HouseName.Columns["ID"] };
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            viewModel.EditFlowID = workFlowBLL.GetByName("楼栋更新").ID;
            
            viewModel.ToStreetID = GetToStreetID(houses);

            return viewModel;
        }

        private SubmitEnterpriseViewModel EnterpriseInit(int? pageIndex) {
            pageIndex = pageIndex ?? 1;
            string pager = string.Empty;
            int pageSize = 14;
            SubmitEnterpriseViewModel viewModel = new SubmitEnterpriseViewModel();
            #region where
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, SQLFilterType>, object>();
            if (!string.IsNullOrWhiteSpace(Request["name"])) {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.IN), Request["name"]);
                viewModel.ParaName = Request["name"];
            }
            #endregion
            DataTable buildings = BuildingsDataBLL.GetCurrentUsersOrganizeList(pageIndex.Value, pageSize, where, out pager);
            viewModel.List = buildings;
            viewModel.Pager = pager;
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            viewModel.CreateFlowID = workFlowBLL.GetByName("企业入驻").ID;

            DataTable houses = ElementOrganizeBLL.GetCurrentBuildingModify();
            viewModel.ToStreetID = GetToStreetID(houses);
            return viewModel;
        }

        private SubmitEnterpriseModifyViewModel EnterpriseModifyInit(int? pageIndex) {
            pageIndex = pageIndex ?? 1;
            string pager = string.Empty;
            int pageSize = 14;
            SubmitEnterpriseModifyViewModel viewModel = new SubmitEnterpriseModifyViewModel();
            DataTable houses = ElementOrganizeBLL.GetCurrentBuildingModify();
            if (houses.Rows.Count == 0) {
                viewModel.Message = "该用户还没有分配权限，可以联系商务局相关人员询问具体情况。";
                return viewModel;
            }
            DataTable buildings = null;//用于根据楼栋列表查询对应的企业列表
            #region where
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, SQLFilterType>, object>();
            if (!string.IsNullOrWhiteSpace(Request["buildingID"])) {
                viewModel.ParaBuildingID = Request["buildingID"];
                buildings = BuildingsDataBLL.GetByStringID(Request["buildingID"]);
            } else {
                buildings = BuildingsDataBLL.GetCurrentUsersOrganizeList();
            }
            if (!string.IsNullOrWhiteSpace(Request["name"])) {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), Request["name"]);
                viewModel.ParaName = Request["name"];
            }
            #endregion
            #region 用于view中的显示楼栋名称和所属楼栋列表
            viewModel.BuildingsName = BuildingsDataBLL.GetCurrentUsersOrganizeList();
            viewModel.BuildingsName.PrimaryKey = new DataColumn[] { viewModel.BuildingsName.Columns["ID"] };
            #endregion
            //获取企业列表（带翻页）
            if (buildings == null || buildings.Rows.Count ==0)
            {
                viewModel.Message = "需要先新增楼栋信息。";
                return viewModel;
            }
            DataTable list = new EnterpriseAndEnterpriseTaxBLL().GetAllByBuildings(buildings, pageIndex.Value, pageSize, where, out pager);
            viewModel.List = list;
            viewModel.Pager = pager;

            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            viewModel.EditFlowID = workFlowBLL.GetByName("企业更新").ID;
            viewModel.DeleteFlowID = workFlowBLL.GetByName("企业搬出").ID;

            viewModel.ToStreetID = GetToStreetID(houses);
            return viewModel;
        }
        private SubmitMonthViewModel MonthInfoInit(int? pageIndex) {
            pageIndex = pageIndex ?? 1;
            string pager = string.Empty;
            int pageSize = 14;
            SubmitMonthViewModel viewModel = new SubmitMonthViewModel();
            DataTable houses = ElementOrganizeBLL.GetCurrentBuildingModify();
            if (houses.Rows.Count == 0) {
                viewModel.Message = "该用户还没有分配权限，可以联系商务局相关人员询问具体情况。";
                return viewModel;
            }
            #region where
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, SQLFilterType>, object>();
            if (!string.IsNullOrWhiteSpace(Request["name"])) {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), Request["name"]);
                viewModel.ParaName = Request["name"];
            }
            #endregion
            DataTable dt = BuildingsDataBLL.GetCurrentUsersOrganizeList(pageIndex.Value, pageSize, where, out pager);
            viewModel.List = dt;
            viewModel.Pager = pager;
            viewModel.MonthInfos = new BuildingMonthInfoDataBLL().GetListByBuildings(dt);
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            viewModel.CreateFlowID = workFlowBLL.GetByName("每月报送").ID;

            viewModel.ToStreetID = GetToStreetID(houses);
            return viewModel;
        }

        private SubmitMonthModifyViewModel MonthInfoModifyInit(int? pageIndex) {
            pageIndex = pageIndex ?? 1;
            string pager = string.Empty; 
            int pageSize = 14;
            SubmitMonthModifyViewModel viewModel = new SubmitMonthModifyViewModel();
            #region where
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, SQLFilterType>, object>();
            if (!string.IsNullOrWhiteSpace(Request["name"])) {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), Request["name"]);
                viewModel.ParaName = Request["name"];
            }
            if (!string.IsNullOrWhiteSpace(Request["timeArea"])) {
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.EQUAL), Request["timeArea"].Replace("-",""));
                viewModel.ParaTimeArea = Request["timeArea"];
            }
            #endregion
            BuildingMonthInfoDataBLL buildingMonthInfoDataBLL = new BuildingMonthInfoDataBLL();
            DataTable dt = buildingMonthInfoDataBLL.GetPagerData(out pager, pageSize, pageIndex.Value, where);
            viewModel.List = dt;
            viewModel.Pager = pager;
            WorkFlowBLL workFlowBLL = new WorkFlowBLL();
            viewModel.EditFlowID = workFlowBLL.GetByName("每月更新").ID;

            //通过BuildingID获取关联的街道ID
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DataRow dr in dt.Rows)
            {
                DataTable streets = ElementOrganizeBLL.GetToStreetByBuildingID(dr["BuildingID"]);
                if (streets.Rows.Count != 0)
                {
                    dictionary.Add(dr["BuildingID"].ToString(), streets.Rows[0]["OrganizeID"]);
                }
            }
            viewModel.ToStreetID = dictionary;
            return viewModel;
        }
        #endregion

        /// <summary>
        ///  获取关联的街道ID
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetToStreetID(DataTable dt)
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DataRow dr in dt.Rows)
            {
                DataTable streets = ElementOrganizeBLL.GetToStreetByHouseID(dr["ElementID"]);
                if (streets.Rows.Count != 0)
                {
                    dictionary.Add(dr["ElementID"].ToString(), streets.Rows[0]["OrganizeID"]);
                }
            }
            return dictionary;
        }
        
    }
}