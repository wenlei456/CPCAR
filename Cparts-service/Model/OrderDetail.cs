using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cparts_service.Model
{
    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetail
    {
        public DAO.Order order { get; set; }
        public List<DAO.OrderProList> opList { get; set; }
        public List<DAO.ImgStock> pimgList { get; set; }
        public DAO.information xiangxi { get; set; }
    }
    public class Commlect
    {
        public List<commselect> sede { get; set; }
    }
    public class commselect
    {
        public string name { get; set; }
        public int ID { get; set; }
        public Nullable<int> userID { get; set; }
        public string ProductID { get; set; }
        public string mentsname { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public Nullable<int> shown { get; set; }
    }
    public class Crowshows
    {
        public List<CrowShow> shod { get; set; }
    }
    public class CrowShow
    {
            public int ID { get; set; }
            public int Pid { get; set; }
            public System.DateTime ShowTime { get; set; }
            public System.DateTime StarTime { get; set; }
            public Nullable<System.DateTime> EndTime { get; set; }
            public Nullable<int> isBool { get; set; }
            public string ListTille { get; set; }
            public string Banner { get; set; }
            public string PartName { get; set; }
            public Nullable<int> Number { get; set; }
    }
    public class SendOrderBM
    {
        public string OrderNumber { get; set; }
     public string SendName{get;set;}
     public string SandNumber{get;set;}
    
    }
}
