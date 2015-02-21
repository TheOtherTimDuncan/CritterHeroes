using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string Email
        {
            get;
            set;
        }
    }
}