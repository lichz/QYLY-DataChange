using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace RoadFlow.Web.Model {
    public class SubmitIndexViewModel {

        /// <summary>
        /// 楼盘列表
        /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 所有楼盘，用户页面搜索ID对应名称
        /// </summary>
        public DataTable HouseName { get; set; }

        /// <summary>
        /// 报送流程ID
        /// </summary>
        public Guid CreateFlowID { get; set; }


        /// <summary>
        /// 关联的街道id
        /// </summary>
        public Dictionary<string, object> ToStreetID { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

    }

    public class SubmitBuildingModifyViewModel {
        /// <summary>
        /// 楼栋列表
        /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 所有楼盘，用户页面搜索ID对应名称
        /// </summary>
        public DataTable HouseName { get; set; }

        /// <summary>
        /// 更新流程ID
        /// </summary>
        public Guid EditFlowID { get; set; }

        /// <summary>
        /// 关联的街道id
        /// </summary>
        public Dictionary<string,object> ToStreetID { get; set; }

        public string Pager { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        #region where
        public string ParaName { get; set; }
        #endregion
    }
    

    public class SubmitEnterpriseViewModel {
        /// <summary>
        /// 楼栋列表
        /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 报送流程ID
        /// </summary>
        public Guid CreateFlowID { get; set; }

        /// <summary>
        /// 关联的街道id
        /// </summary>
        public Dictionary<string, object> ToStreetID { get; set; }

        public string Pager { get; set; }
     
        #region where
        public string ParaBuildingID { get; set; }
        public string ParaName { get; set; }
        #endregion
    }

    public class SubmitEnterpriseModifyViewModel {
        /// <summary>
        /// 企业列表
        /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 所有楼栋，用户页面搜索ID对应名称
        /// </summary>
        public DataTable BuildingsName { get; set; }

        /// <summary>
        /// 更新流程ID
        /// </summary>
        public Guid EditFlowID { get; set; }

        /// <summary>
        /// 搬出流程ID
        /// </summary>
        public Guid DeleteFlowID { get; set; }

        /// <summary>
        /// 关联的街道id
        /// </summary>
        public Dictionary<string, object> ToStreetID { get; set; }

        public string Pager { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }

        #region where
        public string ParaBuildingID { get; set; }
        public string ParaName { get; set; }
        #endregion
    }


    public class SubmitMonthViewModel {
        /// <summary>
        /// 楼栋列表
        /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 每月信息
        /// </summary>
        public List<RoadFlow.Data.Model.BuildingMonthInfoModel> MonthInfos { get; set; }

        /// <summary>
        /// 报送流程ID
        /// </summary>
        public Guid CreateFlowID { get; set; }

        /// <summary>
        /// 关联的街道id
        /// </summary>
        public Dictionary<string, object> ToStreetID { get; set; }

        public string Pager { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string Message { get; set; }
        #region where
        public string ParaName { get; set; }
        #endregion
    }

    public class SubmitMonthModifyViewModel {
        /// <summary>
        /// 每月更新列表
        /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 更新流程ID
        /// </summary>
        public Guid EditFlowID { get; set; }

        public string Pager { get; set; }

        /// <summary>
        /// 关联的街道id
        /// </summary>
        public Dictionary<string, object> ToStreetID { get; set; }

        #region where
        public string ParaTimeArea { get; set; }
        public string ParaName { get; set; }
        #endregion
    }
}
