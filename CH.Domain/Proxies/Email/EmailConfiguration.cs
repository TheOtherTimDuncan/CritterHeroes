using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CH.Domain.Contracts.Email;

namespace CH.Domain.Proxies.Email
{
    public class EmailConfiguration : IEmailConfiguration
    {
        public string DefaultFrom
        {
            get
            {
                return null;
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
