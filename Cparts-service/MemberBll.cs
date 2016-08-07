using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using AOP;
using CommonUtils;
namespace Cparts_Service
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class MemberBll: BaseBusiness
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.MemberBase> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from MemberBase");
           if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where "+strWhere);
            }
           if (!string.IsNullOrEmpty(orderby))
           {
               sql.Append(" order by  " + orderby + " " + asc);
           }
          List<DAO.MemberBase> list= db.Database.SqlQuery<DAO.MemberBase>(sql.ToString()).ToList();
          return list;
        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="car">实体</param>
        /// <returns></returns>
        public bool AddCar(DAO.MemberBase car)
        {
           db.MemberBase.Add(car);
          int i= db.SaveChanges();
          if (i > 0)
          {
              return true;
          }
          else {
              return false;
          }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool updateCar(DAO.MemberBase m)
        {
            DAO.MemberBase c = db.MemberBase.Where(mc => mc.ID == m.ID).FirstOrDefault() ;
            c.Email = m.Email;
            c.MemberType = m.MemberType;
            c.Mobile = m.Mobile;
            c.Name = m.Name;
            c.PassWord = m.PassWord;
            c.State = m.State; 
            db.Entry(c).State = System.Data.EntityState.Modified;
            int result = db.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else {
                return false;
            }         
         
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool DelCar(string id,string strWhere=null)
        {
            int r=0;
            if (string.IsNullOrEmpty(strWhere))
            {
               r= db.Database.ExecuteSqlCommand("DELETE FROM [MemberBase] where id={0}", id);
            }
            else {
               r= db.Database.ExecuteSqlCommand("DELETE FROM [MemberBase] where "+strWhere );
            }
            if (r > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public DAO.AdminAccount Login(string loginName, string passWord)
        {
            passWord = Md5Util.PwdMd5(passWord);
            return db.AdminAccount.Where(m => m.AccountName == loginName && m.PassWord == passWord).FirstOrDefault();

        }
    }
}
