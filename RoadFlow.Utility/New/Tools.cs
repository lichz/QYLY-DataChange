//========================================
// Copyright © 2017
// 
// CLR版本 	: 4.0.30319.42000
// 计算机  	: USER-20170420WC
// 文件名  	: Tools.cs
// 创建人  	: kaifa5
// 创建时间	: 2017/9/25 14:06:40
// 文件版本	: 1.0.0
// 文件描述	: 
//========================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadFlow.Utility.New {
    /// <summary>
    /// 辅助工具静态封装类。
    /// </summary>
    public static class Tools {
        /// <summary>
        /// 获取指定长度的随机数字字符串（可用作数字验证码）。
        /// </summary>
        /// <param name="length">随机字符串的长度。</param>
        /// <returns>随机产生的字符串。</returns>
        public static string GetRandomNum(int length) {
            if (length < 1) {
                return string.Empty;
            }
            Guid seed = Guid.NewGuid();//保证循环快速调用时生成不同的随机数。
            Random random = new Random(seed.GetHashCode());
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                int number = random.Next();
                char code = Convert.ToChar(number % 10 + 48);
                builder.Append(code);
            }
            return builder.ToString();
        }

        /// <summary>
        /// 获取指定长度的随机字符串（包含小写字母、数字和大写字母）。
        /// </summary>
        /// <param name="length">随机字符串的长度。</param>
        /// <returns>随机产生的字符串。</returns>
        public static string GetRandomString(int length) {
            if (length < 1) {
                return string.Empty;
            }
            Guid seed = Guid.NewGuid();//保证循环快速调用时生成不同的随机数。
            Random random = new Random(seed.GetHashCode());
            StringBuilder builder = new StringBuilder(length);
            for (int i = 0; i < length; i++) {
                char code;
                int number = random.Next();
                switch (number % 3) {
                    case 0:
                        //小写字母
                        code = Convert.ToChar(number % 26 + 97);
                        break;
                    case 1:
                        //数字
                        code = Convert.ToChar(number % 10 + 48);
                        break;
                    default:
                        //大写字母
                        code = Convert.ToChar(number % 26 + 65);
                        break;
                }
                builder.Append(code);
            }
            return builder.ToString();
        }
    }
}