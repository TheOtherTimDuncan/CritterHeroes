using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Configuration;
using CritterHeroes.Web.Domain.Contracts.Events;

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

        public override async Task<IEnumerable<PersonSource>> GetAllAsync(params SearchFilter[] searchFilters)
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
    }
}
