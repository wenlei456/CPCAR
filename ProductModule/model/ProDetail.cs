using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductModule.Models
{
    public class ProDetail
    {
        public int ID { get; set; }
        public string PartName { get; set; }
        public int CategoryID { get; set; }
        public string PartTitle { get; set; }
        public string PartSubtitle { get; set; }
        public Nullable<int> PartBrand { get; set; }
        public string PartModel { get; set; }
        public string PartSuppyNo { get; set; }
        public decimal Price { get; set; }
        public string PartNumber { get; set; }
        public string ToCars { get; set; }
        public string Describe { get; set; }
        public string mDescribe { get; set; }
        public string SpecParma { get; set; }
        public string DetailList { get; set; }
        public string SAS { get; set; }
        public string Common { get; set; }
        public int Orderby { get; set; }
        public System.DateTime ShowDate { get; set; }
        public int State { get; set; }
        public int ProType { get; set; }
        //public string CategoryName{get;set;}      
        public string BrandName{get;set;}
        public string BrandDescribe { get; set; }
        public string supply { get; set; }
        public string storeName { get; set; }
        public string material { get; set; }
        public string specifications { get; set; }
        public string useSite { get; set; }
        public string ParsColour { get; set; }

    }
    public class CommList
    {
        public string mentsname { get; set; }
        public string wxHeadimgurl { get; set; }
        public string LoginName { get; set; }
    }

    public class ProductDetail
    { 
      public ProDetail proDetail{get;set;}
      public List<CommList> commList { get; set; }
      public List<DAO.ImgStock> imgStock{get;set;}
      public List<PartAttrOutModel> partExt { get; set; }
      public DAO.PartsCategory partCate { get; set; }
      public List<DAO.CarM> carList { get; set; }
      public DAO.PartStock partStockList { get; set; }
    }

    public class CarProDetail
    {
        public int ID { get; set; }
        public int CategoryID { get; set; }
        public string PartName { get; set; }
        public string PartModel { get; set; }
        public string PartSuppyNo { get; set; }
        public decimal Price { get; set; }
        public int ProType { get; set; }
        public string BrandName { get; set; }
    }

    public class CarProductDetail
    {
        public CarProDetail proDetail { get; set; }
        public DAO.ImgStock imgStock { get; set; }     
        public DAO.PartsCategory partCate { get; set; }
        public List<DAO.PartsExtend> partExt { get; set; }
        public int Qty { get; set; }
        public string lastAttr { get; set; }
        public DAO.Active activeM { get; set; }
    }

    public class GoOrder
    {
        public DAO.OrderProList  prodList { get; set; }
        public DAO.AddressBook addressList { get; set; }
        public List<CarProductDetail> carProList { get; set; }
        public DAO.MemberLevel userLevel { get; set; }
    }
    public class Pageable
    {
        public int totlaCount { get; set; }
        int pagesize { get; set; }
    }
}