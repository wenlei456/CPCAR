/*************************************************************************************
  * CLR版本：        4.0.30319.18063
  * 类 名 称：       MD5SignUtil
  * 机器名称：       2013-20131220EO
  * 命名空间：       MJZCake.VIEW.wxpay
  * 文 件 名：       MD5SignUtil
  * 创建时间：       2014/6/4 10:59:29
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
    public class MD5SignUtil
    {
        public static String Sign(String content, String key)
        {
            String signStr = "";

            if ("" == key)
            {
                throw new SDKRuntimeException("财付通签名key不能为空！");
            }
            if ("" == content)
            {
                throw new SDKRuntimeException("财付通签名内容不能为空");
            }
            signStr = content + "&key=" + key;

            return Wxpay.MD5Util.MD5(signStr).ToUpper();

        }

        public static bool VerifySignature(String content, String sign,
                String md5Key)
        {
            String signStr = content + "&key=" + md5Key;
            String calculateSign = Wxpay.MD5Util.MD5(signStr).ToUpper();
            String tenpaySign = sign.ToUpper();
            return (calculateSign == tenpaySign);
        }
    }
}
