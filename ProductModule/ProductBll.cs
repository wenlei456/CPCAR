using AOP;
using Cparts_Product;
using ProductModule.model;
using ProductModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProductModule
{
    public class ProductBll : BaseBusiness
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="asc">排序方向，默认正序</param>
        /// <returns></returns>
        public ProductList GetListByType(string id,string carid,string ht)
        {
            ProductList plist = new ProductList();
            string sql ;
            List<Product> listOld;
            if (string.IsNullOrEmpty(ht))
            {
                if (string.IsNullOrEmpty(carid))
                {
                    sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath,a.partSubtitle from Parts a left join ImgStock b on a.ID=b.PartID  where a.State=1 and  a.CategoryID={0} and a.ProType=0 order by a.ShowDate, a.orderby desc";
                }
                else
                {
                    sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath,a.partSubtitle from Parts a left join ImgStock b on a.ID=b.PartID  where a.State=1 and  a.CategoryID={0} and (a.tocars like '%," + carid + ",%' or tocars='*')  and a.ProType=0 order by a.ShowDate, a.orderby desc";

                }
                listOld = db.Database.SqlQuery<Product>(string.Format(sql, string.IsNullOrEmpty(id) ? "1" : id)).ToList();
            }
            else
            {
                

                if (string.IsNullOrEmpty(carid))
                {
                    sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath,a.partSubtitle from Parts a left join ImgStock b on a.ID=b.PartID left join HotTable c on a.HotProductId=c.ID  where a.State=1 and  a.CategoryID={0} and a.ProType=0 and a.HotProductId={1} order by a.ShowDate, a.orderby desc";
                }
                else
                {
                    sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath,a.partSubtitle from Parts a left join ImgStock b on a.ID=b.PartID left join HotTable c on a.HotProductId=c.ID  where a.State=1 and  a.CategoryID={0} and (a.tocars like '%," + carid + ",%' or tocars='*')  and a.ProType=0 and a.HotProductId={1} order by a.ShowDate, a.orderby desc";

                }
                listOld = db.Database.SqlQuery<Product>(string.Format(sql, string.IsNullOrEmpty(id) ? "1" : id, ht)).ToList();
            }
            List<Product> list = listOld.Where((x, i) => listOld.FindIndex(z => z.ID == x.ID) == i).ToList();
            plist.proList = list;
            string CSql = " select * from PartsCategory where ID={0} and IsShow=1 ";
            DAO.PartsCategory pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : id)).FirstOrDefault();
            plist.partCate = pc;
            return plist;
        }
        public ProductList GetListByType_M(string id, string carid)
        {
            ProductList plist = new ProductList();
            string sql;
            if (string.IsNullOrEmpty(carid))
            {
                sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath,a.partSubtitle from Parts a left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) b on a.ID=b.PartID  where a.State=1 and  a.CategoryID={0} and a.ProType=0 order by a.ShowDate, a.orderby desc";
            }
            else
            {
                sql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath,a.partSubtitle from Parts a left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) b on a.ID=b.PartID  where a.State=1 and  a.CategoryID={0} and (a.tocars like '%," + carid + ",%' or tocars='*') and a.ProType=0  order by a.ShowDate, a.orderby desc";

            }
            List<Product> list = db.Database.SqlQuery<Product>(string.Format(sql, string.IsNullOrEmpty(id) ? "1" : id)).ToList();
            plist.proList = list;
            string CSql = " select * from PartsCategory where ID={0} and IsShow=1 ";
            DAO.PartsCategory pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : id)).FirstOrDefault();
            plist.partCate = pc;
            return plist;
        }
        /// <summary>
        /// 根据类型ID获取子类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DAO.PartsCategory> GetChildenCateListByType(string id)
        {
            string CSql = " select * from PartsCategory where ParentID={0} and IsShow=1 ";
            List<DAO.PartsCategory> pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : id)).ToList();
            return pc;  
        }
        /// <summary>
        /// 根据类型ID获取父类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DAO.PartsCategory> GetParentCateListByType(string id)
        {
            string CSql = @"with cte as 
                         (  
                         select a.* from PartsCategory a where IsShow=1 and ID={0}
                         union all   
                         select k.*  from PartsCategory k inner join cte c on  c.ParentID = k.ID  
                         )select * from cte  order by ID ";
            List<DAO.PartsCategory> pc;
            if (id == "-1")
            {
                DAO.PartsCategory m = new DAO.PartsCategory();
                m.CategoryName = "全部产品";
                m.ParentID = -1;
                pc=new List<DAO.PartsCategory> ();
                pc.Add(m);
            }
            else
            {
                pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : id)).ToList();
            }
            return pc;
        }
        private string GetCateByTypePro(string id, string carid)
        {
            //根据车型和顶级父类型获取所有有产品的子类型
            string getCateByProSQL = @"with cte as 
                         (  
                         select a.* from PartsCategory a where IsShow=1 and ID={0} 
                         union all   
                         select k.*  from PartsCategory k inner join cte c on  c.ID = k.ParentID  
                         )select * from cte   where cte.id in (select categoryID from parts where tocars like '%,{1},%' or tocars='*') and cte.id<>{0} order by ParentID";
            string getCateByProSQL2 = @"with cte as 
                         (  
                         select a.* from PartsCategory a where IsShow=1  
                         union all   
                         select k.*  from PartsCategory k inner join cte c on  c.ID = k.ParentID  
                         )select * from cte   where cte.id in (select categoryID from parts where tocars like '%,{0},%' or tocars='*' )  order by ParentID";
            List<DAO.PartsCategory> pcForPro;
            if(id=="-1")
            {
                pcForPro = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(getCateByProSQL2, carid)).ToList();
            }else{
                pcForPro = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(getCateByProSQL, string.IsNullOrEmpty(id) ? "1" : id, carid)).ToList();
            }
                if (pcForPro.Count > 0)
            {
                StringBuilder sb = new StringBuilder("");
                foreach (var item in pcForPro)
                {
                    //if (item.ParentID.ToString() == id || id == "-1")
                    //{
                        sb.Append(item.ID + ",");
                    //}
                    //else
                    //{
                    //    sb.Append(item.ParentID + ",");
                    //}
                }
                return sb.ToString().Substring(0, sb.Length - 1);
            }            
            else {
                return "-1";
            }
        }
        /// <summary>
        /// 根据类型ID,车型ID获取子类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<DAO.PartsCategory> GetChildenCateListByType(string id,string carid)
        {
           string cp= GetCateByTypePro(id,carid);
           //if (cp == "-1")
           //{
           //    return null;
           //}
           //else {
               string CSql = " select * from PartsCategory where id in ({0})  and IsShow=1 ";
               List<DAO.PartsCategory> pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : cp)).ToList();
               return pc;
           //}
          
        }
        /// <summary>
        /// 根据类型ID,车型ID获取父类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
