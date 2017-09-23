using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadFlow.Data.Interface;
using RoadFlow.Platform;

namespace RoadFlow.Platform {
    /// <summary>
    /// 楼宇每月信息(流程)
    /// </summary>
    public class BuildingMonthInfoBLL {
        /// <summary>
        /// 截止日期
        /// </summary>
        public int EndDay { get; private set; }
        /// <summary>
        /// 最后截止日期
        /// </summary>
        public int LastEndDay { get; private set; }
        public IBase BaseDb { get; private set; }
        public BuildingMonthInfoBLL() {
            EndDay = 20;
            LastEndDay = 25;
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase("BuildingMonthInfo", "[CreateTime] desc");
        }

        #region get
        public RoadFlow.Data.Model.BuildingMonthInfoModel Get(Guid id) {
            return BaseDb.Get<RoadFlow.Data.Model.BuildingMonthInfoModel>(new KeyValuePair<string, object>("ID", id));
        }

        public DataTable GetAll(int top, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetAll(top, where);
        }

        #endregion

        #region Modify
        public int ManageUpdate(RoadFlow.Data.Model.BuildingMonthInfoModel model, Guid id) {
            model.ID = null;
            return BaseDb.Update<RoadFlow.Data.Model.BuildingMonthInfoModel>(model, new KeyValuePair<string, object>("ID", id));
        }

        public int Begin(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return 0;
            }
            RoadFlow.Data.Model.BuildingMonthInfoModel model = new Data.Model.BuildingMonthInfoModel();
            model.State = RoadFlow.Data.Model.State.Start;
            return BaseDb.Update<RoadFlow.Data.Model.BuildingMonthInfoModel>(model, new KeyValuePair<string, object>("ID", id));
        }
        /// <summary>
        /// 流程完成时的操作。
        /// </summary>
        /// <returns></returns>
        public void Complete(Guid id) {
            RoadFlow.Data.Model.BuildingMonthInfoModel model = Get(id);
            Guid buildingID = model.BuildingID.Value;
            int timeArea = DateTime.Now.Year*100+DateTime.Now.Month;

            #region 去除流程关联字段
            model.State = null;
            model.CreateTime = null;
            model.UpdateTime = null;
            model.MissionDisplay = null;
            #endregion
            model.BuildingMonthInfoID = id;//设置BuildingMonthInfoData是buildingMonthInfo的哪条数据更新而来。
            #region 每月信息更新
            BuildingMonthInfoDataBLL buildingsDataBLL = new BuildingMonthInfoDataBLL();
            RoadFlow.Data.Model.BuildingMonthInfoModel old = buildingsDataBLL.GetByBuildingIDAndTimeArea(buildingID, timeArea);
            if (old != null) {//已存在,更新每月信息
                model.ID = null;
                model.TimeArea = timeArea;
                model.UpdateTime = DateTime.Now;
                buildingsDataBLL.Update(model, old.ID.Value);
            } else {
                model.ID = Guid.NewGuid();
                model.TimeArea = timeArea;
                buildingsDataBLL.Add(model);
            }
            #endregion
            #region 每月更新次数及分数更新
            BuildingMonthModifyCountBLL buildingMonthModifyCountBLL = new BuildingMonthModifyCountBLL();
            var oldCount = buildingMonthModifyCountBLL.Get(buildingID, timeArea); //GetOperationType(buildingMonthModifyCountBLL, buildingID, timeArea,out countID);
            if (oldCount != null) {//已存在,每月更新次数更新
                RoadFlow.Data.Model.BuildingMonthModifyCountModel buildingMonthModifyCountModel = new Data.Model.BuildingMonthModifyCountModel();
                buildingMonthModifyCountModel.Count = GetCount(buildingID,timeArea,model);
                //buildingMonthModifyCountModel.Timeliness = GetTimeliness();  更新不算及时性评分
                buildingMonthModifyCountModel.Quality = BLLCommon.GetQuality(buildingMonthModifyCountModel.Count.Value + oldCount.EnterpriseModifyCount.Value);
                buildingMonthModifyCountModel.Accuracy = GetAccuracy();
                buildingMonthModifyCountBLL.Update(buildingMonthModifyCountModel, oldCount.ID.Value);
            } else {
                RoadFlow.Data.Model.BuildingMonthModifyCountModel buildingMonthModifyCountModel = new Data.Model.BuildingMonthModifyCountModel();
                buildingMonthModifyCountModel.TimeArea = timeArea;
                buildingMonthModifyCountModel.BuildingID = buildingID;
                buildingMonthModifyCountModel.Count = GetCount(buildingID, timeArea, model);
                buildingMonthModifyCountModel.Timeliness = BLLCommon.GetTimeliness();
                buildingMonthModifyCountModel.Quality = BLLCommon.GetQuality(buildingMonthModifyCountModel.Count.Value);
                buildingMonthModifyCountModel.Accuracy = GetAccuracy();
                buildingMonthModifyCountBLL.Add(buildingMonthModifyCountModel);
            }
            #endregion

        }

