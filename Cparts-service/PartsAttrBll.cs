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
    /// 零件属性管理
    /// </summary>
    public class PartsAttrBll : BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.PartAttr> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from PartAttr");
           if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where "+strWhere);
            }
           if (!string.IsNullOrEmpty(orderby))
           {
               sql.Append(" order by  " + orderby + " " + asc);
           }
           List<DAO.PartAttr> list = db.Database.SqlQuery<DAO.PartAttr>(sql.ToString()).ToList();
          return list;
        }
        /// <summary>
        /// 增加一条汽车记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public bool Add(DAO.PartAttr car)
        {
            db.PartAttr.Add(car);
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
                r = db.Database.ExecuteSqlCommand("DELETE FROM [PartAttr] where id={0}", id);
            }
            else {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [PartAttr] where " + strWhere);
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
