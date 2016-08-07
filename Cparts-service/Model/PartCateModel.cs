using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CParts_Offline.Models.Master
{
    public class PartCateModel
    {
        public  DAO.PartsCategory currItem { get; set; }
        public List<PartCateModel> Children { get; set; }       
    }
    public class PartCateViewModel
    {
        public List<PartCateModel> PCItem { get; set; }
        public List<DAO.PartsCategory> AllItem { get; set; }
    }
}