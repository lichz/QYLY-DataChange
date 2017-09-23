using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //企业变更记录(入驻、搬出。)
    public class EnterpriseUpdateRecordModel {
        /// <summary>
        /// ID
        /// </summary>
        [DisplayName("ID")]
        public int? ID { get; set; }
        /// <summary>
        /// 企业ID
        /// </summary>
        [DisplayName("EnterpriseID")]
        public Guid? EnterpriseID { get; set; }
        /// <summary>
        /// 记录类型
        /// </summary>
        [DisplayName("记录类型")]
        public EnterpriseUpdateRecordType? Type { get; set; }
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

    }
}
