using System;
using System.Collections.Generic;
using System.Web.Mvc;
using log4net;
using System.Reflection;
using OrderModule;
using System.Threading;
namespace m.bestcake.com.pay.weixin
{
    public class WeixinPayController : Controller
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
    
        //
        // GET: /WeixinPay/
        OrderBuss obll = new OrderBuss();
        public ActionResult updateOrderForPayFinished(string oid)
        {
            log.Info("**********************微信同步回调receive**********************");
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            try
            {
                //商户订单号
                string out_trade_no = oid;
                log.Info("获得订单号："+out_trade_no);
                DAO.OrderPay op = obll.GetOrderPay(out_trade_no);
                if (op != null)
                {
                    if (op.PayState == "未支付")
                    {
                        /**
                        op.PayState = "已支付";
                        if (op.PayType == 0)
                        {
                            op.Bank = "微信支付";
                        }
                        op.Remark = string.Format("{1},微信支付{0:yyyy-MM-dd HH:mm:ss},状态为:已支付", DateTime.Now, out_trade_no);
                        //修改OrderPay
                        log.Info("*********即将修改OrderPay**************");
                        bool r = obll.upDataOrderPay(op);
                        log.Info("orderPay 更新结果：" + r);
                        log.Info("orderPay 更新参数：op.Orderid=" + op.OrderId + "  op.PayState-" + op.PayState + "op.Bank-" + op.Bank + "trade_no-" + out_trade_no);
                        //支付成功修改订单状态
                        log.Info("*********即将修改Order状态**************");
                        DAO.Order order = obll.GetOrder(out_trade_no);
                        //计算组合支付总金额==订单金额
                        bool r_vali_price = obll.ValidataPayPrice(out_trade_no, order.TotalPrice);
                        log.Info("订单:" + op.OrderId + "金额，支付金额校验--" + r_vali_price);
                        if (r_vali_price)
                        {
                            bool r_oder = obll.upOrder(out_trade_no);
                            log.Info("订单:" + op.OrderId + " 修改结果--" + r_oder);
                        }
                        **/
                        Thread.Sleep(2000);
                        map.Add("Status", "ok");
                        map.Add("ot", "fff");
                    }
                    else {
                        map.Add("Status", "ok");
                        map.Add("ot", "fff");
                    }
                }
                else
                {
                    log.Info("OrderPay is Null");
                     map.Add("error", "没有找到支付信息");
                }
           
            }
            catch (Exception e)
            {
                map.Add("error", e.Message+e.StackTrace);
            }
            log.Info("**********************微信同步回调结束**********************");
            return Json(map, JsonRequestBehavior.AllowGet);
        
        }

    }
}
