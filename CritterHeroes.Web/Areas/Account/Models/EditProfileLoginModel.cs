using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileLoginModel
    {
        [DataType(DataType.Password)]
        [Placeholder("Password")]
        public string Password
        {
            get;
            set;
        }
    }
}