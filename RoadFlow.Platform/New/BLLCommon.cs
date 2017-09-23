
using RoadFlow.Data.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RoadFlow.Platform;

namespace RoadFlow.Platform
{
    /// <summary>
    /// BLL公共方法类
    /// </summary>
    public class BLLCommon
    {
        /// <summary>
        /// 获取及时性评分
        /// </summary>
        /// <returns></returns>
        public static decimal GetTimeliness()
        {
            //if (DateTime.Now.Day > 20)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return 40;
            //}
            return 40;
        }

        /// <summary>
        /// 获取质量评分
        /// </summary>
        /// <returns></returns>
        public static decimal GetQuality(int count)
        {
            int score = 0;
            if (count == 0)
            {
                return score;
            }
            else
            {
                score = 10;//基础分
            }
            score += (count - 1) * 4;//一个以上部分每个4分。
            if (score > 40)
            {//最多40分
                score = 40;
            }
            return score;
        }
    }
}
