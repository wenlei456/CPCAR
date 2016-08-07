
using CommonUtils;
using DAO;
using m._91cpcar.com.Models;
using m.cpcar.com.Models;
using ProductModule;
using ProductModule.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace m._91cpcar.com.Controllers
{
    public class ProductController : Controller
    {
        [OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            //查询有什么活动
            ActiveModule.ActiveBus bll = new ActiveModule.ActiveBus();
         List<DAO.Active> list=   bll.GetRunActiveList();
            return View(list);
        }
        /// <summary>
        /// 车型数据
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600)]
        public ActionResult GetCarList()
        {

            List<Carinfo> cars = new List<Carinfo>();

            Cparts_Service.CarBll carBll = new Cparts_Service.CarBll();
            List<DAO.CarM> carList = carBll.GetList();
            List<string> brand = carList.GroupBy(m => m.Brand).Select(g => g.Key).ToList();
            foreach (var item in brand)
            {
                List<CarModel> cmList = new List<CarModel>();
                Carinfo c = new Carinfo();
                c.brand = item;
                List<string> models = carList.Where(m => m.Brand == item).GroupBy(m => m.Model).Select(g => g.Key).ToList();
                foreach (var i in models)
                {
                    CarModel cm = new CarModel();
                    cm.model = i;
                    //List<string> years = carList.Where(m => m.Brand == item && m.Model == i).Select(g => g.ProYear).ToList();
                    List<string> years = carList.Where(m => m.Brand == item && m.Model == i).OrderByDescending(g => g.ProYear).Select(g => g.ProYear).ToList();
                    cm.year = years;
                    cmList.Add(cm);
                }
                c.models = cmList;
                cars.Add(c);
            }
            return Json(cars, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 产品列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "id")]
        public ActionResult List(string id, string b, string m, string y)
        {
            // string tid = HttpUtility.HtmlEncode(t);
            string bb = HttpUtility.HtmlEncode(b);
            string mm = HttpUtility.HtmlEncode(m);
            string yy = HttpUtility.HtmlEncode(y);
            string typeId = HttpUtility.HtmlEncode(id);
            string car = "";
            if (!string.IsNullOrEmpty(b) && !string.IsNullOrEmpty(m) && !string.IsNullOrEmpty(y))
            {
                Cparts_Service.CarBll carBll = new Cparts_Service.CarBll();
                DAO.CarM carinfo = carBll.GetModel(bb, mm, yy);
                car = carinfo.id.ToString();
            }

            ProductBll proList = new ProductBll();
            ProductList list = proList.GetListByType(typeId, car,null);
            return View(list);
        }


           [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult TypeListByCar(string t,string b,string m,string y )
        {
            string tid = HttpUtility.HtmlEncode(t);
            string bb = HttpUtility.HtmlEncode(b);
            string mm = HttpUtility.HtmlEncode(m);
            string yy = HttpUtility.HtmlEncode(y);
            Cparts_Service.CarBll carBll = new Cparts_Service.CarBll();
            DAO.CarM carinfo = carBll.GetModel(bb, mm, yy);
            TypeListByCarViewModel vm = new TypeListByCarViewModel();
            ProductBll proBll = new ProductBll();

            if (carinfo != null)
            {
                vm.childenrCate = proBll.GetChildenCateListByType(t, carinfo.id.ToString());
            }
            if (t == "-1")
            {
                CommonModule.CommonBll bll = new CommonModule.CommonBll();
                vm.topCate = bll.GetList();
            }
            vm.parentCate = proBll.GetParentCateListByType(t);
            vm.Carinfo = carinfo;
            return View(vm);
        }
           [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult cList(string t, string b, string m, string y,string h)
        {
            if (b == "0" && m == "0" && y == "0"&& h!="0")
            {
                ProductBll proList = new ProductBll();
                ProductList list = proList.GetListByType(t, null,h);
                
                return View("~/Views/Product/pList.cshtml", list);
            }
            string tid = HttpUtility.HtmlEncode(t);
            string bb = HttpUtility.HtmlEncode(b);
            string mm = HttpUtility.HtmlEncode(m);
            string yy = HttpUtility.HtmlEncode(y);
            Cparts_Service.CarBll carBll = new Cparts_Service.CarBll();
            DAO.CarM carinfo = carBll.GetModel(bb, mm, yy);
            TypeListByCarViewModel vm = new TypeListByCarViewModel();
            ProductBll proBll = new ProductBll();

            if (carinfo != null)
            {
                vm.childenrCate = proBll.GetChildenCateListByType(t, carinfo.id.ToString());
            }
            if (vm.childenrCate.Count > 0)
            {
            CommonModule.CommonBll bll = new CommonModule.CommonBll();
            vm.topCate = bll.GetList();            
            vm.parentCate = proBll.GetParentCateListByType(t);
            vm.Carinfo = carinfo;
            return View(vm);
            }
            else
            {    
                    ProductBll proList = new ProductBll();
                    ProductList list = proList.GetListByType(t, carinfo.id.ToString(),null);
                    return View("~/Views/Product/pList.cshtml",list);
            }                    
          
        }
           [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult ProductDetail(string id)
        {
            ProductBll proDetail = new ProductBll();
            ProductDetail pd = proDetail.GetProDetail(id);
            return View(pd);
        }
     
        public ActionResult GetCar()
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
                    if (item.Qty != null)
                    {
                        if (int.TryParse(item.Attrs, out i))
                        {
                            m = bll.GetProDetail(item.ID, i, item.Type);
                        }
                        else
                        {
                            m = bll.GetProDetail(item.ID, -1, item.Type);
                        }
                        if (m != null)
                        {
                            //m.Qty = int.Parse(item.Qty);
                            //暂时限定所有商品只能购买一件
                            m.Qty = 1;
                            m.lastAttr = item.Attrs;
                            list.Add(m);
                        }
                    }
                }
                return View(list);
            }
            else
            {
                return View();
            }
        
        }

        public ActionResult CrowdFunding()
        {
            List<ProductModule.model.CrowdFundingDeail> crowdfunding = new List<ProductModule.model.CrowdFundingDeail>();
            ProductBll bll = new ProductBll();
            crowdfunding = bll.CrowdFundingList();
            return View(crowdfunding);
        }

        public ActionResult CrowdFindingDeail(string cid)
        {
            ProductModule.model.CrowdFundingDeail crowdfunding = new ProductModule.model.CrowdFundingDeail();
            ProductBll bll = new ProductBll();
            crowdfunding = bll.CrowdFundingListFrist(cid);

            List<DAO.MemberBase> mb = new List<DAO.MemberBase>();
            mb = bll.memberBaseList(cid);
            ProductModule.model.CrowdFundingList crList = new ProductModule.model.CrowdFundingList();
            crList.crowdFundingDeail = crowdfunding;
            crList.memberBaseList = mb;

            return View(crList);
        }

        public ActionResult crowdtDetail(string id)
        {
            ProductBll  bll = new ProductBll();
            ProDetail pd = bll.GetCrowdDeail(id);
            return View(pd);
        }


        //public ActionResult commsele(string productId = null)
        //{
        //    comments commd = new comments();
        //    List<DAO.comments> add = commd.commsele(productId);
        //    return View(add);
        //}
        [HttpPost]
        public ActionResult selShopNum(string productId)
        {

            Dictionary<string, object> mp = new Dictionary<string, object>();
           
            JavaScriptSerializer jss = new JavaScriptSerializer();

            ProductBll proBll = new ProductBll();
            int num = proBll.GetShopNum(productId);
             mp.Add("num", num);
             return Json(mp, JsonRequestBehavior.AllowGet);
            
        }

    }
}
