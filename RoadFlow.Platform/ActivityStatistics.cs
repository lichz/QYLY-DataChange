using System.Collections.Generic;
using System.Data;
using RoadFlow.Data.Factory;
using RoadFlow.Data.Interface;
using RoadFlow.Data.Model;

namespace RoadFlow.Platform {
    /// <summary>
    /// 活跃统计
    /// </summary>
    public class ActivityStatistics {
        private IActivityStatistics iActivityStatistics;

        public ActivityStatistics() {
            iActivityStatistics = Factory.GetActivityStatistics();
        }

        public DataTable GetPagerData(out int allPages, out int count, string query, int pageIndex, int pageSize) {
            return iActivityStatistics.GetPagerData(out allPages, out count, query, pageIndex, pageSize);
        }

        //public DataTable GetExpData(List<ColItem> DisplayItem, string ssjd = "", string wher = ""){
        //    return iBuildings_Statistics.GetExpData(DisplayItem, ssjd, wher);
        //}
    }
}