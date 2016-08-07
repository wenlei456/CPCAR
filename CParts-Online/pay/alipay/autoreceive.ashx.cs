using System;
using System.Collections.Generic;
using System.Web;
using Com.Alipay;
using System.Collections.Specialized;
using System.Xml;
using log4net;
using System.Reflection;
using OrderModule;

namespace Pay.Alipay
{
    /// <summary>
    /// autoreceive 的摘要说明
    /// </summary>
    public class autoreceive : IHttpHandler
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       
        public void ProcessRequest(HttpContext context)
        {
            log.Debug("**********************支付宝回调autoreceive**********************");
            Dictionary<string, string> sPara = GetRequestPost();
            log.Debug("参数个数：" + sPara.Count);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.VerifyNotify(sPara, context.Request.Form["sign"]);
                log.Debug("autoreceive是否支付宝发出的验证：" + verifyResult + "参数sPara：" + sPara + "参数sign：" + context.Request.Form["sign"]);
              
                if (verifyResult)//验证成功
                {
                    log.Debug("autoreceive验证成功");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码
                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                    //解密（如果是RSA签名需要解密，如果是MD5签名则下面一行清注释掉）
                    sPara = aliNotify.Decrypt(sPara);

                    //XML解析notify_data数据
                    try
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(sPara["notify_data"]);
                        //商户订单号
                        string out_trade_no = xmlDoc.SelectSingleNode("/notify/out_trade_no").InnerText;
                        //支付宝交易号
                        string trade_no = xmlDoc.SelectSingleNode("/notify/trade_no").InnerText;

                        string trade_status = xmlDoc.SelectSingleNode("/notify/trade_status").InnerText;
                        log.Debug("autoreceive商户订单号:" + out_trade_no + "支付宝交易号：" + trade_no + "支付宝交易状态：" + trade_status);
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
                            context.Response.Write("success");    //请不要修改或删除

                        }
                        if (trade_status == "TRADE_SUCCESS")
                        {
                            DAO.OrderPay op = obll.GetOrderPay(out_trade_no);
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
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

                            context.Response.Write("success");  //请不要修改或删除
                        }
                        else
                        {
                            context.Response.Write(trade_status);
                        }

                    }
                    catch (Exception exc)
                    {

                        context.Response.Write(exc.ToString());
                        //MJZCake.Utility.Common.Log(exc.Message, MJZCake.Utility.Common.LogLevel.high, "mjz");
                    }



                    //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                }
                else//验证失败
                {
                    context.Response.Write("fail");
                }


                context.Response.End();
            }
        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestPost()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = HttpContext.Current.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], HttpContext.Current.Request.Form[requestItem[i]]);
            }

            return sArray;
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}