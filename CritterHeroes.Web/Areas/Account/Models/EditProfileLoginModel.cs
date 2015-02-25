using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileLoginModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        [DataType(DataType.Password)]
        [Placeholder("Password")]
        public string Password
        {
            get;
            set;
        }
    }
}