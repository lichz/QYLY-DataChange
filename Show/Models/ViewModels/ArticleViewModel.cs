using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using System.Linq;

namespace Show.Models
{
    public class ArticleIndexViewModel
    {
        #region 查询参数
        public string Titles { get; set; }
        public string Type { get; set; }
        #endregion

        public SelectList Types { get; set; }
        public IQueryable<ArticleModel> List { get; set; }
        public ArticleModel Display
        {
            get
            {
                return new ArticleModel();
            }
        }

        public string Pager { get; set; }
    }
    public class SpecialQYIndexViewModel
    {
        public IQueryable<DictionaryModel> ArticleType;
        /// <summary>
        /// 文章内容
        /// </summary>
        public string Content;
        /// <summary>
        /// 文章类型
        /// </summary>
        public string TypeName { get; set; }
    }
    public class BuildingInformationViewModel
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName;
        /// <summary>
        /// 文章类型
        /// </summary>
        public IQueryable<DictionaryModel> ArticleType;
        /// <summary>
        /// 第一篇文章列表(默认)
        /// </summary>
        public IQueryable<ArticleModel> FirstArticles;
        /// <summary>
        /// 文章列表(根据TypeID查找)
        /// </summary>
        public IQueryable<ArticleModel> Articles;
        /// <summary>
        /// 空值
        /// </summary>
        public bool IsNull;
        /// <summary>
        /// 高亮
        /// </summary>
        public bool IsHighLight;
        /// <summary>
        /// 分页
        /// </summary>
        public string Pager;
    }
    public class BuildingInformationDetailViewModel
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        public string TypeName;
        /// <summary>
        /// 文章类型
        /// </summary>
        public IQueryable<DictionaryModel> ArticleType;
        /// <summary>
        /// 文章对象
        /// </summary>
        public ArticleModel Article;

    }
}