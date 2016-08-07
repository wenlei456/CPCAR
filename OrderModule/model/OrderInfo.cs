using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderModule.model
{
    public class Shop
    {
        public string ID { get; set; }
        public string Qty { get; set; }
        public string Price { get; set; }
        public string Attrs { get; set; }
        public string Type { get; set; }
    }
    public class SendInfo
    {
        public string Name { get; set; }
        public string Prov { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Beizhu { get; set; }
    }
}
