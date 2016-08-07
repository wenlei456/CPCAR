using ActiveModule;
using CommonUtils;
using CParts_Online.Models;
using m.cpcar.com.Controllers.master;
using m.cpcar.com.Models;
using MemberModule.Business;
using MemberModule.Model;
using OrderModule;
using OrderModule.model;
using ProductModule.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace m._91cpcar.com.Controllers
{
    public class MemberController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Reg()
        {
            if (Session["memberID"] == null)
            {
                return View();
            }
            else {
                return RedirectToAction("Myhome", "Member");
            }
           // Session["mobile"] = model.Mobile;
          
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Reg(string phone, string phonecode, string fcode)
        {
            //RegMsg RegUserModel model
           
                MemberBLL bll = new MemberBLL();
                if (!bll.VerPhone(phone))
                {                 
                    return Json(new { r = false, msg = "手机号已经被注册！" });
                }
                if (bll.VerRegCode(phone, phonecode))//验证手机号与手机验证码
                {
                    FriendCodeBLL fbll = new FriendCodeBLL();

                    DAO.MemberLevel levelModel = new DAO.MemberLevel();
                    DAO.MemberBase m = new DAO.MemberBase();
                    m.LoginName = phone;
                    m.PassWord = Md5Util.PwdMd5(phone.Substring(5));
                    m.Mobile = phone;
                    //m.Email = "";
                    m.State = 1;
                    int upUser = fbll.GetUpUser(fcode);
                    m.UpUser = upUser;//上级用户
                    m.Levels = 0;
                    m.Integral = 0;
                    m.Source = "self";
                    m.RegDate = DateTime.Now;
                    DAO.MemberBase u = bll.Reg(m); //注册            
                    Session["memberID"] = u.ID;
                    VoucherBus vbll = new VoucherBus();
                    vbll.RegGiveAVoucher(u.ID);//给一张注册优惠券
                    levelModel = bll.GetLevel(u.ID, upUser);//注册初始化级别
                    u.Levels = levelModel.LevelId;
                    u.Integral = levelModel.MinIntegral;
                    DAO.MemberBase allu = bll.UpdateUser(u);
                    if (allu != null)
                    {
                        //给上级积分
                        bll.GiveUpUserInt(allu.ID, upUser);
                    }
                    
                    return Json(new { r = true, msg = "注册成功,初始密码为您手机号后四位！" });
                }
                else
                {
                  
                    return Json(new { r = false, msg = "手机验证码错误" });
                }
                  
        }


        [HttpPost]
        public bool PhoneNumber(string PhoneNumber)
        {
            Random Rdm = new Random();
            int iRdm = Rdm.Next(1000, 9999);
            //string time = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string Numb = iRdm.ToString();
            string data = "action=code&username=" + "460431214" + "&userpass=" + "jiangu.com" + "&mobiles=" + PhoneNumber.Trim() + "&content=" + Numb + "&codeid=" + "3989" + "&extnum=0";
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
            if (Session["memberID"] == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Myhome", "Member");
            }
            
        }
        [HttpPost]
        public ActionResult Login(string loginName,string  passWord)
        {           
                MemberBLL bll = new MemberBLL();
                DAO.MemberBase model = bll.Login(loginName, passWord);
                if (model != null)
                {
                    Session["memberID"] = model.ID;
                    Session["mobile"] = model.Mobile;
                    Session.Timeout = 30;
                    if (Session["ControllerUrl"] != null && Session["ActionUrl"] != null)
                    {
                        string action = Session["ActionUrl"].ToString();
                        string con = Session["ControllerUrl"].ToString();
                        Dictionary<string, object> objs;
                        if (Session[con + action] != null)
                        {
                            objs = (Dictionary<string, object>)Session[con + action];
                            StringBuilder qs = new StringBuilder("/" + con + "/" + action + "?");
                            foreach (var item in objs)
                            {
                                qs.Append(item.Key + "=" + item.Value + "&&");
                            }
                            return Redirect(qs.ToString());
                        }
                        else
                        {
                            return RedirectToAction(action, con);
                        }
                    }
                    else
                    {
                        return RedirectToAction("MyOrder");
                    }
                }
                else
                {
                    ViewBag.loginFail = "用户名或密码错误！";
                    LoginModel m = new LoginModel();
                    m.loginName = loginName;
                    
                    return View(m);
                }
           
        }
        /// <summary>
        /// 用户中心
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult Myhome()
        {
            string uid = Session["memberID"].ToString();
            MemberBLL mbBll = new MemberBLL();
            MemberLevelBLL mlBll = new MemberLevelBLL();
            UserHomeVM u = new UserHomeVM();
            u.Userinfo = mbBll.GetUserByID(uid);//账户信息
            List<LeveExp> leList = new List<LeveExp>();
            List<UserLeveExpBM> ulbm = mlBll.GetLeveExpByUid(uid);
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
            List<DAO.ExpRecord> exList = erBll.GetExpList(uid);
            return View(exList);

        }
        [UserFilter]
        public ActionResult MyInfo()
        {
            string uid = Session["memberID"].ToString();
            MemberBLL mbBll = new MemberBLL();
            MemberLevelBLL mlBll = new MemberLevelBLL();
            UserHomeVM u = new UserHomeVM();
            u.Userinfo = mbBll.GetUserByID(uid);//账户信息
            List<LeveExp> leList = new List<LeveExp>();
            List<UserLeveExpBM> ulbm = mlBll.GetLeveExpByUid(uid);
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
                r.Add("r", false);
                //Session["ControllerUrl"] = "Car";
                //Session["ActionUrl"] = "index";
                return Json(r, JsonRequestBehavior.AllowGet);
            }
            else
            {
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
        public ActionResult UpdatePW(string oldpass, string newpass, string repass)
        {
            if (newpass == repass)
            {
                MemberBLL mbll = new MemberBLL();
                string uid = Session["memberID"].ToString();
                bool r = mbll.ValidataPassWord(int.Parse(uid), oldpass);
                if (r)
                {
                    bool relult = mbll.UpdatePassword(uid, newpass);
                    if (relult)
                    {
                        ViewBag.msg = "密码修改成功";
                    }
                    else
                    {
                        ViewBag.msg = "密码修改失败，请稍后再试";
                    }
                }
                else {
                    ViewBag.msg = "旧密码不正确";
                  
                }
            }
            else {
                ViewBag.msg = "两次输入密码不一致";
              
            }
            return View();
        }



        [HttpPost]
        [UserFilter]
        public ActionResult commentsadd(string comm, string id)
        {
            Dictionary<string, Object> map = new Dictionary<string, Object>();
            DAO.comments add = new DAO.comments();
            string uid = Session["memberID"].ToString();
            add.shown = 1;
            add.mentsname = comm;
            add.userID = Convert.ToInt32(uid);
            add.Time = DateTime.Now;
            MemberBLL mebBll = new MemberBLL();
            add.ProductID = Convert.ToInt32(id);
            bool r = mebBll.adds(add);

            if (r)
            {
                map.Add("data", "1");
                return Json(map, JsonRequestBehavior.AllowGet);
              
            }
            else
            {
                map.Add("data", "0");
                return Json(map, JsonRequestBehavior.AllowGet);

            }
            
        }

        /// <summary>
        /// 我的评论
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult Mycomments()
        {
            string uid = Session["memberID"].ToString();
            MemberBLL mebBll = new MemberBLL();
            List<Comments> info = mebBll.commsele(uid);
            return View(info);
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

        /// <summary>
        /// 评论
        /// </summary>
        /// <returns></returns>
        [UserFilter]
        public ActionResult comment()
        {
            return View();
        }
    }
}
