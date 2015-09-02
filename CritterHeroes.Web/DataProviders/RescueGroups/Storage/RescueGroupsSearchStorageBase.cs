﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class RescueGroupsSearchStorageBase<T> : RescueGroupsStorage<T> where T : class
    {
        public RescueGroupsSearchStorageBase(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
            ResultLimit = 100;
        }

        public override async Task<IEnumerable<T>> GetAllAsync()
        {
            List<T> result = new List<T>();

            JObject request = await CreateRequest();

            JObject response = await GetDataAsync(request);
            JObject data = response.Value<JObject>("data");
            IEnumerable<T> batch = FromStorage(data.Properties());
            result.AddRange(batch);

            int foundRows = response.Value<int>("foundRows");
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

        protected abstract IEnumerable<string> Fields
        {
            get;
        }

        protected IEnumerable<SearchFilter> Filters
        {
            get;
            set;
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
                Fields = Fields
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            JProperty searchProperty = new JProperty("search", JToken.FromObject(search, serializer));
            request.Add(searchProperty);

            return request;
        }
    }
}