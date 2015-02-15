using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Areas.Models.Modal;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ConfirmEmailModel
    {
        public string UserID
        {
            get;
            set;
        }

        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get;
            set;
        }

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