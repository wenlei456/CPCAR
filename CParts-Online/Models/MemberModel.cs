using CommonUtils.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CParts_Online.Models
{
    public class UpdatePassWordVM
    {
        [Required(ErrorMessage = ValidateMsg.requiredPassWord)]
        [CustomValidation(typeof(ModelVaildate), "OldPWIsValid")]      
        public string oldPW { get; set; }
        [Required(ErrorMessage = ValidateMsg.requiredPassWord)]
        [StringLength(30, MinimumLength = 4, ErrorMessage = ValidateMsg.lengthPassWord)]    
        public string newPW { get; set; }
        [Compare("newPW", ErrorMessage = ValidateMsg.confirmPassWord)]
        public string RnewPW { get; set; }
    }
}