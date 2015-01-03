using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Storage;
using Newtonsoft.Json.Linq;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.RescueGroups.Storage
{
    public abstract class RescueGroupsStorage<T> : ISecondaryStorageContext<T> where T : class
    {
        private IRescueGroupsConfiguration _configuration;

        public RescueGroupsStorage(IRescueGroupsConfiguration configuration)
        {
            ThrowIf.Argument.IsNull(configuration, "configuration");
            _configuration = configuration;
        }

        public abstract string ObjectType
        {
            get;
        }

        public virtual string ObjectAction
        {
            get
            {
                return "list";
            }
        }

        public abstract bool IsPrivate
        {
            get;
        }

        public virtual async Task<T> GetAsync(string entityID)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            JObject request = CreateRequest
            (
                new JProperty("objectType", ObjectType),
                new JProperty("objectAction", ObjectAction)
            );

            if (IsPrivate)
            {
                IEnumerable<JProperty> loginResult = await LoginAsync();
                foreach (JProperty property in loginResult)
                {
                    request.Add(property);
                }
            }

            JObject response = await GetDataAsync(request);
            ValidateResponse(response);

            JObject data = response.Value<JObject>("data");
            return FromStorage(data.Properties());
        }

        public virtual async Task SaveAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task SaveAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public virtual async Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual async Task DeleteAllAsync()
        {
            throw new NotImplementedException();
        }

        public abstract IEnumerable<T> FromStorage(IEnumerable<JProperty> tokens);

        public JObject CreateRequest(params JProperty[] requestProperties)
        {
            // API key is required for all requests
            JObject result = new JObject(new JProperty("apikey", _configuration.APIKey));
            foreach (JProperty property in requestProperties)
            {
                result.Add(property);
            }
            return result;
        }

        public async Task<IEnumerable<JProperty>> LoginAsync()
        {
            JObject request = CreateRequest
            (
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

            ValidateResponse(response);

            JObject data = response.Value<JObject>("data");
            return new JProperty[]
            {
                data.Property("token"),
                data.Property("tokenHash")
            };
        }

        public void ValidateResponse(JObject response)
        {
            string status = response.Value<string>("status");
            if (!status.SafeEquals("ok"))
            {
                string message = response.Value<string>("message");
                throw new RescueGroupsException(string.Format("Status: {0}, Message: {1}", status, message));
            }
        }

        public async Task<JObject> GetDataAsync(JObject request)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(_configuration.Url, new StringContent(request.ToString(), Encoding.UTF8, "application/json"));

                if (!response.IsSuccessStatusCode)
                {
                    throw new RescueGroupsException("Unsuccesful status code: {0} - {1}; URL: {2}", (int)response.StatusCode, response.StatusCode, _configuration.Url);
                }

                string content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content);
            }
        }
    }
}
