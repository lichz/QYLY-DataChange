using RoadFlow.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RoadFlow.Platform;

namespace RoadFlow.Platform
{
    /// <summary>
    /// 文章
    /// </summary>
    public class ArticleBLL
    {
        private static string _tableName = "Article";
        private static string _order = "[PublishTime] desc";
        IBase baseDb = RoadFlow.Data.Factory.Factory.GetBase(_tableName, _order);

        #region get
        public DataTable GetPagerData(out string pager, int size, int pageIndex, Dictionary<KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>, object> where)
        {
            where.Add(new KeyValuePair<string, RoadFlow.Data.Model.SQLFilterType>("Status", RoadFlow.Data.Model.SQLFilterType.EQUAL), RoadFlow.Data.Model.Status.Normal );
            return baseDb.GetPagerData(out pager, size, pageIndex, where);
        }
        public RoadFlow.Data.Model.ArticleModel Get(string id)
        {
            return baseDb.Get<RoadFlow.Data.Model.ArticleModel>(new KeyValuePair<string, object>("ID", id.ToGuid()));
        }
        #endregion

        #region Modify
        public int Add(RoadFlow.Data.Model.ArticleModel model)
        {
            return baseDb.Add<RoadFlow.Data.Model.ArticleModel>(model);
        }
        public int Update(RoadFlow.Data.Model.ArticleModel model, Guid? id)
        {
            return baseDb.Update<RoadFlow.Data.Model.ArticleModel>(model, new KeyValuePair<string, object>("Id", id));
        }
        #endregion
    }
}
