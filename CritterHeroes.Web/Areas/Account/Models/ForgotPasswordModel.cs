using System;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ForgotPasswordModel
    {
        [DataType(DataType.EmailAddress)]
        [Placeholder("Email")]
        public string ResetPasswordEmail
        {
            get;
            set;
        }
    }
}