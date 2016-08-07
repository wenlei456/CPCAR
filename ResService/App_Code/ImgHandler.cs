using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace myHandlers
{
    /// <summary>
    /// ImgHandler 的摘要说明
    /// </summary>
    public class ImgHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            //站点的域名          
            string domain = ConfigAppSetting.imgHandlerDomain;
            if (context.Request.UrlReferrer == null ||
                context.Request.UrlReferrer.Host.ToLower().IndexOf(domain) < 0)
            {
                //如果是通过浏览器直接访问或者是通过其他站点访问过来的，则显示“资源不存在”图片
                context.Response.ContentType = "image/JPEG";
                context.Response.WriteFile(context.Server.MapPath("~/upload/noimg.jpg"));
            }
            else
            {
                //如果是通过站内访问的，这正常显示图片
                context.Response.ContentType = "image/JPEG";
                context.Response.WriteFile(context.Request.PhysicalPath);
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