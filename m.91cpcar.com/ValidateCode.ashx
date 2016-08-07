<%@ WebHandler Language="C#" Class="ValidateCode" %>

using System;
using System.Web;
using System.Drawing;
using System.Web.SessionState;
public class ValidateCode : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        
        context.Response.ContentType = "image/Jpeg";
        string checkCode = GetRandomCode(4);
        context.Session["VerCode"] = checkCode;
        CreateImage(checkCode, context);
       
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
            int t = rand.Next(35);
            if (temp == t)
            {
                return CreateRandomCode(codeCount);//性能问题
            }
            temp = t;
            randomCode += allCharArray[t];
        }
        return randomCode;
    }
    private string GetRandomCode(int CodeCount)
    {
        string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,i,J,K,M,N,P,Q,R,S,T,U,W,X,Y,Z";
        string[] allCharArray = allChar.Split(',');
        string RandomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < CodeCount; i++)
        {
            if (temp != -1)
            {
                rand = new Random(temp * i * ((int)DateTime.Now.Ticks));
            }

            int t = rand.Next(33);

            while (temp == t)
            {
                t = rand.Next(33);
            }

            temp = t;
            RandomCode += allCharArray[t];
        }

        return RandomCode;
    }
    private void CreateImage(string checkCode,HttpContext context)
    {
        int iwidth = (int)(checkCode.Length * 16);
        System.Drawing.Bitmap image = new System.Drawing.Bitmap(100, 50);// new System.Drawing.Bitmap(iwidth, 30);
        Graphics g = Graphics.FromImage(image);
        Font f = new System.Drawing.Font("Arial ", 20);//, System.Drawing.FontStyle.Bold);
        Brush b = new System.Drawing.SolidBrush(Color.Black);
        Brush r = new System.Drawing.SolidBrush(Color.FromArgb(166, 8, 8));

        //g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
        //			g.Clear(Color.AliceBlue);//背景色
        g.Clear(System.Drawing.ColorTranslator.FromHtml("#99C1CB"));//背景色

        char[] ch = checkCode.ToCharArray();
        for (int i = 0; i < ch.Length; i++)
        {
            if (ch[i] >= '0' && ch[i] <= '9')
            {
                //数字用红色显示
                g.DrawString(ch[i].ToString(), f, r, 3 + (i * 15), 3);
            }
            else
            {   //字母用黑色显示
                g.DrawString(ch[i].ToString(), f, b, 3 + (i * 15), 3);
            }
        }

        //for循环用来生成一些随机的水平线
        			Pen blackPen = new Pen(Color.Black, 0);
                    Random rand = new Random();
                    //for (int i=0;i<5;i++)
                    //{
                    //    int y = rand.Next(image.Height);
                    //    g.DrawLine(blackPen,0,y,image.Width,y);
                    //}

                    //画图片的背景噪音点
                    for (int i = 0; i < 15; i++)
                    {
                        int x1 = rand.Next(image.Width);
                        int x2 = rand.Next(image.Width);
                        int y1 = rand.Next(image.Height);
                        int y2 = rand.Next(image.Height);
                        g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                    }

                    //画图片的背景噪音线 
                    for (int i = 0; i < 2; i++)
                    {
                        int x1 = 0;
                        int x2 = image.Width;
                        int y1 = rand.Next(image.Height);
                        int y2 = rand.Next(image.Height);
                        if (i == 0)
                        {
                            g.DrawLine(new Pen(Color.Gray, 2), x1, y1, x2, y2);
                        }

                    }
        
        
        
        
        
        

        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //history back 不重复 
        context.Response.Cache.SetNoStore();//这一句 		
        context.Response.ClearContent();
        context.Response.ContentType = "image/Jpeg";
        context.Response.BinaryWrite(ms.ToArray());
        g.Dispose();
        image.Dispose();
    }       

}