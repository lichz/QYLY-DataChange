using RoadFlow.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoadFlow.Platform
{
   public class TableAttribute
    {
         private RoadFlow.Data.Interface.ITableAttribute tableAttribute;


         public TableAttribute() 
       {
           this.tableAttribute = Data.Factory.Factory.GetTableAttribute();
       }

         public List<Column> GetTableAttr(string table)
         {
             return tableAttribute.GetTableAttr(table);
         }
    }
}
