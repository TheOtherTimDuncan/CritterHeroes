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
using CritterHeroes.Web.DataProviders.RescueGroups.Responses;
using CritterHeroes.Web.Models.LogEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class RescueGroupsStorage<TEntity> : IRescueGroupsStorageContext<TEntity> where TEntity : BaseSource
    {
        private IRescueGroupsConfiguration _configuration;
        private IHttpClient _client;
        private IAppEventPublisher _publisher;

        private string _token;
        private string _tokenHash;

        private JsonSerializer _serializer;
        private List<TEntity> _entityTracker;

        public RescueGroupsStorage(IRescueGroupsConfiguration configuration, IHttpClient client, IAppEventPublisher publisher)
        {
            ThrowIf.Argument.IsNull(configuration, nameof(configuration));

            this._configuration = configuration;
            this._client = client;
            this._publisher = publisher;

            this.ResultLimit = 100;

            this._serializer = new JsonSerializer();
            this._serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();

            this._entityTracker = new List<TEntity>();
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

        protected abstract string KeyField
        {
            get;
        }

        public IEnumerable<SearchFilter> Filters
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

        public virtual async Task<TEntity> GetAsync(string entityID)
        {
            SearchFilter filter = new SearchFilter()
            {
                FieldName = KeyField,
                Operation = SearchFilterOperation.Equal,
                Criteria = entityID
            };

            IEnumerable<TEntity> result = await GetAllAsync(filter);
            return result.SingleOrDefault();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(params SearchFilter[] searchFilters)
        {
            Filters = searchFilters;
            SearchModel search = null;

            if (searchFilters.IsNullOrEmpty())
            {
                ObjectAction = ObjectActions.List;
            }
            else
            {
                ObjectAction = ObjectActions.Search;

                search = new SearchModel()
                {
                    ResultStart = 0,
                    ResultLimit = ResultLimit,
                    ResultSort = SortField,
                    Filters = Filters,
                    FilterProcessing = FilterProcessing,
                    Fields = Fields.Where(x => x.IsSelected).SelectMany(x => x.FieldNames)
                };
            }

            return await GetEntitiesAsync(search);
        }

        protected virtual async Task<IEnumerable<TEntity>> GetEntitiesAsync(SearchModel searchModel = null)
        {
            List<TEntity> result = new List<TEntity>();

            RequestData requestData = null;
            if (searchModel != null)
            {
                requestData = new RequestData("search", searchModel);
            }

            JObject request = await CreateRequest(requestData);

            DataListResponseModel<TEntity> response = await SendRequestAsync<DataListResponseModel<TEntity>>(request);

            if (!response.Data.IsNullOrEmpty())
            {
                result.AddRange(response.Data.Select(x => x.Value));
            }

            if (searchModel != null)
            {
                int foundRows = response.FoundRows;
                searchModel.ResultStart += ResultLimit;
                while (searchModel.ResultStart < foundRows)
                {
                    searchModel.ResultStart += ResultLimit;
                    request = await CreateRequest(requestData);
                    response = await SendRequestAsync<DataListResponseModel<TEntity>>(request);
                    if (!response.Data.IsNullOrEmpty())
                    {
                        result.AddRange(response.Data.Select(x => x.Value));
                    }
                }
            }

            _entityTracker.AddRange(result);

            return result;
        }

        protected virtual async Task<JObject> CreateRequest(RequestData requestData = null)
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

            if (requestData != null)
            {
                JProperty dataProperty = new JProperty(requestData.Key, JToken.FromObject(requestData.Data, _serializer));
                request.Add(dataProperty);
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
                    new JProperty("action", ObjectActions.Login)
                );

                DataResponseModel<LoginResponseData> response;
                try
                {
                    response = await SendRequestAsync<DataResponseModel<LoginResponseData>>(request);
                }
                catch (RescueGroupsException ex)
                {
                    throw new RescueGroupsException("Login", ex);
                }

                _token = response.Data.Token;
                _tokenHash = response.Data.TokenHash;
            }

            return new JProperty[]
            {
                new JProperty("token", _token),
                new JProperty("tokenHash", _tokenHash)
            };
        }

        protected void ValidateResponse(BaseResponseModel response)
        {
            if (!response.Status.SafeEquals("ok"))
            {
                string errorMessage;

                if (response.Messages == null || response.Messages.GeneralMessages.IsNullOrEmpty())
                {
                    errorMessage = "Error response not found";
                }
                else
                {
                    errorMessage = String.Join(", ", response.Messages.GeneralMessages.Select(x => x.MessageText));
                }

                throw new RescueGroupsException(errorMessage);
            }
        }

        protected async Task<TResponse> SendRequestAsync<TResponse>(JObject request) where TResponse : BaseResponseModel
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

            TResponse responseModel = JsonConvert.DeserializeObject<TResponse>(content);
            ValidateResponse(responseModel);

            return responseModel;
        }
    }
}
