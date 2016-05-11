using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CritterHeroes.Web.Contracts.Email;

namespace CritterHeroes.Web.Shared.Email
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string DefaultFrom
        {
            get
            {
                return ConfigurationManager.AppSettings["DefaultEmailFrom"]; ;
            }
        }

        public string Username
        {
            get
            {
                return ConfigurationManager.AppSettings["SendGridUsername"];
            }
        }

        public string Password
        {
            get
            {
                return ConfigurationManager.AppSettings["SendGridPassword"];
            }
        }
    }
}
