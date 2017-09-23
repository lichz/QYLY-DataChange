using System.ComponentModel;

namespace Show.Models {

    /// <summary>
    /// 数据状态
    /// </summary>
    public enum Status {
        [Description("正常")]
        Normal = 0,
        [Description("删除")]
        Deleted = 255,
    }
    public enum IsImportant {
        [Description("重点楼宇")]
        Important = 0,
        [Description("其他楼宇")]
        Other = 255,
    }
}
