using ActiveModule;
using CommonModule;
using CParts_Online.Controllers.master;
using Cparts_Service;
using OrderModule;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CParts_Online.Controllers
{
    public class CommonController : Controller
    {
        OrderBuss obbll = new OrderBuss();
        VoucherBus vbll = new VoucherBus();
        CommonBll cbll = new CommonBll();
        CarBll carBll = new CarBll();
        /// <summary>
        /// 获取导航产品类型
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult Nav(int type)
        {

            List<DAO.PartsCategory> list = cbll.GetList();
            if (type == 0)
            {
                return View(list);
            }
            else
            {
                return View("Nav2", list);
            }
        }

        /// <summary>
        /// 车型数据
        /// </summary>
        /// <returns></returns>
        [OutputCache(Duration = 600, VaryByParam = "*")]
        public ActionResult GetCarList()
        {
            List<DAO.CarM> carList = carBll.GetList();
            return View(carList);
        }
        [UserFilter]
        public ActionResult PayOrder(string oid)
        {
            if (Session["memberID"] != null)
            {
                int uid = Convert.ToInt32(Session["memberID"]);
                PayOrderOM outMode = new PayOrderOM();
                outMode.voucher = vbll.GetVoucherByUid(uid, 0);//用户可以使用的优惠券
                if (!string.IsNullOrEmpty(oid))
                {
                    outMode.order = obbll.GetOrder(oid);
                }
                return View(outMode);
            }
            else
            {
                return RedirectToAction("Login", "member");
            }
        }

        [HttpPost]
        public ActionResult PayOrder(PayOrderModel o)
        {
            try
            {
                if (Session["memberID"] != null)
                {
                    int uid = Convert.ToInt32(Session["memberID"]);
                    List<DAO.OrderPay> ol = new List<DAO.OrderPay>();
                    DAO.Order order = obbll.GetOrder(o.oid);
                    decimal totle = order.TotalPrice;
                    decimal dis = 0;
                    decimal payprice = 0;
                    if (!string.IsNullOrEmpty(o.coupon))//复合付款有优惠券
                    {
                        //验证优惠券
                        DAO.Voucher voucher = vbll.GetVoucherModel(uid, o.coupon, 0);
                        if (voucher == null)
                        {
                            return Json(new { r = "error", msg = "优惠券验证失败！" });
                        }
                        else
                        {

                            dis = voucher.Price;//优惠券优惠金额
                            DAO.OrderPay io = new DAO.OrderPay();
                            io.Amount = dis;
                            io.Bank = "优惠券";
                            io.OrderId = o.oid;
                            io.OrderType = o.ordertype;
                            io.PayState = "未支付";
                            io.PayType = 1;//直接支付1优惠券
                            io.RecordState = 1;
                            io.Uid = Convert.ToInt32(Session["memberID"]);
                            io.Remark = voucher.VoucherNumber;
                            ol.Add(io);
                        }
                    }
                    payprice = totle - dis;
                    if (payprice > 0)
                    {
                        DAO.OrderPay io = new DAO.OrderPay();
                        io.Amount = payprice;
                        if (o.paytype == "alipay")
                        {
                            io.Bank = "支付宝";
                        }
                        io.OrderId = o.oid;
                        io.OrderType = o.ordertype;
                        io.PayState = "未支付";
                        io.PayType = 0;//直接支付1优惠券
                        io.RecordState = 1;
                        io.Uid = Convert.ToInt32(Session["memberID"]);
                        ol.Add(io);
                    }
                    bool r = obbll.AddOrderPay(o.oid, ol);
                    return Json(new { r = "ok" });
                }
                else
                {
                    return Json(new { r = "nologin" });
                }
            }
            catch (Exception e)
            {
                return Json(new { r = "error", msg = "异常：" + e.Message });
            }
        }



        [HttpPost]
        public ActionResult GetPartModel(string brand)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            CarBll carBll = new CarBll();
            List<DAO.CarNew> carList = carBll.GetListCar(brand);
            for (int i = 0; i < carList.Count; i++)
            {
                DAO.CarNew newCar = carList[i];
                Dictionary<string, object> map = new Dictionary<string, object>();

                map.Add("brand", newCar.Brand);

                map.Add("model", newCar.Model);
                list.Add(map);

            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetPartModel2(string brand, string model)
        {
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();

            CarBll carBll = new CarBll();
            List<DAO.CarNewYear> carList = carBll.GetListCarByYear(brand, model);
            for (int i = 0; i < carList.Count; i++)
            {
                DAO.CarNewYear newCar = carList[i];
                Dictionary<string, object> map = new Dictionary<string, object>();

                map.Add("year", newCar.ProYear);

                map.Add("model", newCar.Model);
                list.Add(map);

            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
      
    }
    public class PayOrderOM
    {
        public DAO.Order order { get; set; }
        public List<DAO.Voucher> voucher { get; set; }
    }
    public class PayOrderModel
    {
        public string oid { get; set; }//订单号
        public string paytype { get; set; }//支付类型
        public decimal payprice { get; set; }//支付价格
        public string coupon { get; set; }//优惠券号
        public decimal couponprice { get; set; }//优惠金额
        public int ordertype { get; set; }
    }
}