using CommonUtils.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CParts_Online.Models.Master
{
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredLoginName)]
        [StringLength(30, MinimumLength = 4, ErrorMessage = ValidateMsg.lengthLoginName)]
        public string loginName { get; set; }
        [Required(ErrorMessage = ValidateMsg.requiredPassWord)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = ValidateMsg.lengthPassWord)]
        public string passWord { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = ValidateMsg.requiredVeriCode)]
        [CustomValidation(typeof(ModelVaildate), "ValiCodeIsValid")]
        public string VeriCode { get; set; }



    }
}