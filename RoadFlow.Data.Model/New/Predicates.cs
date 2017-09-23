using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model
{
    /// <summary>
    /// 谓词，用于筛选条件
    /// </summary>
    public class Predicates
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// 操作
        /// </summary>
        public SQLFilterType Operator { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }
    }
}
