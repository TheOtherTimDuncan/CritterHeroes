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
    public class PersonSourceStorage : ContactSourceStorageBase<PersonSource>
    {
        private IEnumerable<SearchField> _fields;

        public PersonSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = "contactClass",
                Criteria = "Individual/Family",
                Operation = "equals"
            };
            this.Filters = new[] { filter };

            this._fields = new[]
            {
                new SearchField("contactID"),
                new SearchField("contactType"),
                new SearchField("contactClass"),
                new SearchField("contactName"),
                new SearchField("contactCompany"),
                new SearchField("contactFirstname"),
                new SearchField("contactLastname"),
                new SearchField("contactAddress"),
                new SearchField("contactCity"),
                new SearchField("contactState"),
                new SearchField("contactPostalcode"),
                new SearchField("contactPlus4"),
                new SearchField("contactEmail"),
                new SearchField("contactPhoneHome"),
                new SearchField("contactPhoneWork"),
                new SearchField("contactPhoneWorkExt"),
                new SearchField("contactPhoneCell"),
                new SearchField("contactFax"),
                new SearchField("contactActive"),
                new SearchField("contactGroups")
            };
        }

        public override IEnumerable<PersonSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x =>
            {
                PersonSource result = new PersonSource()
                {
                    ID = x.Value.Value<string>("contactID"),
                    FirstName = x.Value.Value<string>("contactFirstname"),
                    LastName = x.Value.Value<string>("contactLastname"),
                    Email = x.Value.Value<string>("contactEmail"),
                    Address = x.Value.Value<string>("contactAddress"),
                    City = x.Value.Value<string>("contactCity"),
                    State = x.Value.Value<string>("contactState").NullSafeToUpper(),
                    Zip = GetZip(x),
                    PhoneHome = CleanupPhone(x.Value.Value<string>("contactPhoneHome")),
                    PhoneWork = CleanupPhone(x.Value.Value<string>("contactPhoneWork")),
                    PhoneWorkExtension = x.Value.Value<string>("contactPhoneWorkExt"),
                    PhoneCell = CleanupPhone(x.Value.Value<string>("contactPhoneCell")),
                    PhoneFax = CleanupPhone(x.Value.Value<string>("contactFax")),
                    GroupNames = GetGroupNames(x)
                };

                string active = x.Value.Value<string>("contactActive");
                result.IsActive = (active.SafeEquals("Yes"));

                return result;
            });
        }

        protected override IEnumerable<SearchField> Fields
        {
            get
            {
                return _fields;
            }
        }
    }
}
