using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Linq;
using System;
using System.Web.Mvc;

namespace Show.Models
{
    public class TestIndexViewModel
    {

        #region 查询参数
        public string Titles { get; set; }
        public string Type { get; set; }
        #endregion

        public SelectList Types { get; set; }
        public IQueryable<ArticleModel> List { get; set; }
        public ArticleModel Display { get {
            return new ArticleModel();
        }  }

        public string Pager { get; set; }
    }
}