using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CH.Website.Models
{
    public class EditProfileModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        public string OriginalUsername
        {
            get;
            set;
        }

        [Required(ErrorMessage="Please enter a username")]
        public string Username
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please enter your first name")]
        [Display(Name="First Name")]
        public string FirstName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please enter your last name")]
        [Display(Name = "Last Name")]
        public string LastName
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage="Please enter a valid email address")]
        public string Email
        {
            get;
            set;
        }
    }
}