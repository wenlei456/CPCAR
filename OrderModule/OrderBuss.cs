using Cparts_Product;
using MemberModule.Business;
using OrderModule.model;
using ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOP;
using ProductModule.Models;
using log4net;
using System.Reflection;
using ActiveModule;

namespace OrderModule
{
    public class OrderBuss:BaseBusiness
    {     
        /// <summary>
        /// 订单价格校验
        /// </summary>
        /// <param name="shopList"></param>
        /// <param name="oAmount"></param>
        /// <returns></returns>
        public bool OrderAmountCheck(List<Shop> shopList, decimal oAmount, decimal ratio, out decimal shopListTotle)
        {
            shopListTotle = 0;
            decimal normTotle = 0;
            decimal ActTotle = 0;
            PartExtendBll pbll = new PartExtendBll();
            ProductBll proBll = new ProductBll();            
            foreach (Shop item in shopList)
            { 
                var qty=0;
               if(int.TryParse(item.Qty,out qty))
               {
                //产品
                DAO.Parts product = proBll.GetPartModel(item.ID);
                //先检查产品是否有属性，是否是最后一个属性
                if (!string.IsNullOrEmpty(item.Attrs))
                {                   
                    //先检查是否是最终属性，没有子属性
                    DAO.PartsExtend childAttr = pbll.GetChildAttrModel(item.ID, item.Attrs);
                    if (childAttr == null)
                    {   //没有子属性为有效的属性
                        DAO.PartsExtend currAttr = pbll.GetPartAttrModel(item.ID, item.Attrs);
                        if (currAttr == null)
                        {
                            //属性没有效 
                            return false;
                        }
                        else 
                        {
                            if (item.Type != "-1")
                            {
                                int aid=int.Parse(item.Type);
                                int pid=int.Parse(item.ID);
                                int tid=int.Parse(item.Attrs);
                               DAO.ActiveAttr att= db.ActiveAttr.Where(m=>m.ActiveID==aid&&m.AProID==pid&&m.AtrrID==tid).FirstOrDefault();
                               if (att != null)
                               {
                                   ActTotle += qty * att.NewPrice;
                               }
                               else {
                                   normTotle += qty * currAttr.AttrPrice;
                               }
                            }
                            else
                            {
                                normTotle += qty * currAttr.AttrPrice;
                            }
                        }
                    }
                    else 
                    {
                        //不是最终属性 错误                      
                        return false;
                    }                   
                }
                else 
                {
                    //没有属性
                    if (item.Type != "-1")
                    {
                        int aid = int.Parse(item.Type);
                        int pid = int.Parse(item.ID);
                        DAO.ActivePros pro = db.ActivePros.Where(m => m.ActiveID == aid && m.ProID == pid ).FirstOrDefault();
                        if (pro != null)
                        {
                            ActTotle += qty * pro.NewPrice;
                        }
                        else {
                            normTotle += qty * product.Price;
                        }
                    }
                    else
                    {
                        //没有活动
                        normTotle += qty * product.Price;
                    }
                }
               }else{
                   //数量不是数字                  
                   return false;
               }
            }
            //获取会员等级折扣
            shopListTotle =Math.Round(ActTotle + Math.Round(normTotle * 1, 2),2);           
            if (shopListTotle == oAmount)
            {
                //价格验证成功
                return true;
            }
            else {
                //价格验证失败
                return false;
            }
        }

