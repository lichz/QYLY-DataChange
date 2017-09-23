using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using RoadFlow.Platform;
using RoadFlow.Data.Model;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 字典
    /// </summary>
    public class DictionaryBLL {
        private static string _tableName = "Dictionary";
        private static string _order = "[Sort]";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        /// <summary>
        /// 获取当前用户对应组织名称，然后用组织名称查询字典中对应的ssjd的ID.（仅用于街道）
        /// </summary>
        /// <returns></returns>
        public Guid? GetCurrentSSJD() {
            var relation = new RoadFlow.Platform.OrganizeBLL().GetByID(RoadFlow.Platform.Users.CurrentFirstRelationID);
            var dictionary = new DictionaryBLL().GetByName(relation.Name.Replace("街道", ""));
            return dictionary == null ? Guid.NewGuid() : dictionary.ID;
        }

        public DictionaryModel GetByID(Guid id) {
            return baseDb.Get<RoadFlow.Data.Model.DictionaryModel>(new KeyValuePair<string, object>("ID", id));
        }

        public DictionaryModel GetByName(string name) {
            return baseDb.Get<RoadFlow.Data.Model.DictionaryModel>( new KeyValuePair<string, object>("Title", name));
        }

        /// <summary>
        /// 按父节点code获取子节点列表
        /// </summary>
        /// <param name="code">父节点code</param>
        /// <returns></returns>
        public DataTable GetListByCode(string code) {
            DataTable parent = baseDb.GetAllByPara(0, new KeyValuePair<string, object>("Code", code));
            if(parent.Rows.Count>0){
                return baseDb.GetAllByPara(0,new KeyValuePair<string,object>("ParentID",parent.Rows[0]["ID"]));
            } else {
                return new DataTable();
            }
        }

        public List<RoadFlow.Data.Model.DictionaryModel> GetListAll() {
            return baseDb.GetAll(0, new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>()).ToList<RoadFlow.Data.Model.DictionaryModel>();
        }
        #endregion


    }
}
