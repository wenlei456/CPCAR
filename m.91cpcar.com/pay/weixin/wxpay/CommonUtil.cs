﻿/*************************************************************************************
  * CLR版本：        4.0.30319.18063
  * 类 名 称：       CommonUtil
  * 机器名称：       2013-20131220EO
  * 命名空间：       MJZCake.VIEW.wxpay
  * 文 件 名：       CommonUtil
  * 创建时间：       2014/6/4 10:57:05
  * 作    者：       潘明高
  * 
  * 修改时间：
  * 修 改 人：
 *************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wxpay
{
    public class CommonUtil
    {

        public static String CreateNoncestr(int length)
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static String CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static string FormatQueryParaMap(Dictionary<string, string> parameters)
        {

            string buff = "";
            try
            {

                var result = from pair in parameters orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {
                        buff += pair.Key + "="
                                + System.Web.HttpUtility.UrlEncode(pair.Value) + "&";
                    }
                }
                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                throw new SDKRuntimeException(e.Message);
            }

            return buff;
        }

        public static string FormatBizQueryParaMap(Dictionary<string, string> paraMap,
                bool urlencode)
        {

            string buff = "";
            try
            {
                var result = from pair in paraMap orderby pair.Key select pair;
                foreach (KeyValuePair<string, string> pair in result)
                {
                    if (pair.Key != "")
                    {

                        string key = pair.Key;
                        string val = pair.Value;
                        if (urlencode)
                        {
                            val = System.Web.HttpUtility.UrlEncode(val, System.Text.Encoding.GetEncoding("GB2312")).Replace("+","%20");
                           
                        }
                        buff += key.ToLower() + "=" + val + "&";

                    }
                }

                if (buff.Length == 0 == false)
                {
                    buff = buff.Substring(0, (buff.Length - 1) - (0));
                }
            }
            catch (Exception e)
            {
                throw new SDKRuntimeException(e.Message);
            }
            return buff;
        }

        public static bool IsNumeric(String str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ArrayToXml(Dictionary<string, string> arr)
        {
            String xml = "<xml>";

            foreach (KeyValuePair<string, string> pair in arr)
            {
                String key = pair.Key;
                String val = pair.Value;
                if (IsNumeric(val))
                {
                    xml += "<" + key + ">" + val + "</" + key + ">";

                }
                else
                    xml += "<" + key + "><![CDATA[" + val + "]]></" + key + ">";
            }

            xml += "</xml>";
            return xml;
        }

    }
}
