﻿using System;
using System.Collections.Generic;
using System.Linq;
using CH.Domain.Contracts;
using Microsoft.Owin;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.StringHelpers;

namespace CH.Domain.StateManagement
{
    public abstract class StateManager<T> : IStateManager<T>
    {
        private IOwinContext _owinContext;
        private IStateSerializer _serializer;

        private string _key;

        private const string baseKey = "CritterHeroes.";

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
            string cookie = _owinContext.Request.Cookies[_key];
            if (cookie.IsNullOrEmpty())
            {
                return default(T);
            }

            try
            {
                T context = _serializer.Deserialize<T>(cookie);
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
            string cookie = _serializer.Serialize(context);
            _owinContext.Response.Cookies.Append(_key, cookie, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true
            });
        }

        public void ClearContext()
        {
            _owinContext.Response.Cookies.Delete(_key);
        }

        protected abstract bool IsValid(T context);
    }
}
