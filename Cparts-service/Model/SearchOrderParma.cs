using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cparts_service.Model
{
  public class SearchOrderParma
    {
      public string orderStatus { get; set; }
      public string payStatus { get; set; }
      public string orderNumber { get; set; }
      public string orderBeginTime { get; set; }
      public string orderEndTime { get; set; }
      public string consignee { get; set; }
    }
}
