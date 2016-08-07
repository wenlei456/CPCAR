using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cparts_service.Model
{
    //select a.PartName,a.CategoryID,b.CategoryName,a.PartBrand,c.BrandName,
    //a.PartModel,a.PartSuppyNo,a.Price,a.Orderby,a.ShowDate,a.[State],d.StockNUM
    //from Parts a left join PartsCategory b on a.CategoryID=b.ID  left join PartBrand c on c.ID=a.PartBrand left join PartStock d on d.PartID=a.ID  
    //where d.StockType=0
    public class PartSimpleModel
    {
        public int ID { get; set; }
        public string PartName { get; set; }
        public string CategoryName { get; set; }
        public string BrandName { get; set; }
        public string PartModel { get; set; }
        public string PartSuppyNo { get; set; }
        public Nullable<decimal> Price { get; set; }
        public int Orderby { get; set; }
        public DateTime ShowDate { get; set; }
        public int State { get; set; }
        public Nullable<int> StockNUM { get; set; }
        public string herf { get; set; }
        public int count { get; set; }

    }

    public class PartUpdateModel
    {
        public int ID { get; set; }
        public string PartName { get; set; }
        public int CategoryID { get; set; }
        public int PartBrand { get; set; }
        public int HotTable { get; set; }
        public string PartModel { get; set; }
        public string PartSuppyNo { get; set; }
        public decimal Price { get; set; }
        public int Orderby { get; set; }
        public int State { get; set; }
        public string ToCars { get; set; }
        public string PartTitle { get; set; }
        public string PartSubtitle { get; set; }
        public string Describe { get; set; }
        public int StockNUM { get; set; }
        public string mDescribe { get; set; }
        public int ProType { get; set; }
        public string supply { get; set; }
        public string storeName { get; set; }
        public string material { get; set; }
        public string specifications { get; set; }
        public string useSite { get; set; }
        public string ParsColour { get; set; }
        public string herf { get; set; }
    }
}
