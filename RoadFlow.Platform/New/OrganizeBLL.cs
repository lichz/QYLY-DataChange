using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using System.Data.SqlClient;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public class OrganizeBLL {
        private static string _tableName = "Organize";
        private static string _order = "[Sort]";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        /// <summary>
        /// 获取组织机构
        /// </summary>
        /// <returns></returns>
        public RoadFlow.Data.Model.OrganizeModel GetByID(Guid id) {
            return baseDb.Get<RoadFlow.Data.Model.OrganizeModel>(new KeyValuePair<string, object>("ID", id));
        }


        /// <summary>
        /// 获取所有街道
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllStreet() {
            RoadFlow.Data.Model.OrganizeModel parent = baseDb.Get<RoadFlow.Data.Model.OrganizeModel>(new KeyValuePair<string,object>("Name","街道办"));
            return baseDb.GetAllByPara(0, new KeyValuePair<string, object>("ParentID", parent.ID));//0表示取所有，大于取前几条。
        }

        /// <summary>
        /// 获取所有报送单位
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllSubmit() {
            RoadFlow.Data.Model.OrganizeModel parent = baseDb.Get<RoadFlow.Data.Model.OrganizeModel>(new KeyValuePair<string, object>("Name", "报送单位"));
            return baseDb.GetAllByPara(0, new KeyValuePair<string, object>("ParentID", parent.ID));//0表示取所有，大于取前几条。
        }
        #endregion

    }
}
