using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace RoadFlow.Web.Model {
    public class AssessIndexViewModel {
        public List<RoadFlow.Data.Model.BuildingMonthModifyCountModel> List { get; set; }
        public string Pager { get; set; }

        #region where
        public string Name { get; set; }
        public string TimeArea { get; set; }
        #endregion
    }
}
