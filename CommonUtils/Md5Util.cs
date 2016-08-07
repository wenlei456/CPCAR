using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CommonUtils
{
    public class Md5Util
    {
        /// <summary>
        /// 用md5加密当前字符串
        /// </summary>
        /// <param name="value">要加密的字符串</param>
        /// <returns>string,加密后的字符串</returns>
        public static string PwdMd5(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(md5.ComputeHash(Encoding.ASCII.GetBytes(value)));
        }
    }
}
