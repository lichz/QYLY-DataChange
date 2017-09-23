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
    /// 楼宇企业(流程)
    /// </summary>
    public class EnterpriseBLL {
        public IBase BaseDb { get; private set; }
        public EnterpriseBLL()
        {
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase("Enterprise", "[BuildingID]");
        }

        #region get
 
        public RoadFlow.Data.Model.EnterpriseModel Get(Guid id) {
            return BaseDb.Get<RoadFlow.Data.Model.EnterpriseModel>(new KeyValuePair<string, object>("ID", id));
        }

        #endregion

        #region Modify
        public int Begin(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return 0;
            }
            RoadFlow.Data.Model.EnterpriseModel model = new Data.Model.EnterpriseModel();
            model.State = RoadFlow.Data.Model.State.Start;
            return BaseDb.Update<RoadFlow.Data.Model.EnterpriseModel>(model, new KeyValuePair<string, object>("ID", id));
        }

        /// <summary>
        /// 流程完成时的操作。
        /// </summary>
        /// <returns></returns>
        public void Complete(Guid id) {
            RoadFlow.Data.Model.EnterpriseModel model = Get(id);
            #region 去除流程关联字段
            model.State = null;
            model.CreateTime = null;
            model.UpdateTime = null;
            #endregion
            EnterpriseAndEnterpriseTaxBLL enterpriseAndEnterpriseTaxBLL = new EnterpriseAndEnterpriseTaxBLL();
            if (enterpriseAndEnterpriseTaxBLL.Get(id) != null)
            {//企业已存在,更新信息
                model.ID = null;
                model.UpdateTime = DateTime.Now;
                if (enterpriseAndEnterpriseTaxBLL.Update(model, id) > 0)
                {
                    //更新企业变更记录
                    EnterpriseUpdateRecordBLL recordBLL = new EnterpriseUpdateRecordBLL();
                    RoadFlow.Data.Model.EnterpriseUpdateRecordType type = Data.Model.EnterpriseUpdateRecordType.Modify;
                    if (model.Status == RoadFlow.Data.Model.Status.Deleted)
                    {//搬出
                        type = Data.Model.EnterpriseUpdateRecordType.Delete;
                    }
                    recordBLL.Add(id, type);
                }
            } else {
                model.CreateTime = DateTime.Now;
                model.UpdateTime = DateTime.Now;
                if (enterpriseAndEnterpriseTaxBLL.Add(model) > 0)
                {
                    //更新企业变更记录
                    EnterpriseUpdateRecordBLL recordBLL = new EnterpriseUpdateRecordBLL();
                    recordBLL.Add(id, RoadFlow.Data.Model.EnterpriseUpdateRecordType.Add);
                }
            }

            #region 每月更新次数及分数更新
            Guid buildingID = model.BuildingID.Value;
            int timeArea = DateTime.Now.Year * 100 + DateTime.Now.Month;
            BuildingMonthModifyCountBLL buildingMonthModifyCountBLL = new BuildingMonthModifyCountBLL();
            var oldCount = buildingMonthModifyCountBLL.Get(buildingID, timeArea); 
            if (oldCount != null)
            {//已存在,每月更新次数更新
                RoadFlow.Data.Model.BuildingMonthModifyCountModel buildingMonthModifyCountModel = new Data.Model.BuildingMonthModifyCountModel();
                buildingMonthModifyCountModel.EnterpriseModifyCount = GetCount(buildingID);
                buildingMonthModifyCountModel.Quality = BLLCommon.GetQuality(buildingMonthModifyCountModel.EnterpriseModifyCount.Value+oldCount.Count.Value);
                buildingMonthModifyCountModel.Accuracy = GetAccuracy();
                buildingMonthModifyCountBLL.Update(buildingMonthModifyCountModel, oldCount.ID.Value);
            }
            else
            {
                RoadFlow.Data.Model.BuildingMonthModifyCountModel buildingMonthModifyCountModel = new Data.Model.BuildingMonthModifyCountModel();
                buildingMonthModifyCountModel.TimeArea = timeArea;
                buildingMonthModifyCountModel.BuildingID = buildingID;
                buildingMonthModifyCountModel.EnterpriseModifyCount = GetCount(buildingID);
                buildingMonthModifyCountModel.Timeliness = BLLCommon.GetTimeliness();
                buildingMonthModifyCountModel.Quality = BLLCommon.GetQuality(buildingMonthModifyCountModel.EnterpriseModifyCount.Value);
                buildingMonthModifyCountModel.Accuracy = GetAccuracy();
                buildingMonthModifyCountBLL.Add(buildingMonthModifyCountModel);
            }
            #endregion
        }

        /// <summary>
        /// 管理员直接更改。
        /// </summary>
        /// <returns></returns>
        public int MangeAdd(RoadFlow.Data.Model.EnterpriseModel model)
        {
            model.State = RoadFlow.Data.Model.State.Finish;
            return BaseDb.Add(model);
        }

        /// <summary>
        /// 管理员直接更改。
        /// </summary>
        /// <returns></returns>
        public int MangeUpdate(RoadFlow.Data.Model.EnterpriseModel model,Guid id)
        {
            model.ID = null;
            model.State = null;
            model.CreateTime = null;
            model.UpdateTime = null;
            return BaseDb.Update(model,new KeyValuePair<string,object>("ID",id));
        }

        public int MangeDelete(Guid id) 
        {
            return BaseDb.Delete(id);
        }
        #endregion

        /// <summary>
        /// 获取企业变更数量
        /// </summary>
        /// <returns></returns>
        private int GetCount(Guid buildingID) {
            DateTime begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime end = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
            EnterpriseUpdateRecordBLL enterpriseUpdateRecordBLL = new EnterpriseUpdateRecordBLL();
            return enterpriseUpdateRecordBLL.GetAllByBuildingIDAndTimeArea(buildingID, begin, end).Rows.Count;
        }

        private int GetAccuracy()
        {
            return 20;
        }
    }
}
