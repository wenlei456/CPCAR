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
    
    public partial class LogInfo
    {
        public long ID { get; set; }
        public string OpearName { get; set; }
        public string EventName { get; set; }
        public string EvenMsg { get; set; }
        public string InAttr { get; set; }
        public string OutAttr { get; set; }
        public System.DateTime Datetime { get; set; }
    }
}