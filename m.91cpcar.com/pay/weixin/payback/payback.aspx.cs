using MJZCake.Web.pay.weixin.wxpay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MJZCake.Web.pay.weixin
{
    public partial class payback : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //创建支付应答对象
            ResponseHandler resHandler = new ResponseHandler(Context);
            resHandler.init();
            resHandler.setKey(TenpayUtil.key, TenpayUtil.appkey);

            //判断签名
            if (resHandler.isWXsignfeedback())
            {
                //回复服务器处理成功
                Response.Write("OK");
                Response.Write("OK:" + resHandler.getDebugInfo());
            }
            else
            {
                //sha1签名失败
                Response.Write("fail");
                Response.Write("fail:" + resHandler.getDebugInfo());
            }
            Response.End();
        }
    }
}