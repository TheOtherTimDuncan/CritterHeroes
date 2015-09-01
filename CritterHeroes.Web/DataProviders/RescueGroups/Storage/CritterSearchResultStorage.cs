using System;
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
    public class CritterSearchResultStorage : RescueGroupsStorage<CritterSearchResult>
    {
        public CritterSearchResultStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "animals";
            }
        }

        public override string ObjectAction
        {
            get
            {
                return "search";
            }
        }

        public override bool IsPrivate
        {
            get
            {
                return true;
            }
        }

        public override IEnumerable<CritterSearchResult> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new CritterSearchResult()
            {
                ID = x.Value.Value<int>("animalID"),
                Name = x.Value.Value<string>("animalName")
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestProperties"></param>
        /// <returns></returns>
        protected override async Task<JObject> CreateRequest()
        {
            JObject request = await base.CreateRequest();

            SearchFilter filter = new SearchFilter()
            {
                FieldName = "animalStatus",
                Operation = "equal",
                Criteria = "Sponsorship"
            };

            SearchModel search = new SearchModel()
            {
                ResultStart = 0,
                ResultLimit = 100,
                ResultSort = "animalID",
                Filters = new[] { filter },
                Fields = new[] { "animalID", "animalStatus", "animalName" }
            };

            JsonSerializer serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            JProperty searchProperty = new JProperty("search", JToken.FromObject(search, serializer));
            request.Add(searchProperty);

            return request;
        }
    }
}
