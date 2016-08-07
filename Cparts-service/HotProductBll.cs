using AOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cparts_service
{
    public class HotProductBll : BaseBusiness
    {
        /// <summary>
        /// 增加一条汽车记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public bool AddHot(DAO.HotTable ht)
        {
            db.HotTable.Add(ht);
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

        /// <summary>
        /// 修改汽车
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool updateHot(DAO.HotTable ht)
        {
            DAO.HotTable c = db.HotTable.Where(m => m.ID == ht.ID).FirstOrDefault();
            db.Entry(c).State = System.Data.EntityState.Modified;
            int result = db.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>

        public bool updatedz(DAO.Order orders)
        {
            string sql = "UPDATE [Order] SET [City] ='{0}',[Consignee]='{1}',[CellPhone]='{2}',[Prov]='{3}',[Address]='{4}',[Area]='{5}' WHERE OrderId='{6}'";
            int date = db.Database.ExecuteSqlCommand(string.Format(sql, orders.City, orders.Consignee, orders.CellPhone, orders.Prov, orders.Address, orders.Area, orders.OrderId));
            string sqlc = "UPDATE [Addship] SET [scity] ='{0}',[province]='{1}',[region]='{2}',[address]='{3}',[name]='{4}',[phone]='{5}' WHERE UID='{6}'";
            int datec = db.Database.ExecuteSqlCommand(string.Format(sqlc, orders.City, orders.Prov, orders.Area, orders.Address, orders.Consignee, orders.CellPhone, orders.Uid));
            if (date > 0 && datec > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="Orede"></param>
        /// <returns></returns>
        public bool upt(DAO.Order Orede)
        {
            string sql = "UPDATE [Order] SET [OrderStatus]='已处理' where [orderID]='{0}'";
            int date = db.Database.ExecuteSqlCommand(string.Format(sql, Orede.OrderId));
            if (date > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //根据ID 查询地址
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool DelHot(string id, string strWhere = null)
        {
            int r = 0;
            if (string.IsNullOrEmpty(strWhere))
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [HotTable] where id={0}", id);
            }
            else
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [HotTable] where " + strWhere);
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
    }
}
