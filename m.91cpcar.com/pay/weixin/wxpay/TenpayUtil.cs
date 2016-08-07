using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MJZCake.Web.pay.weixin.wxpay
{
    /// <summary>
    /// TenpayUtil 的摘要说明。
    /// 配置文件
    /// </summary>
    public class TenpayUtil
    {
        public static string tenpay = "1";
        public static string partner = "1900000109";                   //商户号
        public static string key = "b202ba093cbf1db40d32058e2d29e8d9";  //密钥
        public static string appid = "wx1898f60d404422c1";//appid
        public static string appkey = "W5exQGeWMoS4WHwTZ1gFn7XWAHqYlGA8pkVXiUP4RTWBTI2xHgvqyAnkTzeFjYedatRhNAeTyl7ebx7SXsQg3IaZ7b1WUwBkrFNS1EjAztMAzhgCJqDgi4WNFyV3DDYt";//paysignkey(非appkey) 
        public static string tenpay_notify = "http://testbbb.bestcake.com/pay/weixin/getnative.aspx"; //支付完成后的回调处理页面,*替换成notify_url.asp所在路径

        public TenpayUtil()
        {

        }
        public static string getNoncestr()
        {
            Random random = new Random();
            return Wxpay.MD5Util.GetMD5(random.Next(1000).ToString(), "GBK");
        }


        public static string getTimestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }


        /** 对字符串进行URL编码 */
        public static string UrlEncode(string instr, string charset)
        {
            //return instr;
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;
            }
        }

        /** 对字符串进行URL解码 */
        public static string UrlDecode(string instr, string charset)
        {
            if (instr == null || instr.Trim() == "")
                return "";
            else
            {
                string res;

                try
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding(charset));

                }
                catch (Exception ex)
                {
                    res = HttpUtility.UrlDecode(instr, Encoding.GetEncoding("GB2312"));
                }


                return res;

            }
        }


        /** 取时间戳生成随即数,替换交易单号中的后10位流水号 */
        public static UInt32 UnixStamp()
        {
            TimeSpan ts = DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            return Convert.ToUInt32(ts.TotalSeconds);
        }
        /** 取随机数 */
        public static string BuildRandomStr(int length)
        {
            Random rand = new Random();

            int num = rand.Next();

            string str = num.ToString();

            if (str.Length > length)
            {
                str = str.Substring(0, length);
            }
            else if (str.Length < length)
            {
                int n = length - str.Length;
                while (n > 0)
                {
                    str.Insert(0, "0");
                    n--;
                }
            }

            return str;
        }

    }
}