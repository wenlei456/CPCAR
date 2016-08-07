using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace m._91cpcar.com.pay.weixin
{
    public partial class test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["memberID"] = 27;
                        //Session["mobile"] = model.Mobile;
                        Session.Timeout = 30;

                        Response.Redirect("/Active/hot/");

            }
        }
    }
}