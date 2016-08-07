using AOP;
using MemberModule.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberModule.Business
{
    public class MemberLevelBLL : BaseBusiness
    {
        /// <summary>
        /// 独立二星
        /// </summary>
        /// <returns></returns>
        public DAO.MemberLevel GetLevelBySelf()
       {
          DAO.MemberLevel level= db.MemberLevel.Where(m=>m.MinIntegral !=0).OrderBy(m => m.MinIntegral).FirstOrDefault();         
          return level;
       }
        /// <summary>
        /// 推荐一星
        /// </summary>
        /// <returns></returns>
        public DAO.MemberLevel GetLevelByUser()
       {
          DAO.MemberLevel level= db.MemberLevel.OrderBy(m => m.MinIntegral).FirstOrDefault();         
          return level;
       }
        /// <summary>
        /// 获取用户等级
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public DAO.MemberLevel GetLeveByUid(string uid)
        {
            string sql = "select * from MemberLevel where (SELECT sum (Exp) as allExp  FROM ExpRecord where [Uid]={0} and Status = 1) between MinIntegral and MaxIntegral ";
           DAO.MemberLevel r=  db.Database.SqlQuery<DAO.MemberLevel>(string.Format(sql,uid)).FirstOrDefault();
           return r;
        }
        /// <summary>
        /// 获取当前等级及其总经验
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public List<UserLeveExpBM> GetLeveExpByUid(string uid)
        {
            string sql = @"select top 2 (SELECT sum (Exp) as allExp  FROM ExpRecord where [Uid]={0} and [Status] = 1) as UExp ,* from MemberLevel where ((SELECT sum (Exp) as allExp  FROM ExpRecord where [Uid]={0} and [Status] = 1) between  MinIntegral and MaxIntegral ) or  (minintegral>(SELECT sum (Exp) as allExp  FROM ExpRecord where [Uid]={0} and [Status] = 1)) order by minintegral ";
            List<UserLeveExpBM> ule = db.Database.SqlQuery<UserLeveExpBM>(string.Format(sql, uid)).ToList();
            return ule;
        }
    }
}
