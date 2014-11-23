using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;

namespace CH.Website.Models
{
    public class LoginModel : IQueryResult, ICommand
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