using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using AOP;
using CParts_Offline.Models.Master;
namespace Cparts_Service
{
    /// <summary>
    /// 零件分类管理
    /// </summary>
    public class PartsCateBll : BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.PartsCategory> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from PartsCategory");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where " + strWhere);
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql.Append(" order by  " + orderby + " " + asc);
            }
            List<DAO.PartsCategory> list = db.Database.SqlQuery<DAO.PartsCategory>(sql.ToString()).ToList();
            return list;
        }
        /// <summary>
        /// 获取层次产品分类列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<PartCateModel> GetListPC()
        {
            List<PartCateModel> list = new List<PartCateModel>();

           string sql = "select * from PartsCategory where ParentID=0 ";  //顶层分类         
           List<DAO.PartsCategory> alllist = db.Database.SqlQuery<DAO.PartsCategory>(sql).ToList();
           foreach (var item in alllist)
           {
               PartCateModel curr=new PartCateModel ();
               curr.currItem=item;
               curr.Children = getChilder(item);
               list.Add(curr);
           }

          return list;
        }
        private List<PartCateModel> getChilder(DAO.PartsCategory p)
        {          
            List<PartCateModel> Allcurr = new List<PartCateModel>();
            string sql = "select * from PartsCategory where ParentID="+p.ID;        
            List<DAO.PartsCategory> alllist = db.Database.SqlQuery<DAO.PartsCategory>(sql).ToList();
            foreach (var item in alllist)
            {
                PartCateModel curr = new PartCateModel();
                curr.currItem = item;
                curr.Children = getChilder(item);
                Allcurr.Add(curr); 
            }
                        
            return Allcurr;

        }

        public DAO.PartsCategory GetModel(int id)
        {
           return  db.PartsCategory.Where(m => m.ID == id).FirstOrDefault();
        }
        /// <summary>
        /// 增加一条汽车记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public bool Add(DAO.PartsCategory car)
        {
            db.PartsCategory.Add(car);
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
        public bool Update(DAO.PartsCategory car)
        {
            DAO.PartsCategory c = db.PartsCategory.Where(m => m.ID == car.ID).FirstOrDefault();
            c.CategoryName = car.CategoryName;             
            c.ParentID = car.ParentID;
            c.OrderBy = car.OrderBy;
            c.IsShow = car.IsShow;
            c.ICOPath = car.ICOPath;
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
                r = db.Database.ExecuteSqlCommand("DELETE FROM [PartsCategory] where id={0}", id);
            }
            else {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [PartsCategory] where " + strWhere);
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
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.HotTable> HtGetlist(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from HotTable");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where " + strWhere);
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql.Append(" order by  " + orderby + " " + asc);
            }
            List<DAO.HotTable> list = db.Database.SqlQuery<DAO.HotTable>(sql.ToString()).ToList();
            return list;
        }

    }
}
