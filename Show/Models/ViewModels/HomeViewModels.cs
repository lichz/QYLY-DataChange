using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Show.Models
{
    public class HomeIndexViewModel
    {
        #region 列表
        /// <summary>
        /// 新闻资讯
        /// </summary>
        public IQueryable<ArticleModel> News;
        /// <summary>
        /// 通知公告
        /// </summary>
        public IQueryable<ArticleModel> Notice;
        /// <summary>
        /// 楼宇政策
        /// </summary>
        public IQueryable<ArticleModel> Policy;
        /// <summary>
        /// 楼宇显示
        /// </summary>
        public IQueryable<BuildingModel> BuildingShow;
        /// <summary>
        /// 楼宇
        /// </summary>
        public IQueryable<BuildingModel> Buildings;
        #endregion
    }

    public class HomeMapViewModel {
        #region 列表
        /// <summary>
        /// 所有街道
        /// </summary>
        public IQueryable<DictionaryModel> Street;
        #endregion
    }
}