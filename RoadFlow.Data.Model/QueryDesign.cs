using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace RoadFlow.Data.Model
{
      [Serializable]
   public class QueryDesign
    {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("名称")]
        public string Name { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [DisplayName("数据表名称")]
        public string TableName { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        [DisplayName("创建人ID")]
        public Guid CreateUserID { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DisplayName("创建人姓名")]
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建人姓名
        /// </summary>
        [DisplayName("数据连接")]
        public Guid ConnectionID { get; set; }

        /// <summary>
        /// 配置数据
        /// </summary>
        [DisplayName("配置数据数据")]
        public string SearchJson { get; set; }

        /// <summary>
        /// 显示字段
        /// </summary>
        [DisplayName("配置显示字段")]
        public string DisplayItem { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        [DisplayName("最后修改时间")]
        public DateTime LastModifyTime { get; set; }

        /// <summary>
        /// 状态：0 保存 1 编译 2作废
        /// </summary>
        [DisplayName("状态：0 保存 1 编译 2作废")]
        public int Status { get; set; }
    }
       [Serializable]
      public class Element
      {
        public string name { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string dictid { get; set; }
        public string sql { get; set; }
        public List<Customopt> customopts { get; set; }
      }

       [Serializable]
       public class Customopt
       {
           public string title { get; set; }
           public string value { get; set; }
       }

       [Serializable]
       public class Form
       {
           public string name { get; set; }
           public string value { get; set; }
       }

       [Serializable]
       public class ColItem
       {
           public string id { get; set; }
           public string value { get; set; }
           public bool chk { get; set; }
           public bool heji { get; set; }
           public int sortid { get; set; }
       }
}
