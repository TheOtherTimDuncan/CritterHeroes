using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CritterHeroes.Web.Areas.Models.Modal;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ForgotUsernameModel
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
    }
}