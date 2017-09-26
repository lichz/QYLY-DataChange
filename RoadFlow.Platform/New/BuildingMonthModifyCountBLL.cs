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
    /// 楼宇每月信息更新数。
    /// </summary>
    public class BuildingMonthModifyCountBLL {
        public IBase BaseDb { get; private set; }
        public BuildingMonthModifyCountBLL() 
        { 
            BaseDb = RoadFlow.Data.Factory.Factory.GetBase("V_BuildingMonthModifyCount_Building","BuildingMonthModifyCount", "[HouseID],[TimeArea],[Timeliness]+[Quality]+[Accuracy] desc,[UpdateTime] desc");
        }


        #region get
        public DataTable GetPagerData(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetPagerData(out pager, size, pageIndex, where);
        }

        public DataTable GetAll(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
            return BaseDb.GetAll(0,where);
        }

        /// <summary>
        /// 按楼盘统计(因为默认有时间段筛选，所以没做时间段相关的排序)
        /// </summary>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.BuildingMonthModifyCountModel> GetPagerDataHouse(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            List<RoadFlow.Data.Model.BuildingMonthModifyCountModel> result = new List<Data.Model.BuildingMonthModifyCountModel>();
            var all = GetAll(where).ToList<RoadFlow.Data.Model.BuildingMonthModifyCountModel>().OrderBy(p=>p.HouseID).OrderBy(p=>p.TimeArea);


            //循环计算楼盘企业和每月更新数量
            Dictionary<Guid,string> houseCount = new Dictionary<Guid,string>();
            int count = 0;
            int enterpriseCount = 0;
            int timeArea = 0;
            Guid houseID = Guid.NewGuid();
            Guid first = houseID;
            foreach(var item in all)
            {
                if(houseID!=item.HouseID)
                {
                    if(houseID!=first)
                    {
                        houseCount.Add(houseID,count+","+enterpriseCount+","+item.TimeArea);
                        count = 0;
                        enterpriseCount = 0;
                    }
                    houseID = item.HouseID.Value;
                }
                count += item.Count.Value;
                enterpriseCount += item.EnterpriseModifyCount.Value;
                timeArea = item.TimeArea.Value;
            }
            if(first!=houseID)
            {
                houseCount.Add(houseID, count + "," + enterpriseCount + "," + timeArea);//添加最后一项
            }
            

            var houseBLL = new RoadFlow.Platform.DictionaryBLL();
            foreach(var item in houseCount)
            {
                var model = new RoadFlow.Data.Model.BuildingMonthModifyCountModel();
                model.BuildingName =  houseBLL.GetByID(item.Key).Title;//这里显示的其实是楼盘名字
                model.Count = int.Parse(item.Value.Split(',')[0]);
                model.EnterpriseModifyCount =int.Parse(item.Value.Split(',')[1]);
                model.TimeArea = int.Parse(item.Value.Split(',')[2]);
                model.Timeliness = 40;
                model.Accuracy = 20;
                model.Quality = RoadFlow.Platform.BLLCommon.GetQuality(model.Count.Value+model.EnterpriseModifyCount.Value);
                model.Score = model.Manual==null? model.Timeliness + model.Accuracy + model.Quality:model.Manual;
                result.Add(model);
            }
            
            //分页
            int allCount = result.Count;
            pager = RoadFlow.Utility.New.Tools.GetPagerHtml(allCount, size, pageIndex);  //生成HTML的分页

            return result.OrderByDescending(p => p.Count + p.EnterpriseModifyCount).Skip((pageIndex - 1) * size).Take(size).ToList();
        }

        /// <summary>
        /// 按楼盘统计(因为默认有时间段筛选，所以没做时间段相关的排序)
        /// </summary>
        /// <returns></returns>
        public List<RoadFlow.Data.Model.BuildingMonthModifyCountModel> GetALLOnHouse(Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            List<RoadFlow.Data.Model.BuildingMonthModifyCountModel> result = new List<Data.Model.BuildingMonthModifyCountModel>();
            var all = GetAll(where).ToList<RoadFlow.Data.Model.BuildingMonthModifyCountModel>().OrderBy(p => p.HouseID).OrderBy(p => p.TimeArea);


            //循环计算楼盘企业和每月更新数量
            Dictionary<Guid, string> houseCount = new Dictionary<Guid, string>();
            int count = 0;
            int enterpriseCount = 0;
            int timeArea = 0;
            Guid houseID = Guid.NewGuid();
            Guid first = houseID;
            foreach (var item in all)
            {
                if (houseID != item.HouseID)
                {
                    if (houseID != first)
                    {
                        houseCount.Add(houseID, count + "," + enterpriseCount + "," + item.TimeArea);
                        count = 0;
                        enterpriseCount = 0;
                    }
                    houseID = item.HouseID.Value;
                }
                count += item.Count.Value;
                enterpriseCount += item.EnterpriseModifyCount.Value;
                timeArea = item.TimeArea.Value;
            }
            if (first != houseID)
            {
                houseCount.Add(houseID, count + "," + enterpriseCount + "," + timeArea);//添加最后一项
            }


            var houseBLL = new RoadFlow.Platform.DictionaryBLL();
            foreach (var item in houseCount)
            {
                var model = new RoadFlow.Data.Model.BuildingMonthModifyCountModel();
                model.BuildingName = houseBLL.GetByID(item.Key).Title;//这里显示的其实是楼盘名字
                model.Count = int.Parse(item.Value.Split(',')[0]);
                model.EnterpriseModifyCount = int.Parse(item.Value.Split(',')[1]);
                model.TimeArea = int.Parse(item.Value.Split(',')[2]);
                model.Timeliness = 40;
                model.Accuracy = 20;
                model.Quality = RoadFlow.Platform.BLLCommon.GetQuality(model.Count.Value + model.EnterpriseModifyCount.Value);
                model.Score = model.Manual == null ? model.Timeliness + model.Accuracy + model.Quality : model.Manual;
                result.Add(model);
            }

            return result.OrderByDescending(p => p.Count + p.EnterpriseModifyCount).ToList();
        }


        //public List<RoadFlow.Data.Model.BuildingMonthModifyCountModel> GetAllByTimeArea(int timeArea, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where) {
        //    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("TimeArea", RoadFlow.Data.Model.SQLFilterType.EQUAL), timeArea);
        //    where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal);
        //    return baseDb.GetAll(0, where).ToList <RoadFlow.Data.Model.BuildingMonthModifyCountModel>();
        //}

        public RoadFlow.Data.Model.BuildingMonthModifyCountModel Get(Guid id) {
            return BaseDb.Get<RoadFlow.Data.Model.BuildingMonthModifyCountModel>(new KeyValuePair<string, object>("ID", id));
        }

        public RoadFlow.Data.Model.BuildingMonthModifyCountModel Get(Guid buildingID, int timeArea) {
            return BaseDb.Get<RoadFlow.Data.Model.BuildingMonthModifyCountModel>(new KeyValuePair<string, object>("BuildingID", buildingID), new KeyValuePair<string, object>("TimeArea", timeArea));
        }
        #endregion

        #region Modify
        public int Add(RoadFlow.Data.Model.BuildingMonthModifyCountModel model) {
            model.ID = Guid.NewGuid();
            return BaseDb.Add<RoadFlow.Data.Model.BuildingMonthModifyCountModel>(model);
        }

        public int Update(RoadFlow.Data.Model.BuildingMonthModifyCountModel model, Guid id) {
            model.UpdateTime = DateTime.Now;
            return BaseDb.Update<RoadFlow.Data.Model.BuildingMonthModifyCountModel>(model, new KeyValuePair<string, object>("ID", id));
        }

        public int ManageDeleteByBuildingID(Guid buildingID)
        {
            return BaseDb.DeleteByPara(new { buildingID });
        }
        #endregion

    }
}
