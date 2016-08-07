using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public  class OrderUtil
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public static string NewOrderId
        {
            get
            {
                Random rd = new Random();
                string o = string.Format("{0:yyMMddHHmmssfff}{1}", DateTime.Now, rd.Next(1000, 9999).ToString());
                return o;
            }

        }
    }

     
}
