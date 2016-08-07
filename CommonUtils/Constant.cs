using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils
{
    public class Constant
    {

    }

    /// <summary>
    /// 支付类型
    /// </summary>
    public static class PayTypeConstant {
        public static string PAYTYPE_CASH = "现金卡";
        public static string COUPON_JZ_CARD = "极致兑换卡";
    }

    /// <summary>
    /// 品牌
    /// </summary>
    public static class BandConstant {
        public static Dictionary<int, string> bandDic = new Dictionary<int, string>()
    {
            {0,"all"},
            {1,"ksk"},
            {2,"jzcake"},
            {3,"lacake"}
     };
        
    }

    /// <summary>
    /// 渠道
    /// </summary>
    public static class ChannelConstant {
        public static Dictionary<int, string> channelDic = new Dictionary<int, string>() { 
            {0,"通用"},
            {1,"PC专用"},
            {2,"微网站专用"},
            {3,"APP专用"} };
    }
    
}
