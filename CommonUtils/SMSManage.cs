using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOP;
using DAO;
namespace CommonUtils
{
  public  class SMSManage:BaseBusiness
    {
    
      /// <summary>
      /// 发送注册短信 验证码
      /// </summary>
      /// <param name="uid"></param>
      /// <param name="phone"></param>
      /// <returns></returns>
      public bool SendSMS(string uid,string phone)
      {
          DAO.PhoneMsg msg = new DAO.PhoneMsg();
          string code =CommonUtils.RandomCode.CreatPhoneRegCode();
          msg.UID = int.Parse(uid);
          msg.PhoneNum = phone;
          msg.Type = "reg";
          msg.State = "0";
          msg.Code = code;
          msg.MSG = string.Format(ConfigurationApp.temp, code);
          db.PhoneMsg.Add(msg);
         return  db.SaveChanges()>0?true:false;
      }
      /// <summary>
      /// 验证手机注册码
      /// </summary>
      /// <param name="code"></param>
      /// <returns></returns>
      public static bool VerRegCode(string pNumber ,string currCode)
      {
          CarPartsEntities db = new CarPartsEntities();
          string SQL = "SELECT Code FROM PhoneMsg where PhoneNum={0} and DATEDIFF(MINUTE,STime,GETDATE())<" + ConfigurationApp.timeout;
          string code = db.Database.SqlQuery<string>(string.Format(SQL, pNumber,currCode)).FirstOrDefault();
          if (code == currCode)
          {
              return true;
          }
          else
          {
              return false;
          }
      }
    }
}
