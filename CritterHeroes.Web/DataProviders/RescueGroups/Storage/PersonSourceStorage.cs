using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public class PersonSourceStorage : RescueGroupsStorage<PersonSource>
    {
        public PersonSourceStorage(IRescueGroupsConfiguration configuration, IHttpClient client)
            : base(configuration, client)
        {
        }

        public override string ObjectType
        {
            get
            {
                return "contacts";
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

        public override IEnumerable<PersonSource> FromStorage(IEnumerable<JProperty> tokens)
        {
            return tokens.Select(x => new PersonSource()
            {
                ID = x.Value.Value<string>("contactID"),
                FirstName = x.Value.Value<string>("contactFirstname"),
                LastName = x.Value.Value<string>("contactLastname")
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestProperties"></param>
        /// <returns></returns>
        public override JObject CreateRequest(params JProperty[] requestProperties)
        {
            JObject request = base.CreateRequest(requestProperties);

            if (!requestProperties.Any(x => x.Name == "action" && x.Value.ToString() == "login"))
            {
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
                    ResultSort = "contactID",
                    // Filters = new[] { filter },
                    Fields = new[] { "contactID", "contactFirstname", "contactLastname" }
                };

                JsonSerializer serializer = new JsonSerializer();
                serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

                JProperty searchProperty = new JProperty("search", JToken.FromObject(search, serializer));
                request.Add(searchProperty);
            }

            return request;
        }
    }
}
