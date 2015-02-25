using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Areas.Models.Modal;
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

        public ModalDialogModel ModalDialog
        {
            get;
            set;
        }
    }
}