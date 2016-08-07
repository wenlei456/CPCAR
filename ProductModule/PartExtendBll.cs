using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using AOP;
using ProductModule.Models;


namespace Cparts_Product
{
    /// <summary>
    /// 零件管理
    /// </summary>
    public class PartExtendBll: BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.PartsExtend> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from PartsExtend ");
           if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" where and State=1 and " + strWhere);
            }
           if (!string.IsNullOrEmpty(orderby))
           {
               sql.Append(" order by  " + orderby + " "+asc);
           }
           List<DAO.PartsExtend> list = db.Database.SqlQuery<DAO.PartsExtend>(sql.ToString()).ToList();
          return list;
        }
        public List<DAO.PartsExtend> GetItems(int pid,string attrName)
        {
            //string sql = "select * from PartsExtend where ID in (select max(id) from PartsExtend where PartID={0} and State=1 group by AttrName ) and   State=1 order by ParentAttr";
           
            List<DAO.PartsExtend> list = db.PartsExtend.Where(m => m.PartID ==pid && m.AttrName == attrName &&m.State==1).ToList();
            return list;
        }
        public List<DAO.PartsExtend> GetCarItems(int pid,int attID)
        {           
            string sql = @" with cte as 
                         (  
                         select a.* from PartsExtend a where PartID={0} and ID={1} and State=1 
                         union all   
                         select k.*  from PartsExtend k inner join cte c on  c.ParentAttr = k.ID  
                         )select * from cte where State=1  order by ParentAttr";
            //string sql = "select * from PartsExtend where  PartID={0} and ID={1} and State=1 order by ParentAttr";
            List<DAO.PartsExtend> list = db.Database.SqlQuery<DAO.PartsExtend>(string.Format(sql, pid, attID)).ToList();
           
            return list;
        }
       
        /// <summary>
        /// 获取所有属性名
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<PartAttrNameModel> GetAttrs(string pid)
        {
            string sql = "select AttrName,ParentAttr from PartsExtend where ID in (select min(id) from PartsExtend where PartID={0} and State=1 group by AttrName ) and   State=1 order by ParentAttr";
            List<PartAttrNameModel> alist = db.Database.SqlQuery<PartAttrNameModel>(string.Format(sql, pid)).ToList();          
         
            return alist;
        }
        
        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public bool IsExist(string partid,string pid )
        {
            String sql = "select AttrName from PartsExtend where PartID={0} and ParentAttr={1} and State=1 group by AttrName";            
            string m = db.Database.SqlQuery<string>(string.Format(sql,partid,pid)).FirstOrDefault();
            if (string.IsNullOrEmpty(m))
            {
                return false;
            }
            else {
                return true;
            }
          
        }   
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public DAO.PartsExtend Add(DAO.PartsExtend model)
        {
            DAO.PartsExtend m = db.PartsExtend.Add(model);
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
        /// 修改
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool Update(DAO.PartsExtend model)
        {
            DAO.PartsExtend c = db.PartsExtend.Where(m => m.ID == model.ID).FirstOrDefault();
            /**
            c.Enable = model.Enable;
            c.MenuBindURL = model.MenuBindURL;
            c.MenuLevel = model.MenuLevel;
            c.MenuName = model.MenuName;
            c.MenuOrderby =model.MenuOrderby;
            c.MenuParent = model.MenuParent;
            c.MenuState = model.MenuState;
             * **/
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
        public bool Del(string type, string pid, string currText)
        {
            int r = 0;//AttrName  AttrValue    type: currType, pid: ParentAttr, cut: currText
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(currText))
            {
                if (type == "AttrName")
                {
                    string sql = "update PartsExtend set state=0 where ID in (select ID from PartsExtend where PartID={0}  and AttrName='{1}')";                 
                     r = db.Database.ExecuteSqlCommand(string.Format(sql,pid,currText));
                
                }
                if (type == "AttrValue")
                {
                    string sql = "update PartsExtend set state=0 where ID in (select ID from PartsExtend where PartID={0}  and (ID={1} or ParentAttr={2}))";
                    r = db.Database.ExecuteSqlCommand(string.Format(sql, pid, currText, currText));
                }
                if (r > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                return false;
            }
           
        }
        /// <summary>
        /// 获取一条属性数据
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="aid"></param>
        /// <returns></returns>
        public DAO.PartsExtend GetPartAttrModel(string pid, string aid)
        {
            string sql = "SELECT *  FROM [PartsExtend] where PartID={0} and ID={1} and state=1 ";
            return db.Database.SqlQuery<DAO.PartsExtend>(string.Format(sql, pid, aid)).FirstOrDefault();
        }
        public DAO.PartsExtend GetChildAttrModel(string pid, string aid)
        {
            string sql = "SELECT *  FROM [PartsExtend] where PartID={0} and ParentAttr={1} and state=1 ";
            return  db.Database.SqlQuery<DAO.PartsExtend>(string.Format(sql, pid, aid)).FirstOrDefault();
           
        }
    }
}
