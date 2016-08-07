using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductModule.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string PartName { get; set; }
        public int CategoryID { get; set; }
        public Nullable<int> PartBrand { get; set; }
        public decimal Price { get; set; }
        public string imgpath { get; set; }
        public string partSubtitle { get; set; }
        public int ProType { get; set; }
        //public string ICOPath { get; set; }
        //public string CategoryName { get; set; }
    }

    public class HotProduct
    {
        public int ID { get; set; }
        public string PartName { get; set; }
        public int CategoryID { get; set; }
        public Nullable<int> PartBrand { get; set; }
        public decimal Price { get; set; }
        public string imgpath { get; set; }
        public string partSubtitle { get; set; }
        public int ProType { get; set; }
        public int HotProductId { get; set; }
        //public string ICOPath { get; set; }
        //public string CategoryName { get; set; }
    }
    /// <summary>
    /// 通过类型查询产品
    /// </summary>
    public class ProductList
    {
        public List<Product> proList { get; set; }
        public DAO.PartsCategory partCate { get; set; }
        public List<PartAttrNameModel> attrList { get; set; }
    }
    /// <summary>
    /// 通过关键字搜索查询产品
    /// </summary>
    public class ProductListBykeyword
    {
        public List<Product> proList { get; set; }
        public int totleCount { get; set; }
        public int pageSize { get; set; }
        public int currentPage { get; set; }
    }
    public class PartAttrNameModel
    {
        public string AttrName { get; set; }
        public int ParentAttr { get; set; }
    }
    public class PartAttrOutModel
    {
        public string AttrName { get; set; }
        public int ParentAttr { get; set; }
        public List<DAO.PartsExtend> AttrList { get; set; }
    }
    public class Addshp
    {
        public int ID { get; set; }
        public string city { get; set; }
        public string provinces { get; set; }
        public string region { get; set; }
        public string address { get; set; }
        public int UID { get; set; }
        public string Thecon { get; set; }
    }
    public class commentsadd
    {
        public int ID { get; set; }
        public string userID { get; set; }
        public string ProductID { get; set; }
        public string mentsname { get; set; }
        public Nullable<System.DateTime> Time { get; set; }
        public string shown { get; set; }
    }
}