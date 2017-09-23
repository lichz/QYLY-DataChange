using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //楼宇每月信息
    public class BuildingMonthInfoModel {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid? ID { get; set; }
        /// <summary>
        /// 商业已使用面积
        /// </summary>
        [DisplayName("商业已使用面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SY_YSY_ZMJ { get; set; }
        /// <summary>
        /// 商业自用面积
        /// </summary>
        [DisplayName("商业自用面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SY_YSY_ZYMJ { get; set; }
        /// <summary>
        /// 商业已使用面积
        /// </summary>
        [DisplayName("商业空置总面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SY_KZ_ZMJ { get; set; }
        /// <summary>
        /// 商业空置可租售面积
        /// </summary>
        [DisplayName("商业空置可租赁面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SY_KZ_KZLMJ { get; set; }
        /// <summary>
        /// 商业已使用面积
        /// </summary>
        [DisplayName("商业空置可销售面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SY_KZ_KXSMJ { get; set; }
        /// <summary>
        /// 商业单层最大空置面积
        /// </summary>
        [DisplayName("商业单层最大空置面积")]
        public decimal? SY_KZ_DCZDZMJ { get; set; }
        /// <summary>
        /// 商业租金
        /// </summary>
        [DisplayName("商业租金")]
        [Required]
        public Guid? SY_ZJ { get; set; }
        /// <summary>
        /// 商业销售均价
        /// </summary>
        [DisplayName("商业销售均价")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SY_XSJJ { get; set; }
        /// <summary>
        /// 商务已使用面积
        /// </summary>
        [DisplayName("商务已使用面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SW_YSY_ZMJ { get; set; }
        /// <summary>
        /// 商务自用面积
        /// </summary>
        [DisplayName("商务自用面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SW_YSY_ZYMJ { get; set; }
        /// <summary>
        /// 商务已使用面积
        /// </summary>
        [DisplayName("商务空置总面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SW_KZ_ZMJ { get; set; }
        /// <summary>
        /// 商务空置可租售面积
        /// </summary>
        [DisplayName("商务空置可租赁面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SW_KZ_KZLMJ { get; set; }
        /// <summary>
        /// 商务已使用面积
        /// </summary>
        [DisplayName("商务空置可销售面积")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SW_KZ_KXSMJ { get; set; }
        /// <summary>
        /// 商务单层最大空置面积
        /// </summary>
        [DisplayName("商务单层最大空置面积")]
        public decimal? SW_KZ_DCZDZMJ { get; set; }
        /// <summary>
        /// 商务租金
        /// </summary>
        [DisplayName("商务租金")]
        [Required]
        public Guid? SW_ZJ { get; set; }
        /// <summary>
        /// 商务销售均价
        /// </summary>
        [DisplayName("商务销售均价")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public decimal? SW_XSJJ { get; set; }
        /// <summary>
        /// 时间区间
        /// </summary>
        [DisplayName("时间区间")]
        public int? TimeArea { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        [DisplayName("数据状态")]
        public Status? Status { get; set; }

        /// <summary>
        /// 流程状态
        /// </summary>
        [DisplayName("流程状态")]
        public State? State { get; set; }

        /// <summary>
        /// 报送时间
        /// </summary>
        [DisplayName("添加时间")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? UpdateTime { get; set; }

        //楼宇ID
        public Guid? BuildingID { get; set; }

        public Guid? BuildingMonthInfoID { get; set; }

        /// <summary>
        /// 流程任务显示名称
        /// </summary>
        public string MissionDisplay { get; set; }
    }
}
