using CommonUtils.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CParts_Online.Models
{
    public class Forgetpassword
    {
        /// <summary>
        /// 手机号
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredPhone)]
        [RegularExpression(@"^1[3|4|5|7|8][0-9]\d{8}$", ErrorMessage = ValidateMsg.formatPhone)]
        [CustomValidation(typeof(ModelVaildate), "PhoneIsValidFor")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// 手机验证码
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredPhoneCode)]
        [CustomValidation(typeof(ModelVaildate), "PhoneNumberCodeIsValid")]
        public string PhoneCode { get; set; }

        [StringLength(30, MinimumLength = 4, ErrorMessage = ValidateMsg.lengthPassWord)]
        public string newPW { get; set; }
        [Compare("newPW", ErrorMessage = ValidateMsg.confirmPassWord)]
        public string RnewPW { get; set; }
    }
}