//        public List<DAO.PartsCategory> GetParentCateListByType(string id, string carid)
//        {
//            string CSql = @"with cte as 
//                         (  
//                         select a.* from PartsCategory a where IsShow=1 and ID={0}
//                         union all   
//                         select k.*  from PartsCategory k inner join cte c on  c.ParentID = k.ID  
//                         )select * from cte  order by ID ";
//            List<DAO.PartsCategory> pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : id)).ToList();
//            return pc;
//        }

        public ProDetail GetCrowdDeail(string id, int ptype = 0)
        {
           
            // string sql = "select a.*,c.CategoryName,b.BrandName,b.Describe as BrandDescribe from Parts a left join PartsCategory c on a.CategoryID=c.ID left join PartBrand b on a.PartBrand=b.ID  where a.ID={0} ";
            string sql = "select a.*,b.BrandName,b.Describe as BrandDescribe from Parts a left join PartBrand b on a.PartBrand=b.ID where a.ID={0} and a.ProType={1} ";
            ProDetail model = db.Database.SqlQuery<ProDetail>(string.Format(sql, id, ptype)).FirstOrDefault();

            return model;

        }
        /// <summary>
        /// 产品详情页
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ProductDetail GetProDetail(string id,int ptype=0)
        {
           ProductDetail proDetail=new ProductDetail ();
           // string sql = "select a.*,c.CategoryName,b.BrandName,b.Describe as BrandDescribe from Parts a left join PartsCategory c on a.CategoryID=c.ID left join PartBrand b on a.PartBrand=b.ID  where a.ID={0} ";
           string sql = "select a.*,b.BrandName,b.Describe as BrandDescribe from Parts a left join PartBrand b on a.PartBrand=b.ID where a.ID={0} and a.ProType={1} ";
           ProDetail model = db.Database.SqlQuery<ProDetail>(string.Format(sql, id, ptype)).FirstOrDefault();
           proDetail.proDetail=model;

           string PartComm = " select s.mentsname,m.wxHeadimgurl,m.LoginName from comments s left join MemberBase m on s.userID= m.ID where ProductID={0}";
           List<CommList> commlists = db.Database.SqlQuery<CommList>(string.Format(PartComm, id)).ToList();
           proDetail.commList = commlists;

           string imgSql=" select * from ImgStock where PartID={0} ";
           List<DAO.ImgStock> imgList= db.Database.SqlQuery<DAO.ImgStock>(string.Format(imgSql,id)).ToList(); 
           proDetail.imgStock=imgList;
           //string attSql = " select * from PartsExtend where PartID={0} and State=1 ";
           //List<DAO.PartsExtend> attList = db.Database.SqlQuery<DAO.PartsExtend>(string.Format(attSql, id)).ToList();
           //proDetail.partExt = attList;
           string CSql = " select * from PartsCategory where ID={0} and IsShow=1 ";
           DAO.PartsCategory pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql,model.CategoryID)).FirstOrDefault();
           proDetail.partCate = pc;

           string PartStockSql = " select * from PartStock where PartID={0}";
           DAO.PartStock PartStockList = db.Database.SqlQuery<DAO.PartStock>(string.Format(PartStockSql, id)).FirstOrDefault();
           proDetail.partStockList = PartStockList;


           List<PartAttrOutModel> attlist = GetExtends(int.Parse(id));
           proDetail.partExt = attlist;

           string carSql = "select * from CarM where id in ({0}) ";
            if (model.ToCars != "*")
            {
                string strTocar = model.ToCars.Replace(",,", ",");
                strTocar = strTocar.Substring(1, strTocar.Length - 2);
                List<DAO.CarM> carList = db.Database.SqlQuery<DAO.CarM>(string.Format(carSql, strTocar)).ToList();
                proDetail.carList = carList;
            }
            else { 
              proDetail.carList = null;           
            }
            //partBrand
           return proDetail;            
        }

        /// <summary>
        /// 产品详情页
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        private List<PartAttrOutModel> GetExtends(int pid)
        {
            List<PartAttrOutModel> list = new List<PartAttrOutModel>();

            PartExtendBll extBll = new PartExtendBll();
            List<PartAttrNameModel> allAttr = extBll.GetAttrs(pid.ToString());
            if (allAttr != null)
            {
                foreach (var item in allAttr)
                {
                    PartAttrOutModel model = new PartAttrOutModel();
                    model.AttrName = item.AttrName;
                    model.ParentAttr = item.ParentAttr;
                    model.AttrList = extBll.GetItems(pid, item.AttrName);
                    list.Add(model);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取购物车数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="AttID"></param>
        /// <returns></returns>
        public CarProductDetail GetProDetail(int id,int AttID,string atID="-1")
        {
            CarProductDetail proDetail = new CarProductDetail();
            string sql = "select a.ID,a.CategoryID,a.PartName,a.PartModel,a.PartSuppyNo,a.Price,b.BrandName  from Parts a left join PartBrand b on a.PartBrand=b.ID where a.ID={0} ";
            CarProDetail model = db.Database.SqlQuery<CarProDetail>(string.Format(sql, id)).FirstOrDefault();
            if (model != null)
            {
                string imgSql = " select * from ImgStock where PartID={0} ";
                DAO.ImgStock img = db.Database.SqlQuery<DAO.ImgStock>(string.Format(imgSql, id)).FirstOrDefault();
                proDetail.imgStock = img;
                string CSql = " select * from PartsCategory where ID={0} and IsShow=1 ";
                DAO.PartsCategory pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, model.CategoryID)).FirstOrDefault();
                proDetail.partCate = pc;

                PartExtendBll extBll = new PartExtendBll();
                List<DAO.PartsExtend> att = extBll.GetCarItems(id, AttID);
                proDetail.partExt = att;
                if (att.Count > 0)
                {
                    if (atID != "-1" && atID!=null)
                    {
                        int aid=int.Parse(atID);
                        DAO.ActiveAttr a = db.ActiveAttr.Where(o => o.AProID == id && o.AtrrID == AttID && o.ActiveID == aid).FirstOrDefault();
                        if (a != null)
                        {
                            model.Price = a.NewPrice;                         
                            proDetail.activeM = db.Active.Where(o => o.ID == aid).FirstOrDefault();
                        }
                    }
                    else
                    {
                        model.Price = att.Last().AttrPrice;
                    }
                }
                else {
                    if (atID != "-1" && atID != null)
                    {
                        int aid = int.Parse(atID);
                    DAO.ActivePros p = db.ActivePros.Where(o=>o.ActiveID==aid&&o.ProID==id).FirstOrDefault();
                    if (p != null)
                    {
                        model.Price = p.NewPrice;
                        proDetail.activeM = db.Active.Where(o => o.ID == aid).FirstOrDefault();
                    }
                       }
                }
                
                proDetail.proDetail = model;
                return proDetail;
            }
            else {

                return null;
            }
           
        }


        /// <summary>
        /// 获取用户是否购买过活动产品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public bool GetCrowdFunding(string id)
        {
            string sql = "SELECT id  FROM CrowdFunding  where Pid={0}";
            int num = db.Database.SqlQuery<int>(string.Format(sql, Convert.ToInt32(id))).FirstOrDefault();
            if (num > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        /// <summary>
        /// 是否参加众筹
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public List<DAO.OrderProList> GetOrderIs(string uid, int id)
        {
            string sql = "SELECT *  FROM OrderProList a left join [Order] b on a.OrderId=b.OrderId where b.Uid ='{0}' and a.ProductID={1}";
            return db.Database.SqlQuery<DAO.OrderProList>(string.Format(sql, uid, id)).ToList();
        }


        /// <summary>
        /// 库存
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public int getStockNum(int id)
        {
            string sql = "SELECT StockNUM  FROM PartStock where PartID={0}";
            return db.Database.SqlQuery<int>(string.Format(sql, id)).FirstOrDefault();
        }



        /// <summary>
        /// 获取一个产品
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public DAO.Parts GetPartModel(string pid)
        {
            string sql = "SELECT *  FROM [Parts] where ID ={0} and [State]=1 ";
            return   db.Database.SqlQuery<DAO.Parts>(string.Format(sql,pid)).FirstOrDefault();
        }
        /// <summary>
        /// 通过keyword查询所有产品
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public ProductListBykeyword SearchProductByKeyword(string keyword)
        {
            ProductListBykeyword plist = new ProductListBykeyword();
            string catesql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath from Parts a left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) b on a.ID=b.PartID  where a.State=1 and  a.CategoryID in (select id from PartsCategory where  CategoryName like '%{0}%' ) and a.ProType=0 order by a.ShowDate, a.orderby desc";
            List<Product> list1 = db.Database.SqlQuery<Product>(string.Format(catesql, keyword)).ToList();

            string prosql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath from Parts a left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) b on a.ID=b.PartID  where a.State=1 and  a.PartName like '%{0}%' and a.ProType=0 order by a.ShowDate, a.orderby desc";
            List<Product> list2 = db.Database.SqlQuery<Product>(string.Format(prosql, keyword)).ToList();
            if (list1.Count == 0)
            {
                list1 = list2;
            }
            else
            {
                if (list2.Count > 0)
                {
                    list2.AddRange(list1);
                }
            }
            list1 = list1.Where((x, i) => list1.FindIndex(z => z.ID == x.ID) == i).ToList();

            string carsql = "select id from CarM where brand like '%{0}%' or model like '%{0}%'";
            List<int> carList = db.Database.SqlQuery<int>(string.Format(carsql, keyword)).ToList();
            string carProsql = "select a.ID,a.PartName,a.CategoryID,a.PartBrand,a.Price,b.imgpath from Parts a left join  (select *  from ImgStock where id in (select min(id)  from ImgStock group by PartID)) b on a.ID=b.PartID  where a.State=1 and  a.tocars like '%,{0},%'  and a.ProType=0 order by a.ShowDate, a.orderby desc";
            foreach (var item in carList)
            {
               List<Product> list3 =  db.Database.SqlQuery<Product>(string.Format(carProsql, item)).ToList();
               if (list1.Count == 0)
               {
                   list1 = list3;
               }
               else
               {
                   if (list3.Count > 0)
                   {
                       list3.AddRange(list1);
                   }
               }
             
            }
            list1 = list1.Where((x, i) => list1.FindIndex(z => z.ID == x.ID) == i).ToList();
            //List<Product> list3 = db.Database.SqlQuery<Product>(string.Format(prosql, keyword)).ToList();

           
            plist.proList = list1;
            //string CSql = " select * from PartsCategory where ID={0} and IsShow=1 ";
            //DAO.PartsCategory pc = db.Database.SqlQuery<DAO.PartsCategory>(string.Format(CSql, string.IsNullOrEmpty(id) ? "1" : id)).FirstOrDefault();
            //plist.partCate = pc;
            return plist;
        
        
        
        
        }

        /// <summary>
        /// 获取所有产品分类
        /// </summary>
        /// <returns></returns>
        public List<DAO.PartsCategory> GetAllCates()
        {
            string catesql = " select * from PartsCategory where IsShow=1";
            List<DAO.PartsCategory> list = db.Database.SqlQuery<DAO.PartsCategory>(catesql).ToList();
            return list;
        }


        public int GetShopNum(string pid)
        {
            string catesql = " select count(0) from [OrderProList] where ProductID={0}";
            int num = db.Database.SqlQuery<int>(catesql,pid).FirstOrDefault();
            return num;
        }


        public List<CrowdFundingDeail> CrowdFundingList()
        {
            string sql = "select a.id,a.Pid,a.Banner,a.ListTille,a.isBool,a.StarTime,a.EndTime,b.PartName,b.Price,c.StockNUM,a.Number from CrowdFunding a left join Parts b on a.pid=b.id left join PartStock c on a.pid=c.PartID where a.EndTime>'" + DateTime.Now + "' and a.isBool=1";
            List<CrowdFundingDeail> crowfunding = db.Database.SqlQuery<CrowdFundingDeail>(sql.ToString()).ToList();
            return crowfunding;
        }

        public CrowdFundingDeail CrowdFundingListFrist(string cid)
        {
            string sql = "select a.id,a.Pid,a.Banner,a.ListTille,a.isBool,a.StarTime,a.EndTime,b.PartName,b.Price,c.StockNUM,a.Number from CrowdFunding a left join Parts b on a.pid=b.id left join PartStock c on a.pid=c.PartID where a.EndTime>'" + DateTime.Now + "' and a.isBool=1 and a.ID={0}";
            CrowdFundingDeail crowfunding = db.Database.SqlQuery<CrowdFundingDeail>(sql.ToString(),Convert.ToInt32(cid)).FirstOrDefault();
            return crowfunding;
        }

        public List<DAO.MemberBase> memberBaseList(string cid)
        {
            string sql = "select d.* from CrowdFunding a left join OrderProList b on a.Pid=b.ProductID left join [Order] c on b.OrderId=c.OrderId left join MemberBase d on c.Uid=d.ID where a.EndTime>'" + DateTime.Now + "' and a.isBool=1 and a.ID={0}";
            List<DAO.MemberBase> memberBase = db.Database.SqlQuery<DAO.MemberBase>(sql.ToString(), Convert.ToInt32(cid)).ToList();
            return memberBase;
        }

    }
}
