using ProductModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m.cpcar.com.Controllers
{
    public class ActiveController : Controller
    {
        //
        // GET: /Active/
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult Index(int aid=0)
        {
            OutModel outmodel = new OutModel();
            ActiveModule.ActiveBus bll = new ActiveModule.ActiveBus();
          
            DAO.Active m= bll.GetActive(aid);
            
            if (m != null)
            {
                outmodel.active = m;
                outmodel.parts = bll.GetListByActive(aid);
                outmodel.activePro = bll.GetActiveListByActive(aid);
                outmodel.aimg = bll.GetActiveImg();
            }
          
            return View(outmodel);
        }

        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult OldIndex(int aid = 0)
        {
            OutModel outmodel = new OutModel();
            ActiveModule.ActiveBus bll = new ActiveModule.ActiveBus();

            DAO.Active m = bll.GetActive(aid);

            if (m != null)
            {
                outmodel.active = m;
                outmodel.parts = bll.GetListByActive(aid);
                outmodel.activePro = bll.GetActiveListByActive(aid);
                outmodel.aimg = bll.GetActiveImg();
            }

            return View(outmodel);
        }

       [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult ProDetail(int id,int aid)
        {
            ProductBll proDetail = new ProductBll();
            ProductModule.Models.ProductDetail pd = proDetail.GetProDetail(id.ToString(),1);
            ActiveModule.ActiveBus abll = new ActiveModule.ActiveBus();
            ViewBag.ActiveSet= abll.GetActiveDetail(id,aid);         
            return View(pd);
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

    public class OutModel 
    {
        public DAO.Active active { get;set;}
        public List<ProductModule.Models.Product> parts { get; set; }
        public List<DAO.ActivePros> activePro { get; set; }
        public List<DAO.ActiveImg> aimg { get; set; }
    }
}
