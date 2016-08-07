using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonUtils
{
    public static class RegExpUtil
    {
        /// <summary>
        /// 手机验证
        /// </summary>
        public const string PHONE = "^(13[0-9]|14[5]|14[7]|15[0-3]|15[5-9]|18[0-9])\\d{8}$";
        public static bool Isphone(string number)
        { 
        Regex reg=new Regex(PHONE);
        return  reg.IsMatch(number);
        }

    }
}
