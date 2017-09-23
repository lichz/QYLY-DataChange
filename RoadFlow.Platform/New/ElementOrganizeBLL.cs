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
    /// 新增的一种权限方式
    /// </summary>
    public class ElementOrganizeBLL {
        private static string _tableName = "ElementOrganize";
        private static string _order = "[CreateTime] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get

        /// <summary>
        /// 获取楼栋对应楼盘关联的街道
        /// </summary>
        /// <returns></returns>
        public DataTable GetToStreetByBuildingID(object buildingID)
        {
            return GetByTypeAndElementID(RoadFlow.Data.Model.ElementOrganizeType.ToStreet, new RoadFlow.Platform.BuildingsDataBLL().Get(Guid.Parse(buildingID.ToString())).HouseID.Value);
        }

        //#region 企业变更
        ///// <summary>
        ///// 获取当前用户企业变更权限列表
        ///// </summary>
        ///// <returns></returns>
        //public DataTable GetCurrentEnterpriseModify() {
        //    return GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseModify, RoadFlow.Platform.Users.CurrentFirstRelationID);
        //}

        ///// <summary>
        ///// 按组织获取企业变更权限列表
        ///// </summary>
        ///// <param name="organizeID">组织ID</param>
        ///// <returns></returns>
        //public DataTable GetEnterpriseModifyByOrganizeID(Guid organizeID) {
        //    return GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseModify, organizeID);
        //}
        //#endregion

        //#region 企业税收
        /// <summary>
        /// 获取当前用户企业税收权限列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentEnterpriseTax() {
            return GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseTax, RoadFlow.Platform.Users.CurrentFirstRelationID);
        }

        ///// <summary>
        ///// 按组织获取企业税收权限列表
        ///// </summary>
        ///// <param name="organizeID">组织ID</param>
        ///// <returns></returns>
        //public DataTable GetEnterpriseTaxByOrganizeID(Guid organizeID) {
        //    return GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseTax, organizeID);
        //}
        //#endregion

        //#region 楼栋变更
        /// <summary>
        /// 获取当前用户楼栋变更权限列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetCurrentBuildingModify() {
            return GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.BuildingModify, RoadFlow.Platform.Users.CurrentFirstRelationID);
        }

        
        /// <summary>
        /// 获取楼盘关联的街道信息
        /// </summary>
        /// <returns></returns>
        public DataTable GetToStreetByHouseID(object houseID)
        {
            return GetByTypeAndElementID(RoadFlow.Data.Model.ElementOrganizeType.ToStreet, houseID);
        }


        ///// <summary>
        ///// 按组织获取楼栋变更权限列表
        ///// </summary>
        ///// <param name="organizeID">组织ID</param>
        ///// <returns></returns>
        //public DataTable GetBuildingModifyByOrganizeID(Guid organizeID) {
        //    return GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseTax, organizeID);
        //}
        //#endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="organizeID">组织机构ID</param>
        /// <returns></returns>
        public DataTable GetByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType type, Guid organizeID) {
            return baseDb.GetAllByPara(0, new KeyValuePair<string, object>("Type", type), new KeyValuePair<string, object>("OrganizeID", organizeID), new KeyValuePair<string, object>("Status", RoadFlow.Data.Model.Status.Normal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public DataTable GetByTypeAndElementID(RoadFlow.Data.Model.ElementOrganizeType type, object elementID)
        {
            return baseDb.GetAllByPara(0, new KeyValuePair<string, object>("Type", type), new KeyValuePair<string, object>("ElementID", elementID), new KeyValuePair<string, object>("Status", RoadFlow.Data.Model.Status.Normal));
        }
        #endregion

        #region Modify
        ///// <summary>
        ///// 更新企业税收权限
        ///// </summary>
        ///// <param name="organizeID">组织ID</param>
        ///// <param name="elementIds">楼栋编号列表</param>
        //public void ModifyEnterpriseTaxByOrganizeID(Guid organizeID, List<Guid> elementIds) {
        //    ModifyByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseTax, organizeID, elementIds);
        //}
        ///// <summary>
        ///// 更新企业变更权限
        ///// </summary>
        ///// <param name="organizeID">组织ID</param>
        ///// <param name="elementIds">楼栋编号列表</param>
        //public void ModifyEnterpriseModifyByOrganizeID(Guid organizeID, List<Guid> elementIds) {
        //    ModifyByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.EnterpriseModify, organizeID, elementIds);
        //}
        ///// <summary>
        ///// 更新楼栋变更权限
        ///// </summary>
        ///// <param name="organizeID">组织ID</param>
        ///// <param name="elementIds">楼栋编号列表</param>
        //public void ModifyBuildingModifyByOrganizeID(Guid organizeID, List<Guid> elementIds) {
        //    ModifyByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType.BuildingModify,organizeID,elementIds);
        //}

        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="type">所属分类</param>
        /// <param name="organizeID">组织ID</param>
        /// <param name="elementIds">需新增的元素列表</param>
        /// <returns></returns>
        public void ModifyByTypeAndOrganizeID(RoadFlow.Data.Model.ElementOrganizeType type, Guid organizeID, List<Guid> elementIds) {
            #region 先删除，再新增
            #region 删除
            RoadFlow.Data.Model.ElementOrganizeModel model = new Data.Model.ElementOrganizeModel();
            model.Status = RoadFlow.Data.Model.Status.Deleted;
            baseDb.Update<RoadFlow.Data.Model.ElementOrganizeModel>(model, new KeyValuePair<string, object>("Type", type), new KeyValuePair<string, object>("OrganizeID", organizeID));
            if (type == Data.Model.ElementOrganizeType.BuildingModify) {//楼盘变更和企业变更先做一个权限处理，所以这里把企业变更权限也处理了。
                baseDb.Update<RoadFlow.Data.Model.ElementOrganizeModel>(model, new KeyValuePair<string, object>("Type", Data.Model.ElementOrganizeType.EnterpriseModify), new KeyValuePair<string, object>("OrganizeID", organizeID));
            }
            #endregion
            #region 新增
            if (elementIds != null)
            {
                foreach (var item in elementIds) {
                    RoadFlow.Data.Model.ElementOrganizeModel add = new Data.Model.ElementOrganizeModel();
                    add.ElementID = item;
                    add.OrganizeID = organizeID;
                    add.Type = type;
                    baseDb.Add<RoadFlow.Data.Model.ElementOrganizeModel>(add);
                }
            }
            if (type == Data.Model.ElementOrganizeType.BuildingModify) {//楼盘变更和企业变更先做一个权限处理，所以这里把企业变更权限也处理了。
                if (elementIds != null) {
                    foreach (var item in elementIds) {
                        RoadFlow.Data.Model.ElementOrganizeModel add = new Data.Model.ElementOrganizeModel();
                        add.ElementID = item;
                        add.OrganizeID = organizeID;
                        add.Type = Data.Model.ElementOrganizeType.EnterpriseModify;
                        baseDb.Add<RoadFlow.Data.Model.ElementOrganizeModel>(add);
                    }
                }
            }


            #endregion
            #endregion
        }
        #endregion
    }
}
