using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //根据需求新增的一种权限方式。某项操作和一一岗位对应
    public class ElementOrganizeModel {
        [DisplayName("ID")]
        public int? Id { get; set; }
        [DisplayName("元素ID")]
        public Guid? ElementID { get; set; }
        [DisplayName("组织ID")]
        public Guid? OrganizeID { get; set; }
        [DisplayName("操作类型")]
        public ElementOrganizeType? Type { get; set; }
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }
        [DisplayName("数据状态")]
        public Status? Status { get; set; }

    }

}