              /// <summary>
        /// 查询用户是否有地址
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public DAO.Addship isAddressByUid(string uid)
        {
            string orderSql = " select * from [Addship] where Uid={0}";
            DAO.Addship o = db.Database.SqlQuery<DAO.Addship>(string.Format(orderSql, Convert.ToInt32(uid))).FirstOrDefault();
            if (o != null)
            {
                return o;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 提交订单
        /// </summary>
        /// <param name="shopList">商品列表</param>
        /// <param name="sendinfo">配送信息</param>
        /// <param name="uid">当前用户</param>
        /// <param name="ratio">当前用户折扣</param>
        /// <param name="orderPrice">订单金额</param>
        /// <param name="shopListTotle">商品列表总金额</param>
        /// <returns>订单号</returns>
        public string AddOrder(List<Shop> shopList, SendInfo sendinfo, string uid, decimal ratio, decimal orderPrice, decimal shopListTotle)
        {
            PartExtendBll pbll = new PartExtendBll();
            ProductBll proBll = new ProductBll();
            MemberBLL memBll = new MemberBLL();

            DAO.MemberBase user = memBll.GetUserByID(uid);
            DAO.Order order = new DAO.Order();
            order.OrderId = CommonUtils.OrderUtil.NewOrderId;
            if (order != null)
            {
                order.Uid = user.ID;
                order.UserName = user.LoginName;
                order.UserPhone = user.Mobile;
            }
            order.OrderSource = "0";
            order.OrderTime = DateTime.Now;
            order.OrderStatus = "未处理";//订单初始状态
            order.Consignee = sendinfo.Name;
            order.Address = sendinfo.Address;
            order.CellPhone = sendinfo.Phone;
            order.PayStatus = "未支付";
            order.Payment="";
            order.Remarks = "";
            order.Ratio=ratio;
            decimal disAmout = shopList.Where(h => h.Type == "-1").Sum(h=>(decimal.Parse(h.Price)));
            order.DiscountAmount = (1 - ratio) * disAmout;
            order.ShippingCost = 0;
            order.TotalPrice = orderPrice;
            order.SearchTag = "";
            order.OperatorRemarks = "";
            order.OrderType = "0";
            order.OrderFrom = 0;//网站
            order.Prov = sendinfo.Prov;
            order.City = sendinfo.City;
            order.Area = sendinfo.Area;

            db.Order.Add(order);
            DAO.Addship addShip = new DAO.Addship();
            addShip = isAddressByUid(uid);

            if (addShip != null)
            {
                upAddShip(sendinfo.Name, uid, sendinfo.City, sendinfo.Prov, sendinfo.Area, sendinfo.Address, sendinfo.Phone);
            }
            else
            {
                DAO.Addship addShipS = new DAO.Addship();
                addShipS.name = sendinfo.Name;
                addShipS.province = sendinfo.Prov;
                addShipS.scity = sendinfo.City;
                addShipS.region = sendinfo.Area;
                addShipS.address = sendinfo.Address;
                addShipS.UID = Convert.ToInt32(uid);
                addShipS.phone = sendinfo.Phone;
                db.Addship.Add(addShipS);
                db.SaveChanges();
            }
           
           
            //db.SaveChanges();
            foreach (Shop item in shopList)
            {                  
                        CarProductDetail m;
                        int i = -1;
                        int pid=-1;
                        if (int.TryParse(item.ID,out pid))
                        {
                            if (!string.IsNullOrEmpty(item.Attrs) && int.TryParse(item.Attrs, out i))
                            {
                                m = proBll.GetProDetail(pid, i,item.Type);
                            }
                            else
                            {
                                m = proBll.GetProDetail(pid, -1,item.Type);
                            }
                            if (m != null)
                            {
                                m.Qty = int.Parse(item.Qty);
                                m.lastAttr = item.Attrs;
                                DAO.OrderProList pro=new DAO.OrderProList ();
                                pro.OrderId= order.OrderId;
                                pro.ProductID=m.proDetail.ID;
                                pro.ProductName=m.proDetail.PartName;
                                pro.SupplyNo=m.proDetail.PartSuppyNo;
                                pro.Num=m.Qty;
                                pro.Price=m.proDetail.Price;
                                pro.InputTime=DateTime.Now;
                                pro.PType=m.proDetail.CategoryID;
                                pro.AttrID =i;
                                pro.IsFlag =int.Parse(item.Type);//活动号
                                pro.activeName = m.activeM == null ? "" : m.activeM.Name;
                                StringBuilder sb=new StringBuilder ("");
                                 foreach (var ext in m.partExt)
	                            {
                                     sb.Append("|"+ext.AttrName+":"+ext.AttrValue+"|");
		 
	                            }
                                pro.AttrDecribe=sb.ToString();
                                db.OrderProList.Add(pro);
                                opActiveStock(pro.IsFlag, pro.ProductID, pro.AttrID, pro.Num);

                                OrderBuss obll = new OrderBuss();
                                int number = Convert.ToInt32(obll.getPartStock(m.proDetail.ID).StockNUM) - 1;

                                obll.upPartStock(m.proDetail.ID, number);

                                //db.SaveChanges();
                            }
                        }
            }
            int addResult = db.SaveChanges();//db.SaveChanges();
            if (addResult > 0)
            {
                return order.OrderId.ToString();
            }
            else {
                return null ;
            
            }
        }

        private bool upAddShip(string name, string UID,string scity,string province,string region,string address,string phone)
        {


            string sql = "UPDATE [Addship] SET [scity] ='{0}',[province] ='{1}',[region] ='{2}',[address] ='{3}',[name] ='{4}',[phone] ='{5}' WHERE uid={6}";
            int s = db.Database.ExecuteSqlCommand(string.Format(sql, scity, province, region, address, name,phone,Convert.ToInt32(UID)));
            if (s > 0)
            {
                return true;
            }
            else
            {
                return false;
            }


        }


        private static object o = new object();  

        private bool opActiveStock(int? aid,int? pid,int? atid,int qty)
        {
          
            lock (o)
            {
                if (aid != -1)
                {
                    if (atid != null && atid > 0)
                    {
                        string sql = "UPDATE [ActiveAttr] SET [Stock] =Stock-{0} WHERE [ActiveID]={1} and AProID={2} and AtrrID={3} and Stock>={0}";
                        db.Database.ExecuteSqlCommand(string.Format(sql,qty,aid,pid,atid,qty));
                    }
                    else 
                    {
                        string sql = "UPDATE [ActivePros] SET [Stock] =Stock-{0} WHERE [ActiveID]={1} and ProID={2} and Stock>={0}";
                        db.Database.ExecuteSqlCommand(string.Format(sql, qty, aid, pid, qty));
                    }

                }
            
            
            }
            return true;
        
        
        }
        /// <summary>
        /// 我的订单
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public MyOrderInfo GetMyorder(string uid)
        {
            MyOrderInfo info = new MyOrderInfo();
            string orderSql = " select * from [Order] where Uid={0} order by OrderTime desc";
            List<DAO.Order> oList= db.Database.SqlQuery<DAO.Order>(string.Format(orderSql,uid)).ToList();
            string opSql = " SELECT *  FROM [OrderProList] where [OrderId] in (select [OrderId] from [order] where Uid={0})";
            List<DAO.OrderProList> opList = db.Database.SqlQuery<DAO.OrderProList>(string.Format(opSql,uid)).ToList();
            string imgSql = "select * from [ImgStock] where id in ( SELECT id  FROM [ImgStock] where [PartID] in (select ProductID from OrderProList where [OrderId] in (select [OrderId] from [order] where Uid={0})))";
            List<DAO.ImgStock> imgList = db.Database.SqlQuery<DAO.ImgStock>(string.Format(imgSql, uid)).ToList();
            info.orderList = oList;
            info.opList = opList;
            info.pimgList = imgList;
            return info;
        }
        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public OrderDetail OrderDetail(string uid, string orderNum)
        {
            OrderDetail order = new model.OrderDetail();
            string orderSql = " select * from [Order] where Uid={0} and OrderId={1}";
            DAO.Order o = db.Database.SqlQuery<DAO.Order>(string.Format(orderSql, uid, orderNum)).FirstOrDefault();
            string opSql = " SELECT *  FROM [OrderProList] where [OrderId] ={0}";
            List<DAO.OrderProList> opList = db.Database.SqlQuery<DAO.OrderProList>(string.Format(opSql, orderNum)).ToList();
            string imgSql = "select * from imgstock where partID in (select ProductID from orderProList where orderID={0})";
            List<DAO.ImgStock> imgList = db.Database.SqlQuery<DAO.ImgStock>(string.Format(imgSql, orderNum)).ToList();
            string expSql = "select * from ExpRecord where orderNum ='{0}'";
            DAO.ExpRecord ex = db.Database.SqlQuery<DAO.ExpRecord>(string.Format(expSql,orderNum)).FirstOrDefault();
            int? exp = 0;
            if (ex != null)
            {
                exp = ex.Exp;
            }
            order.order = o;
            order.opList = opList;
            order.pimgList = imgList;
            order.exp = exp == null ? 0 : exp;
            return order;
        }
        public OrderDetail OrderDetail( string orderNum)
        {
            OrderDetail order = new model.OrderDetail();
            string orderSql = " select * from [Order] where  OrderId={0}";
            DAO.Order o = db.Database.SqlQuery<DAO.Order>(string.Format(orderSql,  orderNum)).FirstOrDefault();
            string opSql = " SELECT *  FROM [OrderProList] where [OrderId] ={0}";
            List<DAO.OrderProList> opList = db.Database.SqlQuery<DAO.OrderProList>(string.Format(opSql, orderNum)).ToList();               
            order.order = o;
            order.opList = opList;     
            return order;
        }
        public DAO.Order GetOrder(string orderNum)
        {          
            DAO.Order o = db.Order.Where(m=>m.OrderId==orderNum).FirstOrDefault();
            return o;
        }
        /// <summary>
        /// 获取所有支付方式
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>
        public DAO.OrderPay GetOrderPay(string oid)
        {
           DAO.OrderPay opList= db.OrderPay.Where(m=>m.OrderId==oid && m.RecordState==1 && m.PayType==0).FirstOrDefault();
            return opList;
        }
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public DAO.PartStock getPartStock(int id)
        {
            DAO.PartStock opList = db.PartStock.Where(m => m.PartID == id).FirstOrDefault();
            return opList;
        }

        /// <summary>
        /// 修改库存
        /// </summary>
        /// <param name="oid"></param>
        /// <returns></returns>

        public bool upPartStock(int id,int num)
        {
            VoucherBus vbl = new VoucherBus();

            string sql = @"UPDATE [PartStock] SET StockNUM=" + num + " where PartID=" + id + "";
            int result = db.Database.ExecuteSqlCommand(sql);
            if (result > 0)
            {
                log.Info("修改库存记录成功");
               
                return true;
            }
            else
            {
                log.Info("修改库存记录--失败");
                return false;
            }
        }

        public bool upDataOrderPay(DAO.OrderPay o)
        {
            VoucherBus vbl = new VoucherBus();
          
            string sql = @"UPDATE [OrderPay] SET PayState='已支付',Bank='支付宝',Remark='"+o.Remark+ "' where id="+o.id+"";
            string sql2 = @"UPDATE [OrderPay] SET PayState='已支付' where RecordState=1 and OrderId='"+o.OrderId+ "' and PayType<>0";
            int result= db.Database.ExecuteSqlCommand(sql);       
            if (result > 0)
            {
                log.Info("修改OrderPay 支付宝记录成功");
                DAO.OrderPay op = db.OrderPay.Where(m=>m.OrderId==o.OrderId&&m.PayType==1&&m.RecordState==1).FirstOrDefault();
                if (op != null)
                {
                   vbl.UseVoucher(op.Remark);
                }
                //  vbl.UseVoucher();
                int result2 = db.Database.ExecuteSqlCommand(sql2);//更改其他方式付款状态--组合付款
                log.Info("修改OrderPay特殊支付记录：" + result2);
                    return true;
            }
            else
            {
                log.Info("修改OrderPay 支付宝记录--失败");
                return false;
            }
        }
        public bool upOrder(string o)       
        {          
            string sql = @"UPDATE [Order] SET PayStatus='已支付' where OrderId=" + o + "";
            int result = db.Database.ExecuteSqlCommand(sql);
            //订单状态改变以后改变所有积分，返利生效         
            string uexp = "update ExpRecord set status=1 where OrderNum='{0}' ";
            string ur = " update RebateRecord set Status=1 where LowerOrder='{0}' ";
           db.Database.ExecuteSqlCommand(string.Format(uexp,o));
           db.Database.ExecuteSqlCommand(string.Format(ur, o));

         
            if (result > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
        public bool ValidataPayPrice(string oid,decimal oprice)
        {
            decimal payPrice = db.OrderPay.Where(m=>m.OrderId==oid && m.PayState=="已支付" && m.RecordState==1).Sum(m=>m.Amount);
            if (payPrice == oprice)
            {
                return true;
            }
            else {
                return false;
            }
        }
        public bool AddOrderPay(string oid,List<DAO.OrderPay> ol)
        {
            if (ol.Count > 0)
            {
                //以前所有方式删除
                string sql = "UPDATE [OrderPay] set  [RecordState]=0 where [OrderId]='{0}' ";
                db.Database.ExecuteSqlCommand(string.Format(sql,oid));
                foreach (DAO.OrderPay item in ol)
                {
                    db.OrderPay.Add(item);
                }
               
                int i = db.SaveChanges();
                if (i > 0)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            else {
                return false;
            }
        }
        public DAO.Order getAddress(string uid)
        {
            string ordersql = "select * from [Order] where Uid={0}";
            DAO.Order carder = db.Database.SqlQuery<DAO.Order>(string.Format(ordersql, Convert.ToInt32(uid))).FirstOrDefault();
            return carder;
        }

    }
}
