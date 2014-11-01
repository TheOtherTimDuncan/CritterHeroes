﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AR.Domain.Models
{
    public class Organization
    {
        public Organization()
        {
            ID = Guid.NewGuid();
        }

        public Organization(Guid organizationID)
        {
            this.ID = organizationID;
        }

        public Guid ID
        {
            get;
            private set;
        }

        public string FullName
        {
            get;
            set;
        }

        public string ShortName
        {
            get;
            set;
        }

        public string AzureTableName
        {
            get;
            set;
        }

        public IEnumerable<string> SupportedCritters
        {
            get;
            set;
        }
    }
}
