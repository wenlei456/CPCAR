using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Com.Alipay;
using System.Xml;
using System.Text;


namespace Pay.Alipay
{
    public class AlipayM
    {
        public static string ConfirmSend(long oid)
        {

         //by lu  GetOutBean op = new PayedComplete().GetOrderPay(oid);
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("partner", Config.Partner);
            sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
            sParaTemp.Add("service", "send_goods_confirm_by_platform");
          //by llu  sParaTemp.Add("trade_no", op.Remark.Split(',')[0]);
            sParaTemp.Add("logistics_name", "诚配科技");
            sParaTemp.Add("invoice_no", oid.ToString());
            sParaTemp.Add("transport_type", "EXPRESS");


            XmlDocument xmlDoc = new XmlDocument();
           // string r = Com.Alipay.Submit.BuildRequest(sParaTemp);
            string r = "";
            try
            {               
                xmlDoc.LoadXml(r);
                StringBuilder sbxml = new StringBuilder();
                string nodeIs_success = xmlDoc.SelectSingleNode("/alipay/is_success").InnerText;
                if (nodeIs_success != "T")//请求不成功的错误信息
                {
                    string errorMsg = xmlDoc.SelectSingleNode("/alipay/error").InnerText;
                    return errorMsg;
                }
                else//请求成功的支付返回宝处理结果信息
                {
                    return "ok";
                }
            }
            catch{
                return r;
            }
        }
    }
}