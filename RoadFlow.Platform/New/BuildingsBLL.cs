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
    /// 楼宇(流程)，楼宇（流程）是用用于整个流程处理的，而BuildingsData是流程完成以后的楼宇信息，除了流程以外别的展示，统计都是用的BuildingsData
    /// </summary>
    public class BuildingsBLL {
        private static string _tableName = "Buildings";
        private static string _order = "[HouseID],[CreateTime] desc";
        IBase BaseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);
        public RoadFlow.Data.Model.BuildingsModel Get(string id) {
            return BaseDb.Get<RoadFlow.Data.Model.BuildingsModel>(new KeyValuePair<string, object>("ID", id));
        }

        public int ManageUpdate(RoadFlow.Data.Model.BuildingsModel model, Guid id) {
            model.ID = null;
            return BaseDb.Update<RoadFlow.Data.Model.BuildingsModel>(model, new KeyValuePair<string, object>("ID", id));
        }

        public int Begin(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return 0;
            }
            RoadFlow.Data.Model.BuildingsModel model = new Data.Model.BuildingsModel();
            model.State = RoadFlow.Data.Model.State.Start;
            return BaseDb.Update<RoadFlow.Data.Model.BuildingsModel>(model, new KeyValuePair<string, object>("ID", id));
        }
        /// <summary>
        /// 流程完成时的操作。
        /// </summary>
        /// <returns></returns>
        public int Complete(Guid id) {
            RoadFlow.Data.Model.BuildingsModel model = Get(id.ToString());
            #region 去除流程关联字段
            model.State = null;
            model.CreateTime = null;
            model.UpdateTime = null;
            #endregion
            BuildingsDataBLL buildingsDataBLL = new BuildingsDataBLL();
            if (buildingsDataBLL.Get(id) != null) {//楼栋已存在,更新楼栋信息
                model.ID = null;
                model.UpdateTime = DateTime.Now;
                buildingsDataBLL.Update(model, id);
            } else {
                buildingsDataBLL.Add(model);
            }
            //更新合成表
            BuildingsAndBuildingMonthInfoBLL buildingsAndBuildingMonthInfoBLL = new BuildingsAndBuildingMonthInfoBLL();
            if (buildingsAndBuildingMonthInfoBLL.Get(id)!=null)
            {
                model.ID = null;
                model.UpdateTime = DateTime.Now;
                buildingsAndBuildingMonthInfoBLL.Update(model,id);
            }
            else
            {
                buildingsAndBuildingMonthInfoBLL.Add(model);
            }

            return 1;
        }

        
        public int MangeUpdate<T>(T model, Guid id) {
            return BaseDb.Update<T>(model, new KeyValuePair<string, object>("ID", id));
        }

        public int ManageDelete(Guid id)
        {
            return BaseDb.Delete(id);
        }
    }
}
