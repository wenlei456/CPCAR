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
    
    public partial class ExpRecord
    {
        public int ID { get; set; }
        public int Uid { get; set; }
        public string Source { get; set; }
        public Nullable<int> LowerUid { get; set; }
        public string OrderNum { get; set; }
        public Nullable<decimal> OrderPrice { get; set; }
        public Nullable<int> Exp { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public Nullable<int> Status { get; set; }
    }
}