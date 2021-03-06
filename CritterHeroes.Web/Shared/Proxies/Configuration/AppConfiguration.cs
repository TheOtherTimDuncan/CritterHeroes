﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Configuration;

namespace CritterHeroes.Web.Shared.Proxies.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        public Guid OrganizationID
        {
            get
            {
                Guid result;
                string setting = ConfigurationManager.AppSettings["Organization"];
                if (Guid.TryParse(setting, out result))
                {
                    return result;
                }
                throw new ApplicationException("Organization ID not found or invalid: " + setting);
            }
        }

        public string BlobBaseUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["BlobBaseUrl"];
            }
        }
    }
}
