
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Script.Serialization;
namespace CommonUtils
{
   public class ReturnMsg
    {
       public bool Status { get; set; }
       public string StatusString { get; set; }
       public object Tag { get; set; }
       public string Msg { get; set; }

       /// <summary>
       /// 反序列化参数
       /// </summary>
       /// <param name="json"></param>
       /// <returns></returns>
       public Dictionary<string, string> Args2Obj(string json)
       {
           JavaScriptSerializer jss = new JavaScriptSerializer();
         return  jss.Deserialize<Dictionary<string,string>>(json);       
       }
       public Dictionary<string, string> Args2Obj(object obj)
       {
           var dic = (Dictionary<string, string>)obj;

           return dic;           
       }
    }
}
