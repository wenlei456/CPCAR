using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonUtils.Message
{
   public class ValidateMsg
    {
       public const string requiredLoginName="用户名不能为空";
       public const string lengthLoginName="用户名长度在4-30之间";
       public const string LoginNameIsValid = "用户名已存在，请重新输入";
       public const string requiredPassWord="密码不能为空";
       public const string lengthPassWord="密码长度在8-20之间";
       public const string confirmPassWord="两次密码不一致";
       public const string requiredMail="邮件不能为空";
       public const string formatMail="邮件格式不正确";
       public const string EmailIsValid = "邮箱已注册，请重新输入";
       public const string requiredPhone="手机不能为空";
       public const string PhoneIsValid = "手机已注册，请重新输入";
       public const string PhoneIsValidFor = "你输入的手机没有注册过会员";
       public const string formatPhone = "手机格式不正确";
       public const string requiredPhoneCode="手机校验码不能为空";
       public const string PhoneCodeIsValid="手机校验码不正确";
       public const string requiredVeriCode="输入验证码";
       public const string VeriCodeIsValid="验证码不正确";
       public const string FriendCodeValid = "推荐人错误";
       public const string OldPassWordError = "旧密码错误";
    }
}


