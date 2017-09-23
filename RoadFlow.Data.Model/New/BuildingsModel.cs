using System;
using System.ComponentModel;

namespace RoadFlow.Data.Model {

    public class BuildingsModel {
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

        [DisplayName("详细地址")]
        public string LYXXDZ { get; set; }

        /// <summary>
        /// 所属街道
        /// </summary>
        [DisplayName("所属街道")]
        public Guid? SSJD { get; set; }

        public DictionaryModel SSJDModel { get; set; }

        [DisplayName("建设阶段")]
        public Guid? JSJD { get; set; }

        [DisplayName("竣工时间")]
        public DateTime? JGSJ { get; set; }

        [DisplayName("楼宇类型")]
        public Guid? LYLX { get; set; }

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

        [DisplayName("招商方向")]
        public string ZSFX { get; set; }

        [DisplayName("入驻优惠")]
        public string RZYH { get; set; }

        [DisplayName("百度地图坐标")]
        public string BDDW { get; set; }

        [DisplayName("统筹招商")]
        public Guid? TCZS { get; set; }

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
        /// 楼盘
        /// </summary>
        [DisplayName("楼盘")]
        public DictionaryModel HouseModel { get; set; }

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
        /// 重点楼宇
        /// </summary>
        [DisplayName("重点楼宇")]
        public Guid? IsImportant { get; set; }

        /// <summary>
        /// 流程状态
        /// </summary>
        public State? State { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// 楼宇名称
        /// </summary>
        [DisplayName("楼宇名称")]
        public string Name { get; set; }

    }

}
