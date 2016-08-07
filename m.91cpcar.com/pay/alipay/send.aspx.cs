using System;
using System.Collections.Generic;
using Com.Alipay;
using log4net;
using System.Reflection;
using OrderModule;
using OrderModule.model;

namespace ECommerce.Web.pay.alipay
{
    
    public partial class send : System.Web.UI.Page
    {
        OrderBuss obll = new OrderBuss();
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        { 
            log.Info("*******************支付宝发送处理开始****************************");
            string oid = this.Request.QueryString["oid"];
            //商户订单号，商户网站订单系统中唯一订单号，必填
            string out_trade_no = oid;
            log.Info("订单号："+oid);
            //订单名称，必填
            string subject = "";
            //付款金额，必填
            string total_fee = "";
            //商品描述，可空
            string body = "";

            if (!string.IsNullOrEmpty(oid))
            {
                log.Info("进入主方法");           
                OrderDetail orderDetail = obll.OrderDetail(oid);
                OrderDetailPay od = new OrderDetailPay();
                od.OrderDetailsPayInit(oid, orderDetail);
                //商户订单号            
                log.Info(" 订单号:" + out_trade_no);
                total_fee =od.Price.ToString();
                log.Info(" 订单金额:" + total_fee);
                //float postPrice = float.Parse(od.Freight.Substring(1));
               // log.Info(" 运费:" + postPrice);
               /** if (postPrice > 0)
                {
                    this.total_fee = goodPrice.ToString("f2");
                    this.subject = od.Description + " ,运费：" + postPrice.ToString("f2");
                    log.Info("  合计 有运费的:" + this.subject);
                }**/
                    subject = od.Description;
                    body = od.Rinfo;
               //  log.Info("  合计:" + this.subject);
            }
            ////////////////////////////////////////////////////////////////////////////////////////////////

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", Config.service);
            sParaTemp.Add("partner", Config.partner);
            sParaTemp.Add("seller_id", Config.seller_id);
            sParaTemp.Add("_input_charset", Config.input_charset.ToLower());
            sParaTemp.Add("payment_type", Config.payment_type);
            sParaTemp.Add("notify_url", Config.notify_url);
            sParaTemp.Add("return_url", Config.return_url);
            sParaTemp.Add("anti_phishing_key", Config.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", Config.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", out_trade_no);
            sParaTemp.Add("subject", subject);
            sParaTemp.Add("total_fee", total_fee);
            sParaTemp.Add("body", body);
            //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
            //如sParaTemp.Add("参数名","参数值");

            //建立请求
            string sHtmlText = Submit.BuildRequest(sParaTemp, "post", "确认");
            Response.Write(sHtmlText);

            log.Info(" *******************支付宝发送处理结束**************************");
           
        }
    }
}