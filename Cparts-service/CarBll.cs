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
    public class CarBll : BaseBusiness
    {
        /// <summary>
        /// 获取汽车列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public List<DAO.CarM> GetList(string banner = null)
        {
            StringBuilder sql = new StringBuilder("select * from CarM where 1=1 ");
            if (!string.IsNullOrEmpty(banner))
            {
                sql.Append(" and  Brand='" + banner + "'");
            }
            List<DAO.CarM> list = db.Database.SqlQuery<DAO.CarM>(sql.ToString()).ToList();
            return list;
        } 

        public List<string> GetCarBrand()
        {
            List<string> list = db.Database.SqlQuery<string>(string.Format("select distinct(Brand) from CarM")).ToList();
            return list;
        }
        public List<string> GetCarYear(string banner)
        {
            List<string> list = db.Database.SqlQuery<string>(string.Format("select distinct(Proyear) from CarM where brand='{0}'", banner)).ToList();
            return list;
        }
        public List<DAO.CarM> GetCarModel(string banner, string year)
        {
            List<DAO.CarM> list = db.Database.SqlQuery<DAO.CarM>(string.Format(" select  distinct(Proyear),id,Model,Brand from CarM where brand='{0}' and Proyear='{1}'", banner, year)).ToList();
            return list;
        }

        public List<DAO.CarM> GetCarM(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                string sql = "select * from CarM where id in ({0})";
                string id = ids.Replace(",,", ",");
                id = id.Substring(1, id.Length - 2);
                List<DAO.CarM> list = db.Database.SqlQuery<DAO.CarM>(string.Format(sql, id)).ToList();
                return list;
            }
            else
            {
                return null;
            }
        }

        public List<DAO.CarNew> GetListCar(string brand)
        {
            string sql = "select BrandEnglish,Brand,ModelEnglish,Model from CarM where Brand='{0}' group by BrandEnglish,Brand,ModelEnglish,Model";
            List<DAO.CarNew> m = db.Database.SqlQuery<DAO.CarNew>(string.Format(sql, brand)).ToList();

            return m;
        }

        public List<DAO.CarNewYear> GetListCarByYear(string brand, string model)
        {
            string sql = "select proyear,Model from CarM where Brand='{0}' and model='{1}' ";
            List<DAO.CarNewYear> m = db.Database.SqlQuery<DAO.CarNewYear>(string.Format(sql, brand, model)).ToList();

            return m;
        }


        /// <summary>
        /// 增加一条汽车记录
        /// </summary>
        /// <param name="car">汽车实体</param>
        /// <returns></returns>
        public bool AddCar(DAO.CarM car)
        {
            db.CarM.Add(car);
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
        public bool updateCar(DAO.CarM car)
        {
            DAO.CarM c = db.CarM.Where(m => m.id == car.id).FirstOrDefault();
            c.Model = car.Model;
            c.Oil = car.Oil;
            c.ProYear = car.ProYear;
            c.State = car.State;
            c.Brand = car.Brand;
            c.Engine = car.Engine;
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
        public bool DelCar(string id, string strWhere = null)
        {
            int r = 0;
            if (string.IsNullOrEmpty(strWhere))
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [CarM] where id={0}", id);
            }
            else
            {
                r = db.Database.ExecuteSqlCommand("DELETE FROM [CarM] where " + strWhere);
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

        public DAO.CarM GetModel(string brand, string model, string year)
        {
            string sql = "select * from CarM where Brand='{0}' and Model='{1}' and  ProYear='{2}' ";
            DAO.CarM m = db.Database.SqlQuery<DAO.CarM>(string.Format(sql, brand, model, year)).FirstOrDefault();
            return m;
        }
        public DAO.CarM GetModel(string id)
        {
            string sql = "select * from CarM where id={0} ";
            DAO.CarM m = db.Database.SqlQuery<DAO.CarM>(string.Format(sql, id)).FirstOrDefault();
            return m;
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        public bool AddCar(DAO.tml car)
        {
            db.tml.Add(car);
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

    }
}
