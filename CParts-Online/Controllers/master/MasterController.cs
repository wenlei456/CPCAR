
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cparts_Service;
using CParts_Offline.Models;
using Cparts_service.Model;
using System.IO;
using CParts_Offline.Models.Master;
using CParts_Online.Models.Master;
using CParts_Online.Controllers.master;
using System.Text;
using ActiveModule;
using ProductModule.model;
using Cparts_service;
using MemberModule.Business;
using System.Net;
using System.Web.Script.Serialization;

namespace CParts_Offline.Controllers.master
{
    public class MasterController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginModel m)
        {
            //VerCode
            if (ModelState.IsValid)
            {
                MemberBll bll = new MemberBll();
                DAO.AdminAccount model = bll.Login(m.loginName, m.passWord);
                if (model != null)
                {
                    Session["AdminID"] = model.ID;
                    if (Session["ControllerUrl"] != null && Session["ActionUrl"] != null)
                    {
                        return RedirectToAction(Session["ActionUrl"].ToString(), Session["ControllerUrl"].ToString());
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ViewBag.loginFail = "用户名或密码错误！";
                    return View(m);
                }
            }
            else
            {

                return View(m);
            }
        }
        /// <summary>
        /// 订单列表
        /// </summary>
        /// <param name="parm"></param>
        /// <returns></returns>
        [AdminFilter]        
        public ActionResult Index(SearchOrderParma parm)
        {
            OrderBll bll = new OrderBll();
            List<DAO.Order> orderList = bll.GetList(parm); //bll.GetList(null, "OrderTime", "desc");

            return View(orderList);
        }
        [AdminFilter]
        public ActionResult OrderDetail(string OrderNumber)
        {
            if (string.IsNullOrEmpty(OrderNumber))
            {
                return RedirectToAction("Index");
            }
            else
            {
                OrderBll oBll = new OrderBll();
                OrderDetail info = oBll.OrderDetail(OrderNumber);
                return View(info);
            }
        
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult SendOrder(SendOrderBM bm)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            try
            {
                ProductBll part = new ProductBll();
           
                DAO.Order orderS = part.selOrderId(bm.OrderNumber);

                List<DAO.OrderProList> orderList = part.selPartName(bm.OrderNumber);
                string ParsNames = "";

                for (int i = 0; i < orderList.Count(); i++)
                {
                    if (i == orderList.Count() - 1)
                    {
                        ParsNames += orderList[i].ProductName;
                    }
                    else
                    {
                        ParsNames += orderList[i].ProductName + ",";
                    }


                }

                OrderBll oBll = new OrderBll();
                bool r = oBll.SendOrder(bm);
                bool isPhone = PhoneNumber(orderS.CellPhone, bm.SandNumber, ParsNames);
                map.Add("status", r);
            }
            catch (Exception e)
            {
                map.Add("status", false);
                map.Add("msg", e.Message+e.StackTrace);
            }
            return Json(map,JsonRequestBehavior.AllowGet);
        }
        #region 汽车管理
        /// <summary>
        /// 汽车管理主页
        /// </summary>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult CarsManage(string banner=null)
        {
            //汽车列表
            CarBll carbll = new CarBll();
            List<DAO.CarM> list;
            if (banner == "all" || string.IsNullOrEmpty(banner))
            {
                list = carbll.GetList();
            }
            else {
                list = carbll.GetList(banner);
            }
            return View(list);
        }
        /// <summary>
        /// 添加汽车
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult AddCar(DAO.CarM car)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //添加汽车
            CarBll carbll = new CarBll();
            bool r = carbll.AddCar(car);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("CarsManage");
        }

        /// <summary>
        /// 修改汽车
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditCar(DAO.CarM car)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //修改汽车
            CarBll carbll = new CarBll();
            bool r = carbll.updateCar(car);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
            // return RedirectToAction("CarsManage");
        }


        [HttpPost]
        public ActionResult editg(DAO.Order edit)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //修改汽车
             HotProductBll carbll = new HotProductBll();
             bool r = carbll.updatedz(edit);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        //
        /// <summary>
        /// 删除汽车
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult DelCar(string id)
        {
            //删除汽车
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            CarBll carbll = new CarBll();
            bool r = carbll.DelCar(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 零部件分类管理

        /// <summary>
        ///零部件分类管理主页
        /// </summary>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult PartsCateManage()
        {
            //列表
            PartsCateBll partbll = new PartsCateBll();
            PartCateViewModel vm = new PartCateViewModel();
            vm.PCItem = partbll.GetListPC();
            vm.AllItem = partbll.GetList();
            return View(vm);
        }
        /// <summary>
        /// 添加零部件分类管理
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult AddPartsCate(DAO.PartsCategory partcate)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //添加
            PartsCateBll carbll = new PartsCateBll();
            bool r = carbll.Add(partcate);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("CarsManage");
        }

        /// <summary>
        /// 修改零部件分类管理
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult EditPartsCate(DAO.PartsCategory partcate)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            //修改
            PartsCateBll carbll = new PartsCateBll();
            bool r = carbll.Update(partcate);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
            // return RedirectToAction("CarsManage");
        }
        /// <summary>
        /// 产品分类ico图表图片
        /// </summary>
        /// <param name="Upimg"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult AddPartsCImg(string Upimg)
        {

            HttpPostedFileBase file = Request.Files[0];
            string path = "/Upload/ICO/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string uploadpath = Server.MapPath(path);
            if (file != null)
            {
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
            }
            file.SaveAs(uploadpath + file.FileName);

            PartsCateBll carbll = new PartsCateBll();
            DAO.PartsCategory model = carbll.GetModel(int.Parse(Upimg));
            model.ICOPath = path + file.FileName;
            bool r = carbll.Update(model);
            return RedirectToAction("PartsCateManage");
        }
        /// <summary>
        /// 产品分类banner图片
        /// </summary>
        /// <param name="Upimg"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult AddBannerImg(string Upbanner)
        {

            HttpPostedFileBase file = Request.Files[0];
            string path = "/Upload/Banner/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string uploadpath = Server.MapPath(path);
            if (file != null)
            {
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
            }
            string nname = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now);
            file.SaveAs(uploadpath + nname + file.FileName);

            PartsCateBll carbll = new PartsCateBll();
            DAO.PartsCategory model = carbll.GetModel(int.Parse(Upbanner));
            model.BannerPath = path + nname + file.FileName;
            bool r = carbll.Update(model);
            return RedirectToAction("PartsCateManage");
        }
        /// <summary>
        /// 删除零部件分类管理
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult DelPartsCate(string id)
        {
            //删除
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            PartsCateBll partcbll = new PartsCateBll();
            bool r = partcbll.Del(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 零件品牌管理
        /// <summary>
        /// 零件品牌管理
        /// </summary>
        /// <returns></returns>   
        [AdminFilter]
        public ActionResult PartsBrandManage()
        {
            PartsBrandBll bll = new PartsBrandBll();
            return View(bll.GetList());
        }

        /// <summary>
        /// 添加零件品牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [AdminFilter]
        public ActionResult AddPartsBrand(DAO.PartBrand model)
        {
            PartsBrandBll bll = new PartsBrandBll();
            DAO.PartBrand m = model;
            bll.Add(m);
            return RedirectToAction("PartsBrandManage");

        }


        /// <summary>
        /// 修改零件品牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [AdminFilter]
        public ActionResult EditPartsBrand(DAO.PartBrand model)
        {
            PartsBrandBll bll = new PartsBrandBll();
            DAO.PartBrand m = model;
            bll.Update(m);
            return RedirectToAction("PartsBrandManage");
        }
        /// <summary>
        /// 删除零部品牌
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult DelPartBrand(string id)
        {
            //删除
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            PartsBrandBll partcbll = new PartsBrandBll();
            bool r = partcbll.Del(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 零件属性管理
        /// <summary>
        /// 零件属性管理
        /// </summary>
        /// <returns></returns>   
        [AdminFilter]
        public ActionResult PartsAttrManage()
        {
            PartsAttrBll bll = new PartsAttrBll();
            return View(bll.GetList());
        }

        /// <summary>
        /// 添加零件品牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult AddPartsAttr(DAO.PartAttr model)
        {
            PartsAttrBll bll = new PartsAttrBll();
            DAO.PartAttr m = model;
            bll.Add(m);
            return RedirectToAction("PartsAttrManage");

        }
        /// <summary>
        /// 修改零件品牌
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        /// <summary>
        /// 删除零部品牌
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult DelPartsAttr(string id)
        {
            //删除
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            PartsAttrBll partcbll = new PartsAttrBll();
            bool r = partcbll.Del(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }

        #endregion
        #region 产品管理
        /// <summary>
        /// 零件管理
        /// </summary>
        /// <returns></returns> 
        [AdminFilter]
        public ActionResult PartsManage(string SelectByCate, int pageIndex = 1, string tName = null, string idate = "ShowDate")
        {
            int pageSize = 30;      //数量 

            PartOutModel model = new PartOutModel();
            ProductBll partBll = new ProductBll();
            List<PartSimpleModel> partList;
            int count=0;
            if (string.IsNullOrEmpty(SelectByCate) || SelectByCate == "-1")
            {
                partList = partBll.GetSimpleList(pageIndex, " a.State=1 and a.PartName like '%" + tName + "%'", idate, "asc");
            }
            else
            {
                partList = partBll.GetSimpleList(pageIndex, " a.State=1 and a.PartName like '%" + tName + "%' and a.CategoryID=" + SelectByCate, idate, "desc");
            }
            PartsCateBll cateBll = new PartsCateBll();
            List<DAO.PartsCategory> PartCateList = cateBll.GetList();
            PartsBrandBll partBrandBll = new PartsBrandBll();
            List<DAO.PartBrand> PartBrandList = partBrandBll.GetList("", "BrandName");
            PartsAttrBll attrBll = new PartsAttrBll();
            List<DAO.PartAttr> PartAttrList = attrBll.GetList("", "Name");
            PartsStockBll stockBll = new PartsStockBll();
            List<DAO.PartStock> PartStockList = stockBll.GetList();
            CarBll carBll = new CarBll();
            List<string> carList = carBll.GetCarBrand();
            List<DAO.HotTable> ht = cateBll.HtGetlist("", "px");
            model.CarBrands = carList;
            model.PartAttrList = PartAttrList;
            model.PartBrandList = PartBrandList;
            model.PartCateList = PartCateList;
            model.PartStockList = PartStockList;
            model.ht = ht;
            var page = (from c in partList  select c).Skip((pageIndex - 1) * pageSize).Take(pageSize);//跳过前50条数据，取10条
            //Result r = new Result();
            //r.DataList = page.ToList();
            //r.totleCount = partList.Count;          
            //r.pageSize = pageSize;
            //r.currentPage = pageIndex;
            model.totleCount = partList.Count;
            model.pageSize = pageSize;
            model.currentPage = pageIndex;
            model.PartList = page.ToList();
            return View(model);
        }
        [AdminFilter]
        //[OutputCache(Duration = 120, VaryByParam = "*")]
        public ActionResult DPartsManage(string SelectByCate, int pageIndex = 1)
        {
            int pageSize = 30;      //数量 

            PartOutModel model = new PartOutModel();
            ProductBll partBll = new ProductBll();
            List<PartSimpleModel> partList;
            if (string.IsNullOrEmpty(SelectByCate) || SelectByCate == "-1")
            {
                partList = partBll.GetSimpleList(pageIndex," a.State=0 ", "ShowDate", "asc");
            }
            else
            {
                partList = partBll.GetSimpleList(pageIndex," a.State=0 and a.CategoryID=" + SelectByCate, "ShowDate", "desc");
            }
            PartsCateBll cateBll = new PartsCateBll();
            List<DAO.PartsCategory> PartCateList = cateBll.GetList();
            PartsBrandBll partBrandBll = new PartsBrandBll();
            List<DAO.PartBrand> PartBrandList = partBrandBll.GetList("", "BrandName");
            PartsAttrBll attrBll = new PartsAttrBll();
            List<DAO.PartAttr> PartAttrList = attrBll.GetList("", "Name");
            PartsStockBll stockBll = new PartsStockBll();
            List<DAO.PartStock> PartStockList = stockBll.GetList();
            CarBll carBll = new CarBll();
            List<DAO.CarM> carList = carBll.GetList();
            model.Cars = carList;
            model.PartAttrList = PartAttrList;
            model.PartBrandList = PartBrandList;
            model.PartCateList = PartCateList;
            model.PartStockList = PartStockList;

           // var page = (from c in partList select c).Skip((pageIndex - 1) * pageSize).Take(pageSize);//跳过前50条数据，取10条
            //Result r = new Result();
            //r.DataList = page.ToList();
            //r.totleCount = partList.Count;          
            //r.pageSize = pageSize;
            //r.currentPage = pageIndex;
            model.totleCount = 10000;
            model.pageSize = pageSize;
            model.currentPage = pageIndex;
            model.PartList = partList;
            return View(model);
        }
        /// <summary>
        /// 添加零件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [AdminFilter]
        public ActionResult AddPart(PartInBean model)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            ProductBll bll = new ProductBll();
            DAO.Parts m = new DAO.Parts();
            m.PartName = model.PartName;
            m.CategoryID = model.CategoryID;
            m.PartBrand = model.PartBrand;
            m.PartModel = model.PartModel;
            m.HotProductId = model.HotTable;
            m.PartSuppyNo = model.PartSuppyNo;
            m.Price = model.Price;
            m.Orderby = model.Orderby;
            m.State = model.State;
            m.ToCars = model.ToCars;
            m.Describe = model.Describe;
            m.PartTitle = model.PartTitle;
            m.PartSubtitle = model.PartSubtitle;
            m.ShowDate = DateTime.Now;
            m.insertDate = DateTime.Now;
            m.mDescribe = model.mDescribe;
            m.ProType =model.ProType;
            m.supply = model.supply;
            m.storeName = model.storeName;
            m.material = model.material;
            m.specifications = model.specifications;
            m.useSite = model.useSite;
            m.ParsColour = model.ParsColour;

            //添加网站
            m.herf = model.herf;
            DAO.Parts r = bll.Add(m);
            if (r != null)
            {
                DAO.PartStock stock = new DAO.PartStock();
                stock.PartID = r.ID;
                stock.StockNUM = model.StockNUM;
                stock.StockType = 0;
                PartsStockBll stockBll = new PartsStockBll();
                bool sr = stockBll.Add(stock);
                if (sr)
                {
                    map.Add("code", 200);//200 添加产品成功
                    map.Add("newPart", r);
                }
                else
                {
                    map.Add("code", 98);//98 添加产品成功 库存失败
                }
            }
            else
            {
                map.Add("code", 99);//99 添加产品失败
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加物流信息
        /// </summary>
        /// <param name="model"> </param>
        /// <returns></returns>
        [ValidateInput(false)]
        [AdminFilter]
        [HttpPost]
        public void wuliupart(info model)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            ProductBll part = new ProductBll();
            DAO.information m = new DAO.information();
            m.ordID = model.ordID;
            m.isshouhuo = 0;
            m.wuliuhao = model.wuliuhao;
            m.time = DateTime.Now;
            DAO.Order orderS = part.selOrderId(m.ordID);
            if (orderS != null)
            {
                DAO.information r = part.AddOrderNum(m);
                List<DAO.OrderProList> orderList = part.selPartName(m.ordID);
                string ParsNames = "";

                for (int i = 0; i < orderList.Count(); i++)
                {
                    if (i == orderList.Count() - 1)
                    {
                        ParsNames += orderList[i].ProductName;
                    }
                    else
                    {
                        ParsNames += orderList[i].ProductName + ",";
                    }

                    
                }
                   
                bool isPhone = PhoneNumber(orderS.CellPhone, model.wuliuhao, ParsNames);

                if (r != null && part.updateStatePars(m.ordID) && isPhone)
                {


                    Response.Write("<script>alert('添加成功!')</script>");
                    Response.Redirect("/Master/Index?l_4_1");
                }
                else
                {
                    Response.Write("<script>alert('添加失败!')</script>");
                    Response.Redirect("/Master/Index?l_4_1");
                }
            }
            else
            {
                Response.Write("<script>alert('添加失败!')</script>");
                Response.Redirect("/Master/Index?l_4_1");
            }
        }

        /// <summary>
        /// 已发货，给客户发
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public bool PhoneNumber(string PhoneNumber,string wuliu,string parsNames)
        {

            string content = parsNames + "已经发货。邮政小哥正在派送，订单号为" + wuliu;

            string data = "action=code&username=" + "460431214" + "&userpass=" + "jiangu.com" + "&mobiles=" + PhoneNumber.Trim() + "&content=" + content + "&codeid=" + "4197" + "&extnum=0";
            string returnNumber = PostSend("http://sms.jiangukj.com/SendSms.aspx", data);
            if (Convert.ToInt32(returnNumber) > 0)
            {

                MemberBLL bll = new MemberBLL();
                bool send = bll.SendOrderSMS(content, PhoneNumber);
                if (send)
                {

                    return true;
                }
            }

            return false;

        }


        private string PostSend(string url, String data)
        {
            String outdata = "0";
            try
            {
                //此demo是使用http  post方法发送短信。 
                Encoding utf8 = Encoding.UTF8;
                byte[] tmp = utf8.GetBytes(data);
                string result = utf8.GetString(tmp);
                byte[] buffer = Encoding.UTF8.GetBytes(result);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/x-www-form-urlencoded";
                req.ContentLength = buffer.Length;
                Stream reqst = req.GetRequestStream();
                reqst.Write(buffer, 0, buffer.Length);
                reqst.Flush();
                reqst.Close();
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                Stream resst = res.GetResponseStream();
                StreamReader sr = new StreamReader(resst);        //sr.ToString().
                outdata = sr.ReadToEnd();
                sr.Close();
                resst.Close();
            }
            catch (Exception exp)
            {
                outdata = "0";
            }
            return outdata;
        }

        /// <summary>
        /// 上架、下架
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult setPartStatus(int proid, int status)
        {
            ProductBll partBll = new ProductBll();
            bool r = partBll.setStatus(proid, status);
            return Json(r, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改产品 获取信息
        /// </summary>
        /// <param name="proID"></param>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult GetPartModel(int proID)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            ProductBll bll = new ProductBll();
            map.Add("Part", bll.GetPartModel(proID));
            PartsStockBll stockBll = new PartsStockBll();
            map.Add("PartStock", stockBll.GetModelByPartID(proID));
            return Json(map, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 查询物流订单
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult GetPamodel(string stringNumber=null)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            OrderBll bll = new OrderBll();
            List<DAO.information> info = new List<DAO.information>();
            info = bll.Lista(stringNumber);
            return View(info);
        }
        /// <summary>
        /// 修改产品信息
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        [ValidateInput(false)]
        public ActionResult UpdatePart(PartUpdateModel m)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            ProductBll bll = new ProductBll();
            bool r = bll.Update(m);
            PartsStockBll stockBll = new PartsStockBll();
            DAO.PartStock stock = stockBll.GetModelByPartID(m.ID);
            bool rr;
            if (stock == null)
            {
                stock = new DAO.PartStock();
                stock.StockNUM = 0;
                stock.PartID = m.ID;
                stock.StockType = 0;
                rr = stockBll.Add(stock);
            }
            else
            {
                stock.StockNUM = m.StockNUM;
                rr = stockBll.Update(stock);
            }
            int code = 0;
            if (r)
            {
                if (rr)
                {
                    code = 200;
                }
                else
                {
                    code = 99;
                }
            }
            else
            {
                if (rr)
                {
                    code = 98;
                }
                else
                {
                    code = 500;
                }
            }
            map.Add("code", code);
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 库存管理
        /// <summary>
        /// 修改库存数量
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="pp"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult UpdateStock(string pid, string pn)
        {
            int code;
            PartsStockBll bll = new PartsStockBll();
            DAO.PartStock m = bll.GetModel(pid);
            if (m != null)
            {
                m.StockNUM = int.Parse(pn);
                bool r = bll.Update(m);
                if (r)
                {
                    code = 200;//成功
                }
                else
                {
                    code = 99;//失败
                }
            }
            else
            {
                code = -1;
            }
            return Json(code, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 高级属性
        /// <summary>
        /// 添加产品扩展属性
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult AddPartExtend(DAO.PartsExtend model)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            PartExtendBll bll = new PartExtendBll();
            model.State = 1;
            if (model.ParentAttr == -1)
            {
                bool v = bll.IsExist(model.PartID.ToString(), "-1");
                if (v)
                {
                    map.Add("code", 98);

                }
                else
                {
                    DAO.PartsExtend r = bll.Add(model);
                    if (r != null)
                    {
                        map.Add("code", 200);
                        map.Add("PartExtends", GetExtends(model.PartID));
                    }
                    else
                    {
                        map.Add("code", 99);

                    }
                }
            }
            else
            {
                DAO.PartsExtend r = bll.Add(model);
                if (r != null)
                {
                    map.Add("code", 200);
                    map.Add("PartExtends", GetExtends(model.PartID));
                }
                else
                {
                    map.Add("code", 99);
                }
            }

            return Json(map, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取高级属性列表
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult GetExtendList(int pid)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            List<PartAttrOutModel> list = GetExtends(pid);
            map.Add("PartExtends", list);
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        private List<PartAttrOutModel> GetExtends(int pid)
        {
            List<PartAttrOutModel> list = new List<PartAttrOutModel>();

            PartExtendBll extBll = new PartExtendBll();
            List<PartAttrNameModel> allAttr = extBll.GetAttrs(pid.ToString());
            if (allAttr != null)
            {
                foreach (var item in allAttr)
                {
                    PartAttrOutModel model = new PartAttrOutModel();
                    model.AttrName = item.AttrName;
                    model.ParentAttr = item.ParentAttr;
                    model.AttrList = extBll.GetItems(pid, item.AttrName);
                    list.Add(model);
                }
            }
            return list;
        }
        [AdminFilter]
        public ActionResult DelExtendAttr(string type, string pid, string cut)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            PartExtendBll bll = new PartExtendBll();
            bool r = bll.Del(type, pid, cut);
            List<PartAttrOutModel> list = GetExtends(int.Parse(pid));
            map.Add("PartExtends", list);
            map.Add("status", r);
            return Json(map, JsonRequestBehavior.AllowGet);

        }

        #endregion
        #region 图片管理
        /// <summary>
        /// 获取产品图片
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult GetImgs(int partId)
        {
            ImgStockBll bll = new ImgStockBll();
            List<DAO.ImgStock> list = bll.GetList(" PartID= " + partId, "UploadDate");
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="partId"></param>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult delImgs(int partId, int imgId)
        {
            ImgStockBll bll = new ImgStockBll();
            bll.Del(imgId);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region 系统管理
        /// <summary>
        /// 导航管理
        /// </summary>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult NavManage()
        {
            //汽车列表
            MenuBll bll = new MenuBll();
            List<DAO.WebsiteMenu> list = bll.GetList();
            return View(list);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AdminFilter]
        public ActionResult DelMenu(string id)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            MenuBll bll = new MenuBll();
            bool r = bll.Del(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 活动管理
        ActiveBus bll = new ActiveBus();
          [AdminFilter]
        public ActionResult ActiveList()
        {
         
           List<DAO.Active> list= bll.GetActiveList("");
            return View(list);
        }
        /// <summary>
        /// 添加活动
        /// </summary>
        /// <param name="file"></param>
        /// <param name="inModel"></param>
        /// <param name="shour"></param>
        /// <param name="smin"></param>
        /// <param name="ehour"></param>
        /// <param name="emin"></param>
        /// <returns></returns>
          [AdminFilter]
          [HttpPost]
          public ActionResult AddActive(HttpPostedFileBase file, DAO.Active inModel, int shour, int smin, int ehour, int emin)
          {
          
              string path = "/Upload/Active/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
              string uploadpath = Server.MapPath(path);           
              DAO.Active m = new DAO.Active();
              if (file != null)
              {
                  if (!Directory.Exists(uploadpath))
                  {
                      Directory.CreateDirectory(uploadpath);
                  }
                  string nname = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now) + CommonUtils.RandomCode.GetRandomCode(8) + file.FileName.Substring(file.FileName.LastIndexOf("."));

              file.SaveAs(uploadpath + nname);
              m.ActiveIMG = path + nname;
              }           
              m.Common = inModel.Common;
              m.CreateDate = DateTime.Now;
              m.Decribe = inModel.Decribe;
              m.EndDate = inModel.EndDate.AddHours(ehour).AddMinutes(emin);
              m.Name = inModel.Name;
              m.StarDate = inModel.StarDate.AddHours(shour).AddMinutes(smin); ;
              m.Status = inModel.Status;
              m.SubName = inModel.SubName;
              bll.AddActive(m);
              return RedirectToAction("ActiveList");
          }
        /// <summary>
        /// 编辑活动
        /// </summary>
        /// <returns></returns>
          [AdminFilter]
          [HttpPost]
          public ActionResult EditActive(HttpPostedFileBase file, DAO.Active inModel, int shour, int smin, int ehour, int emin , int aid)
          {
             
              string path = "/Upload/Active/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
              string uploadpath = Server.MapPath(path);
              DAO.Active m = bll.GetActive(aid);
              if (file != null)
              {
                  if (!Directory.Exists(uploadpath))
                  {
                      Directory.CreateDirectory(uploadpath);
                  }
                  string nname = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now) + CommonUtils.RandomCode.GetRandomCode(8) + file.FileName.Substring(file.FileName.LastIndexOf("."));

                  file.SaveAs(uploadpath + nname);
                  m.ActiveIMG = path + nname;
              }
              m.Common = inModel.Common;
              m.CreateDate = DateTime.Now;
              m.Decribe = inModel.Decribe;
              m.EndDate = inModel.EndDate.AddHours(ehour).AddMinutes(emin);
              m.Name = inModel.Name;
              m.StarDate = inModel.StarDate.AddHours(shour).AddMinutes(smin); ;
              m.Status = inModel.Status;
              m.SubName = inModel.SubName;
              bll.UpActive(m);
              return RedirectToAction("ActiveList");
          }
  
        /// <summary>
        /// 获取活动产品
        /// </summary>
        /// <returns></returns>
            [AdminFilter]     
          public ActionResult GetActivePros(int aid=-1)
          { 
              ProductBll partBll = new ProductBll();
          
              ViewBag.alist = bll.GetActiveList("1");
              List<DAO.Parts> list;
              if (aid == -1)
              {
                  list = partBll.GetList(" ProType=1 and State=1 ", "id", "desc");
              }
              else {
                  list = partBll.GetListByActive(aid);
              }
              return View(list);
          }
            [AdminFilter]     
          public ActionResult GetAttrByPro(int aid,int pid)
          {           
               List<PartAttrOutModel> list = GetExtends(pid);
             
               ActiveProAttr2 apa = bll.GetActiveProAttr(aid, pid);
              return Json(new { atts=list,apa=apa}, JsonRequestBehavior.AllowGet);
          }
            [AdminFilter]
            public ActionResult AddActivePro(int aid, int pid,string pname, decimal newprice, string attid, bool hasAttr, int count)
          {
              Dictionary<string, string> map = new Dictionary<string, string>();
              try
              {
               
                  DAO.ActivePros m = new DAO.ActivePros();
                  m.ActiveID = aid;
                  m.ProID = pid;
                  m.NewPrice = newprice;
                  m.ProName = pname;
                  m.Stock = count;
                  bll.addActivePro(m);
                  if (hasAttr)
                  {
                      DAO.ActiveAttr mm = new DAO.ActiveAttr();
                      mm.ActiveID = aid;
                      mm.AProID = pid;
                      mm.NewPrice = newprice;
                      mm.AtrrID = Convert.ToInt32(attid);
                      mm.Stock = count;
                      bll.addActiveAtts(mm);
                      map.Add("r", "ok");
                  }
              }catch(Exception e){
                  map.Add("r", "error");
                  map.Add("msg", e.Message);
              }
              return Json(map);
          }
            [AdminFilter]     
            public ActionResult DelActivePro(int pid,int aid)
          {           
           
              bll.DelActivePro(pid,aid);
              return Json(new {r=true});
          }
            [AdminFilter]    
             public ActionResult GetActiveImg()
            { 
                
                 List<DAO.ActiveImg> list=   bll.GetActiveImg();
                 return View(list);
            }
            [AdminFilter]
            public ActionResult DelActiveImg(int id)
            {
              
                bool r = bll.DelActiveImg(id);
                return Json(new { r=r});
            }
        #endregion 
        [AdminFilter]
            public ActionResult GetCarYear(string brand)
            {
                CarBll carbll = new CarBll();
              List<string> list=  carbll.GetCarYear(brand);
              return Json(list);
            }
        [AdminFilter]
        public ActionResult GetCarModel(string brand,string year)
        {
            CarBll carbll = new CarBll();
            List<DAO.CarM> list = carbll.GetCarModel(brand,year);
            return Json(list);
        }
        public ActionResult GetCarM(string ids)
        {
            CarBll carbll = new CarBll();
            List<DAO.CarM> list = carbll.GetCarM(ids);
            return Json(list);
        }
        [AdminFilter]
        public ActionResult HotTable()
        {

            DAO.HotTable hotT = new DAO.HotTable();
            ProductBll bll = new ProductBll();
            List<DAO.HotTable> list = bll.GetHotTable();
            return View(list);
        }

        /// <summary>
        /// 添加热卖分类
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public void AddHot()
        {
            DAO.HotTable ht = new DAO.HotTable();
            ht.HotName = Request.Form["Name"].ToString();
            ht.Status = Convert.ToInt32(Request.Form["IsBool"].ToString());
            ht.px = Convert.ToInt32(Request.Form["Px"].ToString());
            Dictionary<string, Object> map = new Dictionary<string, Object>();

            HotProductBll htbll = new HotProductBll();
            bool r = htbll.AddHot(ht);
            if (r)
            {
                Response.Write("<script>alert('添加成功!')</script>");
                Response.Redirect("/Master/HotTable");
            }
            else
            {
                Response.Write("<script>alert('添加失败!')</script>");
                Response.Redirect("/Master/HotTable");
            }
            //return Json(map, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("CarsManage");
        }

        /// <summary>
        /// 修改热卖分类
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditHot()
        {
            DAO.HotTable ht = new DAO.HotTable();
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            ht.ID = Convert.ToInt32(Request.Form["ID"].ToString());
            ht.HotName = Request.Form["Name"].ToString();
            ht.Status = Convert.ToInt32(Request.Form["IsBool"].ToString());
            ht.px = Convert.ToInt32(Request.Form["Px"].ToString());
            HotProductBll htbll = new HotProductBll();
            bool r = htbll.updateHot(ht);
            if (r)
            {
                Response.Write("<script>alert('添加成功!')</script>");
                Response.Redirect("/Master/HotTable");
            }
            else
            {
                Response.Write("<script>alert('添加失败!')</script>");
                Response.Redirect("/Master/HotTable");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
            // return RedirectToAction("CarsManage");
        }
        /// <summary>
        /// 删除热卖分类
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult DelHot(string id)
        {
            //删除汽车
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            HotProductBll htbll = new HotProductBll();
            bool r = htbll.DelHot(id);
            if (r)
            {
                map.Add("result", "ok");
            }
            else
            {
                map.Add("result", "error");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 修改地址
        /// </summary>
        /// <param name="sendInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult editdizd(string sendInfo)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            DAO.Order inModel = jss.Deserialize<DAO.Order>(sendInfo);
            HotProductBll dc = new HotProductBll();
            bool up = dc.updatedz(inModel);
            if (up)
            {
                map.Add("status", "ok");
                map.Add("msg", "修改成功！");
            }
            else
            {
                map.Add("status", "error");
                map.Add("msg", "修改失败！");
            }
            
            return Json(map, JsonRequestBehavior.AllowGet);
        }

       /// <summary>
       /// 修改状态
       /// </summary>
       /// <param name="OrderId"></param>
       /// <returns></returns>
        [HttpPost]
        public ActionResult editzt(string OrderId)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            DAO.Order oredr = new DAO.Order();
            oredr.OrderId = OrderId;
            HotProductBll dc = new HotProductBll();
            bool ue = dc.upt(oredr);
            if (ue)
            {
                map.Add("status", "ok");
                map.Add("msg", "状态修改成功！");
            }
            else
            {
                map.Add("status", "error");
                map.Add("msg", "状态修改失败！");
            }
                 return Json(map, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查询地址
        /// </summary>
        /// <param name="OrderNumber"></param>
        /// <returns></returns>
        [HttpPost]
        [AdminFilter]
        public ActionResult detail(string OrderNumber)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            if (string.IsNullOrEmpty(OrderNumber))
            {
                return RedirectToAction("Index");
            }
            else
            {
                OrderBll obll = new OrderBll();
                DAO.Order info = obll.Detail(OrderNumber);
                map.Add("name", info.Consignee);
                map.Add("Prov", info.Prov);
                map.Add("City", info.City);
                map.Add("Area", info.Area);
                map.Add("Address", info.Address);
                map.Add("CellPhone", info.CellPhone);
                return Json(map, JsonRequestBehavior.AllowGet);
            }
          
        }
        [HttpPost]
        public ActionResult wuliuhapas(string ordID)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            if (string.IsNullOrEmpty(ordID))
            {
                return RedirectToAction("Index");
            }
            else
            {
                OrderBll bll = new OrderBll();
                DAO.information matin = bll.delt(ordID);
                if (matin == null)
                {
                    map.Add("name", "0");
                }
                else
                {
                    map.Add("name", matin.wuliuhao);

                }
                

            }
            return Json(map, JsonRequestBehavior.AllowGet);
        }
        //查询物流订单
        public ActionResult commselect(string stringcommname=null)
        {
            Commlect set = new Commlect();
            OrderBll comm = new OrderBll();
            set.sede = comm.commselect(stringcommname);
            return View(set);
        }
        //删除物流订单
        public ActionResult commedits(int pid)
        {

            bll.commedits(pid);
            return Json(new { r = true });
        }
        //显示 是
        public ActionResult commsf(int commID)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            OrderBll cd = new OrderBll();
            commselect set = new commselect(); ;
            set.ID = commID;
            bool ce = cd.commupdata(set);
            if (ce)
            {
                map.Add("status", "ok");
                map.Add("msg", "更新成功！");
            }
            else
            {
                map.Add("status", "error");
                map.Add("msg", "更新失败！");
            }
            return Json(map, JsonRequestBehavior.AllowGet);
            
        }
        //显示 否
        public ActionResult commfas(int commID)
        {
            Dictionary<string, object> map = new Dictionary<string, object>();
            OrderBll cd = new OrderBll();
            commselect set = new commselect(); ;
            set.ID = commID;
            bool ce = cd.commupfos(set);
            if (ce)
            {
                map.Add("status", "ok");
                map.Add("msg", "更新成功！");
            }
            else
            {
                map.Add("status", "error");
                map.Add("msg", "更新失败！");
            }
            return Json(map, JsonRequestBehavior.AllowGet);

        }
        //查询众筹
        public ActionResult crowslect()
        {
            Crowshows set = new Crowshows();
            OrderBll comm = new OrderBll();
            set.shod = comm.Crowshw();
            return View(set);
        }
        //添加众筹
        [HttpPost]
        // public ActionResult AddcrowdFunding(HttpPostedFileBase file, string add_Act_Name, string Numbers, string Pids, string add_Act_sdate, string add_Act_shour, string add_Act_smin, string add_Act_edate, string add_Act_ehour, string add_Act_emin, string add_Act_status)
        public ActionResult AddcrowdFunding(HttpPostedFileBase file, DAO.CrowdFunding inModel, int shour,int smin,int ehour,int emin)
        {

           
            string path = "/Upload/CrowdFunding/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            JavaScriptSerializer jss = new JavaScriptSerializer();
            string uploadpath = Server.MapPath(path);
            if (file != null)
            {
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }


                string nname = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now) + CommonUtils.RandomCode.GetRandomCode(8) + file.FileName.Substring(file.FileName.LastIndexOf("."));

                file.SaveAs(uploadpath + nname);
                inModel.Banner = path + nname;
            }
            inModel.ShowTime = DateTime.Now;
            inModel.EndTime = inModel.EndTime.AddHours(ehour).AddMinutes(emin);
            inModel.StarTime = inModel.StarTime.AddHours(shour).AddMinutes(smin); ;

            ActiveBus bc = new ActiveBus();
            bool r = bc.funding(inModel);

            return RedirectToAction("crowslect");
        }
        ////点击编辑查询众筹
        //public ActionResult EditcrowdFunding(HttpPostedFileBase file, string Pid)
        //{
        //    Dictionary<string, Object> map = new Dictionary<string, Object>();
        //        OrderBll obll = new OrderBll();
        //        Crowshows set = new Crowshows();
        //        CrowShow info = obll.Crowselect(Pid);
        //        string path = "/Upload/CrowdFunding/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
        //        string uploadpath = Server.MapPath(path);
        //        if (file != null)
        //        {
        //            if (!Directory.Exists(uploadpath))
        //            {
        //                Directory.CreateDirectory(uploadpath);
        //            }
        //            string nname = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now) + CommonUtils.RandomCode.GetRandomCode(8) + file.FileName.Substring(file.FileName.LastIndexOf("."));
        //            file.SaveAs(uploadpath + nname);
        //            info.Banner = path + nname;
        //        }   
        //            map.Add("Banner", info.Banner);
        //            map.Add("ListTille", info.ListTille);
        //            map.Add("StarTime", info.StarTime.ToString());
        //            map.Add("ShowTime", info.ShowTime.ToString());
        //            map.Add("EndTime", info.EndTime.ToString());
        //            map.Add("isBool", info.isBool);
        //            map.Add("Number", info.Number);
        //            map.Add("Pid", info.Pid);
        //        return Json(map, JsonRequestBehavior.AllowGet);
        //}
        //修改众筹
        [HttpPost]
        public ActionResult EditcrowdFundingd(HttpPostedFileBase file, DAO.CrowdFunding inModel, int shour, int smin, int ehour, int emin, int aid)
        {

            string path = "/Upload/CrowdFunding/" + string.Format("{0:yyyyMMdd}", DateTime.Now) + "/";
            string uploadpath = Server.MapPath(path);
            DAO.CrowdFunding cc = bll.cowdFunding(aid);
            if (file != null)
            {
                if (!Directory.Exists(uploadpath))
                {
                    Directory.CreateDirectory(uploadpath);
                }
                string nname = string.Format("{0:yyMMddHHmmssfff}", DateTime.Now) + CommonUtils.RandomCode.GetRandomCode(8) + file.FileName.Substring(file.FileName.LastIndexOf("."));

                file.SaveAs(uploadpath + nname);
                cc.Banner = path + nname;
            }
            cc.Number = inModel.Number;
            cc.isBool = inModel.isBool;
            cc.ListTille = inModel.ListTille;
            cc.Number = inModel.Number;
            cc.ShowTime = DateTime.Now;
            cc.EndTime = inModel.EndTime.AddHours(ehour).AddMinutes(emin);
            cc.StarTime = inModel.StarTime.AddHours(shour).AddMinutes(smin); ;
            bll.editCrowd(cc);
            return RedirectToAction("crowslect");
        }
    }

}


        
