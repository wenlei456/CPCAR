using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CParts_Online.Models
{
    public class ShopCarVM
    {
       public List<ShopCarModel> List { get; set; }

    }
    public class ShopCarModel
    {
        public string Attrs { get; set; }
        public int ID { get; set; }
        public string Price { get; set; }
        public string Qty { get; set; }
        public string Type { get; set; }

    }
    
}