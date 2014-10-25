using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Contracts;
using AR.RescueGroups.Configuration;
using AR.RescueGroups.Mappings;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace AR.RescueGroups
{
    public class RescueGroupsStorage : IStorageContext
    {
        public RescueGroupsStorage()
        {
            RescueGroupsConfigurationSection configSection = ConfigurationManager.GetSection("rescueGroups") as RescueGroupsConfigurationSection;
            if (configSection == null)
            {
                throw new RescueGroupsException("RescueGroups configuration does not exist");
            }

            this.Url = configSection.Url;
            this.APIKey = configSection.APIKey;
            this.Username = configSection.Username;
            this.Password = configSection.Password;
            this.AccountNumber = configSection.AccountNumber;
        }

        public RescueGroupsStorage(string serviceUrl, string apiKey, string username, string password, string accountNumber)
        {
            this.Url = serviceUrl;
            this.APIKey = apiKey;
            this.Username = username;
            this.Password = password;
            this.AccountNumber = accountNumber;
        }

        public int ID
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string APIKey
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string AccountNumber
        {
            get;
            set;
        }

        public async Task<T> GetAsync<T>(string entityID) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
        {
            IRescueGroupsMapping<T> mapping = RescueGroupsMappingFactory.GetMapping<T>();

            JObject request = CreateRequest
            (
                new JProperty("objectType", mapping.ObjectType),
                new JProperty("objectAction", "list")
            );

            if (mapping.IsPrivate)
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
            return mapping.ToModel(data.Properties());
        }

        public async Task SaveAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task SaveAsync<T>(IEnumerable<T> entities) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAllAsync<T>() where T : class
        {
            throw new NotImplementedException();
        }

        private JObject CreateRequest(params JProperty[] requestProperties)
        {
            // API key is required for all requests
            JObject result = new JObject(new JProperty("apikey", APIKey));
            foreach (JProperty property in requestProperties)
            {
                result.Add(property);
            }
            return result;
        }

        private async Task<IEnumerable<JProperty>> LoginAsync()
        {
            JObject request = CreateRequest
            (
                new JProperty("username", this.Username),
                new JProperty("password", this.Password),
                new JProperty("accountNumber", this.AccountNumber),
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

        private async Task<JObject> GetDataAsync(JObject request)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(Url, new StringContent(request.ToString()));

                if (!response.IsSuccessStatusCode)
                {
                    throw new RescueGroupsException("Unsuccesful status code: {0} - {1}; URL: {2}", (int)response.StatusCode, response.StatusCode, Url);
                }

                string content = await response.Content.ReadAsStringAsync();
                return JObject.Parse(content);
            }
        }
    }
}
