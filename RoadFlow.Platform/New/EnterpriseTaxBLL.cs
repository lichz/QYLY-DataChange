using RoadFlow.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 企业税收
    /// </summary>
    public class EnterpriseTaxBLL
    {
        public IBase BaseDb { get; set; }
        public EnterpriseTaxBLL()
        {
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase("V_EnterpriseTax_EnterpriseName", "EnterpriseTax", "[EnterpriseID],[TaxArea] desc,[CreateTime] desc");
        }

        #region get
        public RoadFlow.Data.Model.EnterpriseTaxModel Get(Guid id)
        {
            return BaseDb.Get<RoadFlow.Data.Model.EnterpriseTaxModel>(new KeyValuePair<string, object>("ID", id));
        }

        public DataTable GetPagerData(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetPagerData(out pager, size, pageIndex, where);
        }

        /// <summary>
        /// 获取企业最新税收
        /// </summary>
        /// <returns></returns>
        public RoadFlow.Data.Model.EnterpriseTaxModel GetLastTaxByEnterpristID(Guid enterpriseID)
        {
            return BaseDb.Get<RoadFlow.Data.Model.EnterpriseTaxModel>(new KeyValuePair<string, object>("Status", RoadFlow.Data.Model.Status.Normal), new KeyValuePair<string, object>("EnterpriseID", enterpriseID));
        }

        /// <summary>
        /// 获取企业税收列表
        /// </summary>
        /// <param name="enterpriseID">企业ID</param>
        /// <returns></returns>
        public DataTable GetAllByEnterpriseID(Guid enterpriseID)
        {
            return BaseDb.GetAllByPara(0,new KeyValuePair<string, object>("EnterpriseID",enterpriseID),new KeyValuePair<string,object>("Status", Data.Model.Status.Normal));
        }
        #endregion

        #region Modify
        public RoadFlow.Data.Model.Result<object> Add(RoadFlow.Data.Model.EnterpriseTaxModel model)
        {
            model.ID = Guid.NewGuid();
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                if (BaseDb.Add(model) > 0)
                {
                    var enterpriseAndEnterpriseTaxBLL = new EnterpriseAndEnterpriseTaxBLL();
                    var enterpriseAndEnterpriseTax = enterpriseAndEnterpriseTaxBLL.Get(model.EnterpriseID.Value);
                    if (enterpriseAndEnterpriseTax.TaxArea == null || enterpriseAndEnterpriseTax.TaxArea <= model.TaxArea)
                    {//企业没有税收或者企业税收不是最新的，更新企业税收企业信息合成表
                        var enterpriseAndEnterpriseTaxUpdateModel = new RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel
                        {
                            //enterpriseAndEnterpriseTaxUpdateModel.NationalTax = model.NationalTax;
                            //enterpriseAndEnterpriseTaxUpdateModel.LandTax = model.LandTax;
                            Tax = model.Tax,
                            TaxArea = model.TaxArea
                        };
                        if (enterpriseAndEnterpriseTaxBLL.Update(enterpriseAndEnterpriseTaxUpdateModel, model.EnterpriseID.Value) > 0)
                        {
                            scope.Complete();
                            return new Data.Model.Result<object>() {
                                Success = true,
                                Data = model.ID
                            };
                        }
                    }

                    //不更新企业税收企业信息合成表
                    scope.Complete();
                    return new Data.Model.Result<object>()
                    {
                        Success = true,
                        Data = model.ID
                    };
                }
            }
            return new Data.Model.Result<object>()
            {
                Success = false,
            };
        }

        public int Update(RoadFlow.Data.Model.EnterpriseTaxModel model, Guid id)
        {
            model.ID = null;
            model.UpdateTime = DateTime.Now;
            using (System.Transactions.TransactionScope scope = new System.Transactions.TransactionScope())
            {
                if (BaseDb.Update(model, new KeyValuePair<string, object>("ID", id)) > 0)
                {
                    var oldTax = Get(id);
                    var enterpriseAndEnterpriseTaxBLL = new EnterpriseAndEnterpriseTaxBLL();
                    var enterpriseAndEnterpriseTax = enterpriseAndEnterpriseTaxBLL.Get(oldTax.EnterpriseID.Value);
                    if (enterpriseAndEnterpriseTax.TaxArea == null || model.TaxArea == null || enterpriseAndEnterpriseTax.TaxArea <= model.TaxArea)
                    {//企业没有税收或者企业税收不是最新的，更新企业税收企业信息合成表
                        if (model.Status == RoadFlow.Data.Model.Status.Deleted)
                        {//税收删除
                            //查询企业最新税收
                            var lastTax = GetLastTaxByEnterpristID(oldTax.EnterpriseID.Value);
                            if (lastTax == null)
                            {//没有最新税收
                                //更新合成表税收为初始值。
                                var enterpriseAndEnterpriseTaxUpdateModel = new RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel
                                {
                                    //enterpriseAndEnterpriseTaxUpdateModel.NationalTax = 0;
                                    //enterpriseAndEnterpriseTaxUpdateModel.LandTax = 0;
                                    Tax = 0,
                                    TaxArea = 0
                                };
                                if (enterpriseAndEnterpriseTaxBLL.Update(enterpriseAndEnterpriseTaxUpdateModel, oldTax.EnterpriseID.Value) > 0)
                                {
                                    scope.Complete();
                                    return 1;
                                }
                            }
                            else
                            {//有最新税收，更新合成表税收为最新税收
                                var enterpriseAndEnterpriseTaxUpdateModel = new RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel
                                {
                                    //enterpriseAndEnterpriseTaxUpdateModel.NationalTax = lastTax.NationalTax;
                                    //enterpriseAndEnterpriseTaxUpdateModel.LandTax = lastTax.LandTax;
                                    Tax = lastTax.Tax,
                                    TaxArea = lastTax.TaxArea
                                };
                                if (enterpriseAndEnterpriseTaxBLL.Update(enterpriseAndEnterpriseTaxUpdateModel, oldTax.EnterpriseID.Value) > 0)
                                {
                                    scope.Complete();
                                    return 1;
                                }
                            }
                        }
                        else
                        {//税收更新
                            var enterpriseAndEnterpriseTaxUpdateModel = new RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel
                            {
                                //enterpriseAndEnterpriseTaxUpdateModel.NationalTax = model.NationalTax;
                                //enterpriseAndEnterpriseTaxUpdateModel.LandTax = model.LandTax;
                                Tax = model.Tax,
                                TaxArea = model.TaxArea
                            };
                            if (enterpriseAndEnterpriseTaxBLL.Update(enterpriseAndEnterpriseTaxUpdateModel, oldTax.EnterpriseID.Value) > 0)
                            {
                                scope.Complete();
                                return 1;
                            }
                        }

                    }

                    //不更新企业税收企业信息合成表
                    scope.Complete();
                    return 1;
                }
            }

            return 0;
        }

        public int Delete(Guid id)
        {
            return Update(new Data.Model.EnterpriseTaxModel() { Status = Data.Model.Status.Deleted },id);
        }

        /// <summary>
        /// 管理员删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ManageDeleteByEnterpriseID(Guid enterpriseID)
        {
            return BaseDb.DeleteByPara(new{ enterpriseID });
        }
        #endregion


    }
}
