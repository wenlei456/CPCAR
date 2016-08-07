using CommonUtils.Message;
using MemberModule.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
namespace CParts_Online.Models
{
    /// <summary>
    /// 自定义验证
    /// </summary>
    public class ModelVaildate
    {
        #region 用户验证 
        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="loginName">loginName</param>
        /// <returns></returns>
        public static ValidationResult LoginNameIsValid(string loginName)
        {
            MemberBLL bll = new MemberBLL();
            if (!bll.VerLoginName(loginName))
            {
                return new ValidationResult(ValidateMsg.EmailIsValid);
            }
            return ValidationResult.Success;
        
        }
        /// <summary>
        /// 邮箱是否绑定
        /// </summary>
        /// <param name="eMail">e-mail</param>
        /// <returns></returns>
        public static ValidationResult EmailIsValid(string eMail)
        {
            MemberBLL bll = new MemberBLL();
            if (!bll.VerEmail(eMail))
            {
                return new ValidationResult(ValidateMsg.EmailIsValid);
            }
            return ValidationResult.Success;

        }
        /// <summary>
        /// 验证码是否一致
        /// </summary>
        /// <param name="VeriCode"></param>
        /// <returns></returns>
        public static ValidationResult ValiCodeIsValid(string VeriCode)
        {
           string sessionCode= HttpContext.Current.Session["VerCode"]!=null?HttpContext.Current.Session["VerCode"].ToString().ToLower():"0";
           VeriCode = VeriCode != null ? VeriCode.ToLower() : null;
            if ( sessionCode!=VeriCode)
            {
                return new ValidationResult(ValidateMsg.VeriCodeIsValid);
            }
            return ValidationResult.Success;

        }

        /// <summary>
        /// 手机验证码是否正确
        /// </summary>
        /// <param name="VeriCode"></param>
        /// <returns></returns>
        public static ValidationResult PhoneNumberCodeIsValid(string VeriCode)
        {
            string phoneNumberCode = HttpContext.Current.Session["phoneNumberCode"].ToString();
          
            VeriCode = VeriCode != null ? VeriCode.ToLower() : null;
            if (phoneNumberCode != VeriCode)
            {
                return new ValidationResult(ValidateMsg.PhoneCodeIsValid);
            }
            return ValidationResult.Success;

        }
       
        /// <summary>
        /// 手机是否已经注册
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public static ValidationResult PhoneIsValid(string PhoneNumber)
        {
            MemberBLL bll = new MemberBLL();
            if (!bll.VerPhone(PhoneNumber))
            {
                return new ValidationResult(ValidateMsg.PhoneIsValid);
            }
            return ValidationResult.Success;

        }

        /// <summary>
        /// 忘记密码手机是否绑定会员
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <returns></returns>
        public static ValidationResult PhoneIsValidFor(string PhoneNumber)
        {
            MemberBLL bll = new MemberBLL();
            if (bll.VerPhone(PhoneNumber))
            {
                return new ValidationResult(ValidateMsg.PhoneIsValidFor);
            }
            return ValidationResult.Success;

        }


        /// <summary>
        /// 邀请码验证
        /// </summary>
        /// <param name="FriendCode"></param>
        /// <returns></returns>
        public static ValidationResult FriendCodeValid(string FriendCode)
        {           
            if (!string.IsNullOrEmpty(FriendCode))
            {
                MemberBLL mbll = new MemberBLL();
                DAO.MemberBase mb= mbll.GetUserByPhone(FriendCode);
                if (mb != null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(ValidateMsg.FriendCodeValid);
                }
            }
            return ValidationResult.Success;
        }



        public static ValidationResult OldPWIsValid(string oldPW)
         {
             MemberBLL mbll = new MemberBLL();
            // HttpSessionStateBase Session=;
              //System.Web.HttpSessionStateBase s=HttpSessionStateBase
            
             //System.Web.SessionState.HttpSessionState Session = new HttpSessionState();
             string uid = System.Web.HttpContext.Current.Session["memberID"].ToString();
             bool r = mbll.ValidataPassWord(int.Parse(uid),oldPW);
             if (!r)
             {
                 return new ValidationResult(ValidateMsg.OldPassWordError);
             }
             return ValidationResult.Success;
         }
        
        #endregion
    }
}