﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TOTD.Mvc.FluentHtml.Attributes;

namespace CritterHeroes.Web.Areas.Account.Models
{
    public class EditProfileModel
    {
        [Placeholder("First Name")]
        [Display(Name = "First Name")]
        public string FirstName
        {
            get;
            set;
        }

        [Placeholder("Last Name")]
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

        [Display(Name = "Unconfirmed Email")]
        public string UnconfirmedEmail
        {
            get;
            set;
        }
    }
}