using System;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Areas.Models.Modal;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ForgotPasswordModel
    {
        public ModalDialogModel ModalDialog
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