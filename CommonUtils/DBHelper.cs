/*****************************************
*
*创建时间：2015/3/4 10:48:28
*作者：陆敬宗
*说明：
*
*
*
******************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAO;
using AOP;
using System.Data.SqlClient;
using System.Reflection;
namespace CommonUtils
{
    /// <summary>
    /// 通用增删改
    /// </summary>
   public class DBHelper:BaseBusiness
    {
       List<SqlParameter> parameters = new List<SqlParameter>();
       /// <summary>
       /// 添加
       /// </summary>
       /// <param name="model">Model</param>
       /// <returns>影响行</returns>
       public  int Add(object model)
       {                  
           StringBuilder sb = new StringBuilder();
           StringBuilder att = new StringBuilder();
           StringBuilder va = new StringBuilder();
           string TableName = getModelTableName(model);
            Type tp=model.GetType();
            PropertyInfo[] p = tp.GetProperties();//获取该model的所有属性
            foreach (PropertyInfo i in p)//遍历model属性值
            {
                object vl = i.GetValue(model, null);
                if (vl != null && vl.ToString() != "")//数据有效
                {
                    att.Append(i.Name + ",");
                    //参数化执行
                    va.Append("@" + i.Name + ",");
                    AddSqlParams(new SqlParameter("@" + i.Name, vl));
                }
            }
           sb.Append("INSERT INTO  ");
           sb.Append(TableName+"(");
           sb.Append(att.ToString().Remove(att.Length - 1)+")");
           sb.Append(" VALUES (");
           sb.Append(va.ToString().Remove(va.Length - 1)+")");

           int result = db.Database.ExecuteSqlCommand(sb.ToString(), parameters.ToArray());
          return result;
       }
       /// <summary>
       /// 获取第一行第一列
       /// </summary>
       /// <returns></returns>
       public object GetExecuteScalar(string  TName,string col,string strwhere)
       {
           string sql = " select "+col+" from ["+TName+"] ";
           if (strwhere.Length > 0)
           { 
           sql+=" where "+strwhere;
           }
          return db.Database.SqlQuery<object>(sql);
       }
       /// <summary>
       ///修改
       /// </summary>
       /// <param name="model"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public int Update(object model,string strWhere)
       {
           StringBuilder sb = new StringBuilder();
           StringBuilder setval = new StringBuilder();
           StringBuilder va = new StringBuilder();
           string TableName = getModelTableName(model);
           Type tp = model.GetType();
           PropertyInfo[] p = tp.GetProperties();//获取该model的所有属性
           foreach (PropertyInfo i in p)//遍历model属性值
           {
               object vl = i.GetValue(model, null);
               if (vl != null && vl.ToString() != "")//数据有效
               {
                   setval.Append(i.Name + "=" + "@" + i.Name + "," );
                   //参数化执行                  
                   AddSqlParams(new SqlParameter("@" + i.Name, vl));
               }
           }
           sb.Append("UPDATE " + TableName + " SET  ");
           sb.Append(setval.ToString().Remove(setval.Length - 1));         
           sb.Append(" where "+strWhere);
           int result = db.Database.ExecuteSqlCommand(sb.ToString(), parameters.ToArray());
           return result;
       }
       /// <summary>
       /// 是否存在
       /// </summary>
       /// <param name="TName"></param>
       /// <param name="col"></param>
       /// <param name="strwhere"></param>
       /// <returns></returns>
       public bool Exist(string TName, string col, string strwhere)
       {
         int re=Convert.ToInt32( GetExecuteScalar(TName,col,strwhere));
         if (re > 0)
         {
             return true;
         }
         else {
             return false;
         }
       }
       /// <summary>
       /// 获得一对象
       /// </summary>
       /// <param name="model"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public object SelectToModel(object obj,string strWhere,params string[] arr)
       {
           string columnstring="";
           string sql = "select ";
           if (arr.Length > 0)
           {
               foreach (string i in arr)
               {
                   columnstring += (i + ",");
               }
               columnstring = columnstring.Substring(0, columnstring.Length-1);
           }
           if(string.IsNullOrEmpty(columnstring))
           {
           sql+="*";
           }else{
           sql+=columnstring;
           }
           sql += " from " + getModelTableName(obj) ;
             if (!string.IsNullOrEmpty(strWhere))
             {
                 sql += " where ";
             }
             sql += strWhere;
           object model= db.Database.SqlQuery<object>(sql).FirstOrDefault();
           return model;
       }
       
       /// <summary>
       /// SelectToDictionary
       /// </summary>
       /// <param name="model"></param>
       /// <param name="strWhere"></param>
       /// <param name="arr"></param>
       /// <returns></returns>
       public Dictionary<string,object> SelectToDictionary(object model, string strWhere, params string[] arr)
       {
           if (model != null)
           {
               string columnstring = "";
               string sql = "select top 1 ";
               if (arr.Length > 0)
               {
                   foreach (string i in arr)
                   {
                       columnstring += (i + ",");
                   }
                   columnstring = columnstring.Substring(0, columnstring.Length - 1);
               }
               if (string.IsNullOrEmpty(columnstring))
               {
                   sql += "*";
               }
               else
               {
                   sql += columnstring;
               }
               sql += " from " + getModelTableName(model) + " where " + strWhere;
               object obj = db.Database.SqlQuery<object>(sql).FirstOrDefault();
               return DataToDictionary(obj);
           }
           return null;
       }
       public Dictionary<string, object> SelectToDictionary(string sql)
       {
           object obj = db.Database.SqlQuery<object>(sql).FirstOrDefault();
           return DataToDictionary(obj);
       }
       /// <summary>
       /// 获取列表
       /// </summary>
       /// <param name="TName"></param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
       public List<object> GetList(object model, string strWhere, params string[] arr)
       {
           if (model != null)
           {
               string columnstring = "";
               string sql = "select ";
               if (arr.Length > 0)
               {
                   foreach (string i in arr)
                   {
                       columnstring += (i + ",");
                   }
                   columnstring = columnstring.Substring(0, columnstring.Length - 1);
               }
               if (string.IsNullOrEmpty(columnstring))
               {
                   sql += "*";
               }
               else
               {
                   sql += columnstring;
               }
               sql += " from " + getModelTableName(model)  ;
               if (!string.IsNullOrEmpty(strWhere))
               {
                   sql += " where ";
               }
               sql += strWhere;//model.GetType()    
               
               return db.Database.SqlQuery<object> (sql).ToList();
           } return null;
       }
       
       public int Del()
       {
           return 0;
       }
       /// <summary>
       /// 获取Model对应表名
       /// </summary>
       /// <param name="model"></param>
       /// <returns></returns>
       private static string getModelTableName(object model)
       {
           return "[" + model.GetType().Name + "]";
           //return model.GetType().GetField("tablename").GetValue(model).ToString();
       }
       /// <summary>
       /// 装载参数
       /// </summary>
       /// <param name="p"></param>
       private  void AddSqlParams(SqlParameter p)
       {
          parameters.Add(p);
       }
       private Dictionary<string, object> DataToDictionary(object obj)
       {
           Dictionary<string, object> dic = new Dictionary<string, object>();
           Type tp = obj.GetType();
           PropertyInfo[] p = tp.GetProperties();
           foreach(PropertyInfo i in p)
           {
               if (!dic.ContainsKey(i.Name))
               {
                   dic.Add(i.Name, i.GetValue(obj,null));
               }
               else
               {
                   dic[i.Name] = i.GetValue(obj, null);
               }
           }
           return dic;
       }

    }
}
