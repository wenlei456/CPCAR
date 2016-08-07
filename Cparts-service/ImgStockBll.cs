using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using AOP;
using Cparts_service.Model;
namespace Cparts_Service
{
    /// <summary>
    /// 零件管理
    /// </summary>
    public class ImgStockBll : BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.ImgStock> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from ImgStock");
           if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where "+strWhere);
            }
           if (!string.IsNullOrEmpty(orderby))
           {
               sql.Append(" order by  " + orderby + " " + asc);
           }
           List<DAO.ImgStock> list = db.Database.SqlQuery<DAO.ImgStock>(sql.ToString()).ToList();
          return list;
        }
       
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="car">实体</param>
        /// <returns></returns>
        public DAO.ImgStock Add(DAO.ImgStock model)
        {
            DAO.ImgStock m = db.ImgStock.Add(model);
          int i= db.SaveChanges();
          if (i > 0)
          {
              return m;
          }
          else {
              return null;
          }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool Del(int id,string strWhere=null)
        {
            int r=0;
            if (string.IsNullOrEmpty(strWhere))
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [ImgStock] where id={0}", id);
            }
            else {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [ImgStock] where " + strWhere);
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
        /// 获取Model
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public DAO.ImgStock GetPartModel(int partId)
        {
            DAO.ImgStock part = db.ImgStock.Where(m => m.ID == partId).FirstOrDefault();
           return part;
        }
    }
}
