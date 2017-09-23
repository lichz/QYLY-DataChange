using System;
using System.ComponentModel;

namespace RoadFlow.Web.Model {
    [Serializable]
    public class ActivityStatistics {
        /// <summary>
        /// 单位名称
        /// </summary>
        [DisplayName("dwname")]
        public string dwname { get; set; }
        /// <summary>
        /// 填报楼宇数
        /// </summary>
        [DisplayName("BuildingSubmitedNum")]
        public string BuildingSubmitedNum { get; set; }
        /// <summary>
        /// 未填报楼宇数
        /// </summary>
        [DisplayName("BuildingNoSubmitedNum")]
        public string BuildingNoSubmitedNum { get; set; }
        /// <summary>
        /// 填报企业数
        /// </summary>
        [DisplayName("CompanyNoSubmitedNum")]
        public string CompanyNoSubmitedNum { get; set; }
        /// <summary>
        /// 楼宇更新提交数
        /// </summary>
        [DisplayName("BuildingModifiedNum")]
        public string BuildingModifiedNum { get; set; }
        /// <summary>
        /// 楼宇更新未提交数
        /// </summary>
        [DisplayName("BuildingNoModifiedNum")]
        public string BuildingNoModifiedNum { get; set; }
        /// <summary>
        /// 街道审核数
        /// </summary>
        [DisplayName("StreetReviewedNum")]
        public string StreetReviewedNum { get; set; }
        /// <summary>
        /// 街道未审核数
        /// </summary>
        [DisplayName("StreetNoReviewedNum")]
        public string StreetNoReviewedNum { get; set; }
        /// <summary>
        /// 商务局审核数
        /// </summary>
        [DisplayName("BusinessBureauReviewedNum")]
        public string BusinessBureauReviewedNum { get; set; }
        /// <summary>
        /// 商务局未审核数
        /// </summary>
        [DisplayName("BusinessBureauNoReviewedNum")]
        public string BusinessBureauNoReviewedNum { get; set; }
        /// <summary>
        /// 诉求发布数
        /// </summary>
        [DisplayName("AppealNum")]
        public string AppealNum { get; set; }
        /// <summary>
        /// 诉求回复
        /// </summary>
        [DisplayName("ReplyNum")]
        public string ReplyNum { get; set; }
        /// <summary>
        /// 通知公告发布
        /// </summary>
        [DisplayName("AnnouncementReleasedNum")]
        public string AnnouncementReleasedNum { get; set; }
        /// <summary>
        /// 通知公告查阅
        /// </summary>
        [DisplayName("AnnouncementReadedNum")]
        public string AnnouncementReadedNum { get; set; }
        /// <summary>
        /// 拟制政策
        /// </summary>
        [DisplayName("PolicyNum")]
        public string PolicyNum { get; set; }
        /// <summary>
        /// 政策查阅
        /// </summary>
        [DisplayName("PolicyReadedNum")]
        public string PolicyReadedNum { get; set; }
        
    }
}
