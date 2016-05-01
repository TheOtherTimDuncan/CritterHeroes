using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using TOTD.Utility.EnumerableHelpers;

namespace CH.RescueGroupsExplorer.Helpers
{
    public abstract class BaseStorageHelper<TEntity> where TEntity : BaseSource
    {
        public BaseStorageHelper(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            StorageContext = CreateStorageContext(configuration, client, publisher);
        }

        public IRescueGroupsStorageContext<TEntity> StorageContext
        {
            get;
        }

        protected abstract IRescueGroupsStorageContext<TEntity> CreateStorageContext(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher);

        public abstract TEntity CreateEntity();

        public async Task SearchAsync(CheckedListBox.CheckedItemCollection checkedItems)
        {
            IEnumerable<SearchFilter> filters = null;
            StorageContext.Fields.ForEach(x => x.IsSelected = false);

            filters = StorageContext.Fields.Where(x => checkedItems.Contains(x.Name)).Select(x =>
            {
                x.IsSelected = true;
                return new SearchFilter()
                {
                    FieldName = x.Name,
                    Criteria = SearchFilterOperation.NotBlank
                };
            });

            StorageContext.FilterProcessing = string.Join(" or ", filters.Select((SearchFilter filter, int i) => i + 1));

            await StorageContext.GetAllAsync(filters.ToArray());
        }

        public async Task ListAsync()
        {
            await StorageContext.GetAllAsync();
        }

        public async Task GetAsync(int entityID)
        {
            await StorageContext.GetAsync(entityID);
        }
    }
}
