using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AR.Website.Models
{
    public class LoginModel
    {
        public string ReturnUrl
        {
            get;
            set;
        }

        [Required]
        public string Username
        {
            get;
            set;
        }

        [Required]
        [DataType(DataType.Password)]
        public string Password
        {
            get;
            set;
        }
    }
}