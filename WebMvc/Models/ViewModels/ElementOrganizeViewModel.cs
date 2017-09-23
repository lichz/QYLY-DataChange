using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace RoadFlow.Web.Model {
    public class ElementOrganizeIndexViewModel {
        /// <summary>
        /// 报送单位列表
       /// </summary>
        public DataTable List { get; set; }

        /// <summary>
        /// 拥有权限
        /// </summary>
        public Dictionary<Guid, string> Permission { get; set; }

        public string Pager { get; set; }

        
    }
}
