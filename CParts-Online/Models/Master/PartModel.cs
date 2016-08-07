using Cparts_service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CParts_Offline.Models
{
    public class PartOutModel
    {
        public List<PartSimpleModel> PartList { get; set; }
        public List<DAO.PartsCategory> PartCateList { get; set; }
        public List<DAO.PartBrand> PartBrandList { get; set; }
        public List<DAO.PartAttr> PartAttrList { get; set; }
        public List<DAO.PartStock> PartStockList { get; set; }
        public List<DAO.CarM> Cars { get; set; }
        public List<DAO.HotTable> ht { get; set; }
          public  int totleCount {get;set;}        
            public  int pageSize  {get;set;}
             public  int currentPage  {get;set;}
             public List<string> CarBrands { get; set; }
    }
    public class PartInBean
    {
     
        public string PartName { get; set; }
        public int  CategoryID { get; set; }
        public int PartBrand { get; set; }
        public int HotTable { get; set; }
        public string PartModel { get; set; }
        public string PartSuppyNo { get; set; }
        public decimal Price { get; set; }
        public int Orderby { get; set; }
        public int State { get; set; }
        public string ToCars { get; set; }
        public string Describe { get; set; }
        public string PartTitle { get; set; }
        public string PartSubtitle { get; set; }   
        public int StockNUM { get; set; }
        public string mDescribe { get; set; }
        public int ProType { get; set; }
        public string supply { get; set; }
        public string storeName { get; set; }
        public string material { get; set; }
        public string specifications { get; set; }
        public string useSite { get; set; }
        public string ParsColour { get; set; }
        //链接
        public string herf { get; set; }
        
    }
    public class info
    {
        public string ordID { get; set; }
        public string wuliuhao { get; set; }
    }
    public class wangzhi
    {
        public string ID { get; set; }
        public string pro { get; set; }
    }
}