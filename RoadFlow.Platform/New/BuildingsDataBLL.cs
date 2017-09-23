using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using System.Data.SqlClient;
using RoadFlow.Data.Model;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 楼宇
    /// </summary>
    public class BuildingsDataBLL {
        private static string _tableName = "BuildingsData";
        private static string _order = "[CreateTime] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        public DataTable GetPagerData(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);//正常的数据。
            return baseDb.GetPagerData(out pager, size, pageIndex, where);
        }
    
        public DataTable GetByStringID(string id) {
            return baseDb.GetAllByPara(0, new KeyValuePair<string, object>("ID", id), new KeyValuePair<string, object>("Status", RoadFlow.Data.Model.Status.Normal));//0表示取所有，大于取前几条。
        }

        public RoadFlow.Data.Model.BuildingsModel Get(Guid id) {
            return baseDb.Get<RoadFlow.Data.Model.BuildingsModel>(new KeyValuePair<string, object>("ID", id), new KeyValuePair<string, object>("Status", RoadFlow.Data.Model.Status.Normal));
        }

        /// <summary>
        /// 获取当前用户的报送楼宇
        /// </summary>
        public DataTable GetCurrentUsersOrganizeList() {
            //获取当前用户所属组织有的楼盘权限，并将ID放入List中，用作后边查询楼栋的筛选条件
            ElementOrganizeBLL elementOrganizeBLL = new ElementOrganizeBLL();
            DataTable houses = elementOrganizeBLL.GetCurrentBuildingModify();
            List<string> housesID = new List<string>();
            foreach (DataRow dr in houses.Rows) {
                if (!housesID.Contains(dr["ElementID"].ToString())) {
                    housesID.Add(dr["ElementID"].ToString());
                }
            }

            if(housesID.Count==0){
                return new DataTable();
            }

            //查询满足条件的所有楼栋
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("HouseID", RoadFlow.Data.Model.SQLFilterType.IN), housesID);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);//正常的数据。
            return baseDb.GetAll(0, where);
        }

        /// <summary>
        /// 当前用户组织所有能变更的楼栋(带翻页。)
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentUsersOrganizeList(int pageIndex, int size, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where, out string pager) {
            //获取当前用户所属组织有的楼盘权限，并将ID放入List中，用作后边查询楼栋的筛选条件
            ElementOrganizeBLL elementOrganizeBLL = new ElementOrganizeBLL();
            DataTable houses = elementOrganizeBLL.GetCurrentBuildingModify();
            List<string> housesID = new List<string>();
            foreach (DataRow dr in houses.Rows) {
                if (!housesID.Contains(dr["ElementID"].ToString())) {
                    housesID.Add(dr["ElementID"].ToString());
                }
            }

            if (housesID.Count == 0) {
                pager = string.Empty;
                return new DataTable();
            }

            //查询满足条件的所有楼栋
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("HouseID", RoadFlow.Data.Model.SQLFilterType.IN), housesID);
            return GetPagerData(out pager, size, pageIndex, where);
        }

        public DataTable GetAll(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);//正常的数据。
            return baseDb.GetAll(0,where);
        }

        /// <summary>
        /// 根据楼栋名称获取楼栋ID列表
        /// </summary>
        /// <returns></returns>
        public List<string> GetBuildingIDsByBuildingName(string buildingName) { 
            List<string> result = new List<string>();

            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string,SQLFilterType>,object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Name", RoadFlow.Data.Model.SQLFilterType.CHARINDEX), buildingName);
            DataTable buildings = GetAll(where);
            foreach(DataRow dr in buildings.Rows){
                result.Add(dr["ID"].ToString());
            }
            return result;
        }
        #endregion

        #region Modify
        public int Add(RoadFlow.Data.Model.BuildingsModel model) {
            model.CreateTime = DateTime.Now;
            model.UpdateTime = DateTime.Now;
            if(baseDb.Add<RoadFlow.Data.Model.BuildingsModel>(model)>0){
                //更新合成表
                BuildingsAndBuildingMonthInfoBLL buildingsAndBuildingMonthInfoBLL = new BuildingsAndBuildingMonthInfoBLL();
                return buildingsAndBuildingMonthInfoBLL.Add(model);
            }
            return 0;
        }

        //不能直接调用，需要从BuildingsBLL中的方法调用这个方法。因为更新的时候应该从源头BuildingsBLL开始更新。
        public int Update(RoadFlow.Data.Model.BuildingsModel model,Guid id) {
            model.CreateTime = null;
            model.UpdateTime = DateTime.Now;
            return baseDb.Update<RoadFlow.Data.Model.BuildingsModel>(model, new KeyValuePair<string, object>("ID", id));
        }

        public int ManageUpdate<T>(T model, Guid id) {
            return baseDb.Update<T>(model, new KeyValuePair<string, object>("ID", id));
        }

        public int ManageDelete(Guid id)
        {
            return baseDb.Delete(id);
        }
        #endregion
    }
}
