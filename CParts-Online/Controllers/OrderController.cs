﻿using CommonUtils;
using CParts_Online.Controllers.master;
using CParts_Online.Models;
using MemberModule.Business;
using OrderModule;
using OrderModule.model;
using ProductModule;
using ProductModule.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace CParts_Online.Controllers
{
    public class OrderController : Controller
    {
        [UserFilter]
        public ActionResult goOrder()
        {
            GoOrder goOrder = new GoOrder();

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
                //car list
                goOrder.carProList = list;
                //会员等级
                string uid = Session["memberID"].ToString();
                MemberLevelBLL memLeveBll = new MemberLevelBLL();
                DAO.MemberLevel memLevel = memLeveBll.GetLeveByUid(uid);
                goOrder.userLevel = memLevel;
                //

                return View(goOrder);
            }
            else
            {
                return RedirectToAction("index", "Car");
            }
        
        }
        [HttpPost]
        [UserFilter]
       public ActionResult AddOrder(string orderInfo)
        {
            Dictionary<string, object> mp = new Dictionary<string, object>();
            string UID = Session["memberID"].ToString();
            JavaScriptSerializer jss = new JavaScriptSerializer();
            OrderInfo oInfo = jss.Deserialize<OrderInfo>(orderInfo);
            if (oInfo != null && oInfo.shopList.Count > 0)
            {
                List<OrderModule.model.Shop> slist = new List<OrderModule.model.Shop>();
                foreach (var item in oInfo.shopList)
                {
                    OrderModule.model.Shop shop = new OrderModule.model.Shop();
                    shop.ID = item.ID;
                    shop.Price = item.Price;
                    shop.Qty = item.Qty;
                    shop.Attrs = item.Attrs;
                    shop.Type = item.Type;
                    slist.Add(shop);
                    //购物车中活动产品的活动是否过期
                    if (item.Type != "-1")
                    {
                        string msg="";
                        ActiveModule.ActiveBus abll = new ActiveModule.ActiveBus();
                         bool result=  abll.ValiActivePro(int.Parse(item.Type),int.Parse(item.ID),item.Attrs,int.Parse(item.Qty), ref msg);
                         if (!result)
                         {
                             mp.Add("status", "error");
                             mp.Add("msg", msg);
                             return Json(mp, JsonRequestBehavior.AllowGet);
                         }
                    }
                    //------
                }
                OrderBuss obll = new OrderBuss();
                //产品验证 活动产品=》活动是否过期
                //价格验证
                //增加积分1.自己 2.上级
                //给上级返利 
                MemberBLL mbmBll = new MemberBLL();
                DAO.MemberBase mb = mbmBll.GetUserByID(UID);
                MemberLevelBLL memLeveBll = new MemberLevelBLL();
                DAO.MemberLevel memLevel = memLeveBll.GetLeveByUid(UID);
                decimal orderPrice = decimal.Parse(oInfo.shopTotlePrice);
                decimal shopListTotle=0;
                bool vr = obll.OrderAmountCheck(slist, orderPrice, memLevel.Ratio,out shopListTotle);
                if (vr)
                {
                    if (ValidateSendInfo(oInfo.sendInfo))
                    {
                        OrderModule.model.SendInfo sendinfo = new OrderModule.model.SendInfo();
                        sendinfo.Address = oInfo.sendInfo.Address;
                        sendinfo.Area = oInfo.sendInfo.Area;
                        sendinfo.City = oInfo.sendInfo.City;
                        sendinfo.Name = oInfo.sendInfo.Name;
                        sendinfo.Phone = oInfo.sendInfo.Phone;
                        sendinfo.Prov = oInfo.sendInfo.Prov;
                        //价格验证通过增加订单
                        string OrderNumber = obll.AddOrder(slist, sendinfo, UID, memLevel.Ratio, orderPrice, shopListTotle);

                        if (OrderNumber != null)
                        {
                            //订单增加成功
                            mp.Add("status", "ok");
                            mp.Add("order", OrderNumber);
                            //给当前用户增加积分，给上级增加积分，给上级返利
                            //反积分
                            ExpRecordBLL expBll = new ExpRecordBLL();
                            expBll.GiveExp(mb, orderPrice, OrderNumber);
                            //返利如果有上级
                            if (mb.UpUser != -1 && mb.UpUser != 0)
                            {
                                RebateRecordBLL rrBll = new RebateRecordBLL();
                                rrBll.GiveParentRebate(UID, mb.LoginName, mb.UpUser, OrderNumber, orderPrice);
                            }
                            return Json(mp, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            //订单增加异常
                            mp.Add("status", "catch");
                            return Json(mp, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else {
                        //价格验证失败
                        mp.Add("status", "error");
                        mp.Add("msg", "配送信息验证失败！");
                        return Json(mp, JsonRequestBehavior.AllowGet);
                    }
                  
                }
                else
                {
                    //价格验证失败
                    mp.Add("status", "error");
                    mp.Add("msg", "价格验证失败！");
                    return Json(mp, JsonRequestBehavior.AllowGet);
                }

            }
            else {
                //价格验证失败
                mp.Add("status", "error");
                mp.Add("msg", "没有产品！");
                return Json(mp, JsonRequestBehavior.AllowGet);
            }         
           
        }
        [UserFilter]
        public ActionResult Detail(string id)
        {
            string uid = Session["memberID"].ToString();
            OrderBuss oBll = new OrderBuss();
            OrderDetail info = oBll.OrderDetail(uid, id);
            return View(info);
        
        }
        private bool ValidateSendInfo(CParts_Online.Models.SendInfo info)
        {           
            if (string.IsNullOrEmpty(info.Name))
            {               
                return false;
            }
            if (string.IsNullOrEmpty(info.Address))
            {
                return false;
            }
            if (string.IsNullOrEmpty(info.Area))
            {
                return false;
            } if (string.IsNullOrEmpty(info.City))
            {
                return false;
            } if (string.IsNullOrEmpty(info.Phone) || !RegExpUtil.Isphone(info.Phone))
            {
                return false;
            } if (string.IsNullOrEmpty(info.Prov))
            {
                return false;
            }            
            return true;
        
        }
    }
}
