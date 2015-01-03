using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace CritterHeroes.Web.Common.Proxies
{
    // This is only to assist with validation the dependency container
    public class FakeOwinContext : IOwinContext
    {
        public IAuthenticationManager Authentication
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDictionary<string, object> Environment
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IOwinRequest Request
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IOwinResponse Response
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IOwinContext Set<T>(string key, T value)
        {
            throw new NotImplementedException();
        }

        public TextWriter TraceOutput
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
