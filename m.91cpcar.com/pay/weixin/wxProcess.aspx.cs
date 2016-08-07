using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace m._91cpcar.com.pay.weixin
{
    public partial class wxProcess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string appid = "wxce17e97c663c90a0";
                string reurl = "";
                //传递参数，获取用户信息后，可跳转到自己定义的页面，想怎么处理就怎么处理

                if (Request.QueryString["id"] != "" && Request.QueryString["id"] != null)
                {
                    reurl = "http://m.91cpcar.com/Product/ProductDetail?id=" + Request.QueryString["id"];
                }
                else
                {
                    reurl = "http://m.91cpcar.com/Active/hot";
                }


                string code = "";

                //弹出授权页面(如在不弹出授权页面基础下未获得openid，弹出授权页面，提示用户授权)
                //if (Request.QueryString["auth"] != null && Request.QueryString["auth"] != "" && Request.QueryString["auth"] == "1")
                //{
                    Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=http://m.91cpcar.com/pay/weixin/wxProcess2.aspx?reurl=" + reurl + "&response_type=code&scope=snsapi_userinfo&state=1#wechat_redirect");
                //}
                //else
                //{
                //    //不弹出授权页面
                //    Response.Redirect("https://open.weixin.qq.com/connect/oauth2/authorize?appid=" + appid + "&redirect_uri=http://m.91cpcar.com/pay/weixin/wxProcess2.aspx?reurl=" + reurl + "&response_type=code&scope=snsapi_base&state=1#wechat_redirect");
                //}
            }
        }
    }
}