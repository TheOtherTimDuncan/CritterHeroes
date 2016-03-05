using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Logging;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class ContactSourceStorageBase<T> : RescueGroupsSearchStorageBase<T> where T : class
    {
        public ContactSourceStorageBase(IRescueGroupsConfiguration configuration, IHttpClient client, IAppLogger logger)
            : base(configuration, client, logger)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "contacts";
            }
        }

        protected override string SortField
        {
            get
            {
                return "contactID";
            }
        }

        protected IEnumerable<string> GetGroupNames(JProperty jsonProperty)
        {
            string groupNames = jsonProperty.Value.Value<string>("contactGroups");
            IEnumerable<string> result = groupNames.NullSafeSplit(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            return result;
        }

        protected string GetZip(JProperty jsonProperty)
        {
            string zip = jsonProperty.Value.Value<string>("contactPostalcode");

            string zipExtended = jsonProperty.Value.Value<string>("contactPlus4");
            if (!zipExtended.IsNullOrEmpty())
            {
                zip += zipExtended;
            }

            return zip;
        }

        protected string CleanupPhone(string phoneNumber)
        {
            if (phoneNumber.IsNullOrEmpty())
            {
                return phoneNumber;
            }

            return Regex.Replace(phoneNumber, @"\D", "");
        }
    }
}
