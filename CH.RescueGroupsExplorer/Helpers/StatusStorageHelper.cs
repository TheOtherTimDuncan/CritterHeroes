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
    public class StatusStorageHelper : BaseStorageHelper<CritterStatusSource>
    {
        public StatusStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
            : base(configuration, client, publisher)
        {
        }

        protected override IRescueGroupsStorageContext<CritterStatusSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            return new CritterStatusSourceStorage(configuration, client, publisher);
        }
    }
}
