using log4net;
using OrderModule;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WxPayAPI
{
    /// <summary>
    /// 支付结果通知回调处理类
    /// 负责接收微信支付后台发送的支付结果并对订单有效性进行验证，将验证结果反馈给微信支付后台
    /// </summary>
    public class ResultNotify:Notify
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ResultNotify(Page page):base(page)
        {
        }
        OrderBuss obll = new OrderBuss();
        public override void ProcessNotify()
        {
            WxPayData notifyData = GetNotifyData();
            WxPayData res = new WxPayData();
            if (notifyData.GetValue("return_code").ToString() == "SUCCESS")
            {
                log.Info("return_code：SUCCESS");
                bool result=  notifyData.GetValue("result_code").ToString()=="SUCCESS"?true:false;
               // out_trade_no
                if (result)
                {
                    string out_trade_no = notifyData.GetValue("out_trade_no").ToString();
                    log.Info("获得订单号：" + out_trade_no);
                    DAO.OrderPay op = obll.GetOrderPay(out_trade_no);
                    if (op != null)
                    {
                        if (op.PayState == "未支付")
                        {
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
                          
                        }
                        res.SetValue("return_code", "SUCCESS");
                        res.SetValue("return_msg", "OK");
                        log.Info("return_code :SUCCESS");
                        page.Response.Write(res.ToXml());
                        page.Response.End();
                    }
                    else
                    {
                     
                        res.SetValue("result_code", "FAIL");
                        res.SetValue("return_msg", "OrderPay is Null");
                        log.Error("支付结果失败result_code :FAil ==>OrderPay is Null");
                        page.Response.Write(res.ToXml());
                        page.Response.End();
                    }
                }
                else {
                    res.SetValue("result_code", "FAIL");
                    res.SetValue("return_msg", "result_code fail");
                    log.Error("支付结果失败result_code :FAil ==>" + res.ToXml());
                    page.Response.Write(res.ToXml());
                    page.Response.End();
                }
            }
            else {               
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "retrun_code fail");
                log.Error("return_code :FAil ==>" + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
          /**  
            //检查支付结果中transaction_id是否存在
            if (!notifyData.IsSet("transaction_id"))
            {
                //若transaction_id不存在，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "支付结果中微信订单号不存在");
                Log.Error("The Pay result is error : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }

            string transaction_id = notifyData.GetValue("transaction_id").ToString();

            //查询订单，判断订单真实性
            if (!QueryOrder(transaction_id))
            {
                //若订单查询失败，则立即返回结果给微信支付后台
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "FAIL");
                res.SetValue("return_msg", "订单查询失败");
                Log.Error("Order query failure : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }
            //查询订单成功
            else
            {
                WxPayData res = new WxPayData();
                res.SetValue("return_code", "SUCCESS");
                res.SetValue("return_msg", "OK");
                Log.Info("order query success : " + res.ToXml());
                page.Response.Write(res.ToXml());
                page.Response.End();
            }*****/

        }

        //查询订单
        private bool QueryOrder(string transaction_id)
        {
            WxPayData req = new WxPayData();
            req.SetValue("transaction_id", transaction_id);
            WxPayData res = WxPayApi.OrderQuery(req);
            if (res.GetValue("return_code").ToString() == "SUCCESS" &&
                res.GetValue("result_code").ToString() == "SUCCESS")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}