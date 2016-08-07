<%@ WebHandler Language="C#" Class="imageUp" %>
using System;
using System.Web;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class imageUp : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentEncoding = System.Text.Encoding.UTF8;

      
        //List<string> mess = new List<string>();
        //mess.Add("开始日志**");
        string upDomain = ConfigAppSetting.upDomain;
        //mess.Add("upDomain**" + upDomain);
        string requestHost = context.Request.UrlReferrer.Host;
        //mess.Add("requestHost**" + requestHost);
        //mess.Add("requestHost.IndexOf(upDomain)**" + requestHost.IndexOf(upDomain));
        if (requestHost.IndexOf(upDomain)<0)
        {
            //mess.Add("进入小于0");
            context.Response.ContentType = "text/html";
            context.Response.Write("UpLoad Error!");
        }
        else
        {
            //mess.Add("上传文件");
            HttpPostedFile file = context.Request.Files[0];
           
            //上传配置
            string pathbase = "upload/images/";//保存路径
            int size = 10;                     //文件大小限制,单位mb                                                                                   //文件大小限制，单位KB
            string[] filetype = { ".gif", ".png", ".jpg", ".jpeg", ".bmp" };//文件允许格式
            string callback = context.Request["callback"];
            string editorId = context.Request["editorid"];
            //上传图片
            Hashtable info;
            CommonCode.Uploader up = new CommonCode.Uploader();
            info = up.upFile(context, pathbase, filetype, size); //获取上传状态
            string json = BuildJson(info);

            context.Response.ContentType = "text/html";
            if (callback != null)
            {
                context.Response.Write(String.Format("<script>{0}(JSON.parse(\"{1}\"));</script>", callback, json));
            }
            else
            {
                //mess.Add("<script>document.domain='" + upDomain + "'</script>");
                //Log.writeLog(mess);                
                context.Response.Write("<script>document.domain='" + upDomain + "'</script>");
                context.Response.Write(json);
            }
        }
    
        
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
    /// <summary>
    /// 返回json的格式
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private string BuildJson(Hashtable info)
    {
        List<string> fields = new List<string>();
        string[] keys = new string[] { "originalName", "name", "url", "size", "state", "type" };
        for (int i = 0; i < keys.Length; i++)
        {
            fields.Add(String.Format("\"{0}\": \"{1}\"", keys[i], info[keys[i]]));
        }
        return "{" + String.Join(",", fields) + "}";
    }

}