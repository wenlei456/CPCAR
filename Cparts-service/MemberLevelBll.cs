using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using AOP;
namespace Cparts_Service
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class MemberLevelBll: BaseBusiness
    {
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.MemberLevel> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from MemberLevel");
           if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where "+strWhere);
            }
           if (!string.IsNullOrEmpty(orderby))
           {
               sql.Append(" order by  " + orderby + " " + asc);
           }
           List<DAO.MemberLevel> list = db.Database.SqlQuery<DAO.MemberLevel>(sql.ToString()).ToList();
          return list;
        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="car">实体</param>
        /// <returns></returns>
        public bool Add(DAO.MemberLevel car)
        {
            db.MemberLevel.Add(car);
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
        public bool update(DAO.MemberLevel m)
        {
            DAO.MemberLevel c = db.MemberLevel.Where(mc => mc.ID == m.ID).FirstOrDefault();
            c.Name = m.Name;
            c.MinIntegral = m.MinIntegral;
            c.MaxIntegral = m.MaxIntegral;
            c.Ratio = m.Ratio;
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
        public bool Del(string id,string strWhere=null)
        {
            int r=0;
            if (string.IsNullOrEmpty(strWhere))
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [MemberLevel] where id={0}", id);
            }
            else {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [MemberLevel] where " + strWhere);
            }
            if (r > 0)
            {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
