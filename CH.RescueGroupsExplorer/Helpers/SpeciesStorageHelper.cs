using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;

namespace CH.RescueGroupsExplorer.Helpers
{
    public class SpeciesStorageHelper : BaseStorageHelper<SpeciesSource>
    {
        public SpeciesStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        protected override IRescueGroupsStorageContext<SpeciesSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            return new SpeciesSourceStorage(configuration, client, publisher);
        }
    }
}
