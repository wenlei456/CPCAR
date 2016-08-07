using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AOP;
using DAO;
namespace CommonModule
{
    public class CommonBll:BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        public List<DAO.PartsCategory> GetList()
        {
            return db.PartsCategory.Where(m => m.IsShow == 1 && m.ParentID==0).OrderByDescending(m => m.OrderBy).ToList();
          
        }
    }
}
