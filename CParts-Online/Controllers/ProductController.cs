using CParts_Online.Models;
using Cparts_Product;
using ProductModule;
using ProductModule.Models;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;
namespace CParts_Online.Controllers
{
    public class ProductController : Controller
    {
        [OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            ProductBll bll = new ProductBll();
            List<DAO.PartsCategory> pcs = bll.GetAllCates();
            return View(pcs);
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
            ProductList list = proList.GetListByType(typeId, car, null);
            return View(list);
        }
        /// <summary>
        /// 产品类型列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult TypeList(string id)
        {
            string typeId = HttpUtility.HtmlEncode(id);
            PartCateListViewModel vm = new PartCateListViewModel();
            ProductBll proBll = new ProductBll();
            vm.childenrCate = proBll.GetChildenCateListByType(typeId);
            vm.parentCate = proBll.GetParentCateListByType(typeId);
            return View(vm);
        }
        /// <summary>
        /// 产品类型列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult TypeListByCar(string t, string b, string m, string y)
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
        /// <summary>
        /// 产品详细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "id")]
        public ActionResult Detail(string id)
        {
            ProductBll proDetail = new ProductBll();
            ProductDetail pd = proDetail.GetProDetail(id);
            return View(pd);
        }


        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult TypeListByCar2(string t, string c)
        {
            string tid = HttpUtility.HtmlEncode(t);

            Cparts_Service.CarBll carBll = new Cparts_Service.CarBll();
            DAO.CarM carinfo = carBll.GetModel(c);
            // select *  FROM  Parts   where tocars like '%,1,%' and  [state]=1
            TypeListByCarViewModel vm = new TypeListByCarViewModel();
            ProductBll proBll = new ProductBll();
            vm.childenrCate = proBll.GetChildenCateListByType(t);
            if (vm.childenrCate != null && vm.childenrCate.Count == 1)
            {
                if (vm.childenrCate[0].ID.ToString() == t)
                {
                    vm.childenrCate = null;
                }
            }
            vm.parentCate = proBll.GetParentCateListByType(t);
            vm.Carinfo = carinfo;
            return View("TypeListByCar", vm);
        }
        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult SearchProduct(string keyword, int pageIndex = 1)
        {
            int pageSize = 32;      //数量 
            string kd = HttpUtility.HtmlEncode(keyword.Trim());
            ProductBll bll = new ProductBll();
            ProductListBykeyword proList = bll.SearchProductByKeyword(kd);
            List<Product> plist = proList.proList;
            var page = (from c in plist select c).Skip((pageIndex - 1) * pageSize).Take(pageSize);//跳过前50条数据，取10条
            proList.proList = page.ToList();
            proList.totleCount = plist.Count;
            proList.pageSize = pageSize;
            proList.currentPage = pageIndex;
            return View(proList);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult hot(int aid = 0)
        {
            HotModel hotModel = new HotModel();
            ActiveModule.ActiveBus bll = new ActiveModule.ActiveBus();

            List<DAO.HotTable> m = bll.GetHotTable();

            if (m != null)
            {
                hotModel.HotTable = m;
                hotModel.parts = bll.GetListByHotTable(m);

            }

            return View(hotModel);
        }

    }


    public class HotModel
    {
        public List<DAO.HotTable> HotTable { get; set; }
        public List<ProductModule.Models.HotProduct> parts { get; set; }

    }
}
