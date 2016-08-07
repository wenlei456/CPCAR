using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderModule.model
{
    /// <summary>
    /// 订单列表详情
    /// </summary>
    public class MyOrderInfo
    {
       public List<DAO.Order> orderList { get; set; }
       public List<DAO.OrderProList> opList { get; set; }
       public List<DAO.ImgStock> pimgList { get; set; } 
    }

    /// <summary>
    /// 订单详情
    /// </summary>
    public class OrderDetail
    {
        public DAO.Order order { get; set; }
        public List<DAO.OrderProList> opList { get; set; }
        public List<DAO.ImgStock> pimgList { get; set; }
        public int? exp { get; set; }
    }
}
