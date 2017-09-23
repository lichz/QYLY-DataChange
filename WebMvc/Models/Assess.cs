using System;
using System.ComponentModel;

namespace RoadFlow.Web.Model {
    [Serializable]
    public class Assess {
        /// <summary>
        /// 报送单位
        /// </summary>
        [DisplayName("报送单位")]
        public string Name { get; set; }
        /// <summary>
        /// 填报工作量得分
        /// </summary>
        [DisplayName("填报工作量得分")]
        public decimal Count { get; set; }
        /// <summary>
        /// 填报质量得分
        /// </summary>
        [DisplayName("填报质量得分")]
        public decimal Quality { get; set; }
        /// <summary>
        /// 更新频次得分
        /// </summary>
        [DisplayName("更新频次得分")]
        public decimal Frequency { get; set; }
        /// <summary>
        /// 得分排名
        /// </summary>
        [DisplayName("得分排名")]
        public decimal Score { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string More { get; set; }
        
        
    }
}
