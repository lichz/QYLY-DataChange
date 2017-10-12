using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Mvc;

namespace RoadFlow.Web.Model {
    //视图模型类名称拆分（Controller+Action+ViewModel）

    public class BuildingIndexViewModel {
        public List<ColItem> Display { get; set; }

        public DataTable List { get; set; }

        public string Pager { get; set; }

        public List<DictionaryModel> Dictionarys { get; set; }

        #region where
        public string Name { get; set; }
        public string SSJD { get; set; }
        #endregion
        public SelectList SSJDList { get; set; }
    }

    public class BuildingEditViewModel {
        public BuildingsAndBuildingMonthInfoModel EditModel { get; set; }

        public SelectList IsImportantList { get; set; }

        public SelectList LYJBList { get; set; }
        public SelectList SSJDList { get; set; }
        public SelectList JSJDList { get; set; }
        public SelectList LYLXList { get; set; }
        public SelectList LYCQQKList { get; set; }
        public SelectList ZYKTList { get; set; }
        public SelectList TCZSList { get; set; }

        public SelectList SY_ZJList { get; set; }
        public SelectList SW_ZJList { get; set; }
    }

    public class BuildingManageEnterpriseViewModel
    {
        public BuildingsAndBuildingMonthInfoModel Building { get; set; }

        public DataTable Enterprises { get; set; }

        public List<DictionaryModel> Dictionarys { get; set; }

    }

    public class BuildingEditEnterpriseViewModel
    {
        public EnterpriseAndEnterpriseTaxModel Enterprise { get; set; }

        /// <summary>
        /// 企业类型
        /// </summary>
        public List<DictionaryModel> Dictionarys { get; set; }
    }


    public class BuildingDetailViewModel {
        public BuildingsAndBuildingMonthInfoModel BuildingsAndBuildingMonthInfo { get; set; }

        public DataTable Enterprises { get; set; }


        public List<DictionaryModel> Dictionarys { get; set; }
    }

    public class BuildingBindHouseIDViewModel {
        public BuildingsModel Building { get; set; }

        public List<DictionaryModel> Dictionary { get; set; }

    }
}
