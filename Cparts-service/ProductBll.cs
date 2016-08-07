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
    public class ProductBll : BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.Parts> GetList(string strWhere = null, string orderby = null, string asc = "asc")
        {
            StringBuilder sql = new StringBuilder("select * from Parts where 1=1 ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql.Append(" and " + strWhere);
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql.Append(" order by  " + orderby + " " + asc);
            }
            List<DAO.Parts> list = db.Database.SqlQuery<DAO.Parts>(sql.ToString()).ToList();
            return list;
        }
        //获取活动下产品
        public List<DAO.Parts> GetListByActive(int aid)
        {
            string sql = "select * from Parts where ID in (select ProID from activePros where ActiveID={0}) and  ProType=1  and State=1 ";
            List<DAO.Parts> list = db.Database.SqlQuery<DAO.Parts>(string.Format(sql, aid)).ToList();
            return list;
            // ProType=1 and State=1 
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
        public List<PartSimpleModel> GetSimpleList(int pageIndex, string strWhere = null, string orderby = null, string asc = "asc")
        {
            string sql = @"select a.ID, a.PartName,a.CategoryID,b.CategoryName,a.PartBrand,c.BrandName,a.PartModel,a.PartSuppyNo,a.Price,a.Orderby,a.ShowDate,a.[State],a.herf,d.StockNUM
 from Parts a left join PartsCategory b on a.CategoryID=b.ID  left join PartBrand c on c.ID=a.PartBrand left join PartStock d on d.PartID=a.ID where 1=1 ";
            //      string sql2 = @"select a.ID, a.PartName,a.CategoryID,b.CategoryName,a.PartBrand,c.BrandName,a.PartModel,a.PartSuppyNo,a.Price,a.Orderby,a.ShowDate,a.[State],d.StockNUM
            //from Parts a left join PartsCategory b on a.CategoryID=b.ID  left join PartBrand c on c.ID=a.PartBrand left join PartStock d on d.PartID=a.ID where 1=1 "; 
            if (!string.IsNullOrEmpty(strWhere))
            {
                sql += " and " + strWhere;
                //         sql2 += " and " + strWhere;
            }
            if (!string.IsNullOrEmpty(orderby))
            {
                sql += " order by  " + orderby + " " + asc;
                //          sql2 += " order by  " + orderby + " " + asc;
            }
            // count =db.Database.SqlQuery(int)();
            // List<PartSimpleModel> list = db.Database.SqlQuery<PartSimpleModel>(sql.ToString()).Skip((pageIndex - 1) * 30).Take(30).ToList();
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
        /// 修改产品状态
        /// </summary>
        /// <param name="model">修改产品状态</param>
        /// <returns></returns>

        public bool updateStatePars(string orderId)
        {


            string sql = "update [Order] set OrderStatus='已发货' where OrderId='{0}'";
            int i = db.Database.ExecuteSqlCommand(string.Format(sql, orderId));
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
        /// 订单是否存在
        /// </summary>
        /// <param name="model">订单是否存在</param>
        /// <returns></returns>

        public DAO.Order selOrderId(string orderId)
        {
            DAO.Order order = db.Order.Where(m => m.OrderId == orderId).FirstOrDefault();
            return order;
        }


        /// <summary>
        /// 查询产品名称
        /// </summary>
        /// <param name="model">查询产品名称</param>
        /// <returns></returns>

        public List<DAO.OrderProList> selPartName(string orderId)
        {
            List<DAO.OrderProList> order = db.OrderProList.Where(m => m.OrderId == orderId).ToList();
            return order;
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <param name="model">添加物流信息</param>
        /// <returns></returns>
        public DAO.information AddOrderNum(DAO.information model)
        {

            string sql = "UPDATE [Order] SET [wuliuhao]='{0}' where [orderID]='{1}'";
            int date = db.Database.ExecuteSqlCommand(string.Format(sql, model.wuliuhao,model.ordID));
            DAO.information m = db.information.Add(model);
            int i = db.SaveChanges();
            if (i > 0&&date>0)
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
            c.HotProductId = model.HotTable;
            c.PartModel = model.PartModel;
            c.PartSuppyNo = model.PartSuppyNo;
            c.Price = model.Price;
            c.Orderby = model.Orderby;
            c.mDescribe = model.mDescribe;
            c.ProType = model.ProType;
            c.State = model.State;
            c.supply = model.supply;
            c.storeName = model.storeName;
            c.material = model.material;
            c.specifications = model.specifications;
            c.useSite = model.useSite;
            c.ParsColour = model.ParsColour;
            c.herf = model.herf;
            if (model.ToCars == ",")
            {
                c.ToCars = "*";
            }
            else
            {
                c.ToCars = model.ToCars;
            }

            c.PartTitle = model.PartTitle;
            c.PartSubtitle = model.PartSubtitle;
            c.Describe = model.Describe;
            c.ShowDate = DateTime.Now;




            string sql = "update [Parts] set PartName='" + model.PartName + "', CategoryID=" + model.CategoryID + ", PartTitle='" + model.PartTitle + "', PartSubtitle='" + model.PartSubtitle + "', PartBrand='" + model.PartBrand + "', PartModel='" + model.PartModel + "', PartSuppyNo='" + model.PartSuppyNo + "'," +
            "Price=" + model.Price + ", ToCars='" + model.ToCars + "', Describe='" + model.Describe + "',  Orderby='" + model.Orderby + "',State='" + model.State + "',mDescribe='" + model.mDescribe + "',ProType='" + model.ProType + "'," +
            "HotProductId='" + model.HotTable + "',supply='" + model.supply + "',storeName='" + model.storeName + "',material='" + model.material + "',specifications='" + model.specifications + "',useSite='" + model.useSite + "',ParsColour='" + model.ParsColour + "',herf='" + model.herf + "',ShowDate='" + DateTime.Now + "' where ID={0}";
            int i = db.Database.ExecuteSqlCommand(string.Format(sql, model.ID));
            if (i > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            db.Entry(c).State = System.Data.EntityState.Modified;
            try
            {
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
            catch (Exception ex)
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

        public List<DAO.HotTable> GetHotTable()
        {
            string sql = "select * from HotTable";
            List<DAO.HotTable> ht = db.Database.SqlQuery<DAO.HotTable>(sql.ToString()).ToList();
            return ht;
        }

    }
}
