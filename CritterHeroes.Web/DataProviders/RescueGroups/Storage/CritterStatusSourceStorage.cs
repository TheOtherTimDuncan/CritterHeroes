using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class CritterStatusSourceStorage : RescueGroupsStorage<CritterStatusSource>
    {
        public CritterStatusSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
            this.Fields = new[]
            {
                new SearchField("statusID"),
                new SearchField("statusName"),
                new SearchField("statusDescription")
            };
        }

        public override string ObjectType
        {
            get
            {
                return "animalStatuses";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<SearchField> Fields
        {
            get;
        }

        protected override string SortField
        {
            get
            {
                return "statusID";
            }
        }

        protected override string KeyField
        {
            get
            {
                return "statusID";
            }
        }

        public override Task<IEnumerable<CritterStatusSource>> GetAllAsync(params SearchFilter[] searchFilters)
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = KeyField,
                Operation = SearchFilterOperation.NotBlank
            };
            return base.GetAllAsync(filter);
        }
    }
}
