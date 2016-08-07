using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

namespace CommonUtils
{
    public class ApiJsonUtil
    {
        private static readonly JavaScriptSerializer jss = new JavaScriptSerializer();
        /// <summary>
        /// 把当前对象转换为JSON格式的字符串
        /// </summary>
        /// <param name="o">要转换的对象</param>
        /// <returns></returns>
        public static string ToJson(object o)
        {
            return jss.Serialize(o);
        }
        /// <summary>
        /// 转换json里的DateTime类型
        /// </summary>
        /// <param name="o"></param>
        /// <param name="changeDateTimeType"></param>
        /// <returns></returns>
        public static string ToJson(object o, bool changeDateTimeType)
        {
            string str = jss.Serialize(o);
            if (changeDateTimeType)
            {
                str = Regex.Replace(str, @"\\/Date\(([-\d]+|[\d]+)\)\\/", match =>
                {
                    DateTime dt = new DateTime(1970, 01, 01);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                });
            }
            return str;
        }
        public static string ToJson(object o, bool changeDateTimeType, string Dateformat)
        {
            string str = jss.Serialize(o);
            if (changeDateTimeType)
            {
                str = Regex.Replace(str, @"\\/Date\(([-\d]+|[\d]+)\)\\/", match =>
                {
                    DateTime dt = new DateTime(1970, 01, 01);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString(Dateformat);
                });
            }
            return str;
        }

        /// <summary>
        /// 根据json字符串，反序列化成对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        public static T FromJson<T>(string json)
        {
            return jss.Deserialize<T>(json);
        }
    }
}
