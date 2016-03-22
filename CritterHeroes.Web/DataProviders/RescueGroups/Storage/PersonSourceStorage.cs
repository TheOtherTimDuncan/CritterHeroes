using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class PersonSourceStorage : ContactSourceStorageBase<PersonSource>
    {
        public PersonSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this.Fields = new[]
            {
                new SearchField("contactID"),
                new SearchField("contactClass"),
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

        public override async Task<IEnumerable<PersonSource>> GetAllAsync()
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = "contactClass",
                Criteria = "Individual/Family",
                Operation = "equals"
            };
            return await base.GetAllAsync(filter);
        }

        public override IEnumerable<SearchField> Fields
        {
            get;
        }

        public override IEnumerable<PersonSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => x.Value.ToObject<PersonSource>());
        }
    }
}
