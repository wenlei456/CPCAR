using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
   public class ConfigurationApp
    {
       /// <summary>
       ///短信模板
       /// </summary>
        public static readonly string temp = System.Configuration.ConfigurationManager.AppSettings["SMSTemp"];
        /// <summary>
        ///短信验证码超时
        /// </summary>
        public static readonly string timeout = System.Configuration.ConfigurationManager.AppSettings["SMSTimeOut"];
        //PV值
        public static readonly string PV = System.Configuration.ConfigurationManager.AppSettings["PV"];
        //独立注册下单积分倍数 
        public static readonly string SM = System.Configuration.ConfigurationManager.AppSettings["sm"];
        //推荐注册下单积分倍数
        public static readonly string CM = System.Configuration.ConfigurationManager.AppSettings["cm"];  
       //推荐会员注册可以得到的积分
        public static readonly string TJJF = System.Configuration.ConfigurationManager.AppSettings["TJJF"];
        //消费积分系数XFJFXS
        public static readonly string XFJFXS = System.Configuration.ConfigurationManager.AppSettings["XFJFXS"];
       //注册送优惠券金额
        public static readonly string regVoucherPrice = System.Configuration.ConfigurationManager.AppSettings["regVoucherPrice"];
        //注册送介绍人优惠券金额
        public static readonly string regVoucherPriceJie = System.Configuration.ConfigurationManager.AppSettings["regVoucherPriceJie"];
        //注册送优惠券过期
        public static readonly string regVoucherEnd = System.Configuration.ConfigurationManager.AppSettings["regVoucherEnd"];

    }
}
