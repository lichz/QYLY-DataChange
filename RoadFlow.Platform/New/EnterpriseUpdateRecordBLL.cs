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
    /// 企业变更记录
    /// </summary>
    public class EnterpriseUpdateRecordBLL
    {
        public IBase BaseDb { get; private set; }
        public EnterpriseUpdateRecordBLL()
        {
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase("EnterpriseUpdateRecord", "[CreateTime] desc");
        }


        #region get
        /// <summary>
        /// 时间段里该楼栋的变动数据
        /// </summary>
        /// <param name="buildingID"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DataTable GetAllByBuildingIDAndTimeArea(Guid buildingID, DateTime begin, DateTime end)
        {
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, Data.Model.SQLFilterType>, object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("CreateTime", RoadFlow.Data.Model.SQLFilterType.MINNotEqual), begin);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("CreateTime", RoadFlow.Data.Model.SQLFilterType.MAXNotEqual), end);
            //获取楼栋对应所有企业，包含被删除的。
            List<string> list = new List<string>();
            EnterpriseAndEnterpriseTaxBLL enterpriseAndEnterpriseTaxBLL = new EnterpriseAndEnterpriseTaxBLL();
            foreach (DataRow dr in enterpriseAndEnterpriseTaxBLL.GetAllContainDeleteByBuildingID(buildingID.ToString()).Rows)
            {
                list.Add(dr["ID"].ToString());
            }
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("EnterpriseID", RoadFlow.Data.Model.SQLFilterType.IN), list);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);//正常的数据。
            return BaseDb.GetAll(0, where);
        }
        #endregion
        #region Modify
        /// <summary>
        /// 添加变更记录
        /// </summary>
        /// <param name="enterpriseID">企业ID</param>
        /// <returns></returns>
        public int Add(Guid enterpriseID, RoadFlow.Data.Model.EnterpriseUpdateRecordType type)
        {
            RoadFlow.Data.Model.EnterpriseUpdateRecordModel model = new Data.Model.EnterpriseUpdateRecordModel();
            model.EnterpriseID = enterpriseID;
            model.Type = type;
            model.CreateTime = DateTime.Now;
            return BaseDb.Add<RoadFlow.Data.Model.EnterpriseUpdateRecordModel>(model);
        }


        public int MangeDeleteByEnterpriseID(Guid enterpriseID)
        {
            return BaseDb.DeleteByPara(new{ enterpriseID });
        }
        #endregion


    }
}