        public int ManageDeleteByBuildingID(Guid buildingID)
        {
            return BaseDb.DeleteByPara(new { buildingID });
        }
        #endregion

        /// <summary>
        /// 获取更新数
        /// </summary>
        /// <returns></returns>
        private int GetCount(Guid buildingID, int timeArea, RoadFlow.Data.Model.BuildingMonthInfoModel model) {
            int count = 0;
            #region 上月
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> prewhere = GetMonthWhere(buildingID);
            DataTable preDt = GetAll(1, prewhere);
            #endregion
            //对比字段（SY_YSY_ZMJ,SY_YSY_ZYMJ,SY_KZ_ZMJ，SY_KZ_KZLMJ，SY_KZ_KXSMJ，SY_ZJ,SY_XSJJ,SW_YSY_ZMJ,SW_YSY_ZYMJ,SW_KZ_ZMJ,SW_KZ_KZLMJ,SW_KZ_KXSMJ,SW_ZJ，SW_XSJJ）
            if (preDt==null||preDt.Rows.Count==0) {
                count = 16;
            } else {
                if (model.SY_YSY_ZMJ != (decimal?)preDt.Rows[0]["SY_YSY_ZMJ"]) {
                    count++;
                }
                if (model.SY_YSY_ZYMJ != (decimal?)preDt.Rows[0]["SY_YSY_ZYMJ"]) {
                    count++;
                }
                if (model.SY_KZ_ZMJ != (decimal?)preDt.Rows[0]["SY_KZ_ZMJ"]) {
                    count++;
                }
                if (model.SY_KZ_KZLMJ != (decimal?)preDt.Rows[0]["SY_KZ_KZLMJ"]) {
                    count++;
                }
                if (model.SY_KZ_KXSMJ != (decimal?)preDt.Rows[0]["SY_KZ_KXSMJ"]) {
                    count++;
                }
                if (model.SY_KZ_DCZDZMJ != (decimal?)preDt.Rows[0]["SY_KZ_DCZDZMJ"]) {
                    count++;
                }
                if (model.SY_ZJ != (Guid?)preDt.Rows[0]["SY_ZJ"]) {
                    count++;
                }
                if (model.SY_XSJJ != (decimal?)preDt.Rows[0]["SY_XSJJ"]) {
                    count++;
                }
                if (model.SW_YSY_ZMJ != (decimal?)preDt.Rows[0]["SW_YSY_ZMJ"]) {
                    count++;
                }
                if (model.SW_YSY_ZYMJ != (decimal?)preDt.Rows[0]["SW_YSY_ZYMJ"]) {
                    count++;
                }
                if (model.SW_KZ_ZMJ != (decimal?)preDt.Rows[0]["SW_KZ_ZMJ"]) {
                    count++;
                }
                if (model.SW_KZ_KZLMJ != (decimal?)preDt.Rows[0]["SW_KZ_KZLMJ"]) {
                    count++;
                }
                if (model.SW_KZ_KXSMJ != (decimal?)preDt.Rows[0]["SW_KZ_KXSMJ"]) {
                    count++;
                }
                if (model.SW_KZ_DCZDZMJ != (decimal?)preDt.Rows[0]["SW_KZ_DCZDZMJ"]) {
                    count++;
                }
                if (model.SW_ZJ != (Guid?)preDt.Rows[0]["SW_ZJ"]) {
                    count++;
                }
                if (model.SW_XSJJ != (decimal?)preDt.Rows[0]["SW_XSJJ"]) {
                    count++;
                }
            }

            return count;
        }

        /// <summary>
        /// 获取准确率评分
        /// </summary>
        /// <returns></returns>
        private decimal GetAccuracy() {
            return 20;
        }

        /// <summary>
        /// 拼接筛选条件
        /// </summary>
        /// <param name="buildingID"></param>
        /// <param name="timeArea"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        private Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> GetMonthWhere(Guid buildingID) {
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("BuildingID", RoadFlow.Data.Model.SQLFilterType.EQUAL), buildingID);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("CreateTime", RoadFlow.Data.Model.SQLFilterType.MAXNotEqual), new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("State", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.State.Finish);
            return where;
        }
    }
}
