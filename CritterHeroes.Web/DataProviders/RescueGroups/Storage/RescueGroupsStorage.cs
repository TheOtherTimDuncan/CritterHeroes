using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Models.LogEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class RescueGroupsStorage<T> : IRescueGroupsStorageContext<T> where T : class
    {
        private IRescueGroupsConfiguration _configuration;
        private IHttpClient _client;
        private IAppEventPublisher _publisher;

        private string _token;
        private string _tokenHash;

        public RescueGroupsStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            ThrowIf.Argument.IsNull(configuration, nameof(configuration));

            this._configuration = configuration;
            this._client = client;
            this._publisher = publisher;
        }

        public abstract string ObjectType
        {
            get;
        }

        public virtual string ObjectAction
        {
            get;
            protected set;
        }

        public abstract bool IsPrivate
        {
            get;
        }


        public abstract IEnumerable<SearchField> Fields
        {
            get;
        }

        protected abstract string SortField
        {
            get;
        }

        public IEnumerable<SearchFilter> Filters
        {
            get;
            set;
        }

        protected int ResultStart
        {
            get;
            set;
        }

        public int ResultLimit
        {
            get;
            set;
        }

        public string FilterProcessing
        {
            get;
            set;
        }

        public virtual Task<T> GetAsync(string entityID)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync(params SearchFilter[] searchFilters)
        {
            Filters = searchFilters;
            return await GetAllAsync();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            if (Filters.IsNullOrEmpty())
            {
                ObjectAction = ObjectActions.List;
            }
            else
            {
                ObjectAction = ObjectActions.Search;
            }

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

        public virtual Task SaveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task SaveAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerable<T> FromStorage(IEnumerable<JProperty> tokens);

        protected virtual async Task<JObject> CreateRequest()
        {
            JObject request = new JObject();

            if (IsPrivate || ObjectAction == ObjectActions.Search)
            {
                IEnumerable<JProperty> loginResult = await LoginAsync();
                foreach (JProperty property in loginResult)
                {
                    request.Add(property);
                }
            }
            else
            {
                // API key is needed for all public requests
                request.Add(new JProperty("apikey", _configuration.APIKey));
            }

            ThrowIf.Argument.IsNullOrEmpty(ObjectAction, nameof(ObjectAction));

            request.Add(new JProperty("objectType", ObjectType));
            request.Add(new JProperty("objectAction", ObjectAction));

            if (ObjectAction == ObjectActions.Search)
            {
                SearchModel search = new SearchModel()
                {
                    ResultStart = ResultStart,
                    ResultLimit = ResultLimit,
                    ResultSort = SortField,
                    Filters = Filters,
                    FilterProcessing = FilterProcessing,
                    Fields = Fields.Where(x => x.IsSelected).SelectMany(x => x.FieldNames)
                };

                JsonSerializer serializer = new JsonSerializer();
                serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

                JProperty searchProperty = new JProperty("search", JToken.FromObject(search, serializer));
                request.Add(searchProperty);
            }

            return request;
        }

        protected async Task<IEnumerable<JProperty>> LoginAsync()
        {
            if (_token.IsNullOrEmpty() && _tokenHash.IsNullOrEmpty())
            {
                JObject request = new JObject(
                    new JProperty("username", _configuration.Username),
                    new JProperty("password", _configuration.Password),
                    new JProperty("accountNumber", _configuration.AccountNumber),
                    new JProperty("action", "login")
                );

                JObject response;
                try
                {
                    response = await GetDataAsync(request);
                }
                catch (RescueGroupsException ex)
                {
                    throw new RescueGroupsException("Login", ex);
                }

                JObject data = response.Value<JObject>("data");

                _token = data.Property("token").Value.Value<string>();
                _tokenHash = data.Property("tokenHash").Value.Value<string>();

                return new JProperty[]
                {
                    data.Property("token"),
                    data.Property("tokenHash")
                };
            }
            else
            {
                return new JProperty[]
                {
                    new JProperty("token", _token),
                    new JProperty("tokenHash", _tokenHash)
                };
            }
        }

        protected void ValidateResponse(JObject response)
        {
            string status = response.Value<string>("status");
            if (!status.SafeEquals("ok"))
            {
                JToken property = response["messages"]["generalMessages"];
                string errorMessage;
                if (property != null && property.HasValues)
                {
                    errorMessage = property[0]["messageText"].Value<string>();
                }
                else
                {
                    errorMessage = "Unable to parse error response";
                }
                throw new RescueGroupsException(errorMessage);
            }
        }

        protected async Task<JObject> GetDataAsync(JObject request)
        {
            string jsonRequest = request.ToString();
            HttpResponseMessage response = await _client.PostAsync(_configuration.Url, new StringContent(jsonRequest, Encoding.UTF8, "application/json"));

            string content = await response.Content.ReadAsStringAsync();

            RescueGroupsLogEvent logEvent;
            if (request["password"] != null)
            {
                logEvent = RescueGroupsLogEvent.Create(_configuration.Url, "Login", content, response.StatusCode);
            }
            else
            {
                logEvent = RescueGroupsLogEvent.Create(_configuration.Url, jsonRequest, content, response.StatusCode);
            }
            _publisher.Publish(logEvent);

            if (!response.IsSuccessStatusCode)
            {
                throw new RescueGroupsException("Unsuccesful status code: {0} - {1}; URL: {2}", (int)response.StatusCode, response.StatusCode, _configuration.Url);
            }

            JObject result = JObject.Parse(content);
            ValidateResponse(result);

            return result;
        }
    }
}
