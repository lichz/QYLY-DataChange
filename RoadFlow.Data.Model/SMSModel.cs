using System;
using System.Dynamic;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    /// <summary>
    /// 短信模型
    /// </summary>
    public class SMSModel {
        public Guid Id { get; set; }
        /// <summary>
        /// 短信内容
        /// </summary>
        [Required(ErrorMessage = "短信内容不能为空")]
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 发送时间(保留字段)
        /// </summary>
        public DateTime? SendTime { get; set; }
        /// <summary>
        /// 发送者
        /// </summary>
        public Guid SendUser { get; set; }
        /// <summary>
        /// 发送者昵称
        /// </summary>
        public string SendUserName { get; set; }
        /// <summary>
        /// 发送到（,隔开的UserId）
        /// </summary>
        [Required(ErrorMessage = "您还没选择要发给谁。")]
        public string SendTo { get; set; }
        /// <summary>
        /// 状态（是否删除）
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 是否发送（1是已发送，0是未发送）保留字段
        /// </summary>
        public int IsSended { get; set; }
    }
}