using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using RoadFlow.Platform;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 任务
    /// </summary>
    public class WorkFlowTaskBLL {
        private static string _tableName = "WorkFlowTask";
        private static string _order = "[SenderTime] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        /// <summary>
        /// 获取当前用户的报送任务列表
        /// </summary>
        public List<string> GetCurrentUsersList() {
            List<string> list = new List<string>();
            DataTable dt = baseDb.GetAllByPara(0, new KeyValuePair<string, object>("SenderID", Users.CurrentUserID), new KeyValuePair<string, object>("StepName", "物业报送"));
            foreach(DataRow dr in dt.Rows){
                if (!list.Contains((string)dr["InstanceID"])) {
                    list.Add((string)dr["InstanceID"]);
                }
            }
            return list;
        }
        #endregion


    }
}
