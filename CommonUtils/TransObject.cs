/*****************************************
*
*创建时间：2015/4/7 19:22:28
*作者：陆敬宗
*说明：
*
*
*
******************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CommonUtils
{
  public  class TransObject
    {
      /// <summary>
      /// 常用语DAO与自定义Model相互包装
      /// </summary>
      /// <param name="inObj"></param>
      /// <param name="outObj"></param>
      /// <returns></returns>
      public static object OToO(object inObj,object outObj)
      {
          Type in_tp = inObj.GetType();
          PropertyInfo[] in_popinfo = in_tp.GetProperties();//获取该model的所有属性

          Type out_tp = outObj.GetType();
          PropertyInfo[] out_popinfo = out_tp.GetProperties();          
           List<string> atts_dao=GetAtts(out_popinfo);
          foreach (PropertyInfo i in in_popinfo)//遍历model属性值
          {     
              object vl = i.GetValue(inObj, null);
              if (vl != null && vl.ToString() != "")//数据有效
              {
                  if (atts_dao.Contains(i.Name))
                  {
                     PropertyInfo property= out_tp.GetProperty(i.Name);    
                      if (!property.PropertyType.IsGenericType)
                      {
                          //非泛型                         
                          property.SetValue(outObj, string.IsNullOrEmpty(vl.ToString()) ? null : Convert.ChangeType(vl, property.PropertyType), null);
                      }
                      else
                      {
                          //泛型Nullable<>
                          Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                          if (genericTypeDefinition == typeof(Nullable<>))
                          {
                              property.SetValue(outObj, string.IsNullOrEmpty(vl.ToString()) ? null : Convert.ChangeType(vl, Nullable.GetUnderlyingType(property.PropertyType)), null);
                          }
                      }
                  }                 
              }
          }
          return outObj;
      }
      /// <summary>
      /// 
      /// </summary>
      /// <param name="pinfo"></param>
      /// <returns></returns>
      private static List<string> GetAtts(PropertyInfo[] pinfo)
      {
          List<string> list=new List<string> ();
          //string[] atts;
          foreach (var a in pinfo)
          {
              list.Add(a.Name);
          }
          return list;
      }
    }
}
