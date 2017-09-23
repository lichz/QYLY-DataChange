using System.Collections.Generic;
using System.Data;

namespace RoadFlow.Web.Model
{

    /// <summary>
    /// 企业列表页面视图
    /// </summary>
    public class BuildingEnterpriseIndexViewModel {
        public DataTable List { get; set; }

        public List<RoadFlow.Data.Model.DictionaryModel> Dictionary { get; set; }

        public List<RoadFlow.Data.Model.ColItem> Display { get; set; }

        public string Pager { get; set; }
        #region where
        public string ParaName { get; set; }
        #endregion
        
    }

    /// <summary>
    /// 企业税收导入列表页面视图
    /// </summary>
    public class BuildingEnterpriseTaxImportViewModel
    {
        public DataTable List { get; set; }

        public List<RoadFlow.Data.Model.DictionaryModel> Dictionary { get; set; }

        public List<RoadFlow.Data.Model.ColItem> Display { get; set; }

        public string Pager { get; set; }
        #region where
        public string ParaName { get; set; }
        #endregion

    }

    /// <summary>
    /// 企业税收管理页面视图
    /// </summary>
    public class BuildingEnterpriseTaxManageViewModel
    {
        /// <summary>
        /// 企业
        /// </summary>
        public RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel Enterprise;

        /// <summary>
        /// 企业税收记录
        /// </summary>
        public DataTable Taxs;
    }

    /// <summary>
    /// 企业税收编辑页面视图
    /// </summary>
    public class BuildingEnterpriseTaxEditViewModel
    {
        public RoadFlow.Data.Model.EnterpriseTaxModel Mdel { get; set; }
   

    }
}
