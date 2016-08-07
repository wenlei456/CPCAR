using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using CommonUtils.Message;
namespace CParts_Online.Models
{
    public class RegUserModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredLoginName)]
        //[RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = ValidateMsg.formatLoginName)]
        [StringLength(30, MinimumLength = 4, ErrorMessage = ValidateMsg.lengthLoginName)]      
        [CustomValidation(typeof(ModelVaildate), "LoginNameIsValid")]
        public string loginName { get; set; }
        [Required(ErrorMessage = ValidateMsg.requiredPassWord)]
        //[RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = ValidateMsg.formatPassWord)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = ValidateMsg.lengthPassWord)]
        public string passWord { get; set; }
        [Required(ErrorMessage = ValidateMsg.requiredPassWord)]
        [Compare("passWord", ErrorMessage = ValidateMsg.confirmPassWord)]
        public string confirmPassWord { get; set; }
        /// <summary>
        /// 邮箱验证
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredMail)]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = ValidateMsg.formatMail)]
        [CustomValidation(typeof(ModelVaildate), "EmailIsValid")]
        public string eMail { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredVeriCode)]
        [CustomValidation(typeof(ModelVaildate),"ValiCodeIsValid")]      
         public string VeriCode { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredPhone)]
        [RegularExpression(@"^1[3|4|5|7|8][0-9]\d{8}$", ErrorMessage = ValidateMsg.formatPhone)]
        [CustomValidation(typeof(ModelVaildate), "PhoneIsValid")]     
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredPhoneCode)]        
        public string PhoneCode { get; set; }
        /// <summary>
        /// 邀请码
        /// </summary>
        [CustomValidation(typeof(ModelVaildate), "FriendCodeValid")]
        public string FriendCode { get; set; }


    }
}

