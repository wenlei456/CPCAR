using CommonUtils;
using CParts_Online.Models;
using ProductModule;
using ProductModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CParts_Online.Controllers
{
    public class CarController : Controller
    {
        //
        // GET: /Car/
      
        public ActionResult Index()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string car = CookieUtil.GetCookie("ShoppingCartObj");
            ShopCarVM carList = jss.Deserialize<ShopCarVM>(car);
            if (carList != null)
            {
                ProductBll bll = new ProductBll();
                List<CarProductDetail> list = new List<CarProductDetail>();
                foreach (var item in carList.List)
                {
                    CarProductDetail m;
                    int i = -1;
                    if (int.TryParse(item.Attrs, out i))
                    {
                        m = bll.GetProDetail(item.ID, i,item.Type);
                    }
                    else
                    {
                        m = bll.GetProDetail(item.ID, -1, item.Type);
                    }
                    if (m != null)
                    {
                        m.Qty = int.Parse(item.Qty);
                        m.lastAttr = item.Attrs;
                        list.Add(m);
                    }
                }
                return View(list);
            }
            else
            {
                return View();
            }
        }

    }
}
