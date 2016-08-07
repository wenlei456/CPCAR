<%@ WebHandler Language="C#" Class="UpLoadPage" %>

using System;
using System.Web;
using System.IO;
public class UpLoadPage : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        context.Response.ContentType = "text/plain";        
        HttpPostedFile file = context.Request.Files["FileData"];
        string path = context.Request["folder"] + "/Porduct/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
        string uploadpath = context.Server.MapPath(path);
        string tid =HttpUtility.HtmlEncode(context.Request.QueryString["pid"]);
        if (file != null)
        {
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
            string fileNew = CreateRandomCode(22)+file.FileName.Substring(file.FileName.LastIndexOf("."));//  file.FileName;

            Cparts_Service.ImgStockBll bll = new Cparts_Service.ImgStockBll();
            DAO.ImgStock img = new DAO.ImgStock();
            img.ImgPath = path + fileNew;
            img.PartID =int.Parse(tid);
            img.UploadDate = DateTime.Now;
            bll.Add(img);
            //DownRes upfiles = new DownRes();
            //upfiles.DT = Convert.ToInt32(tid);
            //upfiles.FileName = fileNew;
            //upfiles.FileSize = GetSize(file.ContentLength) ;
            //upfiles.UpDate = DateTime.Now;
            //upfiles.Add();
            file.SaveAs(uploadpath + fileNew);        
         
            context.Response.Write("1");
        }
        else
        {
           context.Response.Write("0");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
    private string CreateRandomCode(int codeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
        string[] allCharArray = allChar.Split(',');
        string randomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < codeCount; i++)
        {
            
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(32);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);//性能问题
                }
                temp = t;
                randomCode += allCharArray[t];
         
        }
        return randomCode;
    }
    public string GetSize(int filesize)
    {
        int i = Convert.ToInt32(filesize);
        string size = "";

        if (i > 0)
        {
            if ((i / 1024) < 1024)
            {
                size = i / 1024 + "K";
            }
            else
            {
                size = (i / 1024) / 1024 + "M";

            }
            return size;
        }
        else
        {
            return "0";
        }


    }
}