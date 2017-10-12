using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RoadFlow.Web.Model
{
    public class ArticleIndexViewModel
    {
        public RoadFlow.Data.Model.ArticleModel Model { get; set; }
        public List<RoadFlow.Data.Model.DictionaryModel> ArticleTypes { get; set; }
    }

    public class ArticleManageIndexViewModel
    {
        public DataTable Articles { get; set; }
        public List<RoadFlow.Data.Model.DictionaryModel> ArticleTypes { get; set; }
        /// <summary>
        /// 翻页Html
        /// </summary>
        public string Pager { get; set; }
        /// <summary>
        /// 标题关键字搜索内容
        /// </summary>
        public string Title { get; set; }
    }


    public class ArticleManageEditViewModel
    {
        public RoadFlow.Data.Model.ArticleModel ArticleModel { get; set; }
        public List<RoadFlow.Data.Model.DictionaryModel> ArticleTypes { get; set; }
        /// <summary>
        /// 更新失败执行的Script文件
        /// </summary>
        public string Script { get; set; }
    }
}