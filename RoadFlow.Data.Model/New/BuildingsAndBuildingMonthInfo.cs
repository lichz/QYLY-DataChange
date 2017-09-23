using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    /// <summary>
    /// 两张表的一个合成（将building表和对应的最新填报的BuildingMonthInfo数据合成）
    /// </summary>
    public class BuildingsAndBuildingMonthInfoModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid? ID { get; set; }

        public string Num { get; set; }

        /// <summary>
        /// 楼栋栋数
        /// </summary>
        public string LYDS { get; set; }

        /// <summary>
        /// 楼宇级别
        /// </summary>
        [DisplayName("楼宇级别")]
        public Guid? LYJB { get; set; }

        /// <summary>
        /// 楼宇级别(对应显示名称)
        /// </summary>
        public string LYJBName { get; set; }

        [DisplayName("详细地址")]
        public string LYXXDZ { get; set; }

        /// <summary>
        /// 所属街道
        /// </summary>
        [DisplayName("所属街道")]
        public Guid? SSJD { get; set; }

        /// <summary>
        /// 所属街道(对应显示名称)
        /// </summary>
        public string SSJDName { get; set; }

        [DisplayName("建设阶段")]
        public Guid? JSJD { get; set; }

        /// <summary>
        /// 建设阶段(对应显示名称)
        /// </summary>
        public string JSJDName { get; set; }

        [DisplayName("竣工时间")]
        public DateTime? JGSJ { get; set; }

        [DisplayName("楼宇类型")]
        public Guid? LYLX { get; set; }

        /// <summary>
        /// 楼宇类型(对应显示名称)
        /// </summary>
        public string LYLXName { get; set; }

        [DisplayName("总建筑面积")]
        public decimal? ZJZMJ { get; set; }

        [DisplayName("商业总面积")]
        public decimal? SY_ZMJ { get; set; }

        [DisplayName("商务总面积")]
        public decimal? SW_ZMJ { get; set; }

        [DisplayName("商业物管费")]
        public decimal? SY_WGF { get; set; }

        [DisplayName("商务物管费")]
        public decimal? SW_WGF { get; set; }

        [DisplayName("楼宇产权情况")]
        public Guid? LYCQQK { get; set; }

        /// <summary>
        /// 楼宇产权情况（对应显示名称）
        /// </summary>
        public string LYCQQKName { get; set; }

        [DisplayName("管理运营方")]
        public string LYGLYYF { get; set; }

        [DisplayName("主要业主")]
        public string Owner { get; set; }

        [DisplayName("自持产权面积（㎡）")]
        public decimal? ZCCQMJ { get; set; }

        [DisplayName("车位数")]
        public int? CWS { get; set; }

        [DisplayName("电梯（部/每栋楼）")]
        public int? DTS { get; set; }

        [DisplayName("中央空调")]
        public Guid? ZYKT { get; set; }

        /// <summary>
        /// 中央空调（对应显示名称）
        /// </summary>
        public string ZYKTName { get; set; }

        [DisplayName("招商方向")]
        public string ZSFX { get; set; }

        [DisplayName("入驻优惠")]
        public string RZYH { get; set; }

        [DisplayName("百度地图坐标")]
        public string BDDW { get; set; }

        [DisplayName("统筹招商")]
        public Guid? TCZS { get; set; }

        /// <summary>
        /// 统筹招商（对应显示名称）
        /// </summary>
        public string TCZSName { get; set; }

        [DisplayName("效果图、现状图")]
        public string XGT { get; set; }

        [DisplayName("备注")]
        public string Note { get; set; }

        [DisplayName("楼层总数")]
        public int? NumberOfFloors { get; set; }

        public int? GroundFloorNumber { get; set; }

        public int? UndergroundFloorNumber { get; set; }

        public decimal? MonolayerArea { get; set; }

        public decimal? CeilingHeight { get; set; }

        public string PromotionForm { get; set; }

        public string BuildingSystem { get; set; }

        public string AirConditionAndFreshAirSystem { get; set; }

        public string Traffic { get; set; }

        public string PropertyQualifications { get; set; }

        public string PropertyAsALegalPerson { get; set; }

        public string ChinaMerchantsTel { get; set; }

        public int? GroundFloorParkingSpace { get; set; }

        public int? UndergroundFloorParkingSpace { get; set; }

        public string MonthParking { get; set; }

        public string HourParking { get; set; }

        /// <summary>
        /// 楼盘ID
        /// </summary>
        [DisplayName("楼盘ID")]
        public Guid? HouseID { get; set; }

        /// <summary>
        /// 楼盘名称
        /// </summary>
        [DisplayName("楼盘名称")]
        public string HouseName { get; set; }

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
        public Status? Status { get; set; }

        /// <summary>
        /// 楼宇名称
        /// </summary>
        [DisplayName("楼宇名称")]
        public string Name { get; set; }



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
        public decimal? SY_KZ_KXSMJ { get; set; }
        /// <summary>
        /// 商业单层空置最大面积
        /// </summary>
        [DisplayName("商业单层空置最大面积")]
        [Required]
        public decimal? SY_KZ_DCZDZMJ { get; set; }
        
        /// <summary>
        /// 商业租金
        /// </summary>
        [DisplayName("商业租金")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public Guid? SY_ZJ { get; set; }

        /// <summary>
        /// 商业租金(对应显示名称)
        /// </summary>
        public string SY_ZJName { get; set; }

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
        /// 商务单层空置最大面积
        /// </summary>
        [DisplayName("商务单层空置最大面积")]
        [Required]
        public decimal? SW_KZ_DCZDZMJ { get; set; }
        /// <summary>
        /// 商务租金
        /// </summary>
        [DisplayName("商务租金")]
        [Required]
        [RegularExpression("^[0-9]+(.[0-9]{1,2})?$", ErrorMessage = "请输入两位小数正实数。")]
        public Guid? SW_ZJ { get; set; }

        /// <summary>
        /// 商务租金(对应显示名称)
        /// </summary>
        public string SW_ZJName { get; set; }

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
        /// 重点楼宇
        /// </summary>
        [DisplayName("重点楼宇")]
        public Guid? IsImportant { get; set; }
    }
}