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
    /// 汽车管理
    /// </summary>
    public class MenuBll: BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.WebsiteMenu> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from WebsiteMenu");
           if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where "+strWhere);
            }
           if (!string.IsNullOrEmpty(orderby))
           {
               sql.Append(" order by  " + orderby + " " + asc);
           }
           List<DAO.WebsiteMenu> list = db.Database.SqlQuery<DAO.WebsiteMenu>(sql.ToString()).ToList();
          return list;
        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public bool Add(DAO.WebsiteMenu model)
        {
            db.WebsiteMenu.Add(model);
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
        public bool Update(DAO.WebsiteMenu model)
        {
            DAO.WebsiteMenu c = db.WebsiteMenu.Where(m => m.ID == model.ID).FirstOrDefault();
            c.Enable = model.Enable;
            c.MenuBindURL = model.MenuBindURL;
            c.MenuLevel = model.MenuLevel;
            c.MenuName = model.MenuName;
            c.MenuOrderby =model.MenuOrderby;
            c.MenuParent = model.MenuParent;
            c.MenuState = model.MenuState;
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
                r = db.Database.ExecuteSqlCommand("DELETE FROM [WebsiteMenu] where id={0}", id);
            }
            else {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [WebsiteMenu] where " + strWhere);
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
