using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ResetPasswordModel : LoginModel
    {
        public string Code
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Placeholder("Confirm password")]
        public string ConfirmPassword
        {
            get;
            set;
        }

        public bool? IsSuccess
        {
            get;
            set;
        }
    }
}