using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOP;
using CommonUtils;
using System.Reflection;
using LogModule;
using MemberModule.Model;
namespace MemberModule.Business
{
    /// <summary>
    /// 用户Bll
    /// 注册条件 1.验证信息重复 2.手机验证码3.
    /// </summary>
   public  class MemberBLL:BaseBusiness
    {
     
       /// <summary>
       /// 注册新用户
       /// </summary>
       /// <param name="user">用户</param>
       /// <param name="phoneCode">手机校验码</param>
       /// <returns></returns>
       public DAO.MemberBase Reg(DAO.MemberBase user)
       {  
         DAO.MemberBase m=  db.MemberBase.Add(user);           
               if(db.SaveChanges()>0)
               {                 
                   return m;
               }else{                  
                   return null;
               }
       
       }
       public bool SendSMS(string number, string phone)
       {
           DAO.PhoneMsg msg = new DAO.PhoneMsg();
           string code = number;
           msg.UID = 0;
           msg.PhoneNum = phone;
           msg.Type = "reg";
           msg.State = "0";
           msg.Code = code;
           msg.STime = DateTime.Now;
           msg.MSG = string.Format(ConfigurationApp.temp, code);
           db.PhoneMsg.Add(msg);
           return db.SaveChanges() > 0 ? true : false;
       }

       public bool SendOrderSMS(string number, string phone)
       {
           DAO.PhoneMsg msg = new DAO.PhoneMsg();
           string code = number;
           msg.UID = 0;
           msg.PhoneNum = phone;
           msg.Type = "order";
           msg.State = "0";
           msg.Code = code;
           msg.STime = DateTime.Now;
           msg.MSG = string.Format(ConfigurationApp.temp, code);
           db.PhoneMsg.Add(msg);
           return db.SaveChanges() > 0 ? true : false;
       }


