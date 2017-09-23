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
    /// 楼宇每月信息
    /// </summary>
    public class BuildingMonthInfoDataBLL {
        private static string _tableName = "V_BuildingMonthInfoData_BuildingsName";
        private static string _modifyTableName = "BuildingMonthInfoData";
        private static string _order = "[BuildingID],[TimeArea] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName,_modifyTableName, _order);

        #region get
        public DataTable GetPagerData(out string pager,int size,int pageIndex,Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return baseDb.GetPagerData(out pager,size,pageIndex,where);
        }

        public RoadFlow.Data.Model.BuildingMonthInfoModel Get(Guid id) {
            return baseDb.Get<RoadFlow.Data.Model.BuildingMonthInfoModel>(new KeyValuePair<string,object>("ID",id));
        }

        public RoadFlow.Data.Model.BuildingMonthInfoModel GetByBuildingIDAndTimeArea(Guid buildingID,int timeArea) {
            return baseDb.Get<RoadFlow.Data.Model.BuildingMonthInfoModel>(new KeyValuePair<string,object>("BuildingID",buildingID),new KeyValuePair<string,object>("TimeArea",timeArea));
        }

        public DataTable GetAll(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return baseDb.GetAll(0,where);
        }

        /// <summary>
        /// 获取楼栋列表对应的每月数据
        /// </summary>
        /// <param name="buildings">楼栋列表</param>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.BuildingMonthInfoModel> GetListByBuildings(DataTable buildings) {
            List<string> buildingsID = new List<string>();
            foreach (DataRow dr in buildings.Rows) {
                if (!buildingsID.Contains(dr["ID"].ToString())) {
                    buildingsID.Add(dr["ID"].ToString());
                }
            }

            if (buildingsID.Count == 0) {
                return new List<RoadFlow.Data.Model.BuildingMonthInfoModel>();
            }

            //查询满足条件的所有每月数据
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("BuildingID", RoadFlow.Data.Model.SQLFilterType.IN), buildingsID);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);//正常的数据。
            DataTable dt = baseDb.GetAll(0, where);

            return dt.ToList<RoadFlow.Data.Model.BuildingMonthInfoModel>();
        }

        /// <summary>
        /// 获取楼栋列表对应的每月数据
        /// </summary>
        /// <param name="buildings">楼栋列表</param>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.BuildingMonthInfoModel> GetListByBuildingID(Guid buildingID) {
            //查询满足条件的所有每月数据
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("BuildingID", RoadFlow.Data.Model.SQLFilterType.EQUAL), buildingID);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);//正常的数据。
            DataTable dt = baseDb.GetAll(0, where);

            return dt.ToList<RoadFlow.Data.Model.BuildingMonthInfoModel>();
        }
        #endregion

        #region Modify
        public int Add(RoadFlow.Data.Model.BuildingMonthInfoModel model) {
            if (baseDb.Add<RoadFlow.Data.Model.BuildingMonthInfoModel>(model)>0) {
                Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.MINNotEqual), model.TimeArea);
                if (GetAll(where).Rows.Count==0) {//是否是最新的每月数据。
                    //更新合成表
                    BuildingsAndBuildingMonthInfoBLL buildingsAndBuildingMonthInfoBLL = new BuildingsAndBuildingMonthInfoBLL();
                    Guid id =model.BuildingID.Value;
                    //排除不需要更新的字段。
                    model.ID = null;
                    model.BuildingID = null;
                    model.BuildingMonthInfoID = null;
                    model.TimeArea = null;
                    return buildingsAndBuildingMonthInfoBLL.Update(model, id);
                }
                #region 注释掉的代码
                //RoadFlow.Data.Model.BuildingsAndBuildingMonthInfo monthModel = new RoadFlow.Data.Model.BuildingsAndBuildingMonthInfo();
                //monthModel.SY_YSY_ZMJ = model.SY_YSY_ZMJ;
                //monthModel.SY_YSY_ZYMJ = model.SY_YSY_ZYMJ;
                //monthModel.SY_KZ_ZMJ = model.SY_KZ_ZMJ;
                //monthModel.SY_KZ_KZLMJ = model.SY_KZ_KZLMJ;
                //monthModel.SY_KZ_KXSMJ = model.SY_KZ_KXSMJ;
                //monthModel.SY_ZJ = model.SY_ZJ;
                //monthModel.SY_XSJJ = model.SY_XSJJ;
                //monthModel.SW_YSY_ZMJ = model.SW_YSY_ZMJ;
                //monthModel.SW_YSY_ZYMJ = model.SW_YSY_ZYMJ;
                //monthModel.SW_KZ_ZMJ = model.SW_KZ_ZMJ;
                //monthModel.SW_KZ_KZLMJ = model.SW_KZ_KZLMJ;
                //monthModel.SW_KZ_KXSMJ = model.SW_KZ_KXSMJ;
                //monthModel.SW_ZJ = model.SW_ZJ;
                //monthModel.SW_XSJJ = model.SW_XSJJ;
                #endregion
                
            }
            return 0;
        }

        public int Update(RoadFlow.Data.Model.BuildingMonthInfoModel model,Guid id) {
            if (baseDb.Update<RoadFlow.Data.Model.BuildingMonthInfoModel>(model, new KeyValuePair<string, object>("ID", id))>0) {
                Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
                where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.MINNotEqual), model.TimeArea);
                if (GetAll(where).Rows.Count == 0) {//是否是最新的每月数据。
                    //更新合成表
                    BuildingsAndBuildingMonthInfoBLL buildingsAndBuildingMonthInfoBLL = new BuildingsAndBuildingMonthInfoBLL();
                    Guid buildingID = model.BuildingID.Value;
                    //排除不需要更新的字段。
                    model.ID = null;
                    model.BuildingID = null;
                    model.TimeArea = null;
                    model.BuildingMonthInfoID = null;
                    return buildingsAndBuildingMonthInfoBLL.Update(model, buildingID);
                }
            }
            return 0;
        }

        public int ManageUpdate(RoadFlow.Data.Model.BuildingMonthInfoModel model, Guid id) {
            model.ID = null;
            return baseDb.Update<RoadFlow.Data.Model.BuildingMonthInfoModel>(model, new KeyValuePair<string, object>("ID", id));
        }


        public int ManageDeleteByBuildingID(Guid buildingID)
        {
            return baseDb.DeleteByPara(new { buildingID });
        }
        #endregion

    }
}
