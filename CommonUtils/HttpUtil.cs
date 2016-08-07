using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CommonUtils
{
   public  class HttpUtil
    {
       /// <summary>
       /// http请求
       /// </summary>
       /// <param name="url"></param>
       /// <returns></returns>
       public string HttpRequest(string url) {
           ASCIIEncoding encoding = new ASCIIEncoding();

           // Prepare web request...
           HttpWebRequest myRequest =
            (HttpWebRequest)WebRequest.Create(url);

           myRequest.Method = "GET";
           myRequest.ContentType = "application/x-www-form-urlencoded";

           // Get response
           HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
           StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
           string content = reader.ReadToEnd();
           return content;
       }


        public Dictionary<string, object> GetHttpData(string url)
        {
            string content = this.HttpRequest(url);
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("(", "").Replace(")", "").Replace(";", "");
                content = content.ToString().Replace("var", "").Replace("remote_ip_info", "").Replace("=", "");
                content = HttpUtility.UrlDecode(content, Encoding.GetEncoding("UTF-8"));
            }
            Dictionary<string, object> result = JsonToDictionary(content);
            return result;

        }
        /// <summary>
        /// 将json数据反序列化为Dictionary
        /// </summary>
        /// <param name="jsonData">json数据</param>
        /// <returns></returns>
        public Dictionary<string, object> JsonToDictionary(string jsonData)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 获取本机的网络IP
        /// </summary>
        /// <returns></returns>
        public string GetIp()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            return result;
        }

        /// <summary>
        /// 获取服务器地址
        /// </summary>
        /// <returns></returns>
        public string GetServerIp()
        {
            WebRequest wr = WebRequest.Create("http://www.ip138.com/ips138.asp");
            Stream s = wr.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, Encoding.Default);
            string all = sr.ReadToEnd(); //读取网站的数据

            int start = all.IndexOf("您的IP地址是：[") + 9;
            int end = all.IndexOf("]", start);
            string tempip = all.Substring(start, end - start);
            return tempip;
        }

        /// <summary>
        /// 根据IP获得城市名称
        /// </summary>
        /// <returns></returns>
        public string GetCityByIp()
        {
           string  msg = "";
            string ip = this.GetIp();
            msg += "ip:" + ip;
            if ("::1".Equals(ip))
            {
                ip = this.GetServerIp();
                msg += "serverip:" + ip;
            }
            ip = ip.Split(',')[0];
            //string ip = "218.4.57.233";
            string url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=";
            url += ip;
            ASCIIEncoding encoding = new ASCIIEncoding();
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            
            myRequest.Method = "GET";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            string ret = "";
            try
            {
                // Get response
                HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                StreamReader reader = new StreamReader(myResponse.GetResponseStream(), Encoding.UTF8);
                string content = reader.ReadToEnd();
                msg += "----content:" + content;
            if (!string.IsNullOrEmpty(content))
            {
                content = content.Replace("(", "").Replace(")", "").Replace(";", "");
                content = content.ToString().Replace("var", "").Replace("remote_ip_info", "").Replace("=", "");
                content = HttpUtility.UrlDecode(content, Encoding.GetEncoding("UTF-8"));
            }
                msg += "----resultcontent:" + content;
                Dictionary<string, object> result = JsonToDictionary(content);
                ret = result["city"].ToString();
                msg += "ret:" + ret;
            }
            catch (Exception e)
            {
                msg += "exception:" + e.Message;
            }
            return ret;
            //string ip = this.GetIp();
            //if ("::1".Equals(ip))
            //{
            //    ip = this.GetServerIp();
            //}
            ////string ip = "218.4.57.233";
            //string url = "http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=js&ip=";
            //url += ip;
            
            //string content = HttpRequest(url);
            //if (!string.IsNullOrEmpty(content))
            //{
            //    content = content.Replace("(", "").Replace(")", "").Replace(";", "");
            //    content = content.ToString().Replace("var", "").Replace("remote_ip_info", "").Replace("=", "");
            //    content = HttpUtility.UrlDecode(content, Encoding.GetEncoding("UTF-8"));
            //}
            //Dictionary<string, object> result = JsonToDictionary(content);
            //string ret = result["city"].ToString();
            //return ret;

        }
    }
}
