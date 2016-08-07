using Cparts_service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CParts_Offline.Models
{
    public class PartAttrOutModel
    {
        public string AttrName{ get; set; }      
        public int ParentAttr { get; set; }
        public List<DAO.PartsExtend> AttrList { get; set; }       
    }
    public class information
    {
        public string ID { get; set; }
        public int ordID { get; set; }
        public string wuliuhao { get; set; }
        public string isshouhuo { get; set; }
        public DateTime time { get; set; }
    }
  
}