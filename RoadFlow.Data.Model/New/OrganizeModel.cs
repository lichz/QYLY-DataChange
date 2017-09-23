using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //组织结构
    public class OrganizeModel {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid? ID { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [DisplayName("上级ID")]
        public Guid? ParentID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称")]
        public string Name { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [DisplayName("层级")]
        public int? Type { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [DisplayName("排序")]
        public int? Sort { get; set; }
    }
}
