using System;
using System.ComponentModel;

namespace RoadFlow.Data.Model {
    [Serializable]
    public class Buildings {
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
        /// 所属街道
        /// </summary>
        [DisplayName("所属街道")]
        public string SSJD { get; set; }
        /// <summary>
        /// 商务总面积
        /// </summary>
        [DisplayName("商务总面积")]
        public string SW_ZMJ { get; set; }
        /// <summary>
        /// 商务空置总面积
        /// </summary>
        [DisplayName("商务空置面积")]
        public string SW_KZ_ZMJ { get; set; }
        /// <summary>
        /// 商务空置可销售面积
        /// </summary>
        [DisplayName("商务空置可销售")]
        public string SW_KZ_KXSMJ { get; set; }
        /// <summary>
        /// 商务空置可租赁面积
        /// </summary>
        [DisplayName("商务空置可租赁")]
        public string SW_KZ_KZLMJ { get; set; }
        /// <summary>
        /// 商业总面积
        /// </summary>
        [DisplayName("商业总面积")]
        public string SY_ZMJ { get; set; }
        /// <summary>
        /// 商业空置总面积
        /// </summary>
        [DisplayName("商业空置面积")]
        public string SY_KZ_ZMJ { get; set; }
        /// <summary>
        /// 商业空置可销售面积
        /// </summary>
        [DisplayName("商业空置可销售")]
        public string SY_KZ_KXSMJ { get; set; }
        /// <summary>
        /// 商业空置可租赁面积
        /// </summary>
        [DisplayName("商务空置可租赁")]
        public string SY_KZ_KZLMJ { get; set; }
        /// <summary>
        /// 建设阶段
        /// </summary>
        [DisplayName("建设阶段")]
        public string JSJD { get; set; }
        /// <summary>
        /// 楼宇级别
        /// </summary>
        [DisplayName("楼宇级别")]
        public string LYJB { get; set; }
        /// <summary>
        /// 楼宇类型
        /// </summary>
        [DisplayName("楼宇类型")]
        public string LYLX { get; set; }
        /// <summary>
        /// 报送楼栋
        /// </summary>
        [DisplayName("报送楼栋")]
        public string LYDS { get; set; }
        /// <summary>
        /// 统筹招商
        /// </summary>
        [DisplayName("统筹招商")]
        public string TCZS { get; set; }
        /// <summary>
        /// 竣工时间
        /// </summary>
        [DisplayName("竣工时间")]
        public string JGSJ { get; set; }
        /// <summary>
        /// 报送时间
        /// </summary>
        [DisplayName("报送时间")]
        public string AddTime { get; set; }
        /// <summary>
        /// 入住优惠
        /// </summary>
        [DisplayName("入住优惠")]
        public string RZYH { get; set; }
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
        public string  ZJZMJ { get; set; }
        /// <summary>
        /// 商业已使用面积
        /// </summary>
        [DisplayName("商业已使用面积")]
        public string SY_YSY_ZMJ { get; set; }
        /// <summary>
        /// 商业自用面积
        /// </summary>
        [DisplayName("商业自用面积")]
        public string SY_YSY_ZYMJ { get; set; }
        /// <summary>
        /// 商业单层最大空置面积
        /// </summary>
        [DisplayName("商业单层最大空置面积")]
        public string SY_DC_ZDKZMJ { get; set; }
        /// <summary>
        /// 商业销售均价
        /// </summary>
        [DisplayName("商业销售均价")]
        public string SY_XSJJ { get; set; }
        /// <summary>
        /// 已使用总面积
        /// </summary>
        [DisplayName("已使用总面积")]
        public string SW_YSY_ZMJ { get; set; }
        /// <summary>
        /// 自用面积
        /// </summary>
        [DisplayName("商务自用面积")]
        public string SW_YSY_ZYMJ { get; set; }
        /// <summary>
        /// 单层空置最大面积
        /// </summary>
        [DisplayName("商务单层空置最大面积")]
        public string SW_KZ_DCKZZDMJ { get; set; }
        /// <summary>
        /// 商业租金
        /// </summary>
        [DisplayName("商业租金")]
        public string SY_ZJ { get; set; }
        /// <summary>
        /// 商务租金
        /// </summary>
        [DisplayName("商务租金")]
        public string SW_ZJ { get; set; }
        /// <summary>
        /// 物管费(商业)
        /// </summary>
        [DisplayName("物管费(商业)")]
        public decimal SY_WGF { get; set; }
        /// <summary>
        /// 物管费(商务）
        /// </summary>
        [DisplayName("物管费(商务）")]
        public decimal SW_WGF { get; set; }
        /// <summary>
        /// 楼宇产权情况
        /// </summary>
        [DisplayName("楼宇产权情况")]
        public string LYCQQK { get; set; }
        /// <summary>
        /// 自持产权面积（㎡）
        /// </summary>
        [DisplayName("自持产权面积（㎡）")]
        public string ZCCQMJ { get; set; }
        /// <summary>
        /// 车位数
        /// </summary>
        [DisplayName("车位数")]
        public int CWS { get; set; }
        /// <summary>
        /// 电梯（部/每栋楼）
        /// </summary>
        [DisplayName("电梯（部/每栋楼）")]
        public int DTS { get; set; }
        /// <summary>
        /// 中央空调
        /// </summary>
        [DisplayName("中央空调")]
        public string ZYKT { get; set; }
        /// <summary>
        /// 招商方向
        /// </summary>
        [DisplayName("招商方向")]
        public string ZSFX { get; set; }
        /// <summary>
        /// 百度地图坐标
        /// </summary>
        [DisplayName("百度地图坐标")]
        public string BDDW { get; set; }
        /// <summary>
        /// 效果图
        /// </summary>
        [DisplayName("效果图")]
        public string XGT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DisplayName("备注")]
        public string Note { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("State")]
        public int State { get; set; }
       

    }
}
