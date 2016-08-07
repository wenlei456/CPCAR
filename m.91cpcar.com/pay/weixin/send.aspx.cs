using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using log4net;
using System.Reflection;
using OrderModule;
using OrderModule.model;
using WxPayAPI;
namespace m.cpcar.com.weixin
{
     
    public partial class send : System.Web.UI.Page
    {       
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        OrderBuss obll = new OrderBuss();
        public static string packString  {get;set;} 
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Info("*******************微信支付处理开始****************************");
             string orderId = Request.QueryString["oid"];
             string state = Request.QueryString["state"];
            string openId=Request.QueryString["openid"];
            if (!string.IsNullOrEmpty(state) && string.IsNullOrEmpty(orderId))
            {
                orderId = state;
            }
            log.Info("WXPay.Send---->orderId--->" + orderId + "&&openID=" + openId + "&&state" + state);
             JsApiPay jsApiPay = new JsApiPay(this);
            
            if (string.IsNullOrEmpty(openId))
             {
                 jsApiPay.GetOpenidAndAccessToken(orderId);
                 openId = jsApiPay.openid;
                 log.Info("WXPay.Send 获得---->openid：" + openId);
                 if (string.IsNullOrEmpty(openId))
                 {
                     log.Info("依然没有得到openid");
                     return;
                 }
                 else
                 {
                     log.Info("即将刷新送出openID和Oid");
                     Response.Redirect("/pay/weixin/send.aspx?oid=" + orderId + "&&openid=" + openId);
                 }
             }
             else
             {

                 //付款金额，必填
                 decimal total_fee = 0;
                 if (!string.IsNullOrEmpty(orderId))
                 {
                     log.Info("订单号：" + orderId);
                     try
                     {
                         OrderDetail orderDetail = obll.OrderDetail(orderId);
                         OrderDetailPay od = new OrderDetailPay();
                         od.OrderDetailsPayInit(orderId, orderDetail);
                         total_fee = od.Price * 100;



                         //JSAPI支付预处理
                       //  jsApiPay.total_fee = Convert.ToInt32(total_fee);
                         string body = orderDetail.opList[0].ProductName + "x" + orderDetail.opList[0].Num;
                         WxPayData unifiedOrderResult = jsApiPay.GetUnifiedOrderResult(body, orderId, Convert.ToInt32(total_fee),openId);
                         packString = jsApiPay.GetJsApiParameters();//获取H5调起JS API参数                    
                         log.Info("WXPay.Send---->wxJsApiParam : " + packString);


                         /**
                         Wxpay.WxPayHelper wxPayHelper = new Wxpay.WxPayHelper();
                         //先设置基本信息
                         wxPayHelper.SetAppId(_APPID);
                         wxPayHelper.SetAppKey(_APPSECRET);
                         wxPayHelper.SetPartnerKey(_KEY);
                         wxPayHelper.SetSignType("SHA1");
        
                             //设置请求package信息
                             wxPayHelper.SetParameter("bank_type", "WX");
                             wxPayHelper.SetParameter("body", orderDetail.opList[0].ProductName + "x" + orderDetail.opList[0].Num);
                             log.Info("body：" + orderDetail.opList[0].ProductName + "x" + orderDetail.opList[0].Num);
                             wxPayHelper.SetParameter("partner", _MCHID);
                             wxPayHelper.SetParameter("out_trade_no", orderId.ToString());
                             wxPayHelper.SetParameter("total_fee", total_fee.ToString());
                             log.Info("total_fee:" + total_fee);
                             wxPayHelper.SetParameter("fee_type", "1");
                             wxPayHelper.SetParameter("notify_url", ConfigurationManager.AppSettings["notifyUrl"]);
                             wxPayHelper.SetParameter("spbill_create_ip", Request.UserHostAddress);
                             wxPayHelper.SetParameter("input_charset", "UTF-8");
                             packString = wxPayHelper.CreateBizPackage();****/
                     }
                     catch (Exception ex)
                     {
                         log.Info(ex.Message + ex.StackTrace);
                     }

                 }
                 else
                 {
                     log.Info("xxxOrderID 为空xxx");
                 }
             }
            log.Info("*******************微信支付处理结束****************************");

        }
    }
}