using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Configuration;
using CritterHeroes.Web.Domain.Contracts.Events;
using CritterHeroes.Web.Domain.Contracts.Storage;

namespace CH.RescueGroupsHelper.Explorer
{
    public class RescueGroupsExplorerStorageHelper : BaseStorageHelper<ExplorerSource>
    {
        private RescueGroupsExplorerStorage _storage;

        public RescueGroupsExplorerStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher, string objectType, string objectAction, bool isPrivate)
            : base(configuration, client, publisher)
        {
            _storage.SetObjectTypeAndAction(objectType, objectAction, isPrivate);
        }

        protected override IRescueGroupsStorageContext<ExplorerSource> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            _storage = new RescueGroupsExplorerStorage(configuration, client, publisher);
            return _storage;
        }

        public override ExplorerSource CreateEntity()
        {
            throw new NotImplementedException();
        }

        public override void UpdateEntity(ExplorerSource entity)
        {
            throw new NotImplementedException();
        }
    }
}
