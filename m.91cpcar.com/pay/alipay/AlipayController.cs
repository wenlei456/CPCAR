using Com.Alipay;
using log4net;
using OrderModule;
using OrderModule.model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Mvc;
using System.Xml;


namespace m.bestcake.com.pay.alipay
{
    public class AlipayController : Controller
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       
        // GET: /Alipay/
        //同步通知
        public ActionResult receive()
        {

            log.Info("**********************支付宝回调receive**********************");
            //商户订单号
            string out_trade_no = Request.QueryString["out_trade_no"];
            SortedDictionary<string, string> sPara = GetRequestGet();
            //日志
            log.Info("商户订单号" + out_trade_no + ",参数个数：" + sPara.Count);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);//Request.Form["notify_id"], Request.Form["sign"]
                log.Info("是否支付宝发出的验证：" + verifyResult + "参数sPara：" + sPara.ToString() + "参数sign：" + Request.QueryString["sign"]);
                //日志
                //   Common.Log(out_trade_no + ",回调判断：" + verifyResult, Common.LogLevel.low, "微网站支付宝回调");
                if (verifyResult)//验证成功 verifyResult
                {
                    log.Info("支付宝返回验证成功"); 
                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];
                    //交易状态
                    string trade_status = Request.QueryString["trade_status"];
                    log.Info("支付宝交易号：" + trade_no + "支付宝交易状态：" + trade_status);
                    OrderBuss obll = new OrderBuss();
                    if (trade_status == "TRADE_FINISHED" || trade_status == "TRADE_SUCCESS")
                    {
                        DAO.OrderPay op = obll.GetOrderPay(out_trade_no);
                        if (op != null)
                        {
                            if (op.PayState == "未支付")
                            {
                                op.PayState = "已支付";
                                if (op.PayType == 0)
                                {
                                    op.Bank = "支付宝";
                                }
                                op.Remark = string.Format("{1},由支付宝电子对账单更新{0:yyyy-MM-dd HH:mm:ss},状态为:已支付", DateTime.Now, trade_no);
                                //修改OrderPay
                                log.Info("*********即将修改OrderPay**************");
                                bool r = obll.upDataOrderPay(op);
                                log.Info("orderPay 更新结果：" + r);
                                log.Info("orderPay 更新参数：op.Orderid=" + op.OrderId + "  op.PayState-" + op.PayState + "op.Bank-" + op.Bank + "trade_no-" + trade_no);
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
                        }
                        else {
                            log.Info("OrderPay is Null");
                        }
                        //打印页面
                        //orderpay 订单支付类型 0 正常订单 1 充值订单                       
                        if (op.OrderType == 0)
                        {
                           
                            return Redirect(string.Format("/Order/Detail?oid={0}", out_trade_no));
                        }
                        else if (op.OrderType == 1)
                        {
                            return Redirect("/pay/recharge_success.html");
                        }
                    }
                    else {
                        log.Info("支付宝交易状态未通过");
                    }

                } else//验证失败
                    {
                        log.Info("支付宝返回验证失败");
                        Response.Write("验证失败");
                    }
            }
            else
            {
                Response.Write("无返回参数");
            }
            log.Info("**********************支付宝回调receive结束**********************");
            return View();
        }

        //异步通知
        public ActionResult autoreceive()
        {
            log.Info("**********************支付宝回调autoreceive**********************");
            SortedDictionary<string, string> sPara = GetRequestPost();
            log.Info("参数个数：" + sPara.Count);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"],Request.Form["sign"]);
                log.Info("autoreceive是否支付宝发出的验证：" + verifyResult + "参数sPara：" + sPara + "参数sign：" + Request.Form["sign"]);

                if (verifyResult)//验证成功
                {
                    log.Info("autoreceive验证成功");
                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];
                    //支付宝交易号
                    string trade_no = Request.Form["trade_no"];
                    //交易状态
                    string trade_status = Request.Form["trade_status"];
                    log.Info("autoreceive商户订单号:" + out_trade_no + "支付宝交易号：" + trade_no + "支付宝交易状态：" + trade_status);
                   
                    try
                    {                       
                         OrderBuss obll = new OrderBuss();
                        if (trade_status == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序
                            //注意：
                            //该种交易状态只在两种情况下出现
                            //1、开通了普通即时到账，买家付款成功后。
                            //2、开通了高级即时到账，从该笔交易成功时间算起，过了签约时的可退款时限（如：三个月以内可退款、一年以内可退款等）后。
                          
                        }
                        if (trade_status == "TRADE_SUCCESS")
                        {

                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
                            DAO.OrderPay op = obll.GetOrderPay(out_trade_no);
                            if (op.PayState == "未支付")
                            {
                                op.PayState = "已支付";
                                if (op.PayType == 0)
                                {
                                    op.Bank = "支付宝";
                                }
                                op.Remark = string.Format("{1},由支付宝电子对账单更新{0:yyyy-MM-dd HH:mm:ss},状态为:已支付", DateTime.Now, trade_no);
                                //修改OrderPay
                                log.Info("*********即将修改OrderPay**************");
                                bool r = obll.upDataOrderPay(op);
                                log.Info("orderPay 更新结果：" + r);
                                log.Info("orderPay 更新参数：op.Orderid=" + op.OrderId + "  op.PayState-" + op.PayState + "op.Bank-" + op.Bank + "trade_no-" + trade_no);
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
                        }
                        else
                        {
                            Response.Write(trade_status);
                        }

                        Response.Write("success");  //请不要修改或删除

                    }
                    catch (Exception exc)
                    {

                        Response.Write(exc.ToString());                   
                    }
                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——
                }
                else//验证失败
                {
                    Response.Write("fail");
                }


                Response.End();
            }
            log.Info("**********************支付宝回调autoreceive结束**********************");
            return View();
        
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
        }
        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }
       
    }
}
