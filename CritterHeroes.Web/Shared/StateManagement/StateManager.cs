using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.StateManagement;
using Microsoft.Owin;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.Shared.StateManagement
{
    public abstract class StateManager<T> : IStateManager<T>
    {
        private IOwinContext _owinContext;
        private IStateSerializer _serializer;

        private string _key;
        private T _context;

        private const string baseKey = "CritterHeroes_";

        protected StateManager(IOwinContext owinContext, IStateSerializer serializer, string key)
        {
            ThrowIf.Argument.IsNull(owinContext, "owinContext");
            ThrowIf.Argument.IsNull(serializer, "serializer");
            ThrowIf.Argument.IsNullOrEmpty(key, "key");

            this._owinContext = owinContext;
            this._serializer = serializer;

            this._key = baseKey + key;
        }

        public virtual T GetContext()
        {
            // If we have the context already just return it
            if (!EqualityComparer<T>.Default.Equals(_context, default(T)))
            {
                return _context;
            }

            // Otherwise get the context from the request cookie

            string cookie = _owinContext.Request.Cookies[_key];
            if (cookie.IsNullOrEmpty())
            {
                return default(T);
            }

            try
            {
                _context = _serializer.Deserialize<T>(cookie);
                if (IsValid(_context))
                {
                    return _context;
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
            _context = context;

            string cookie = _serializer.Serialize(context);
            _owinContext.Response.Cookies.Append(_key, cookie, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true
            });
        }

        public void ClearContext()
        {
            _context = default(T);

            _owinContext.Response.Cookies.Delete(_key, new CookieOptions()
            {
                Expires = DateTime.Now.AddYears(-1)
            });
        }

        protected abstract bool IsValid(T context);
    }
}
