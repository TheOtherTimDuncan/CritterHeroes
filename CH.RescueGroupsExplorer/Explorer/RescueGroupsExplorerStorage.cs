using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using Newtonsoft.Json.Linq;

namespace CH.RescueGroupsExplorer.Explorer
{
    public class RescueGroupsExplorerStorage : RescueGroupsStorage<ExplorerSource>
    {
        private string _objectType;
        private bool _isPrivate;

        private string _sortField;
        private string _keyField;

        public RescueGroupsExplorerStorage(IRescueGroupsConfiguration configuration, IHttpClient httpClient, IAppEventPublisher publisher)
            : base(configuration, httpClient, publisher)
        {
        }

        public override string ObjectType
        {
            get
            {
                return _objectType;
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return _isPrivate;
            }
        }

        protected override string SortField
        {
            get
            {
                return _sortField;
            }
        }

        protected override string KeyField
        {
            get
            {
                return _keyField;
            }
        }

        public override IEnumerable<SearchField> Fields
        {
            get;
        }

        public void SetObjectTypeAndAction(string objectType, string objectAction, bool isPrivate)
        {
            this._isPrivate = isPrivate;
            this.ObjectAction = objectAction;

            if (objectType == "people" || objectType == "businesses")
            {
                this._objectType = "contacts";
            }
            else
            {
                this._objectType = objectType;
            }
        }

        public async Task Add(string objectType)
        {
            //this.ObjectType = objectType;
            //this.ObjectAction = ObjectActions.Add;
            //this.isPrivate = true;
            var data = new
            {
                AnimalName = "Test",
                AnimalSpeciesID = "Cat",
                AnimalPrimaryBreedID = 63,
                AnimalStatusID = 6,
                AnimalDescription = "test"
            };
            RequestData requestData = new RequestData("values", new object[] { data });
            JObject request = await CreateRequest(requestData);

            //JObject response = await SendRequestAsync<JObject>(request);
        }

        public async Task<ExplorerSource> Get(string objectType, string keyField, int keyValue)
        {
            this._objectType = objectType;
            this._keyField = keyField;
            this._sortField = keyField;
            return await GetAsync(keyValue);
        }

        public override async Task<IEnumerable<ExplorerSource>> GetAllAsync(params SearchFilter[] searchFilters)
        {
            return await GetEntitiesAsync();
        }
    }
}
