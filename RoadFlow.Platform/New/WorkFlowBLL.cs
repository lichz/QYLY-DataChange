using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 流程
    /// </summary>
    public class WorkFlowBLL {
        private static string _tableName = "WorkFlow";
        private static string _order = "[InstallDate] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        /// <summary>
        /// 按名称获取流程
        /// </summary>
        /// <param name="name">流程名称</param>
        public RoadFlow.Data.Model.WorkFlow GetByName(string name){
            //报送流程ID
            return baseDb.Get<RoadFlow.Data.Model.WorkFlow>(new KeyValuePair<string,object>("Name",name));
        }
        #endregion


    }
}
