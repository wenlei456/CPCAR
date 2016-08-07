using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductModule.model
{
    public class CrowdFundingDeail
    {
        public int id { get; set; }
        public int Pid { get; set; }
        public string Banner { get; set; }
        public string ListTille { get; set; }
        public int isBool { get; set; }
        public DateTime StarTime { get; set; }
        public DateTime EndTime { get; set; }
        public string PartName { get; set; }
        public decimal Price { get; set; }
        public int StockNUM { get; set; }
        public int Number { get; set; }

    }

    public class CrowdFundingList
    {
        public CrowdFundingDeail crowdFundingDeail { get; set; }
        public List<DAO.MemberBase> memberBaseList { get; set; }
    }
}
