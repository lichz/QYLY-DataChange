using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadFlow.Data.Interface
{
  public  interface ITableAttribute
    {
      List<Column> GetTableAttr(string table);
    }
}
