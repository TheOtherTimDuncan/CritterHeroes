using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class BusinessSourceStorage : ContactSourceStorageBase<BusinessSource>
    {
        private IEnumerable<string> _fields;

        public BusinessSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = "contactClass",
                Criteria = "Company",
                Operation = "equals"
            };
            this.Filters = new[] { filter };

            this._fields = new[]
            {
                "contactID",
                "contactName",
                "contactCompany",
                "contactAddress",
                "contactCity",
                "contactState",
                "contactPostalcode",
                "contactPlus4",
                "contactEmail",
                "contactPhoneWork",
                "contactPhoneWorkExt",
                "contactFax",
                "contactGroups"
            };
        }

        public override IEnumerable<BusinessSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x =>
            {
                BusinessSource result = new BusinessSource()
                {
                    ID = x.Value.Value<string>("contactID"),
                    Name = x.Value.Value<string>("contactName"),
                    Company = x.Value.Value<string>("contactCompany"),
                    Email = x.Value.Value<string>("contactEmail"),
                    Address = x.Value.Value<string>("contactAddress"),
                    City = x.Value.Value<string>("contactCity"),
                    State = x.Value.Value<string>("contactState").NullSafeToUpper(),
                    Zip = GetZip(x),
                    PhoneWork = CleanupPhone(x.Value.Value<string>("contactPhoneWork")),
                    PhoneWorkExtension = x.Value.Value<string>("contactPhoneWorkExt"),
                    PhoneFax = CleanupPhone(x.Value.Value<string>("contactFax")),
                    GroupNames = GetGroupNames(x)
                };

                return result;
            });
        }

        protected override IEnumerable<string> Fields
        {
            get
            {
                return _fields;
            }
        }
    }
}
