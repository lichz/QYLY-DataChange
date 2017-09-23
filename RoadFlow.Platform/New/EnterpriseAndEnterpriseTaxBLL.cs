using RoadFlow.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 企业和最新企业税收合成表
    /// </summary>
    public class EnterpriseAndEnterpriseTaxBLL
    {
        public IBase BaseDb { get; set; }
        public EnterpriseAndEnterpriseTaxBLL()
        {
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase("V_EnterpriseAndEnterpriseTax_BuildingName", "EnterpriseAndEnterpriseTax", "[CreateTime]");
        }

        #region get
        /// <summary>
        /// 查询所有记录(带翻页)
        /// </summary>
        public DataTable GetPagerData(out string pager, int size, int pageIndex)
        {
            Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where = new Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object>();
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetPagerData(out pager, size, pageIndex, where);
        }
        public DataTable GetPagerData(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetPagerData(out pager, size, pageIndex, where);
        }
        public RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel Get(Guid id)
        {
            return BaseDb.Get<RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel>(new KeyValuePair<string, object>("ID", id));
        }
        public DataTable GetAllByBuildingID(string buildingID)
        {
            return BaseDb.GetAllByPara(0, new KeyValuePair<string, object>("BuildingID", buildingID), new KeyValuePair<string, object>("Status", RoadFlow.Data.Model.Status.Normal));//0表示取所有，大于取前几条。
        }

        /// <summary>
        /// 根据楼栋ID获取所有企业，包含被删除的。
        /// </summary>
        /// <param name="buildingID"></param>
        /// <returns></returns>
        public DataTable GetAllContainDeleteByBuildingID(string buildingID)
        {
            return BaseDb.GetAllByPara(0, new KeyValuePair<string, object>("BuildingID", buildingID));//0表示取所有，大于取前几条。
        }

        /// <summary>
        /// 获取building列表对应的所有企业。
        /// </summary>
        /// <param name="buildings">building列表</param>
        /// <returns></returns>
        public DataTable GetAllByBuildings(DataTable buildings, int pageIndex, int size, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where, out string pager)
        {
            List<string> buildingsID = new List<string>();
            foreach (DataRow dr in buildings.Rows)
            {
                if (!buildingsID.Contains(dr["ID"].ToString()))
                {
                    buildingsID.Add(dr["ID"].ToString());
                }
            }
            //查询满足条件的所有楼栋
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("BuildingID", RoadFlow.Data.Model.SQLFilterType.IN), buildingsID);
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetPagerData(out pager, size, pageIndex, where);
        }

        public DataTable GetAll(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetAll(0, where);
        }
        #endregion

        #region Modify
        public int Add(RoadFlow.Data.Model.EnterpriseModel model)
        {
            return BaseDb.Add<RoadFlow.Data.Model.EnterpriseModel>(model);
        }
        public int Update<T>(T model, Guid id)
        {
            return BaseDb.Update<T>(model, new KeyValuePair<string, object>("ID", id));
        }

        /// <summary>
        /// 管理员直接更改。
        /// </summary>
        /// <returns></returns>
        public int ManageUpdate(RoadFlow.Data.Model.EnterpriseModel model, Guid id)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    EnterpriseBLL enterpriseBLL = new EnterpriseBLL();
                    #region 更新企业
                    enterpriseBLL.MangeUpdate(model, id);

                    model.UpdateTime = DateTime.Now;
                    BaseDb.Update(model, new KeyValuePair<string, object>("ID", id));
                    #endregion
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    //更新失败。
                    return 0;
                }
            }
            return 1;
        }


        public int ManageAdd(RoadFlow.Data.Model.EnterpriseModel model)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    EnterpriseBLL enterpriseBLL = new EnterpriseBLL();
                    #region 添加企业
                    //添加流程中的企业
                    enterpriseBLL.MangeAdd(model);
                    //添加组合企业
                    model.State = null;
                    BaseDb.Add(model);
                    #endregion
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            return 1;
        }

        public int ManageDelete(Guid id)
        {
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                try
                {
                    EnterpriseBLL enterpriseBLL = new EnterpriseBLL();
                    #region 企业搬出
                    BaseDb.Delete(id);
                    //更新流程中的企业
                    enterpriseBLL.MangeDelete(id);
                    //new EnterpriseUpdateRecordBLL().MangeDeleteByEnterpriseID(id);变更记录不需要删。
                    new EnterpriseTaxBLL().ManageDeleteByEnterpriseID(id);
                    #endregion
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    string path = "D://log.txt";
                    StreamWriter mySw = File.AppendText(path);
                    mySw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ " EnterpriseAndEnterpriseTaxBLL.ManageDelete:" + ex.Message);
                    //关闭日志文件
                    mySw.Close();
                    return 0;
                }
            }
            return 1;
        }
        #endregion


    }
}
