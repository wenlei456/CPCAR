using Cparts_Service;
using m._91cpcar.com.Models;
using ProductModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m._91cpcar.com.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        //首页
        public ActionResult Index()
        {
            return View();
        }
    }
}
