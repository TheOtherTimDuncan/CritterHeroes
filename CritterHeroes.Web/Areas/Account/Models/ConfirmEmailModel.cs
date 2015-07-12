using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ConfirmEmailModel
    {
        [Placeholder("Email")]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get;
            set;
        }

        [Placeholder("Confirmation code")]
        public string ConfirmationCode
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