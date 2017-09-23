using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web.Mvc;

namespace RoadFlow.Web.Model
{
    public class StatisticsIndexViewModel
    {
        public DataTable List { get; set; }

        public List<DictionaryModel> Dictionary { get; set; }
        #region where
        public SelectList ParaSSJD { get; set; }
        #endregion
    }

    public class StatisticsHouseAreaViewModel
    {
        public List<Temp> List { get; set; }
        public string Pager { get; set; }
        #region where
        public string ParaName { get; set; }
        #endregion

        public class Temp
        {
            public string HouseName { get; set; }
            public decimal ZJZMJ { get; set; }
            public decimal SWZMJ { get; set; }
            public decimal SYZMJ { get; set; }
        }
    }

    public class StatisticsSimpleViewModel
    {
        public DataTable List { get; set; }

        /// <summary>
        /// 总空置率
        /// </summary>
        public decimal VacancyRate { get; set; }

        /// <summary>
        /// 总商务空置率
        /// </summary>
        public decimal CommerceVacancyRate { get; set; }

        /// <summary>
        /// 总商业空置率
        /// </summary>
        public decimal BusinessVacancyRate { get; set; }

        /// <summary>
        /// 总落地率
        /// </summary>
        public decimal FloorRate { get; set; }
    }
}
