using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        [Display(Name = "First Name")]
        public string FirstName
        {
            get;
            set;
        }

        [Display(Name = "Last Name")]
        public string LastName
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