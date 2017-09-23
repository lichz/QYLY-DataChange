using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace RoadFlow.Web.Model {
    public class HousesIndexViewModel {
        /// <summary>
        /// 组织机构名称
        /// </summary>
        public string Name { get; set; }

        //public DataTable List { get; set; }

        public List<Guid> Check { get; set; }
        #region Hidden
        public Guid OrganizeID { get; set; }
        public RoadFlow.Data.Model.ElementOrganizeType Type { get; set; }
        #endregion
        
    }
}
