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
    
    public partial class RebateDraw
    {
        public int ID { get; set; }
        public int UID { get; set; }
        public decimal Price { get; set; }
        public string OrderNumber { get; set; }
        public string PType { get; set; }
        public string Payment { get; set; }
        public string BankInfo { get; set; }
        public string CardNumber { get; set; }
        public string CardName { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public Nullable<int> Statu { get; set; }
    }
}