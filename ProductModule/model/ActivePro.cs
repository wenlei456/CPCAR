using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductModule.model
{
   public class ActivePro
    {
       public DAO.Active active { get; set; }
       public DAO.ActivePros activePro { get; set; }
       public List<DAO.ActiveAttr> activeAttr { get; set; }
    }
   public class ActiveProAttr2
   {  
       public List<DAO.ActiveAttr> activeAttr { get; set; }
       public DAO.ActivePros activePro { get; set; }
   }
}
