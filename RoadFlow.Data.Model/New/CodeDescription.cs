using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RoadFlow.Data.Model {
    //用于checkboxList显示的模型。
    public class CodeDescription {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public CodeDescription(string code, string description, string category) {
            this.Code = code;
            this.Description = description;
            this.Category = category;
        }
    }
}
