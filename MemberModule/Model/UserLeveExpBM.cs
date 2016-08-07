using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MemberModule.Model
{
   public class UserLeveExpBM
    {
        public int ID { get; set; }
        public Nullable<int> LevelId { get; set; }
        public string Name { get; set; }
        public int MinIntegral { get; set; }
        public int MaxIntegral { get; set; }
        public decimal Ratio { get; set; }
        public string Remark { get; set; }
        public decimal Income { get; set; }
        public int UExp { get; set; }
    }
}
