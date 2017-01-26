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
    public class BusinessSourceStorage : ContactSourceStorageBase<BusinessSource>
    {
        public BusinessSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
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

        public override IEnumerable<SearchField> Fields
        {
            get;
        }

        public override async Task<IEnumerable<BusinessSource>> GetAllAsync(params SearchFilter[] searchFilters)
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = "contactClass",
                Criteria = "Company",
                Operation = "equals"
            };
            return await base.GetAllAsync(filter);
        }
    }
}
