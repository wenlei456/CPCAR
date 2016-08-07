
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using WxPayAPI;

namespace cpcar.com.weixin
{
    public partial class ResultNotifyPage : System.Web.UI.Page
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected void Page_Load(object sender, EventArgs e)
        {
            log.Info("==========微信异步回调开始===============");
            ResultNotify resultNotify = new ResultNotify(this);
            resultNotify.ProcessNotify();
           

            log.Info("==========微信异步回调结束===============");
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nvc"></param>
        /// <returns></returns>
        public static string GetPost(string url,  Dictionary<string, string> dic)
        {
            NameValueCollection nvc = new NameValueCollection();
            foreach (KeyValuePair<string, string> item in dic)
            {
                if (item.Key != "")
                {
                    nvc.Add(item.Key, item.Value);
                }
            }
            string content = new JavaScriptSerializer().Serialize(dic);
            //Common.Log(content, Common.LogLevel.low, "微信支付回调-postData");
            string result = string.Empty;
            byte[] data = Encoding.UTF8.GetBytes(content);
            HttpWebRequest r = HttpWebRequest.Create(url) as HttpWebRequest;
            r.ContentType = "application/x-www-form-urlencoded";
            r.Method = "POST";
            r.ContentLength = data.Length;
            using (Stream s = r.GetRequestStream())
            {
                s.Write(data, 0, data.Length);
            }
            using (Stream s = r.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(s);
                result = reader.ReadToEnd();
            }
            return result;


        }

        public static string GetRequest(string url)
        {
            string result = string.Empty;
            HttpWebRequest r = HttpWebRequest.Create(url) as HttpWebRequest;
            r.Method = "GET";
            using (Stream s = r.GetResponse().GetResponseStream())
            {
                StreamReader reader = new StreamReader(s);
                result = reader.ReadToEnd();
            }
            return result;
        }

        private static string BuildParms(NameValueCollection nvc)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in nvc)
            {
                sb.AppendFormat("{0}={1}&", HttpUtility.UrlEncode(key, Encoding.UTF8), HttpUtility.UrlEncode(nvc[key], Encoding.UTF8));
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 解析一级Json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static Dictionary<string,string> ChangeList(string json)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string pattern = @"({?(?<key>.*?):(?<value>.*?),)|((?<key>.*?):(?<value>.*)?})";
            MatchCollection matList = Regex.Matches(json, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline);
            foreach (Match mat in matList)
            {
                string key = mat.Groups["key"].Value.Replace("\"", "").Replace(" ", "");
                string value = mat.Groups["value"].Value.Replace("\"", "").Replace(" ", "");
                dic.Add(key, value);
            }
            return dic;
        }
    }
    public class AccessTokenJson
    {
        public string Access_Token { get; set; }
        public string Expires_In { get; set; }
    }
}