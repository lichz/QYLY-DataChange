using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoadFlow.Utility;
using RoadFlow.Data.Model;

namespace RoadFlow.Platform
{
    public  class QueryDesign
    {
        private RoadFlow.Data.Interface.IQueryDesign queryDesign;
        public QueryDesign()
        {
            this.queryDesign = Data.Factory.Factory.GetQueryDesign();
        }

        /// <summary>
        /// 新增
        /// </summary>
        public int Add(RoadFlow.Data.Model.QueryDesign model)
        {
            return queryDesign.Add(model);
        }

        /// <summary>
        /// 更新
        /// </summary>
        public int Update(RoadFlow.Data.Model.QueryDesign model)
        {
            return queryDesign.Update(model);
        }
        public int Delete(Guid id)
        {
            return queryDesign.Delete(id);
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        public List<RoadFlow.Data.Model.QueryDesign> GetAll()
        {
            return queryDesign.GetAll();
        }

        /// <summary>
        /// 查询单条记录
        /// </summary>
        public RoadFlow.Data.Model.QueryDesign Get(Guid id)
        {
            return queryDesign.Get(id);
        }
        /// <summary>
        /// 根据配置名称查询记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public RoadFlow.Data.Model.QueryDesign Get(string name,Guid ryid)
        {
            return queryDesign.Get(name,ryid);
        }

        public List<ColItem> GetColItemList(string name) {
            RoadFlow.Data.Model.QueryDesign Display = new RoadFlow.Platform.QueryDesign().Get(name, RoadFlow.Platform.Users.CurrentUserID);
            List<ColItem> lst = Display.DisplayItem.IsNullOrEmpty() == true ? null : Display.DisplayItem.JsonConvertModel<List<ColItem>>();

            if (!lst.IsNullObj()) {
                //删除没有标题的字段项目
                lst.RemoveAll(x => x.value.IsNullOrEmpty());
                lst.OrderBy(p=>p.sortid);
            }
            return lst;
        }
    }
}
