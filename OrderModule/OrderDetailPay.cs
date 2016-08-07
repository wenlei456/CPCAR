using OrderModule.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrderModule
{
  public  class OrderDetailPay
    {
        public void OrderDetailsPayInit(string  oid, OrderDetail od)
        {
            this.oid = oid;
            StringBuilder sb = new StringBuilder();        
           OrderBuss ob=new OrderBuss ();
           DAO.OrderPay op= ob.GetOrderPay(oid);
            if (string.IsNullOrEmpty(od.order.OrderType) || od.order.OrderType == "0")//正常订单
            {
                //string strsql = "select *  from OrderProductList p where   p.OrderId=" + id;
                //var list = db.Database.SqlQuery<DAO.OrderProductList>(strsql).ToList();
                List<DAO.OrderProList> ordList = od.opList;             
                foreach (DAO.OrderProList p in ordList)
                {
                    sb.AppendFormat("{0}x{1},", p.ProductName, p.Num);
                }

                if (sb.Length > 0)
                {
                    if (sb[sb.Length - 1] == ',')
                    {
                        sb.Remove(sb.Length - 1, 1);
                    }
                }          
             
              
                    this.description = sb.ToString();
                    this.price =Math.Round( op.Amount,2);//Math.Round(od.order.TotalPrice,2);
                    this.Freight = "¥0" ;
                    this.uname = od.order.UserName;
                    this.Pay = "支付宝";
                    this.phone = od.order.CellPhone;
                    this.tel = od.order.CellPhone;
                    this.Rinfo = string.Format("{0}-{1}{2}{3}{4}", od.order.UserName, od.order.Prov, od.order.City, od.order.Area, od.order.Address);
                    this.rtime = "";
                    this.addTime = string.Format("{0:yyyy-MM-dd hh:mm:ss}", od.order.OrderTime); 
                    this.orderType = 0;
                

            }
            //else if (op.OrderType == 1)
            //{
            //    MemberCoinBusiness bmc = new MemberCoinBusiness();
            //    bmc.GetChargeByOrder(this.oid);
            //    this.description = string.Format("充值{0}诚配币x1", bmc.mc.Amount);
            //    this.price = "¥" + op.Amount;
            //    this.Freight = "¥0.00";
            //    this.uname = op.ULoginName;
            //    this.Pay = op.Bank;
            //    this.phone = " ";
            //    this.tel = "";
            //    this.addTime = Convert.ToDateTime(bmc.mc.CreateTime);
            //    this.orderType = 1;
            //}
         
        }
        private int orderType;
        /// <summary>
        /// 订单类型（0 为正常订单 1 充值极致币订单）
        /// </summary>
        public int OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }
        private string uname;
        /// <summary>
        /// 会员登录名
        /// </summary>
        public string Uname
        {
            get { return uname; }
            set { uname = value; }
        }

        private string oid;
        /// <summary>
        /// 订单编号
        /// </summary>
        public string Oid
        {
            get { return oid; }
            set { oid = value; }
        }
        private string description;
        /// <summary>
        /// 此订单描述
        /// </summary>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        private string pay;

        public string Pay
        {
            get { return pay; }
            set { pay = value; }
        }
        private decimal price;

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        private string freight;

        public string Freight
        {
            get { return freight; }
            set { freight = value; }
        }
        private string rinfo;

        public string Rinfo
        {
            get { return rinfo; }
            set { rinfo = value; }
        }
        private string rtime;

        public string Rtime
        {
            get { return rtime; }
            set { rtime = value; }
        }

        private string phone;

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }
        private string tel;

        public string Tel
        {
            get { return tel; }
            set { tel = value; }
        }
        private string addTime;

        public string AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }

    }
}
