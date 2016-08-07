using AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberModule.Business
{
    public class FriendCodeBLL : BaseBusiness
    {
        /// <summary>
        /// 校验邀请码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
       public DAO.FriendCode isFriendCode(string code)
       {          
           DAO.FriendCode cm = db.FriendCode.Where(m=>m.Code==code && m.Status==1 ).FirstOrDefault();
           return cm;       
       }
        /// <summary>
        /// code所属
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
       public int GetUpUser(string code)
       {          
           if (string.IsNullOrEmpty(code))
           {
               return -1;
           }
           else {
               MemberBLL mbll = new MemberBLL();
               DAO.MemberBase mb = mbll.GetUserByPhone(code);
               if (mb != null)
               {
                   return mb.ID;
               }
               else {
                   return -1;
               }
           }
       }
    }
}
