using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CommonUtils
{
    public  class CookieUtil
    {
        /// <summary>
        /// 设置cookies
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="hours"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SetCookie(string obj,int hours,string key) {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddHours(hours);
            cookie.Value = obj;
            HttpContext.Current.Response.Cookies.Add(cookie);
            return true;
        }

        /// <summary>
        /// 获得cookies值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string key)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[key];
            if (ck != null && !string.IsNullOrEmpty(ck.Value))
            {
                return HttpUtility.UrlDecode(ck.Value, Encoding.GetEncoding("UTF-8"));
            }
            return "";
        }

        /// <summary>
        /// 判断cookie是否存在
        /// </summary>
        /// <returns></returns>
        public static bool Exists(string key)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[key];
            bool flag = false;
            if (ck != null)
            {
                if (!string.IsNullOrEmpty(ck.Value))
                {
                    flag = true;
                }
            }
            return flag;

        }

        /// <summary>
        /// 清除 cookie
        /// </summary>
        public static void ClearCookie(string key)
        {
            HttpCookie ck = HttpContext.Current.Request.Cookies[key];
            if (ck != null)
            {
                ck.Values.Remove(key);
                ck.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(ck);
            }

        }
    }
}