       public DAO.MemberBase UpdateUser(DAO.MemberBase user)
       {
           db.Entry(user).State = System.Data.EntityState.Modified;
           int result = db.SaveChanges();         
           if (db.SaveChanges() > 0)
           {
               return user;
           }
           else
           {
               return null;
           }

       }
       public DAO.MemberBase Login(string loginName,string passWord)
       {
         passWord = Md5Util.PwdMd5(passWord);
         return  db.MemberBase.Where(m => (m.LoginName == loginName || m.Mobile == loginName || m.Email == loginName) && m.PassWord ==passWord && m.State==1).FirstOrDefault();
       
       }
       /// <summary>
       /// 检验密码是否正确
       /// </summary>
       /// <param name="loginName"></param>
       /// <param name="passWord"></param>
       /// <returns></returns>
       public bool ValidataPassWord(int uid, string passWord)
       {
            passWord = Md5Util.PwdMd5(passWord);
          DAO.MemberBase mb=  db.MemberBase.Where(m => (m.ID == uid) && m.PassWord == passWord && m.State == 1).FirstOrDefault();
          if (mb == null)
          {
              return false;
          }
          else {
              return true;
          }
       }
       /// <summary>
       /// 注册信息验证
       /// </summary>
       /// <param name="user"></param>
       /// <param name="phoneCode"></param>
       /// <returns></returns>
       public ReturnMsg VerUserInfo(DAO.MemberBase user, string phoneCode)
       {
           ReturnMsg msg = new ReturnMsg();        
           if (!VerLoginName(user.LoginName))
           {
               msg.Status = false;
               msg.Msg = "登录名已存在";
               return msg;
           }
           if (!VerPhone(user.Mobile))
           {
               msg.Status = false;
               msg.Msg = "手机号已注册";
               return msg;
           }
           //if (!VerUcode(user.Ucode))
           //{
           //    msg.Status = false;
           //    msg.Msg = "身份证已注册";
           //    return msg;
           //}
           if (!VerEmail(user.Email))
           {
               msg.Status = false;
               msg.Msg = "邮箱已注册";
               return msg;
           }
           //管理员添加的用户不用验证手机验证码
           if (user.Source != "admin")
           {
               if (!VerRegCode(user.Mobile, phoneCode))
               {
                   msg.Status = false;
                   msg.Msg = "手机验证码错误已存在";
                   return msg;
               }
           }
            msg.Status = true;
            return msg;
       }
       /// <summary>
       /// 验证登录名
       /// </summary>
       /// <param name="loginName"></param>
       /// <returns></returns>
       public bool VerLoginName(string loginName)
       {
          DAO.MemberBase user= db.Database.SqlQuery<DAO.MemberBase>(string.Format("select * from memberbase where loginname='{0}'",loginName)).FirstOrDefault();
          if (user == null)
          {
              return true;
          }
          else {
              return false;
          }
       }
       /// <summary>
       /// 验证手机
       /// </summary>
       /// <param name="Mobile"></param>
       /// <returns></returns>
       public bool VerPhone(string Mobile)
       {
           DAO.MemberBase user = db.Database.SqlQuery<DAO.MemberBase>(string.Format("select * from memberbase where Mobile='{0}'", Mobile)).FirstOrDefault();
           if (user == null)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 验证身份证
       /// </summary>
       /// <param name="Ucode"></param>
       /// <returns></returns>
       public bool VerUcode(string Ucode)
       {
           DAO.MemberBase user = db.Database.SqlQuery<DAO.MemberBase>(string.Format("select Mobile from memberbase where Ucode='{0}' ", Ucode)).FirstOrDefault();
           if (user == null)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 验证邮箱
       /// </summary>
       /// <param name="Email"></param>
       /// <returns></returns>
       public bool VerEmail(string Email)
       {
           DAO.MemberBase user = db.Database.SqlQuery<DAO.MemberBase>(string.Format("select * from memberbase where Email='{0}' ", Email)).FirstOrDefault();
           if (user == null)
           {
               return true;
           }
           else
           {
               return false;
           }
       }
       /// <summary>
       /// 验证手机注册码
       /// </summary>
       /// <param name="Email"></param>
       /// <returns></returns>
       public bool VerRegCode(string pNumber,string code)
       {
           return SMSManage.VerRegCode(pNumber,code);
       }
       /// <summary>
       /// 注册获取等级
       /// </summary>
       /// <param name="upUser"></param>
       /// <returns></returns>
       public DAO.MemberLevel GetLevel(int uid, int upUser)
       {
           ExpRecordBLL expBll = new ExpRecordBLL();
           DAO.ExpRecord expModel = new DAO.ExpRecord();
           MemberLevelBLL mleve=new MemberLevelBLL ();
           DAO.MemberLevel lmodel;
           if (upUser == -1)//独立注册的新会员可自动升级为二星会员 且自主购货累计双倍积分。
           {
               lmodel = mleve.GetLevelBySelf();
               //添加积分记录              
               expModel.Source = "独立注册";              
           }
           else if (upUser == 0)
           {
               lmodel = mleve.GetLevelBySelf();
               //添加积分记录              
               expModel.Source = "微信注册";         
           }
           else
           { //推荐注册 一星会员
               lmodel = mleve.GetLevelByUser();
               //添加积分记录 
               expModel.Source = "邀请码注册";
           }
           expModel.Uid = uid;
           expModel.Exp = lmodel.MinIntegral;
           expModel.Datetime = DateTime.Now;
           expModel.Status = 1;
           expBll.AddRecord(expModel);
           return lmodel;
       }
      /// <summary>
      /// 给上级用户积分
      /// </summary>
       public void GiveUpUserInt(int cuid,int upUser)
       {
           if (upUser != -1)
           {
               ExpRecordBLL expBll = new ExpRecordBLL();
               DAO.ExpRecord expModel = new DAO.ExpRecord();
               expModel.Uid = upUser;
               expModel.Source = "发展注册";
               expModel.LowerUid = cuid;
               expModel.Exp =int.Parse(ConfigurationApp.TJJF);
               expModel.Datetime = DateTime.Now;
               expModel.Status = 1;
               expBll.AddRecord(expModel);
           }
       }

       public DAO.MemberBase GetUserByID(string uid)
       {
           string sql = "SELECT * FROM [MemberBase] where ID={0} and State=1 ";
           return   db.Database.SqlQuery<DAO.MemberBase>(string.Format(sql,uid)).FirstOrDefault();    
       
       }
       public DAO.MemberBase GetUserByPhone(string phone)
       {
           string sql = "SELECT * FROM [MemberBase] where Mobile={0} and State=1 ";
           return db.Database.SqlQuery<DAO.MemberBase>(string.Format(sql, phone)).FirstOrDefault();

       }
       public bool UpdatePassword(string uid, string password)
       {
           string newPass = Md5Util.PwdMd5(password);
           string sql = "UPDATE [MemberBase] SET [PassWord] ='{0}'  WHERE ID={1}";
           int i=db.Database.ExecuteSqlCommand(string.Format(sql,newPass,uid));
           if (i > 0)
           {
               return true;
           }
           else {
               return false;
           }
       
       }

       public bool UpdatePasswordOld(string iphoneNum, string password)
       {
           string newPass = Md5Util.PwdMd5(password);
           string sql = "UPDATE [MemberBase] SET [PassWord] ='{0}'  WHERE Mobile={1}";
           int i = db.Database.ExecuteSqlCommand(string.Format(sql, newPass, iphoneNum));
           if (i > 0)
           {
               return true;
           }
           else
           {
               return false;
           }

       }

       public bool insertWx(OAuthUser OAuthUser_Model)
       {
           string sql = "insert into MemberBase (UpUser,State,wxOpenid,wxNickname,wxSex,wxProvince,wxCity,wxCountry,wxHeadimgurl,wxPrivilege) values (-1,1,'{0}','{1}',{2},'{3}','{4}','{5}','{6}','{7}')";
           int i = db.Database.ExecuteSqlCommand(string.Format(sql, OAuthUser_Model.openid, OAuthUser_Model.nickname, Convert.ToInt32(OAuthUser_Model.sex), OAuthUser_Model.province, OAuthUser_Model.city, OAuthUser_Model.country, OAuthUser_Model.headimgurl, OAuthUser_Model.privilege));
           if (i > 0)
           {
               return true;
           }
           else
           {
               return false;
           }

       }

       public bool updateWx(OAuthUser OAuthUser_Model)
       {
           string sql = "UPDATE [MemberBase] SET wxNickname='{0}',wxSex={1},wxProvince='{2}',wxCity='{3}',wxCountry='{4}',wxHeadimgurl='{5}',wxPrivilege='{6}',LoginName='{7}'  WHERE wxOpenid='{8}'";

           int i = db.Database.ExecuteSqlCommand(string.Format(sql, OAuthUser_Model.nickname, Convert.ToInt32(OAuthUser_Model.sex), OAuthUser_Model.province, OAuthUser_Model.city, OAuthUser_Model.country, OAuthUser_Model.headimgurl, OAuthUser_Model.privilege, OAuthUser_Model.nickname, OAuthUser_Model.openid));
           if (i > 0)
           {
               return true;
           }
           else
           {
               return false;
           }

       }

       public DAO.MemberBase selectWx(string appid)
       {
           string sql = "SELECT * FROM [MemberBase] where wxOpenid='{0}' and State=1 ";
           return db.Database.SqlQuery<DAO.MemberBase>(string.Format(sql, appid)).FirstOrDefault();
       }

       /// <summary>
       /// 查询评论
       /// </summary>
       /// <param name="productId"></param>
       /// <returns></returns>
       public List<Comments> commsele(string uid)
       {
           
           string sql = "select a.ID,a.userID,a.ProductID,a.mentsname,a.shown,b.PartName from comments a left join Parts b on a.ProductID=b.ID where a.shown=1 an a.userID=" + uid;
           List<Comments> ht = db.Database.SqlQuery<Comments>(sql.ToString()).ToList();
           return ht;
       }

       /// <summary>
       /// 添加评论
       /// </summary>
       /// <param name="Model"></param>
       /// <returns></returns>
       public bool adds(DAO.comments Model)
       {
           DAO.comments commadd = db.comments.Add(Model);
           int i = db.SaveChanges();
           if (i > 0)
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
