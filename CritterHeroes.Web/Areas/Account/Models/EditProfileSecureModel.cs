using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileSecureModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        [DataType(DataType.EmailAddress)]
        [Placeholder("Email")]
        public string Email
        {
            get;
            set;
        }
    }
}