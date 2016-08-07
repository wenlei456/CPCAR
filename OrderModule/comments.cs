using Cparts_Product;
using MemberModule.Business;
using OrderModule.model;
using ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOP;
using ProductModule.Models;
using log4net;
using System.Reflection;
using ActiveModule;

namespace OrderModule
{
    public class comments : BaseBusiness
    {
        /// <summary>
        /// 添加评论
        /// </summary>
        /// <param name="Model"></param>
        /// <returns></returns>
        public DAO.comments adds(DAO.comments Model)
        {
            DAO.comments commadd = db.comments.Add(Model);
            int i = db.SaveChanges();
            if (i>0)
            {
                return commadd;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 查询评论
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<DAO.comments> commsele(string productId)
        {
            string sql = "select * from comments where ProductID='" + productId + "'";
            List<DAO.comments> ht = db.Database.SqlQuery<DAO.comments>(sql.ToString()).ToList();
            return ht;
        }
    }
}
