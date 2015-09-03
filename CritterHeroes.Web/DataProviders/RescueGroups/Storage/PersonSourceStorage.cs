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
    public class PersonSourceStorage : RescueGroupsSearchStorageBase<PersonSource>
    {
        private IEnumerable<string> _fields;

        public PersonSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
            this._fields = new[] { "contactID", "contactFirstname", "contactLastname", "contactAddress", "contactCity", "contactState", "contactPostalcode", "contactPlus4", "contactEmail", "contactActive", "contactGroups" };
        }

        public override string ObjectType
        {
            get
            {
                return "contacts";
            }
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
                    State = x.Value.Value<string>("contactState"),
                    Zip = x.Value.Value<string>("contactPostalcode")
                };

                string zipExtended = x.Value.Value<string>("contactPlus4");
                if (!zipExtended.IsNullOrEmpty())
                {
                    result.Zip += zipExtended;
                }

                string active = x.Value.Value<string>("contactActive");
                result.IsActive = (active.SafeEquals("Yes"));

                string groupNames = x.Value.Value<string>("contactGroups");
                result.GroupNames = groupNames.NullSafeSplit(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                return result;
            });
        }

        protected override string SortField
        {
            get
            {
                return "contactID";
            }
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
