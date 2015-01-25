using System;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Areas.Models.Modal;

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
        public string EmailAddress
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }
    }
}