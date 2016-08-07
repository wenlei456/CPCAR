using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace m._91cpcar.com.pay.weixin
{
    public partial class testLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["memberID"] = 2;
            Session.Timeout = 30;

            Response.Redirect("/Active/hot");
        }
    }
}