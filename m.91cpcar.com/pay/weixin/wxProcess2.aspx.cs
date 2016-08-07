﻿using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Net;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data.SqlClient;
using DAO;
using MemberModule.Business;
using MemberModule.Model;
using ActiveModule;


namespace m._91cpcar.com.pay.weixin
{
    public partial class wxProcess2 : System.Web.UI.Page
    {
        public string reurl = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //获取从wxProcess.aspx传递过来的跳转地址reurl
                if (Request.QueryString["reurl"] != null && Request.QueryString["reurl"] != "")
                {
                    reurl = Request.QueryString["reurl"].ToString();
                }

                string code = "";
                if (Request.QueryString["code"] != null && Request.QueryString["code"] != "")
                {
                    //获取微信回传的code
                    code = Request.QueryString["code"].ToString();
                    OAuth_Token Model = Get_token(code);  //获取token
                    OAuthUser OAuthUser_Model = Get_UserInfo(Model.access_token, Model.openid);
                    DAO.MemberBase m = new DAO.MemberBase();
                    MemberBLL bll = new MemberBLL();
                    //if (OAuthUser_Model.openid != null && OAuthUser_Model.openid != "")  //已获取得openid及其他信息
                    //{
                    MemberBase selectUser = bll.selectWx(OAuthUser_Model.openid);
                        if (selectUser != null)
                        {
                            bll.updateWx(OAuthUser_Model);

                            Session["memberID"] = selectUser.ID;
                            //Session["mobile"] = model.Mobile;
                            Session.Timeout = 30;
                        }
                        else
                        {
                           
                            FriendCodeBLL fbll = new FriendCodeBLL();

                            DAO.MemberLevel levelModel = new DAO.MemberLevel();
                            m.LoginName = OAuthUser_Model.nickname;
                            //m.LoginName = phone;
                            //m.PassWord = Md5Util.PwdMd5(phone.Substring(5));
                            //m.Mobile = phone;
                            ////m.Email = "";
                            m.State = 1;
                            //int upUser = fbll.GetUpUser(fcode);
                            m.UpUser = 0;//上级用户
                            //m.Levels = 0;
                            //m.Integral = 0;
                            //m.Source = "self";
                            m.wxOpenid = OAuthUser_Model.openid;
                            m.wxNickname = OAuthUser_Model.nickname;
                            m.wxSex = Convert.ToInt32(OAuthUser_Model.sex);
                            m.wxProvince = OAuthUser_Model.province;
                            m.wxCity = OAuthUser_Model.city;
                            m.wxCountry = OAuthUser_Model.country;
                            m.wxHeadimgurl = OAuthUser_Model.headimgurl;
                            m.wxPrivilege = OAuthUser_Model.privilege;
                            m.RegDate = DateTime.Now;
                            DAO.MemberBase u = bll.Reg(m); //注册            
                            Session["memberID"] = u.ID;
                            VoucherBus vbll = new VoucherBus();
                            vbll.RegGiveAVoucher(u.ID);//给一张注册优惠券
                            levelModel = bll.GetLevel(u.ID, 0);//注册初始化级别
                            u.Levels = levelModel.LevelId;
                            u.Integral = levelModel.MinIntegral;
                            DAO.MemberBase allu = bll.UpdateUser(u);

                            Session["memberID"] = u.ID;
                            //Session["mobile"] = model.Mobile;
                            Session.Timeout = 30;
                        }

                      

                       
                    



                        ////在页面上输出用户信息
                        //Response.Write("用户OPENID:" + OAuthUser_Model.openid + "<br>用户昵称:" + OAuthUser_Model.nickname + "<br>性别:" + OAuthUser_Model.sex + "<br>所在省:" + OAuthUser_Model.province + "<br>所在市:" + OAuthUser_Model.city + "<br>所在国家:" + OAuthUser_Model.country + "<br>头像地址:" + OAuthUser_Model.headimgurl + "<br>用户特权信息:" + OAuthUser_Model.privilege);
                        ////或跳转到自己的页面，想怎么处理就怎么处理
                        Response.Redirect(reurl);
                    //}
                    //else  //未获得openid，回到wxProcess.aspx，访问弹出微信授权页面，提示用户授权
                    //{
                    //    Response.Redirect("wxProcess.aspx?auth=1");
                    //}
                }
            }
        }

        #region 属性
        public string appid = "wxce17e97c663c90a0";  //公众微信平台下可以找到
        public string appsecret = "8a01f029d577b4428508abdbeab8d062";  //公众微信平台下可以找到
        #endregion

        //根据appid，secret，code获取微信openid、access token信息
        protected OAuth_Token Get_token(string Code)
        {
            //获取微信回传的openid、access token
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + appid + "&secret=" + appsecret + "&code=" + Code + "&grant_type=authorization_code");
            //微信回传的数据为Json格式，将Json格式转化成对象
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }

        //刷新Token(好像这个刷新Token没有实际作用)
        protected OAuth_Token refresh_token(string REFRESH_TOKEN)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=" + appid + "&grant_type=refresh_token&refresh_token=" + REFRESH_TOKEN);
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }

        //根据openid，access token获得用户信息
        protected OAuthUser Get_UserInfo(string REFRESH_TOKEN, string OPENID)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID);
            OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
            return OAuthUser_Model;
        }

        //访问微信url并返回微信信息
        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误
            }
            return returnText;
        }


        /// <summary>
        /// token类
        /// </summary>
        public class OAuth_Token
        {
            public OAuth_Token()
            {

                //
                //TODO: 在此处添加构造函数逻辑
                //
            }
            //access_token	网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同
            //expires_in	access_token接口调用凭证超时时间，单位（秒）
            //refresh_token	用户刷新access_token
            //openid	用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
            //scope	用户授权的作用域，使用逗号（,）分隔
            public string _access_token;
            public string _expires_in;
            public string _refresh_token;
            public string _openid;
            public string _scope;
            public string access_token
            {
                set { _access_token = value; }
                get { return _access_token; }
            }
            public string expires_in
            {
                set { _expires_in = value; }
                get { return _expires_in; }
            }

            public string refresh_token
            {
                set { _refresh_token = value; }
                get { return _refresh_token; }
            }
            public string openid
            {
                set { _openid = value; }
                get { return _openid; }
            }
            public string scope
            {
                set { _scope = value; }
                get { return _scope; }
            }

        }

        ///// <summary>
        ///// 用户信息类
        ///// </summary>
        //public class OAuthUser
        //{
        //    public OAuthUser()
        //    { }
        //    #region 数据库字段
        //    private string _openID;
        //    private string _searchText;
        //    private string _nickname;
        //    private string _sex;
        //    private string _province;
        //    private string _city;
        //    private string _country;
        //    private string _headimgUrl;
        //    private string _privilege;
        //    #endregion

        //    #region 字段属性
        //    /// <summary>
        //    /// 用户的唯一标识
        //    /// </summary>
        //    public string openid
        //    {
        //        set { _openID = value; }
        //        get { return _openID; }
        //    }
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    public string SearchText
        //    {
        //        set { _searchText = value; }
        //        get { return _searchText; }
        //    }
        //    /// <summary>
        //    /// 用户昵称 
        //    /// </summary>
        //    public string nickname
        //    {
        //        set { _nickname = value; }
        //        get { return _nickname; }
        //    }
        //    /// <summary>
        //    /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知 
        //    /// </summary>
        //    public string sex
        //    {
        //        set { _sex = value; }
        //        get { return _sex; }
        //    }
        //    /// <summary>
        //    /// 用户个人资料填写的省份
        //    /// </summary>
        //    public string province
        //    {
        //        set { _province = value; }
        //        get { return _province; }
        //    }
        //    /// <summary>
        //    /// 普通用户个人资料填写的城市 
        //    /// </summary>
        //    public string city
        //    {
        //        set { _city = value; }
        //        get { return _city; }
        //    }
        //    /// <summary>
        //    /// 国家，如中国为CN 
        //    /// </summary>
        //    public string country
        //    {
        //        set { _country = value; }
        //        get { return _country; }
        //    }
        //    /// <summary>
        //    /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空
        //    /// </summary>
        //    public string headimgurl
        //    {
        //        set { _headimgUrl = value; }
        //        get { return _headimgUrl; }
        //    }
        //    /// <summary>
        //    /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）其实这个格式称不上JSON，只是个单纯数组
        //    /// </summary>
        //    public string privilege
        //    {
        //        set { _privilege = value; }
        //        get { return _privilege; }
        //    }
        //    #endregion
        //}

        /// <summary>
        /// 将Json格式数据转化成对象
        /// </summary>
        public class JsonHelper
        {
            /// <summary>  
            /// 生成Json格式  
            /// </summary>  
            /// <typeparam name="T"></typeparam>  
            /// <param name="obj"></param>  
            /// <returns></returns>  
            public static string GetJson<T>(T obj)
            {
                DataContractJsonSerializer json = new DataContractJsonSerializer(obj.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    json.WriteObject(stream, obj);
                    string szJson = Encoding.UTF8.GetString(stream.ToArray()); return szJson;
                }
            }
            /// <summary>  
            /// 获取Json的Model  
            /// </summary>  
            /// <typeparam name="T"></typeparam>  
            /// <param name="szJson"></param>  
            /// <returns></returns>  
            public static T ParseFromJson<T>(string szJson)
            {
                T obj = Activator.CreateInstance<T>();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(szJson)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                    return (T)serializer.ReadObject(ms);
                }
            }
        }
    }
}