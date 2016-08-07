using ActiveModule;
using OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace m._91cpcar.com.Controllers
{
    public class CommonController : Controller
    {
        OrderBuss obbll = new OrderBuss();
        VoucherBus vbll = new VoucherBus();
        // GET: /Common/
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
                        if (o.paytype == "weixin")
                        {
                            io.Bank = "微信";
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
