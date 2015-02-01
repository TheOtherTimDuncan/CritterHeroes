using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class ConfirmEmailModel
    {
        [DataType(DataType.EmailAddress)]
        public string EmailAddress
        {
            get;
            set;
        }

        public string ConfirmationCode
        {
            get;
            set;
        }
    }
}