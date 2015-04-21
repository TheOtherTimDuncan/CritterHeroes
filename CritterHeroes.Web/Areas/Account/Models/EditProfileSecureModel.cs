using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileSecureModel
    {
        [DataType(DataType.EmailAddress)]
        [Placeholder("Email")]
        public string NewEmail
        {
            get;
            set;
        }
    }
}