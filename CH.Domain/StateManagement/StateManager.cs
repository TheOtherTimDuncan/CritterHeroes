using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using CH.Domain.Contracts;
using Newtonsoft.Json;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Domain.StateManagement
{
    public abstract class StateManager<T> : IStateManager<T>
    {
        private IHttpContext _httpContext;
        private string _key;

        private const string baseKey = "CritterHeroes.";

        protected StateManager(IHttpContext httpContext, string key)
        {
            ThrowIf.Argument.IsNull(httpContext, "httpContext");
            ThrowIf.Argument.IsNullOrEmpty(key, "key");

            this._httpContext = httpContext;
            this._key = baseKey + key;
        }

        public virtual T GetContext()
        {
            HttpCookie cookie = _httpContext.Request.Cookies[_key];
            if (cookie == null || cookie.Value.IsNullOrEmpty())
            {
                return default(T);
            }

            try
            {
                T context = JsonConvert.DeserializeObject<T>(cookie.Value);
                if (IsValid(context))
                {
                    return context;
                }
                else
                {
                    return default(T);
                }
            }
            catch
            {
                // If the cookie can't be deserialized then return the default value
                // so the cookie can be re-created
                return default(T);
            }
        }

        public void SaveContext(T context)
        {
            string data = JsonConvert.SerializeObject(context);
            HttpCookie cookie = new HttpCookie(_key, data)
            {
                HttpOnly = true,
                Secure = true
            };
            _httpContext.Response.Cookies.Set(cookie);
        }

        public void ClearContext()
        {
            HttpCookie cookie = new HttpCookie(_key, string.Empty)
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTime.Now.AddYears(-1)
            };
            _httpContext.Response.Cookies.Set(cookie);
        }

        protected abstract bool IsValid(T context);
    }
}
