using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

   public class Log
    {

        public static void writeLog(List<string> mess)
        {

            string FileName = "log" + string.Format("{0:yyyyMMdd}", DateTime.Now) + ".txt";
            string FoldName =System.Web.HttpContext.Current.Server.MapPath("~/");
            string AllFILENAME = FoldName + FileName;
       
                if (File.Exists(AllFILENAME))
                {
                    FileStream newfile = new FileStream(AllFILENAME, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                    StreamWriter sw = new StreamWriter(newfile);                  
                    foreach (string ite in mess)
                    {
                        sw.WriteLine(DateTime.Now.ToString() + "\t" + ite);
                    }

                    sw.Dispose();
                    sw.Close();
                    newfile.Dispose();
                    newfile.Close();

                }
                else
                {
                    FileStream newfile = File.Create(AllFILENAME);
                    newfile.Dispose();
                    newfile.Close();
                    writeLog(mess);
                }
            }
           

        }


