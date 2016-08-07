using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using AOP;
using Cparts_service.Model;
using System.Data;
namespace Cparts_Service
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class OrderBll : BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.Order> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from [Order] where 1=1 ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" and " + strWhere);
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql.Append(" order by  " + orderby + " " + asc);
            }
            List<DAO.Order> list = db.Database.SqlQuery<DAO.Order>(sql.ToString()).ToList();
            return list;
        }
        /// <summary>
        /// 通过搜索参数获取列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        public List<DAO.Order> GetList(SearchOrderParma parm)
        {
            StringBuilder sql = new StringBuilder("select * from [Order] where 1=1 ");
            if (string.IsNullOrEmpty(parm.orderNumber))
            {
                string ps = parm.payStatus;
                string os = parm.orderStatus;
                string bt = parm.orderBeginTime;
                string et = parm.orderEndTime;
                if (!string.IsNullOrEmpty(parm.payStatus) && parm.payStatus != "all")
                {
                    sql.Append(" and PayStatus='" + parm.payStatus + "'");
                }
                if (!string.IsNullOrEmpty(parm.orderStatus) && parm.orderStatus != "all")
                {
                    sql.Append(" and OrderStatus='" + parm.orderStatus + "'");
                }
                if (!string.IsNullOrEmpty(parm.orderBeginTime) && !string.IsNullOrEmpty(parm.orderEndTime))
                {
                    sql.Append(" and OrderTime >= '" + parm.orderBeginTime + "' and OrderTime <= '" + parm.orderEndTime + "  23:59:59'");
                }
                if (!string.IsNullOrEmpty(parm.consignee) && parm.consignee != "all")
                {
                    sql.Append(" and Consignee like '%" + parm.consignee + "%'");
                }
            }
            else
            {
                sql.Append(" and OrderId=" + parm.orderNumber);

            }
            sql.Append(" order by OrderTime desc");
            List<DAO.Order> list = db.Database.SqlQuery<DAO.Order>(sql.ToString()).ToList();
            return list;
        }

        /// <summary>
        /// 订单详情
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="orderNum"></param>
        /// <returns></returns>
        public OrderDetail OrderDetail(string orderNum)
        {
            OrderDetail order = new OrderDetail();
            string orderSql = " select * from [Order] where  OrderId={0}";
            DAO.Order o = db.Database.SqlQuery<DAO.Order>(string.Format(orderSql, orderNum)).FirstOrDefault();
            string opSql = " SELECT *  FROM [OrderProList] where [OrderId] ={0}";
            List<DAO.OrderProList> opList = db.Database.SqlQuery<DAO.OrderProList>(string.Format(opSql, orderNum)).ToList();
            string imgSql = "select * from imgstock where partID in (select ProductID from orderProList where orderID={0})";
            List<DAO.ImgStock> imgList = db.Database.SqlQuery<DAO.ImgStock>(string.Format(imgSql, orderNum)).ToList();
            order.order = o;
            order.opList = opList;
            order.pimgList = imgList;
            return order;
        }



        //获取地址
        public Order Detail(string orderNum)
        {

            string ordersql = "select * from [Order] where OrderID='{0}'";
            DAO.Order order = db.Database.SqlQuery<DAO.Order>(string.Format(ordersql, orderNum)).FirstOrDefault();
            return order;
        }
        //查询物流号
        public information delt(string orderNum)
        {
            string sql = "select * from [information] where [ordID]='{0}'";
            DAO.information orders = db.Database.SqlQuery<DAO.information>(string.Format(sql, orderNum)).FirstOrDefault();
            return orders;
        }
        /// <summary>
        /// 下上架
        /// </summary>
        /// <param name="proid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool setStatus(int proid, int status)
        {
            string sql = "update Parts set State={0} where ID={1}";
            int i = db.Database.ExecuteSqlCommand(string.Format(sql, status, proid));
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
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<PartSimpleModel> GetSimpleList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            string sql = @"select a.ID, a.PartName,a.CategoryID,b.CategoryName,a.PartBrand,c.BrandName,a.PartModel,a.PartSuppyNo,a.Price,a.Orderby,a.ShowDate,a.[State],d.StockNUM
 from Parts a left join PartsCategory b on a.CategoryID=b.ID  left join PartBrand c on c.ID=a.PartBrand left join PartStock d on d.PartID=a.ID where 1=1 ";
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql += " and " + strWhere;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += " order by  " + orderby + " " + asc;
            }
            List<PartSimpleModel> list = db.Database.SqlQuery<PartSimpleModel>(sql.ToString()).ToList();
            return list;
        }
        /// <summary>
        /// 增加一条记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public DAO.Parts Add(DAO.Parts model)
        {
            DAO.Parts m = db.Parts.Add(model);
            int i = db.SaveChanges();
            if (i > 0)
            {
                return m;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model">物流信息</param>
        /// <returns></returns>
        public DAO.information Add(DAO.information model)
        {
            DAO.information m = db.information.Add(model);
            int i = db.SaveChanges();
            if (i > 0)
            {
                return m;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool Update(PartUpdateModel model)
        {
            DAO.Parts c = db.Parts.Where(m => m.ID == model.ID).FirstOrDefault();

            c.PartName = model.PartName;
            c.CategoryID = model.CategoryID;
            c.PartBrand = model.PartBrand;
            c.PartModel = model.PartModel;
            c.PartSuppyNo = model.PartSuppyNo;
            c.Price = model.Price;
            c.Orderby = model.Orderby;
            c.State = model.State;
            c.ToCars = model.ToCars;
            c.PartTitle = model.PartTitle;
            c.PartSubtitle = model.PartSubtitle;
            c.Describe = model.Describe;
            c.ShowDate = DateTime.Now;
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
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public bool Del(string id, string strWhere = null)
        {
            int r = 0;
            if (string.IsNullOrEmpty(strWhere))
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [Parts] where id={0}", id);
            }
            else
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [Parts] where " + strWhere);
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
        /// <summary>
        /// 获取Model
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        public DAO.Parts GetPartModel(int partId)
        {
            DAO.Parts part = db.Parts.Where(m => m.ID == partId).FirstOrDefault();
            return part;
        }
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="bm"></param>
        /// <returns></returns>
        public bool SendOrder(SendOrderBM bm)
        {
            //asNoTracking

            DAO.Order order = db.Order.Where(m => m.OrderId == bm.OrderNumber).First();//db.Database.SqlQuery<DAO.Order>(string.Format(sql,bm.OrderNumber)).FirstOrDefault();

            if (order != null)
            {
                order.SendName = bm.SendName;
                order.SendNum = bm.SandNumber;
                if (string.IsNullOrEmpty(bm.SandNumber) && string.IsNullOrEmpty(bm.SendName))
                {
                    order.OrderStatus = "未处理";
                }
                else
                {
                    order.OrderStatus = "已发货";
                }
                var entry = db.Entry(order);
                if (entry.State == EntityState.Detached)
                {
                    db.Entry(order).CurrentValues.SetValues(order);
                }
                else
                {
                    db.Entry(order).State = System.Data.EntityState.Modified;
                }
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
            else
            {
                return false;
            }

        }

        //var entry = db.Entry(order);
        //if (entry.State == EntityState.Detached)
        //{
        //    var set = db.Set<DAO.Order>();
        //    DAO.Order attachedProduct = set.Local.SingleOrDefault(p => p.Id == item.Id);
        //    //如果已经被上下文追踪
        //    if (attachedProduct != null)
        //    {
        //        var attachedEntry = db.Entry(attachedProduct);
        //        attachedEntry.CurrentValues.SetValues(item);
        //    }
        //    else //如果不在当前上下文追踪
        //    {
        //        entry.State = EntityState.Modified;
        //    }
        //}

        /// <summary>
        /// 查询物流订单
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>

        public List<DAO.information> Lista(string number)
        {
            StringBuilder sql = new StringBuilder("select * from [information] where 1=1");
            if (!string.IsNullOrEmpty(number))
            {

                sql.Append(" and wuliuhao like '%" + number + "%' or ordID like '%" + number + "%'");

            }

            sql.Append(" order by [time] desc");
            List<DAO.information> list = db.Database.SqlQuery<DAO.information>(sql.ToString()).ToList();
            return list;
        }

        //查询评论
        public List<commselect> commselect(string mentsname)
        {
            commselect i = new commselect();
            StringBuilder sql = new StringBuilder("select a.ID,a.userID,a.ProductID,a.mentsname,a.[Time],a.shown,b.name from comments a left join Addship b on a.userID=b.[UID] where 1=1");
            if (!string.IsNullOrEmpty(mentsname))
            {
                sql.Append(" and b.Name like '%" + mentsname + "%'");
            }
            List<commselect> orders = db.Database.SqlQuery<commselect>(sql.ToString()).ToList();
            sql.Append("order by [Time] desc");
            return orders;
        }
        public bool commupdata(commselect commID)
        {
            string sql = "UPDATE [comments] SET [shown] = '1' WHERE ID = {0}";
            int orders = db.Database.ExecuteSqlCommand(string.Format(sql, commID.ID));
            if (orders > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool commupfos(commselect commID)
        {
            string sql = "UPDATE [comments] SET [shown] = '2' WHERE ID = {0}";
            int orders = db.Database.ExecuteSqlCommand(string.Format(sql, commID.ID));
            if (orders > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //查询众筹
        public List<CrowShow> Crowshw()
        {
            CrowShow i = new CrowShow();
            StringBuilder sql = new StringBuilder("select a.Number,a.ID,a.Pid,a.ShowTime,a.StarTime,a.EndTime,a.isBool,a.ListTille,a.Banner,b.PartName from CrowdFunding a left join Parts b on a.Pid=b.ID where 1=1");
            List<CrowShow> orders = db.Database.SqlQuery<CrowShow>(sql.ToString()).ToList();
            sql.Append("order by [Time] desc");
            return orders;
        }
        //根据Pid查询众筹
        public CrowShow Crowselect(string Pid)
        {
            string ordersql = "select * from [CrowdFunding] where ID='{0}'";
            CrowShow order = db.Database.SqlQuery<CrowShow>(string.Format(ordersql, Pid)).FirstOrDefault();
            return order;
        }
    }
}