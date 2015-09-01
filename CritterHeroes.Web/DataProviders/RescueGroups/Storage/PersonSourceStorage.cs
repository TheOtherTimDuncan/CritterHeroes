using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class PersonSourceStorage : RescueGroupsSearchStorageBase<PersonSource>
    {
        private IEnumerable<string> _fields;

        public PersonSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
            this._fields = new[] { "contactID", "contactFirstname", "contactLastname" };
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
            return tokens.Select(x => new PersonSource()
            {
                ID = x.Value.Value<string>("contactID"),
                FirstName = x.Value.Value<string>("contactFirstname"),
                LastName = x.Value.Value<string>("contactLastname")
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
