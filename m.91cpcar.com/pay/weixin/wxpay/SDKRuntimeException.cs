/*************************************************************************************
  * CLR版本：        4.0.30319.18063
  * 类 名 称：       SDKRuntimeException
  * 机器名称：       2013-20131220EO
  * 命名空间：       MJZCake.Utility
  * 文 件 名：       SDKRuntimeException
  * 创建时间：       2014/6/4 10:52:30
  * 作    者：       潘明高
  * 
  * 修改时间：
  * 修 改 人：
 *************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace Wxpay
{
    [Serializable]
    public class SDKRuntimeException : Exception
    {

        private const long serialVersionUID = 1L;

        public SDKRuntimeException(String str)
            : base(str)
        {

        }
    }
}
