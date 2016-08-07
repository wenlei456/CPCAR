using AOP;
using CommonUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberModule.Business
{
    public class ExpRecordBLL : BaseBusiness
    {
        //添加记录
       public void AddRecord(DAO.ExpRecord m)
       {
           db.ExpRecord.Add(m);
           db.SaveChanges();
       }


       public  bool GiveExp(DAO.MemberBase mb, decimal orderPrice, string orderNumber)
       {

           DAO.ExpRecord extModel = new DAO.ExpRecord();
           extModel.Uid = mb.ID;
           extModel.Source = "订单";
           extModel.OrderNum = orderNumber;
           extModel.OrderPrice = orderPrice;
           extModel.Datetime = DateTime.Now;
           extModel.Status = 0;
           //判断用户注册类型 计算积分
           if (mb.UpUser == -1)
           {
               //独立注册
               decimal JF = Math.Floor(orderPrice * decimal.Parse(ConfigurationApp.XFJFXS));
               extModel.Exp = Convert.ToInt32(decimal.Parse(ConfigurationApp.SM) * JF);
               if (extModel.Exp > 0)
               {
                   db.ExpRecord.Add(extModel);
               }
           }
           else
           {
               //被推荐注册的               
               decimal JF = Math.Floor(orderPrice * decimal.Parse(ConfigurationApp.XFJFXS));
               extModel.Exp = Convert.ToInt32(decimal.Parse(ConfigurationApp.CM) * JF);
               if (extModel.Exp > 0)
               {
                   db.ExpRecord.Add(extModel);
               }
               //给上级积分             
               extModel.Uid = mb.UpUser;
               extModel.Source = "推广订单";
               extModel.LowerUid =mb.ID;
               extModel.Exp =Convert.ToInt32(Math.Floor(orderPrice / 100));//每100得到1分
               if (extModel.Exp > 0)
               {
                   db.ExpRecord.Add(extModel);
               }
           }
          int r= db.SaveChanges();
          if (r > 0)
          {
              return true;
          }
          else {
              return false;
          }
       }
        /// <summary>
        /// 获取积分记录
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
       public List<DAO.ExpRecord> GetExpList(string uid)
       {
           string sql = "SELECT [ID],[Uid],[Source],[LowerUid],[OrderNum] ,[OrderPrice] ,[Exp] ,[Balance] ,[Datetime] ,[Status] FROM [ExpRecord] where Uid={0} ";
           List<DAO.ExpRecord> expList= db.Database.SqlQuery<DAO.ExpRecord>(string.Format(sql,uid)).ToList();
           return expList;
       }
    }
}
