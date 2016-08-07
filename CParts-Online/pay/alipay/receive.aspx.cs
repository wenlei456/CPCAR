using System;
using System.Collections.Generic;
using Com.Alipay;
using System.Collections.Specialized;
using log4net;
using System.Reflection;
namespace OrderModule.Business.pay.alipay
{
    public partial class receive : System.Web.UI.Page
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Info("**********************支付宝回调receive**********************");
            //商户订单号
            string out_trade_no = Request.QueryString["out_trade_no"];
            Dictionary<string, string> sPara = GetRequestGet();
            //日志
            log.Info("商户订单号" + out_trade_no + ",参数个数：" + sPara.Count);
            if (sPara.Count > 0)//判断是否有带返回参数
            {
                Notify aliNotify = new Notify();
                bool verifyResult = aliNotify.VerifyReturn(sPara, Request.QueryString["sign"]);
                log.Info("是否支付宝发出的验证：" + verifyResult + "参数sPara：" + sPara + "参数sign：" + Request.QueryString["sign"]);
                //日志
                //   Common.Log(out_trade_no + ",回调判断：" + verifyResult, Common.LogLevel.low, "微网站支付宝回调");
                if (verifyResult)//验证成功
                {
                    log.Info("验证成功");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //请在这里加上商户的业务逻辑程序代码

                    //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                    //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                    //支付宝交易号
                    string trade_no = Request.QueryString["trade_no"];
                    
                    //交易状态
                    string trade_status = Request.QueryString["result"];
                    log.Info("支付宝交易号："+trade_no+"支付宝交易状态：" + trade_status);
                    //日志
                    //     Common.Log("订单号：" + out_trade_no + ",交易状态：" + trade_status, Common.LogLevel.low, "微网站支付宝回调");
                    /*liufang 修改了原OrderPay类*/
                    GetOutBean op = new GetOutBean();
                    OrderModule.business.OrderDetailsPay.OrderPayBusiness pc = new OrderModule.business.OrderDetailsPay.OrderPayBusiness();
                   
                    if (!string.IsNullOrEmpty(out_trade_no))
                    {
                        op = pc.Get(long.Parse(out_trade_no));
                        log.Info("OrderPay标示："+op.Bank+op.OrderId+op.PayState+op.ULoginName);
                    }
                    if (trade_status == "success")
                    {
                        //判断该笔订单是否在商户网站中已经做过处理
                        //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                        //如果有做过处理，不执行商户的业务程序
                        //注意：
                        //该种交易状态只在一种情况下出现——开通了高级即时到账，买家付款成功后。
                        //  Common.Log(out_trade_no + "已经进入支付判断", Common.LogLevel.low, "微网站支付宝回调");                     
                        //如果是蛋糕订单
                       if (op.PayState == "未支付" || op.PayState == "等待发货")
                        {
                            //日志
                            // Common.Log(out_trade_no + "|" + ",蛋糕订单变更结果：" + result + ",充值订单变更结果：" + reResult, Common.LogLevel.low, "微网站支付宝回调");
                            op.PayState = "已支付";
                            op.Bank = "支付宝";

                            string str = op.Remark;
                            op.Remark = string.Format("{1},由支付宝电子对账单更新{0:yyyy-MM-dd HH:mm:ss},状态为:等待用户确认收货", DateTime.Now, trade_no);
                            UpdateInBean inbean = new UpdateInBean();
                            inbean = (UpdateInBean)CommonUtils.TransObject.OToO(op, inbean);
                            bool re=pc.Update(inbean);
                            log.Info("****************OrderPay状态修改******************" + re);
                            pc.UpdateCompleteStatus((long)op.OrderId);//支付成功修改订单状态
                            log.Info("****************OrderPay状态修改******************" + re);
                        }

                        //打印页面
                        if (op.OrderType == 0)
                        {
                            Response.Redirect(string.Format("~/order-success/{0}", out_trade_no));
                        }
                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else//验证失败
                    {
                        Response.Write("验证失败");
                    }

                }
                else
                {
                    Response.Write("无返回参数");
                }
            }
        }


        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public Dictionary<string, string> GetRequestGet()
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
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

    }


}