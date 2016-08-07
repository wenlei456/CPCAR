using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m.cpcar.com.Models
{
    public class PartCateListViewModel
    {
        public List<DAO.PartsCategory> parentCate { get; set; }
        public List<DAO.PartsCategory> childenrCate { get; set; }
    }
    public class TypeListByCarViewModel
    {
        public List<DAO.PartsCategory> parentCate { get; set; }
        public List<DAO.PartsCategory> childenrCate { get; set; }
        public DAO.CarM Carinfo { get; set; }
        public List<DAO.PartsCategory> topCate { get; set; }
    }
    
}