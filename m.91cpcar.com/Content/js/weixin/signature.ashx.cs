using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace m.bestcake.com.wx
{
    /// <summary>
    /// signature 的摘要说明
    /// </summary>
    public class signature : IHttpHandler
    {
        private static string APPID = "wx1898f60d404422c1";
        private static string SECRET = "ac638e813f4c8d89c13ee1c9b76e4dcc";
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string url = context.Request.QueryString["url"];
            string noncestr = context.Request.QueryString["noncestr"];
            string timestamp = context.Request.QueryString["timestamp"];
            string re = "";
            if (!string.IsNullOrEmpty(noncestr) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(url))
            {

                // 获取Ticket
                if (HttpContext.Current.Cache["WEIXIN_CONFIG"] == null)
                {
                    this.RefreshConfig();
                }
                if (!url.Contains("http://"))
                {
                    url = "http://" + context.Request.Url.Host + url;
                }
                //url = context.Request.Url.Host + url;
                var config = HttpContext.Current.Cache["WEIXIN_CONFIG"] as WeixinConfig;
                string jsapi_ticket = config.Ticket;
                string str = "jsapi_ticket=JSAPI_TICKET&noncestr=NONCESTR&timestamp=TIMESTAMP&url=HTTPURL";
                str = str.Replace("JSAPI_TICKET", jsapi_ticket).Replace("NONCESTR", noncestr).Replace("TIMESTAMP", timestamp).Replace("HTTPURL", url);
                re = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1").ToLower();
            }
            context.Response.Write("var signature=\""+re+"\";");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private bool RefreshConfig()
        {
            try
            {
                WeixinConfig config = new WeixinConfig();

                //获取access_token （有效期7200秒，开发者必须在自己的服务全局缓存access_token）
                string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=SECRET";
                url = url.Replace("APPID", APPID);
                url = url.Replace("SECRET", SECRET);
                CommonUtils.HttpUtil util = new CommonUtils.HttpUtil();
                Dictionary<string, object> result = util.GetHttpData(url);

                if (!result.ContainsKey("access_token"))
                {
                    return false;
                }

                config.AccessToken = result["access_token"].ToString();

                //获取jsapi_ticket（有效期7200秒，开发者必须在自己的服务全局缓存jsapi_ticket）
                url = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token=ACCESS_TOKEN&type=jsapi";
                url = url.Replace("ACCESS_TOKEN", config.AccessToken);
                result = util.GetHttpData(url);

                if (!result.ContainsKey("ticket"))
                {
                    return false;
                }

                config.Ticket = result["ticket"].ToString();

                // 设置缓存失效时间7175秒
                HttpContext.Current.Cache.Insert("WEIXIN_CONFIG", config, null, DateTime.Now.AddSeconds(7175), System.Web.Caching.Cache.NoSlidingExpiration);

                return true;

            }
            catch
            {
                return false;
            }
        }
        private class WeixinConfig
        {
            /// <summary>
            /// 访问令牌
            /// </summary>
            public string AccessToken { get; set; }

            /// <summary>
            /// 微信jsapi_ticket
            /// </summary>
            public string Ticket { get; set; }
        }
    }
}