using AOP;
using ProductModule.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveModule
{
    public class ActiveBus : BaseBusiness
    {
        /// <summary>
        /// 获取活动列表
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<DAO.Active> GetActiveList(string status)
        {
            List<DAO.Active> list;
            if (string.IsNullOrEmpty(status))
            {
                list = db.Active.ToList();
            }
            else
            {
                list = db.Active.Where(m => m.Status == status).ToList();
            }
            return list;
        }
        /// <summary>
        /// 获得现在线上正常运行活动
        /// </summary>
        /// <returns></returns>
        public List<DAO.Active> GetRunActiveList()
        {
            return db.Active.Where(m => m.StarDate <= DateTime.Now && m.EndDate >= DateTime.Now && m.Status == "1").ToList();

        }
        //获取活动下产品
        public List<ProductModule.Models.Product> GetListByActive(int aid)
        {
            string sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath  from Parts a left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) b  on a.ID=b.PartID   where a.ID in (select ProID from activePros where ActiveID={0}) and  a.ProType=1  and a.State=1 ";
            List<ProductModule.Models.Product> list = db.Database.SqlQuery<ProductModule.Models.Product>(string.Format(sql, aid)).ToList();
            return list;
            // ProType=1 and State=1 
        }
        //  //获取活动产品
        public List<DAO.ActivePros> GetActiveListByActive(int aid)
        {
            List<DAO.ActivePros> list = db.ActivePros.Where(m => m.ActiveID == aid).ToList();
            return list;
            // ProType=1 and State=1 
        }

        /// <summary>
        /// 添加一个活动
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public DAO.Active AddActive(DAO.Active m)
        {
            DAO.Active r = db.Active.Add(m);
            db.SaveChanges();
            return r;
        }
        /// <summary>
        /// 获得一个活动
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public DAO.Active GetActive(int id)
        {
            DAO.Active r = db.Active.Where(m => m.ID == id).FirstOrDefault();
            return r;
        }
        public DAO.CrowdFunding cowdFunding(int id)
        {
            DAO.CrowdFunding i = db.CrowdFunding.Where(m => m.ID == id).FirstOrDefault();
            return i;
        }
        /// <summary>
        /// 获得热卖分类
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public List<DAO.HotTable> GetHotTable()
        {

            List<DAO.HotTable> r = db.HotTable.OrderBy(m => m.px).ToList();
            return r;
        }


        //获取热卖分类产品
        public List<ProductModule.Models.HotProduct> GetListByHotTable(List<DAO.HotTable> HotTable)
        {
            string ids = "";
            for (int i = 0; i < HotTable.Count; i++)
            {
                if (i < HotTable.Count - 1)
                {
                    ids += HotTable[i].ID.ToString() + ",";
                }
                else
                {
                    ids += HotTable[i].ID.ToString();
                }

            }
            string sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,c.imgpath,a.HotProductId  from Parts a left join HotTable b  on a.HotProductId=b.ID left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) c  on a.ID=c.PartID   where a.HotProductId in ({0}) and [State]=1 ORDER BY a.Price";
            List<ProductModule.Models.HotProduct> list = db.Database.SqlQuery<ProductModule.Models.HotProduct>(string.Format(sql, ids)).ToList();
            return list;
            // ProType=1 and State=1 
        }

        public ActivePro GetActiveDetail(int pid, int aid)
        {
            ActivePro model = new ActivePro();
            model.activePro = db.ActivePros.Where(m => m.ActiveID == aid && m.ProID == pid).FirstOrDefault();
            model.active = db.Active.Where(m => m.ID == aid && m.Status == "1").FirstOrDefault();
            model.activeAttr = db.ActiveAttr.Where(m => m.ActiveID == aid && m.AProID == pid).ToList();
            return model;
        }
        public ActiveProAttr2 GetActiveProAttr(int aid, int pid)
        {
            ActiveProAttr2 m = new ActiveProAttr2();
            m.activeAttr = db.ActiveAttr.Where(i => i.AProID == pid && i.ActiveID == aid).ToList();
            m.activePro = db.ActivePros.Where(i => i.ActiveID == aid && i.ProID == pid).FirstOrDefault();
            return m;
        }
        /// <summary>
        /// 修改一个活动
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool UpActive(DAO.Active m)
        {
            db.Entry(m).State = System.Data.EntityState.Modified;
            int r = db.SaveChanges();
            if (r > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool addActivePro(DAO.ActivePros m)
        {
            DAO.ActivePros a = db.ActivePros.Where(b => b.ActiveID == m.ActiveID && b.ProID == m.ProID).FirstOrDefault();
            if (a == null)
            {
                db.ActivePros.Add(m);
            }
            else
            {
                a.NewPrice = m.NewPrice;
                db.Entry(a).State = System.Data.EntityState.Modified;
            }
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
        public bool addActiveAtts(DAO.ActiveAttr m)
        {
            DAO.ActiveAttr a = db.ActiveAttr.Where(b => b.ActiveID == m.ActiveID && b.AProID == m.AProID && b.AtrrID == m.AtrrID).FirstOrDefault();
            if (a == null)
            {
                db.ActiveAttr.Add(m);
            }
            else
            {
                a.NewPrice = m.NewPrice;
                db.Entry(a).State = System.Data.EntityState.Modified;
            }
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
        public void DelActivePro(int pid, int aid)
        {
            string attr_sql = "delete from ActiveAttr where ActiveID={0} and AProID={1}";
            string pro_sql = "delete from ActivePros where ActiveID={0} and ProID={1} ";
            db.Database.ExecuteSqlCommand(string.Format(attr_sql, aid, pid));
            db.Database.ExecuteSqlCommand(string.Format(pro_sql, aid, pid));
        }

        public bool ValiActivePro(int aid, int pid, string attr, int qty, ref string msg)
        {
            DateTime now = DateTime.Now;
            DAO.Active a = db.Active.Where(m => m.ID == aid && m.StarDate <= now && now <= m.EndDate && m.Status == "1").FirstOrDefault();
            if (a != null)
            {

                DAO.ActivePros p = db.ActivePros.Where(m => m.ProID == pid && m.ActiveID == aid).FirstOrDefault();
                if (p != null)
                {
                    if (string.IsNullOrEmpty(attr))
                    {
                        if (p.Stock < qty)
                        {
                            msg = "活动产品:" + p.ProName + "库存数量不足！";
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        int attrID = int.Parse(attr);
                        DAO.ActiveAttr at = db.ActiveAttr.Where(m => m.ActiveID == aid && m.AProID == pid && m.AtrrID == attrID).FirstOrDefault();
                        if (at != null)
                        {
                            if (at.Stock < qty)
                            {
                                msg = "活动产品:" + p.ProName + "该款库存数量不足！";
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            msg = "活动产品:" + p.ProName + "属性错误！";
                            return false;
                        }
                    }



                }
                else
                {
                    msg = "活动产品不存在";
                    return false;
                }
            }
            else
            {
                msg = "活动不存在或已过期";
                return false;
            }
            //&&  m.StarDate<= now && now<=m.EndDate &&m.Status=="1"
        }
        /// <summary>
        /// 获取下期活动轮播图片
        /// </summary>
        /// <returns></returns>
        public List<DAO.ActiveImg> GetActiveImg()
        {
            string sql = " select top 5 * from ActiveImg where Status=1 and ActiveID=-1 order by UploadDate desc";
            return db.Database.SqlQuery<DAO.ActiveImg>(sql).ToList();
        }
        public bool DelActiveImg(int id)
        {
            string sql = " delete from ActiveImg where ID={0}";
            return db.Database.ExecuteSqlCommand(string.Format(sql, id)) > 0 ? true : false;
        }
        public bool AddActiveImg(DAO.ActiveImg m)
        {
            db.ActiveImg.Add(m);
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

        //删除评论
        public void commedits(int ID)
        {
            string commdet = "delete from comments where ID={0}";
            db.Database.ExecuteSqlCommand(string.Format(commdet, ID));
        }
        //添加众筹
        public bool funding(DAO.CrowdFunding m)
        {
            db.CrowdFunding.Add(m);
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
        /// 修改众筹
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public bool editCrowd(DAO.CrowdFunding ced)
        {
            db.Entry(ced).State = System.Data.EntityState.Modified;
            int r = db.SaveChanges();
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
