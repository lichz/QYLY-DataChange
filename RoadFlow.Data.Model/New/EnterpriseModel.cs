using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //楼栋企业
    public class EnterpriseModel {
        /// <summary>
        /// ID
        /// </summary>
        [Display(Name="ID")]
        public Guid? ID { get; set; }
        /// <summary>
        /// BuildingID
        /// </summary>
        [Display(Name = "BuildingID")]
        public Guid? BuildingID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [Display(Name="企业名称")]
        public string Name { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        [Display(Name="统一社会信用代码")]
        public string TYSHXYDM { get; set; }
        /// <summary>
        /// 税收解缴地
        /// </summary>
        [Display(Name = "税收解缴地")]
        public string CSJJD { get; set; }
        /// <summary>
        /// 企业类型
        /// </summary>
        [Display(Name="企业类型")]
        public Guid? Type { get; set; }
        /// <summary>
        /// 注册地
        /// </summary>
        [Display(Name="注册地")]
        public string ZCD { get; set; }
        /// <summary>
        /// 入驻总面积
        /// </summary>
        [Display(Name = "入驻总面积")]
        public decimal? InTotalArea { get; set; }
        /// <summary>
        /// 租用面积
        /// </summary>
        [Display(Name = "租用面积")]
        public decimal? RentArea { get; set; }
        /// <summary>
        /// 自购面积
        /// </summary>
        [Display(Name = "自购面积")]
        public decimal? PersonalUseArea { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间（BuildingData使用）
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [Display(Name = "数据状态")]
        public Status? Status { get; set; }
        /// <summary>
        /// 流程状态
        /// </summary>
        [Display(Name = "流程状态")]
        public State? State { get; set; }
    }
}
