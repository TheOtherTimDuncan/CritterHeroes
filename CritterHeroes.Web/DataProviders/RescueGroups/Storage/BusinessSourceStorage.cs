using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class BusinessSourceStorage : ContactSourceStorageBase<BusinessSource>
    {
        public BusinessSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = "contactClass",
                Criteria = "Company",
                Operation = "equals"
            };
            this.Filters = new[] { filter };

            this.Fields = new[]
            {
                new SearchField("contactID"),
                new SearchField("contactName"),
                new SearchField("contactCompany"),
                new SearchField("contactAddress"),
                new SearchField("contactCity"),
                new SearchField("contactState"),
                new SearchField("contactPostalcode"),
                new SearchField("contactPlus4"),
                new SearchField("contactEmail"),
                new SearchField("contactPhoneWork"),
                new SearchField("contactPhoneWorkExt"),
                new SearchField("contactFax"),
                new SearchField("contactGroups")
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

        public override IEnumerable<SearchField> Fields
        {
            get;
        }
    }
}
