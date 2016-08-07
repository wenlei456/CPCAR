using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberModule.Model
{
    public class Comments
    {

        public int ID { get; set; }
        public int userID { get; set; }
        public int ProductID { get; set; }
        public string mentsname { get; set; }
        public int shown { get; set; }
        public string PartName { get; set; }
       
    }
}
