using System.ComponentModel;

namespace RoadFlow.Data.Model {
    // 
    // 全局枚举

    /// <summary>
    /// 数据库过滤字段类型
    /// </summary>
    public enum SQLFilterType {
        [Description("模糊查询")]
        CHARINDEX = 0,
        [Description("相等")]
        EQUAL = 1,
        [Description("如@Name<=[Name]")]
        MIN = 2,
        [Description("如[Name]<=@Name")]
        MAX = 3,
        [Description("如[Name]<@Name")]
        MAXNotEqual = 4,
        [Description("如@Name<[Name]")]
        MINNotEqual = 5,
        [Description("In")]
        IN = 6,
    }

    /// <summary>
    /// 数据状态
    /// </summary>
    public enum Status {
        [Description("正常")]
        Normal = 0,
        [Description("删除")]
        Deleted = 255,
    }

    /// <summary>
    /// 流程状态
    /// </summary>
    public enum State {
        [Description("开始")]
        Start = 0,
        [Description("完成")]
        Finish = 1,
    }

    /// <summary>
    /// 企业变更状态
    /// </summary>
    public enum EnterpriseUpdateRecordType {
        [Description("搬入")]
        Add = 0,
        [Description("更新")]
        Modify = 64,
        [Description("搬出")]
        Delete = 128,
    }

    /// <summary>
    /// 权限分类
    /// </summary>
    public enum ElementOrganizeType {
        [Description("企业税收")]
        EnterpriseTax = 0,
        [Description("企业变更")]
        EnterpriseModify = 64,
        [Description("楼栋变更")]
        BuildingModify = 128,
        [Description("报送单位关联街道，用于自动发送")]
        ToStreet = 256,
    }
}
