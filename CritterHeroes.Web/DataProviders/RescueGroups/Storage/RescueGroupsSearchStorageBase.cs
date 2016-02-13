using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class RescueGroupsSearchStorageBase<T> : RescueGroupsStorage<T>, IRescueGroupsSearchStorage<T> where T : class
    {
        public RescueGroupsSearchStorageBase(IRescueGroupsConfiguration configuration, IHttpClient client, IRescueGroupsLogger logger)
            : base(configuration, client, logger)
        {
            ResultLimit = 100;
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            List<T> result = new List<T>();
            ResultStart = 0;

            JObject request = await CreateRequest();

            JObject response = await GetDataAsync(request);

            JObject data;
            IEnumerable<T> batch;

            if (response["data"].HasValues)
            {
                data = response.Value<JObject>("data");
                batch = FromStorage(data.Properties());
                result.AddRange(batch);
            }

            int foundRows = response.Value<int>("foundRows");
            ResultStart += ResultLimit;
            while (ResultStart < foundRows)
            {
                ResultStart += ResultLimit;
                request = await CreateRequest();
                response = await GetDataAsync(request);
                if (response["data"].HasValues)
                {
                    data = response.Value<JObject>("data");
                    batch = FromStorage(data.Properties());
                    result.AddRange(batch);
                }
            }

            return result;
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override string ObjectAction
        {
            get
            {
                return "search";
            }
        }

        protected int ResultStart
        {
            get;
            set;
        }

        protected int ResultLimit
        {
            get;
            set;
        }

        protected abstract string SortField
        {
            get;
        }

        public abstract IEnumerable<SearchField> Fields
        {
            get;
        }

        protected override async Task<JObject> CreateRequest()
        {
            JObject request = await base.CreateRequest();

            SearchModel search = new SearchModel()
            {
                ResultStart = ResultStart,
                ResultLimit = ResultLimit,
                ResultSort = SortField,
                Filters = Filters,
                Fields = Fields.Where(x => x.IsSelected).Select(x => x.Name)
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            JProperty searchProperty = new JProperty("search", JToken.FromObject(search, serializer));
            request.Add(searchProperty);

            return request;
        }
    }
}
