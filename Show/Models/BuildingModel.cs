using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Show.Models
{
    [Table("BuildingsAndBuildingMonthInfo")]
    public class BuildingModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid ID { get; set; }
        /// <summary>
        /// Name
        /// </summary>
        [DisplayName("楼宇名称")]
        public string Name { get; set; }
        /// <summary>
        /// 所属楼盘
        /// </summary>
        [DisplayName("楼盘名称")]
        public Guid? HouseID { get; set; }

        [ForeignKey("HouseID")]
        public DictionaryModel HouseIDS { get; set; }

        /// <summary>
        /// 所属街道
        /// </summary>
        [DisplayName("所属街道")]
        public Guid? SSJD { get; set; }

        [ForeignKey("SSJD")]
        public DictionaryModel SSJDS { get; set; }
        /// <summary>
        /// 建设阶段
        /// </summary>
        [DisplayName("建设阶段")]
        public Guid? JSJD { get; set; }
        [ForeignKey("JSJD")]
        public DictionaryModel jsjd;
        /// <summary>
        /// 楼宇级别
        /// </summary>
        [DisplayName("楼宇级别")]
        public Guid? LYJB { get; set; }
        [ForeignKey("LYJB")]
        public DictionaryModel lyjb;
        /// <summary>
        /// 楼宇类型
        /// </summary>
        [DisplayName("楼宇类型")]
        public Guid? LYLX { get; set; }
        [ForeignKey("LYLX")]
        public DictionaryModel lylx;
        /// <summary>
        /// 统筹招商
        /// </summary>
        [DisplayName("统筹招商")]
        public Guid? TCZS { get; set; }
        [ForeignKey("TCZS")]
        public DictionaryModel tczs;
        /// <summary>
        /// 竣工时间
        /// </summary>
        [DisplayName("竣工时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? JGSJ { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        [DisplayName("发布时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 楼宇详细地址
        /// </summary>
        [DisplayName("楼宇详细地址")]
        public string LYXXDZ { get; set; }
        /// <summary>
        /// 楼宇管理运营方
        /// </summary>
        [DisplayName("楼宇管理运营方")]
        public string LYGLYYF { get; set; }
        /// <summary>
        /// 总建筑面积（m2）
        /// </summary>
        [DisplayName("总建筑面积（m2）")]
        public decimal? ZJZMJ { get; set; }
        /// <summary>
        /// 商业总面积
        /// </summary>
        [DisplayName("商业总面积")]
        public decimal? SY_ZMJ { get; set; }
        /// <summary>
        /// 商务总面积
        /// </summary>
        [DisplayName("商务总面积")]
        public decimal? SW_ZMJ { get; set; }
        /// <summary>
        /// 商业空置总面积
        /// </summary>
        [DisplayName("商业空置面积")]
        public decimal? SY_KZ_ZMJ { get; set; }
        /// <summary>
        /// 商务空置总面积
        /// </summary>
        [DisplayName("商务空置面积")]
        public decimal? SW_KZ_ZMJ { get; set; }
        /// <summary>
        /// 商业销售均价
        /// </summary>
        [DisplayName("商业销售均价")]
        public decimal? SY_XSJJ { get; set; }
        /// <summary>
        /// 商务销售均价
        /// </summary>
        [DisplayName("商务销售均价")]
        public decimal? SW_XSJJ { get; set; }
        /// <summary>
        /// 物管费(商业)
        /// </summary>
        [DisplayName("物管费(商业)")]
        public decimal? SY_WGF { get; set; }
        /// <summary>
        /// 物管费(商务)
        /// </summary>
        [DisplayName("物管费(商务)")]
        public decimal? SW_WGF { get; set; }
        /// <summary>
        /// 商业租金
        /// </summary>
        [DisplayName("商业租金")]
        public Guid? SY_ZJ { get; set; }


        [ForeignKey("SY_ZJ")]
        public DictionaryModel SYZJ { get; set; }

        /// <summary>
        /// 商务租金
        /// </summary>
        [DisplayName("商务租金")]
        public Guid? SW_ZJ { get; set; }
        [ForeignKey("SW_ZJ")]
        public DictionaryModel SWZJ { get; set; }
        /// <summary>
        /// 效果图
        /// </summary>
        [DisplayName("效果图")]
        public string XGT { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("Status")]
        public Status Status { get; set; }
        /// <summary>
        /// 招商电话
        /// </summary>
        [DisplayName("招商电话")]
        public string ChinaMerchantsTel { get; set; }
        /// <summary>
        /// 地图坐标
        /// </summary>
        [DisplayName("地图坐标")]
        public string BDDW { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// 是否重点楼宇
        /// </summary>
        [DisplayName("是否重点楼宇")]
        public Guid? IsImportant { get; set; }
    }
}