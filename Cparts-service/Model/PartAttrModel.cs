using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cparts_service.Model
{
   public class PartAttrNameModel
    {
       public string AttrName{get;set;}
       public int ParentAttr { get; set; }
    }
   public class ActiveProAttr
   {
       public int ID { get; set; }
       public int PartID { get; set; }
       public int ParentAttr { get; set; }
       public string AttrName { get; set; }
       public string AttrFlagName { get; set; }
       public string AttrValue { get; set; }
       public decimal AttrPrice { get; set; }
       public int Qty { get; set; }
       public Nullable<int> State { get; set; }
       public string PAN { get; set; }
       public int PState{get;set;}
       public string PAV { get; set; }
   }
}
