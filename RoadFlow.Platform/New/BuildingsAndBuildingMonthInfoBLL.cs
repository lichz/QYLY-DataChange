using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using RoadFlow.Platform;
using System.Data.SqlClient;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 楼栋信息+最新楼栋每月信息
    /// </summary>
    public class BuildingsAndBuildingMonthInfoBLL
    {
        private static string _tableName = "V_BuildingsAndBuildingMonthInfo_Dictionary";
        private static string _modifyTableName = "BuildingsAndBuildingMonthInfo";
        private static string _order = "[HouseID],[UpdateTime] desc";
        private IBase BaseDb;
        public BuildingsAndBuildingMonthInfoBLL()
        {
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _modifyTableName, _order);
        }

        #region get
        public DataTable GetPagerData(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetPagerData(out pager, size, pageIndex, where);
        }

        public RoadFlow.Data.Model.BuildingsAndBuildingMonthInfoModel Get(Guid id)
        {
            return BaseDb.Get<RoadFlow.Data.Model.BuildingsAndBuildingMonthInfoModel>(new KeyValuePair<string, object>("ID", id));
        }

        public DataTable GetAll(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetAll(0, where);
        }

        /// <summary>
        /// 当前用户组织所有能变更的楼栋(带翻页。)
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentUsersOrganizeList(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            //获取当前用户所属组织有的楼盘权限，并将ID放入List中，用作后边查询楼栋的筛选条件
            ElementOrganizeBLL elementOrganizeBLL = new ElementOrganizeBLL();
            DataTable houses = elementOrganizeBLL.GetCurrentBuildingModify();
            List<string> housesID = new List<string>();
            foreach (DataRow dr in houses.Rows)
            {
                if (!housesID.Contains(dr["ElementID"].ToString()))
                {
                    housesID.Add(dr["ElementID"].ToString());
                }
            }

            if (housesID.Count == 0)
            {
                pager = string.Empty;
                return new DataTable();
            }

            //查询满足条件的所有楼栋
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("HouseID", RoadFlow.Data.Model.SQLFilterType.IN), housesID);
            return GetPagerData(out pager, size, pageIndex, where);
        }

        /// <summary>
        /// 当前用户组织所有能变更的楼栋
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentUsersOrganizeList(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            //获取当前用户所属组织有的楼盘权限，并将ID放入List中，用作后边查询楼栋的筛选条件
            ElementOrganizeBLL elementOrganizeBLL = new ElementOrganizeBLL();
            DataTable houses = elementOrganizeBLL.GetCurrentBuildingModify();
            List<string> housesID = new List<string>();
            foreach (DataRow dr in houses.Rows)
            {
                if (!housesID.Contains(dr["ElementID"].ToString()))
                {
                    housesID.Add(dr["ElementID"].ToString());
                }
            }

            //查询满足条件的所有楼栋
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("HouseID", RoadFlow.Data.Model.SQLFilterType.IN), housesID);
            return GetAll(where);
        }


        #region 老调用方式
        /// <summary>
        /// 旧方法调用。
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public DataTable OldGetAll(string query)
        {
            List<SqlParameter> parList = new List<SqlParameter>();
            RoadFlow.Data.Model.Buildings temp = RoadFlow.Utility.ObjectExpand.JsonConvertModel<RoadFlow.Data.Model.Buildings>(query);
            StringBuilder where = new StringBuilder(" 1=1 and Status=" + (int)RoadFlow.Data.Model.Status.Normal);
            SingleLikeBuild("Name", temp.Name, parList, where);
            SingleBuild("SSJD", temp.SSJD, parList, where);//所属街道
            SingleBuild("JSJD", temp.JSJD, parList, where);//建设阶段
            SingleBuild("LYJB", temp.LYJB, parList, where);//楼宇级别
            SingleBuild("LYLX", temp.LYLX, parList, where);//楼宇类型
            //SingleBuild("LYDS", temp.LYDS, parList, where);//报送楼栋
            SingleBuild("TCZS", temp.TCZS, parList, where);//统筹招商
            RangeBuild("SY_ZMJ", temp.SY_ZMJ, parList, where);//商业总面积
            RangeBuild("SY_KZ_ZMJ", temp.SY_KZ_ZMJ, parList, where);//商业空置总面积
            RangeBuild("SY_KZ_KXSMJ", temp.SY_KZ_KXSMJ, parList, where);//商业空置可销售面积
            RangeBuild("SY_KZ_KZLMJ", temp.SY_KZ_KZLMJ, parList, where);//商业空置可租赁面积
            RangeBuild("SW_ZMJ", temp.SW_ZMJ, parList, where);//商务总面积
            RangeBuild("SW_KZ_ZMJ", temp.SW_KZ_ZMJ, parList, where);//商务空置总面积
            RangeBuild("SW_KZ_KXSMJ", temp.SW_KZ_KXSMJ, parList, where);//商务空置可销售面积
            RangeBuild("SW_KZ_KZLMJ", temp.SW_KZ_KZLMJ, parList, where);//商务空置可租赁面积
            RangeBuild("JGSJ", temp.JGSJ, parList, where);//竣工时间

            string sql = "select * from [V_BuildingsAndBuildingMonthInfo_Dictionary] where" + where;//RoadFlow.Utility.L.Tools.GetPaging("[Buildings]", where.ToString(), pageSize, pageIndex);
            //count = Int32.Parse(dbHelper.GetFieldValue("select Count(1) Count from [Buildings] where" + where, parList.ToArray()));
            ExecuteBLL executeBLL = new ExecuteBLL();
            return executeBLL.GetDataTable(sql, parList.ToArray());
        }
        /// <summary>
        /// 简单=组合
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">字段对应值</param>
        private void SingleBuild(string name, string value, List<SqlParameter> parList, StringBuilder where)
        {
            if (!value.IsNullOrEmpty() && value != "-1")
            {//大厦名
                parList.Add(new SqlParameter("@" + name, SqlDbType.NVarChar, 50) { Value = value });
                where.AppendFormat(" and [{0}]=@{0}", name);
            }
        }
        /// <summary>
        /// 简单like组合
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">字段对应值</param>
        private void SingleLikeBuild(string name, string value, List<SqlParameter> parList, StringBuilder where)
        {
            if (!value.IsNullOrEmpty() && value != "-1")
            {//大厦名
                parList.Add(new SqlParameter("@" + name, SqlDbType.NVarChar, 50) { Value = "%" + value + "%" });
                where.AppendFormat(" and [{0}] like @{0}", name);
            }
        }
        /// <summary>
        /// 范围组合
        /// </summary>
        /// <param name="name">字段名</param>
        /// <param name="value">字段对应值</param>
        private void RangeBuild(string name, string value, List<SqlParameter> parList, StringBuilder where)
        {
            //商务总面积
            if (!value.IsNullOrEmpty() && value != "-1")
            {
                var zmj = value.Split(',');
                var begin = zmj[0];
                var end = zmj[1];
                if (begin.IsNullOrEmpty() || end.IsNullOrEmpty())
                {
                    if (begin.IsNullOrEmpty())
                    {
                        parList.Add(new SqlParameter("@" + name, SqlDbType.NVarChar, 50) { Value = end });
                        where.AppendFormat(" and [{0}]<@{0}", name);
                    }
                    if (end.IsNullOrEmpty())
                    {
                        parList.Add(new SqlParameter("@" + name, SqlDbType.NVarChar, 50) { Value = begin });
                        where.AppendFormat(" and [{0}]>@{0}", name);
                    }
                }
                else
                {
                    parList.Add(new SqlParameter("@" + name + "_B", SqlDbType.NVarChar, 50) { Value = begin });
                    parList.Add(new SqlParameter("@" + name + "_E", SqlDbType.NVarChar, 50) { Value = end });
                    where.AppendFormat(" and [{0}]>=@{0}_B and [{0}]<=@{0}_E", name);
                }
            }
        }
        #endregion

        /// <summary>
        /// 按楼盘统计
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataTable GetStatisticsByHouseName(string houseName)
        {
            ExecuteBLL executeBLL = new ExecuteBLL();
            string sql = "select [HouseName],sum(ZJZMJ) ZJZMJ from [dbo].[V_BuildingsAndBuildingMonthInfo_Dictionary] {0} group by [HouseName]";
            if (!string.IsNullOrWhiteSpace(houseName))
            {
                sql = string.Format(sql, "where CHARINDEX('" + houseName + "',[HouseName])>0");
            }
            else
            {
                sql = string.Format(sql, "");
            }
            return executeBLL.GetDataTable(sql);
        }
        #endregion

        #region Modify
        public int Add<T>(T model)
        {
            return BaseDb.Add<T>(model);
        }

        public int Update<T>(T model, Guid id)
        {
            return BaseDb.Update<T>(model, new KeyValuePair<string, object>("ID", id));
        }

        /// <summary>
        /// 绑定楼盘
        /// </summary>
        /// <returns></returns>
        public int ManageBindHouseID(Guid houseID, Guid id)
        {
            RoadFlow.Data.Model.BuildingsAndBuildingMonthInfoModel model = new Data.Model.BuildingsAndBuildingMonthInfoModel();
            model.HouseID = houseID;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    BuildingsBLL buildingsBLL = new BuildingsBLL();
                    BuildingsDataBLL buildingsDataBLL = new BuildingsDataBLL();
                    buildingsBLL.MangeUpdate(model, id);
                    buildingsDataBLL.ManageUpdate(model, id);
                    BaseDb.Update(model, new KeyValuePair<string, object>("ID", id));
                    scope.Complete();
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }


        }

        /// <summary>
        /// 管理员编辑（除了BuildingsAndBuildingMonthInfo外，需要更新BuildingMonthInfoDataBLL，BuildingMonthInfoBLL,BuildingsBLL,BuildingsDataBLL）
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ManageUpdate(RoadFlow.Data.Model.BuildingsModel building, RoadFlow.Data.Model.BuildingMonthInfoModel monthInfo, Guid buildingID)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                bool isUpdate = Update<RoadFlow.Data.Model.BuildingsModel>(building, buildingID) > 0 && Update<RoadFlow.Data.Model.BuildingMonthInfoModel>(monthInfo, buildingID) > 0;//更新组合表
                if (isUpdate)
                {
                    #region BuildingMonthInfo
                    BuildingMonthInfoDataBLL buildingMonthInfoDataBLL = new BuildingMonthInfoDataBLL();
                    var monthDataList = buildingMonthInfoDataBLL.GetListByBuildingID(buildingID);
                    var monthDataLast = monthDataList.OrderByDescending(i => i.TimeArea).FirstOrDefault();
                    if (monthDataLast != null)
                    {//有最新每月信息则更新每月信息
                        BuildingMonthInfoBLL buildingMonthInfoBLL = new BuildingMonthInfoBLL();
                        isUpdate = buildingMonthInfoDataBLL.ManageUpdate(monthInfo, monthDataLast.ID.Value) > 0 && buildingMonthInfoBLL.ManageUpdate(monthInfo, monthDataLast.BuildingMonthInfoID.Value) > 0;
                    }

                    #endregion
                    if (isUpdate)
                    {//更新楼栋基本信息
                        #region Building
                        BuildingsBLL buildingsBLL = new BuildingsBLL();
                        BuildingsDataBLL buildingsDataBLL = new BuildingsDataBLL();
                        building.ID = null;
                        isUpdate = buildingsBLL.ManageUpdate(building, buildingID) > 0 && buildingsDataBLL.ManageUpdate(building, buildingID) > 0;
                        #endregion
                        if (isUpdate)
                        {
                            scope.Complete();
                            return 1;
                        }
                    }
                }
            }
            return 0;

        }

        public int ManageDelete(Guid id)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    BuildingsBLL buildingsBLL = new BuildingsBLL();
                    BuildingsDataBLL buildingsDataBLL = new BuildingsDataBLL();
                    BuildingMonthModifyCountBLL buildingMonthModifyCountBLL = new BuildingMonthModifyCountBLL();
                    BuildingMonthInfoDataBLL buildingMonthInfoDataBLL = new BuildingMonthInfoDataBLL();
                    BuildingMonthInfoBLL buildingMonthInfoBLL = new BuildingMonthInfoBLL();

                    buildingMonthModifyCountBLL.ManageDeleteByBuildingID(id);
                    buildingMonthInfoDataBLL.ManageDeleteByBuildingID(id);
                    buildingMonthInfoBLL.ManageDeleteByBuildingID(id);

                    buildingsBLL.ManageDelete(id);
                    buildingsDataBLL.ManageDelete(id);
                    BaseDb.Delete(id);
                    scope.Complete();
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }


        #endregion


    }
}
