using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //楼宇每月信息更新数及打分
    public class BuildingMonthModifyCountModel {
        /// <summary>
        /// ID
        /// </summary>
        public Guid? ID { get; set; }
        

        /// <summary>
        /// 数据状态
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// 报送时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        //楼宇ID
        public Guid? BuildingID { get; set; }

        [DisplayName("报送单位")]
        public string BuildingName { get; set; }
        /// <summary>
        /// 时间区间
        /// </summary>
        [DisplayName("时间区间")]
        public int? TimeArea { get; set; }
        /// <summary>
        /// 楼栋每月面积更新数
        /// </summary>
        [DisplayName("每月楼栋基本信息更新")]
        public int? Count { get; set; }
        /// <summary>
        /// 楼栋每月企业更新数
        /// </summary>
        [DisplayName("入驻企业变更")]
        public int? EnterpriseModifyCount { get; set; }

        /// <summary>
        /// 分数
        /// </summary>
        [DisplayName("评分")]
        public decimal? Score { get; set; }

        /// <summary>
        /// 及时性
        /// </summary>
        public decimal? Timeliness { get; set; }
        /// <summary>
        /// 质量
        /// </summary>
        public decimal? Quality { get; set; }
        /// <summary>
        /// 准确率
        /// </summary>
        public decimal? Accuracy { get; set; }
        /// <summary>
        /// 手动打分
        /// </summary>
        public decimal? Manual { get; set; }


        public Guid? HouseID { get; set; }
    }

}
