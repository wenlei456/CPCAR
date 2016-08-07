using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m._91cpcar.com.Models
{
  
    public class Carinfo
    {
        public string brand { get; set; }
        public List<CarModel> models { get; set; }       
    }
    public class CarModel {
        public string model { get; set; }
        public List<string> year { get; set; }
    }
}