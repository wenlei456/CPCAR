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
    
    public partial class MemberLevel
    {
        public int ID { get; set; }
        public Nullable<int> LevelId { get; set; }
        public string Name { get; set; }
        public int MinIntegral { get; set; }
        public int MaxIntegral { get; set; }
        public decimal Ratio { get; set; }
        public string Remark { get; set; }
        public decimal Income { get; set; }
    }
}
