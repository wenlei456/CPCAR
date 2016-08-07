
using log4net;
using System;
using System.Reflection;
using System.Web;
using tenpayApp;
namespace m.bestcake.com.pay.weixin.payback
{
    /// <summary>
    /// PaybackHandler 的摘要说明
    /// </summary>
    public class PaybackHandler : IHttpHandler
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
  
        public void ProcessRequest(HttpContext context)
        {
            log.Info("=========微信回调处理开始================");
            log.Info("*******进入PaybackHandler************");
            context.Response.ContentType = "text/plain";
            ResponseHandler resHandler = new ResponseHandler(context);
            resHandler.init();
            resHandler.setKey(TenpayUtil.key, TenpayUtil.appkey);
            //判断签名
            if (resHandler.isTenpaySign())
            {
                log.Debug("判断财付通签名成功");
                if (resHandler.isWXsign())
                {
                    log.Debug("判断微信签名成功");
                    //商户在收到后台通知后根据通知ID向财付通发起验证确认，采用后台系统调用交互模式
                    string notify_id = resHandler.getParameter("notify_id");//通知的id
                    string out_trade_no = resHandler.getParameter("out_trade_no");//商户订单号
                    string transaction_id = resHandler.getParameter("transaction_id");//微信订单号
                    string total_fee = resHandler.getParameter("total_fee");//减去折扣的价格
                    string discount = resHandler.getParameter("discount");//折扣价格
                    string trade_state = resHandler.getParameter("trade_state");//支付结果

                    //即时到账
                    if ("0".Equals(trade_state))
                    {
                        log.Debug("即时到账成功");
                        //------------------------------
                        //处理业务开始
                        //------------------------------

                        //处理数据库逻辑
                        //注意交易单不要重复处理
                        //注意判断返回金额
                        if (!string.IsNullOrEmpty(out_trade_no))
                        {
                            log.Debug("开始修改订单状态");
                            if (SetOrderState(out_trade_no))
                            {
                                log.Debug("订单支付状态修改成功");
                            }
                            else {
                                log.Debug("订单支付状态修改失败");
                            }
                        }
                        //------------------------------
                        //处理业务完毕
                        //------------------------------
                        //给财付通系统发送成功信息，财付通系统收到此结果后不再进行后续通知
                        context.Response.Write("success 后台通知成功");
                    }
                    else
                    {
                        log.Debug("支付失败");
                        context.Response.Write("支付失败");
                    }
                    //回复服务器处理成功
                    context.Response.Write("success");
                    log.Debug("success");
                }

                else
                {//SHA1签名失败
                    context.Response.Write("fail -SHA1 failed");               
                    context.Response.Write(resHandler.getDebugInfo());
                    log.Debug("SHA1签名失败");
                }
            }

            else
            {//md5签名失败
                context.Response.Write("fail -md5 failed");               
                context.Response.Write(resHandler.getDebugInfo());
                log.Debug("md5签名失败");
            }
        }
        /// <summary>
        /// 支付成功设置订单状态
        /// </summary>
        /// <returns></returns>
        public bool SetOrderState(string oid)
        {
       
            try
            {
                log.Info("PaybackHandler.ashx.cs");
                return true;
            }
            catch (Exception e)
            {
                log.Debug("发生异常信息：");
                log.Debug(e.StackTrace);
                log.Debug(e.Message);
                return false;
            }
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