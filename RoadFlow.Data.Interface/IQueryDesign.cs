using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadFlow.Data.Interface
{
    public interface IQueryDesign
    {
        /// <summary>
        /// 新增
        /// </summary>
        int Add(RoadFlow.Data.Model.QueryDesign model);

        /// <summary>
        /// 更新
        /// </summary>
        int Update(RoadFlow.Data.Model.QueryDesign model);
        int Delete(Guid id);

        /// <summary>
        /// 查询所有记录
        /// </summary>
        List<RoadFlow.Data.Model.QueryDesign> GetAll();

        /// <summary>
        /// 查询单条记录
        /// </summary>
        Model.QueryDesign Get(Guid id);

        /// <summary>
        /// 根据配置名称查询单条记录
        /// </summary>
        Model.QueryDesign Get(string name,Guid ryid);
    }
}
