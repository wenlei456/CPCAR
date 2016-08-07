using ActiveModule;
using CommonUtils;
using CParts_Online.Controllers.master;
using CParts_Online.Models;
using MemberModule.Business;
using MemberModule.Model;
using OrderModule;
using OrderModule.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Cparts_Online.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Home/

        public ActionResult RegView()
        {          
           
            return View();
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RegMember(RegUserModel model)
        {            
            if (ModelState.IsValid)
            {
                 MemberBLL bll = new MemberBLL();
                 if (bll.VerRegCode(model.PhoneNumber, model.PhoneCode))
                 {
                     FriendCodeBLL fbll = new FriendCodeBLL();

                     DAO.MemberLevel levelModel = new DAO.MemberLevel();
                     DAO.MemberBase m = new DAO.MemberBase();
                     m.LoginName = model.loginName;
                     m.PassWord = Md5Util.PwdMd5(model.passWord);
                     m.Mobile = model.PhoneNumber;
                     m.Email = model.eMail;
                     m.State = 1;
                     int upUser = fbll.GetUpUser(model.FriendCode);
                     m.UpUser = upUser;//上级用户
                     m.Levels = 0;
                     m.Integral = 0;
                     m.Source = "self";
                     m.RegDate = DateTime.Now;
                     DAO.MemberBase u = bll.Reg(m); //注册
                     Session["memberID"] = u.ID;
                     levelModel = bll.GetLevel(u.ID, upUser);//注册初始化级别
                     VoucherBus vbll = new VoucherBus();
                     vbll.RegGiveAVoucher(u.ID);//给一张注册优惠券
                     u.Levels = levelModel.LevelId;
                     u.Integral = levelModel.MinIntegral;
                     DAO.MemberBase allu = bll.UpdateUser(u);
                     if (allu != null)
                     {
                         //给上级积分
                         bll.GiveUpUserInt(allu.ID, upUser);
                     }
                     return RedirectToAction("UserHome");
                 }
                 else {
                     ViewBag.phoneCodeValid = "手机注册码错误";
                     return View("RegView", model);
                 }
            }
            else {
                return View("RegView", model);
            }
        }
        [HttpPost]
        public bool PhoneNumber(string PhoneNumber)
        {
            Random Rdm = new Random();
            int iRdm = Rdm.Next(1000, 9999);
            //string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string Numb = iRdm.ToString();
            string data = "action=code&username=" + "460431214" + "&userpass=" + "jiangu.com" + "&mobiles=" + PhoneNumber.Trim() + "&content=" + Numb + "&codeid=" + "4069" + "&extnum=0";
            string returnNumber = PostSend("http://sms.jiangukj.com/SendSms.aspx", data);
            if (Convert.ToInt32(returnNumber) > 0)
            {
                Session["phoneNumberCode"] = Numb;

                MemberBLL bll = new MemberBLL();
                bool send = bll.SendSMS(Numb, PhoneNumber);
                if (send)
                {

                    return true;
                }
            }

            return false;

        }

        [HttpPost]
        public bool PhoneNumberFor(string PhoneNumber)
        {
            Random Rdm = new Random();
            int iRdm = Rdm.Next(1000, 9999);
            //string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string Numb = iRdm.ToString();
            string data = "action=code&username=" + "460431214" + "&userpass=" + "jiangu.com" + "&mobiles=" + PhoneNumber.Trim() + "&content=" + Numb + "&codeid=" + "4069" + "&extnum=0";
            string returnNumber = PostSend("http://sms.jiangukj.com/SendSms.aspx", data);
            if (Convert.ToInt32(returnNumber) > 0)
            {
                MemberBLL bll = new MemberBLL();
                bool send = bll.SendSMS(Numb, PhoneNumber);
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
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            //VerCode
            return View();
        }
        [HttpPost]    
        public ActionResult Login(LoginModel m)
        {
            //VerCode
            if (ModelState.IsValid)
            {
                MemberBLL bll = new MemberBLL();
                DAO.MemberBase model = bll.Login(m.loginName, m.passWord);
                if (model != null)
                {                    
                    Session["memberID"] = model.ID;
                    Session["mobile"] = model.Mobile;
                    Session.Timeout = 20;
                    if (Session["ControllerUrl"] != null && Session["ActionUrl"] != null)
                    {
                        string action=Session["ActionUrl"].ToString();
                        string con=Session["ControllerUrl"].ToString();
                        Dictionary<string, object> objs;
                        if (Session[con + action] != null)
                        {
                            objs = (Dictionary<string,object>)Session[con + action];
                            StringBuilder qs = new StringBuilder("/" + con + "/" + action + "?");                          
                                foreach (var item in objs)
                                {
                                    qs.Append(item.Key + "=" + item.Value+"&&");
                                }                             
                            return Redirect(qs.ToString());
                        }
                        else
                        {
                            return RedirectToAction(action, con);
                        }
                        //Session["ControllerUrl"] = null;
                        //Session["ActionUrl"] = null;
                    }
                    else
                    {
                        return RedirectToAction("MyOrder");
                    }
                }
                else
                {
                    ViewBag.loginFail = "用户名或密码错误！";
                    return View(m); 
                }
            }
            else {
              
                return View(m);
            }
        }
        /// <summary>
        /// 用户中心
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult UserHome()
        {
            string uid = Session["memberID"].ToString();
            MemberBLL mbBll=new MemberBLL ();
            MemberLevelBLL mlBll = new MemberLevelBLL();
            UserHomeVM u = new UserHomeVM();
            u.Userinfo = mbBll.GetUserByID(uid);//账户信息
            List<LeveExp> leList = new List<LeveExp>();
            List<UserLeveExpBM> ulbm= mlBll.GetLeveExpByUid(uid);
            foreach (var item in ulbm)
            {
                LeveExp le = new LeveExp();
                le.Income = item.Income;
                le.MaxIntegral = item.MaxIntegral;
                le.MinIntegral = item.MinIntegral;
                le.Name = item.Name;
                le.Ratio = item.Ratio;
                le.UExp = item.UExp;
                leList.Add(le);
            }
            u.UserLevel = leList;//账户当前等级
            return View(u);
        }
        /// <summary>
        /// 积分记录
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult ExpRecord()
        {
           string uid = Session["memberID"].ToString();
           ExpRecordBLL erBll = new ExpRecordBLL();
          List<DAO.ExpRecord> exList= erBll.GetExpList(uid);
          return View(exList);
        
        }
        [UserFilter]
        public ActionResult RebateRecord()
        {
            string uid = Session["memberID"].ToString();
            RebateRecordBLL erBll = new RebateRecordBLL();
            List<DAO.RebateRecord> exList = erBll.GetRebateRecordList(uid);
            return View(exList);

        }
        /// <summary>
        /// 检测是否登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult IsLogin()
        {
            Dictionary<string, bool> r = new Dictionary<string, bool>();
            if (Session["memberID"] == null)
            {
                r.Add("r",false);
                //Session["ControllerUrl"] = "Car";
                //Session["ActionUrl"] = "index";
                return Json(r,JsonRequestBehavior.AllowGet);
            }
            else {
                r.Add("r", true);
                return Json(r, JsonRequestBehavior.AllowGet);
            }
        
        }
        [UserFilter]
        public ActionResult LoginOut()
        {
            Session["memberID"] = null;
            return RedirectToAction("Login");
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult UpdatePW()
        {
            return View();
        }
        [HttpPost]
        [UserFilter]
        public ActionResult UpdatePW(UpdatePassWordVM vm)
        {
            if (ModelState.IsValid)
            {
                string uid = Session["memberID"].ToString();               
                MemberBLL mbBll = new MemberBLL();
               bool r= mbBll.UpdatePasswordOld(uid,vm.newPW);
               if (r)
               {
                   ViewBag.msg = "密码修改成功";
               }
               else {
                   ViewBag.msg = "密码修改失败，请稍后再试";
               }
                return View(vm);
            }
            else
            {
                return View(vm);
            }
        }


        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UpdatePassOld()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdatePassOld(Forgetpassword vm)
        {
            if (ModelState.IsValid)
            {
                //string uid = Session["memberID"].ToString();
                MemberBLL mbBll = new MemberBLL();
                bool r = mbBll.UpdatePasswordOld(vm.PhoneNumber, vm.newPW);
                if (r)
                {
                    ViewBag.msg = "密码修改成功";
                }
                else
                {
                    ViewBag.msg = "密码修改失败，请稍后再试";
                }
                return View(vm);
            }
            else
            {
                return View(vm);
            }
        }

        /// <summary>
        /// 我的订单
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult MyOrder()
        {
            string uid = Session["memberID"].ToString();
            OrderBuss oBll = new OrderBuss();
            MyOrderInfo info = oBll.GetMyorder(uid);
            return View(info);
        }
    }
}
