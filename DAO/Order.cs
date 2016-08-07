//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAO
{
    using System;
    using System.Collections.Generic;
    
    public partial class Order
    {
        public int ID { get; set; }
        public string OrderId { get; set; }
        public Nullable<int> Uid { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string OrderSource { get; set; }
        public Nullable<System.DateTime> OrderTime { get; set; }
        public string OrderStatus { get; set; }
        public string Consignee { get; set; }
        public string Address { get; set; }
        public string CellPhone { get; set; }
        public string PayStatus { get; set; }
        public string Payment { get; set; }
        public string Remarks { get; set; }
        public string Preferential { get; set; }
        public string CashPwd { get; set; }
        public decimal Ratio { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalPrice { get; set; }
        public string SearchTag { get; set; }
        public string OperatorRemarks { get; set; }
        public string OrderType { get; set; }
        public Nullable<int> OrderFrom { get; set; }
        public string SendName { get; set; }
        public string SendNum { get; set; }
        public string Prov { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Beizhu { get; set; }
    }
}
