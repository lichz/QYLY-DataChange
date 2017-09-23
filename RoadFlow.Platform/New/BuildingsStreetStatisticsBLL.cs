using RoadFlow.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RoadFlow.Platform;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 楼栋按街道统计
    /// </summary>
    public class BuildingsStreetStatisticsBLL
    {
        private static string _tableName = "V_BuildingsStreetStatistics";
        private static string _order = "[ZJZMJ] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        public DataTable GetBySSJD(string ssjd) {
            if(!string.IsNullOrWhiteSpace(ssjd)){
                return baseDb.GetAllByPara(0, new KeyValuePair<string, object>("SSJD", ssjd));
            } else {
                return baseDb.GetAllByPara(0);
            }
        }
        #endregion

    }
}
