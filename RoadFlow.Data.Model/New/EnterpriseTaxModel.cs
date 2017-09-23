using System;
using System.ComponentModel;

namespace RoadFlow.Data.Model {

    //企业税收
    public class EnterpriseTaxModel {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public Guid? ID { get; set; }

        /// <summary>
        /// 企业ID
        /// </summary>
        [DisplayName("企业ID")]
        public Guid? EnterpriseID { get; set; }

        public RoadFlow.Data.Model.EnterpriseAndEnterpriseTaxModel EnterpriseModel { get; set; }

        /// <summary>
        /// 国税
        /// </summary>
        [DisplayName("国税")]
        public decimal? NationalTax { get; set; }

        /// <summary>
        /// 地税
        /// </summary>
        [DisplayName("地税")]
        public decimal? LandTax { get; set; }

        /// <summary>
        /// 税收
        /// </summary>
        [DisplayName("税收")]
        public decimal? Tax { get; set; }

        /// <summary>
        /// 税收时间段（如2016或者201601，前者表示全年数据，后者表示月份）
        /// </summary>
        [DisplayName("税收时间段")]
        public int? TaxArea { get; set; }

        /// <summary>
        /// 数据状态
        /// </summary>
        [DisplayName("数据状态")]
        public Status? Status { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        [DisplayName("更新时间")]
        public DateTime? UpdateTime { get; set; }

    }
}
