using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class LoginModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        [Placeholder("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get;
            set;
        }

        [Placeholder("Password")]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
    }
}